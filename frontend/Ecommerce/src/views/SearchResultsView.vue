<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import ProductCardComponent from '@/components/ProductCardComponent.vue'
import { apiGet } from '@/services/api'

const route = useRoute()
const router = useRouter()

/* ----------------- Estado ----------------- */
const results = ref([])           // itens (quando vier array) OU última página (quando vier paginado)
const loading = ref(false)
const error = ref(null)

const pageNumber = ref( Number(route.query.page ?? 1) )
const pageSize   = ref( Number(route.query.size ?? 16) )

const serverTotalCount = ref(null) // se backend informar, usamos
const serverTotalPages = ref(null)

const q = computed(() => String(route.query.q ?? '').trim())

/* ----------------- Helpers comuns ----------------- */
const KABUM_HOST = 'https://www.kabum.com.br'

function fixUrl(u) {
  if (!u || typeof u !== 'string') return null
  if (u.startsWith('http')) return u
  if (u.startsWith('/')) return `${KABUM_HOST}${u}`
  return u
}

function parsePrice(raw) {
  if (raw === undefined || raw === null) return null
  if (typeof raw === 'number') return raw
  if (typeof raw === 'string') {
    const norm = raw.replace(/[^\d,.-]/g, '').replace(/\./g, '').replace(',', '.')
    const n = parseFloat(norm)
    return Number.isNaN(n) ? null : n
  }
  return null
}
function toBRL(n) {
  return n === null ? '' : n.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
}

function normalizeProduct(p = {}) {
  const imgs = [
    p.coverImageUrl, p.imageUrl, p.image_url,
    p.image_url0, p.image_url1, p.image_url2, p.image_url3, p.image_url4
  ].map(fixUrl).filter(Boolean)
  const coverImageUrl = imgs[0] || 'https://placehold.co/400x400?text=Sem+Imagem'

  const original_price = parsePrice(p.original_price ?? p.originalPrice)
  const discount_price = parsePrice(p.discount_price ?? p.discountPrice ?? p.price)

  const stars  = Number(p.stars ?? p.averageStars ?? 0) || 0
  const rating = Number(p.rating ?? p.reviewCount ?? 0) || 0

  const id   = p.id ?? p.code ?? p.productId
  const code = p.code ?? ''
  const name = p.name ?? p.nome ?? ''

  return {
    ...p,
    id, code, name,
    coverImageUrl,
    imageUrl: coverImageUrl,

    original_price,
    discount_price,
    originalPrice: original_price,
    discountPrice: discount_price,

    originalPriceBRL: toBRL(original_price),
    discountPriceBRL: toBRL(discount_price),

    stars, rating,
  }
}

/* ----------------- Derivados de UI ----------------- */
// Quando vier paginação do backend, usamos os totais dele; senão, calculamos pelo array
const totalCount = computed(() => {
  if (serverTotalCount.value != null) return serverTotalCount.value
  return results.value.length
})
const totalPages = computed(() => {
  if (serverTotalPages.value != null) return serverTotalPages.value
  return Math.max(1, Math.ceil(totalCount.value / pageSize.value))
})

const displayed = computed(() => {
  // Backend paginado já devolve os itens da página; só retornamos.
  if (serverTotalPages.value != null) return results.value
  // Fallback: paginação no cliente
  const start = (pageNumber.value - 1) * pageSize.value
  return results.value.slice(start, start + pageSize.value)
})

function pushQuery() {
  router.replace({
    query: {
      ...route.query,
      q: q.value || undefined,
      page: pageNumber.value !== 1 ? String(pageNumber.value) : undefined,
      size: pageSize.value !== 16 ? String(pageSize.value) : undefined,
    }
  })
}

function goToPage(p) {
  if (p < 1 || p > totalPages.value || p === pageNumber.value) return
  pageNumber.value = p
  pushQuery()
  fetchSearch()
}

/* ----------------- Fetch (paginado + fallback) ----------------- */
async function fetchSearch() {
  if (!q.value) {
    results.value = []
    serverTotalCount.value = null
    serverTotalPages.value = null
    return
  }

  loading.value = true
  error.value = null

  try {
    // 1) Tenta rota paginada do backend
    const url = `/products/search?query=${encodeURIComponent(q.value)}&pageNumber=${pageNumber.value}&pageSize=${pageSize.value}`
    const data = await apiGet(url)

    // Suporte a formatos comuns: { items, totalCount, totalPages } ou { data, total, pages }
    const items =
      data?.items ??
      data?.data ??
      (Array.isArray(data) ? data : [])

    const normalized = items.map(normalizeProduct)
    results.value = normalized

    const tCount = data?.totalCount ?? data?.total ?? null
    const tPages = data?.totalPages ?? data?.pages ?? null

    serverTotalCount.value = (typeof tCount === 'number') ? tCount : null
    serverTotalPages.value = (typeof tPages === 'number') ? tPages : null

    // Se veio array puro, considera fallback de paginação cliente
    if (Array.isArray(data)) {
      serverTotalCount.value = null
      serverTotalPages.value = null
    }
  } catch (err1) {
    try {
      // 2) Fallback: rota de sugestões (array simples)
      const arr = await apiGet(`/products/search-suggestions?query=${encodeURIComponent(q.value)}`)
      const normalized = (Array.isArray(arr) ? arr : []).map(normalizeProduct)
      results.value = normalized
      serverTotalCount.value = null
      serverTotalPages.value = null
    } catch (err2) {
      error.value = err2?.message || err1?.message || 'Erro ao buscar produtos'
      results.value = []
      serverTotalCount.value = null
      serverTotalPages.value = null
    }
  } finally {
    loading.value = false
  }
}

async function load() {
  pageNumber.value = Number(route.query.page ?? 1) || 1
  pageSize.value   = Number(route.query.size ?? 16) || 16
  await fetchSearch()
  window.scrollTo({ top: 0, behavior: 'auto' })
}

/* ----------------- Lifecycle ----------------- */
onMounted(load)
watch(() => [route.query.q, route.query.page, route.query.size], load)

function changePageSize(val) {
  pageSize.value = Number(val)
  pageNumber.value = 1
  pushQuery()
  fetchSearch()
}

function handleAddToCart(prod) {
  console.log('Adicionar ao carrinho:', prod)
}
</script>

<template>
  <div class="py-12 bg-black min-h-screen text-white">
    <div class="max-w-7xl mx-auto px-4">
      <h2 class="text-3xl font-bold text-orange-400 mb-6">
        Resultados para: <span class="text-orange-300">"{{ q }}"</span>
      </h2>

      <div class="flex flex-wrap items-center justify-between gap-4 mb-6">
        <div class="text-sm text-orange-200">
          <template v-if="totalCount">Encontrados {{ totalCount }} itens</template>
          <template v-else>Digite outro termo para novos resultados</template>
        </div>

        <div class="flex items-center gap-3">
          <label class="text-sm text-neutral-300">Itens por página</label>
          <select
            :value="pageSize"
            @change="changePageSize($event.target.value)"
            class="bg-neutral-900 border border-neutral-700 rounded px-3 py-2 text-sm"
          >
            <option :value="8">8</option>
            <option :value="12">12</option>
            <option :value="16">16</option>
            <option :value="24">24</option>
          </select>
        </div>
      </div>

      <div v-if="loading" class="text-orange-200">Carregando...</div>
      <div v-else-if="error && !displayed.length" class="text-red-400">{{ error }}</div>

      <div v-else>
        <div v-if="displayed && displayed.length" class="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-4 gap-4">
          <ProductCardComponent
            v-for="p in displayed"
            :key="p.id ?? p.code"
            :product="p"
            @addToCart="handleAddToCart"
          />
        </div>

        <div v-else class="text-neutral-300">Nenhum produto encontrado para "{{ q }}".</div>

        <div v-if="totalPages > 1" class="mt-8 flex flex-wrap items-center justify-center gap-2">
          <button
            class="px-3 py-2 rounded bg-neutral-900 border border-neutral-700 text-sm disabled:opacity-50"
            :disabled="pageNumber === 1"
            @click="goToPage(pageNumber - 1)"
          >Anterior</button>

          <button
            v-for="p in Math.min(totalPages, 7)"
            :key="`p-${p}`"
            class="px-3 py-2 rounded border text-sm"
            :class="p === pageNumber ? 'bg-orange-500 border-orange-500 text-black' : 'bg-neutral-900 border-neutral-700'"
            @click="goToPage(p)"
          >{{ p }}</button>

          <button
            class="px-3 py-2 rounded bg-neutral-900 border border-neutral-700 text-sm disabled:opacity-50"
            :disabled="pageNumber === totalPages"
            @click="goToPage(pageNumber + 1)"
          >Próxima</button>
        </div>
      </div>
    </div>
  </div>
</template>
