<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { apiGet } from '@/services/api'
import { useBrands } from '@/composables/useBrands'
import { useProducts } from '@/composables/useProducts'
import ProductCardComponent from '@/components/ProductCardComponent.vue'

const route = useRoute()
const { fetchBrandById } = useBrands()
const { products, loading: loadingLegacy, error: errorLegacy, fetchProductsByBrand } = useProducts()

// Estado
const brandName = ref('Marca')
const brandLogo = ref(null)

const loading = ref(false)
const error = ref(null)
const pagedItems = ref([])

// Paginação
const pageNumber = ref(1)
const pageSize = ref(16)
const totalPages = ref(1)
const totalCount = ref(0)

// Nome/Logo da marca
async function loadBrand() {
  try {
    const b = await fetchBrandById(route.params.id)
    brandName.value = b?.name || 'Marca'
    brandLogo.value = b?.imageUrl || null
  } catch {
    brandName.value = 'Marca'
    brandLogo.value = null
  }
}

// Chamada preferencial: /products/filter?brandId=...
async function fetchFromFilter() {
  loading.value = true
  error.value = null

  const qs = new URLSearchParams({
    pageNumber: String(pageNumber.value),
    pageSize: String(pageSize.value),
    brandId: String(route.params.id),
  }).toString()

  try {
    const data = await apiGet(`/products/filter?${qs}`)

    const list = Array.isArray(data) ? data : (data?.items ?? [])
    pagedItems.value = list

    totalPages.value = Number(
      data?.totalPages ??
      Math.max(1, Math.ceil((data?.totalCount ?? list.length) / pageSize.value))
    )
    totalCount.value = Number(data?.totalCount ?? list.length)

    // se vier vazio, cai pro fallback para não quebrar
    if (!list.length) await fetchLegacy()
  } catch (e) {
    await fetchLegacy()
    error.value = e?.message || null
  } finally {
    loading.value = false
  }
}

// Fallback: busca todos e filtra no cliente (useProducts)
async function fetchLegacy() {
  try {
    await fetchProductsByBrand(route.params.id)
    totalCount.value = products.value.length
    totalPages.value = Math.max(1, Math.ceil(products.value.length / pageSize.value))
  } catch (_) {
    // mantém erro do legacy se houver
  }
}

// Carregar tudo
async function loadAll() {
  pageNumber.value = 1
  await Promise.all([loadBrand(), fetchFromFilter()])
}

// Lifecycle
onMounted(loadAll)
watch(() => route.params.id, async () => {
  await loadAll()
  window.scrollTo({ top: 0, behavior: 'auto' })
})
watch([pageNumber, pageSize], async () => {
  await fetchFromFilter()
  window.scrollTo({ top: 0, behavior: 'auto' })
})

// Lista exibida (normaliza coverImageUrl)
const displayed = computed(() => {
  const list = pagedItems.value?.length ? pagedItems.value : (() => {
    const start = (pageNumber.value - 1) * pageSize.value
    return products.value.slice(start, start + pageSize.value)
  })()

  return list.map(p => ({
    ...p,
    coverImageUrl:
      p.coverImageUrl ||
      p.imageUrl ||
      p.additionalImageUrl1 ||
      p.additionalImageUrl2 ||
      p.additionalImageUrl3 ||
      p.additionalImageUrl4 ||
      'https://placehold.co/400x400?text=Sem+Imagem'
  }))
})

function goToPage(p) {
  if (p < 1 || p > totalPages.value || p === pageNumber.value) return
  pageNumber.value = p
}

const showingRange = computed(() => {
  const start = (pageNumber.value - 1) * pageSize.value + 1
  const end = Math.min(pageNumber.value * pageSize.value, totalCount.value || (pageNumber.value * pageSize.value))
  return { start, end }
})

function handleAddToCart(prod) {
  console.log('Adicionar ao carrinho:', prod)
}
</script>

<template>
  <div class="py-12 bg-black min-h-screen text-white">
    <div class="max-w-7xl mx-auto px-4">
      <!-- Cabeçalho -->
      <div class="flex flex-col items-center gap-3 text-center mb-6">
        <div
          v-if="brandLogo"
          class="w-full max-w-[280px] bg-neutral-900/80 border border-neutral-800 rounded-2xl p-4 flex justify-center items-center"
        >
          <img :src="brandLogo" :alt="brandName" class="max-h-24 w-auto object-contain" />
        </div>
        <h2 class="text-3xl font-bold text-orange-400">{{ brandName }}</h2>
      </div>

      <!-- Top bar -->
      <div class="flex flex-wrap items-center justify-between gap-4 mb-6">
        <div class="text-sm text-orange-200">
          <template v-if="totalCount">
            Mostrando {{ showingRange.start }}–{{ showingRange.end }} de {{ totalCount }}
          </template>
        </div>

        <div class="flex items-center gap-3">
          <label class="text-sm text-neutral-300">Itens por página</label>
          <select
            v-model.number="pageSize"
            class="bg-neutral-900 border border-neutral-700 rounded px-3 py-2 text-sm"
          >
            <option :value="8">8</option>
            <option :value="12">12</option>
            <option :value="16">16</option>
            <option :value="24">24</option>
          </select>
        </div>
      </div>

      <!-- Estados -->
      <div v-if="loading || loadingLegacy" class="text-orange-200">Carregando...</div>
      <div v-else-if="(error || errorLegacy) && !displayed.length" class="text-red-400">
        {{ error || errorLegacy }}
      </div>

      <!-- Grid -->
      <div v-else>
        <div v-if="displayed && displayed.length" class="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-4 gap-4">
          <ProductCardComponent
            v-for="p in displayed"
            :key="p.id ?? p.code"
            :product="p"
            @addToCart="handleAddToCart"
          />
        </div>

        <div v-else class="text-neutral-300">Nenhum produto encontrado.</div>

        <!-- Paginação -->
        <div v-if="totalPages > 1" class="mt-8 flex flex-wrap items-center justify-center gap-2">
          <button
            class="px-3 py-2 rounded bg-neutral-900 border border-neutral-700 text-sm disabled:opacity-50"
            :disabled="pageNumber === 1"
            @click="goToPage(pageNumber - 1)"
          >
            Anterior
          </button>

          <button
            v-for="p in Math.min(totalPages, 7)"
            :key="`p-${p}`"
            class="px-3 py-2 rounded border text-sm"
            :class="p === pageNumber ? 'bg-orange-500 border-orange-500 text-black' : 'bg-neutral-900 border-neutral-700'"
            @click="goToPage(p)"
          >
            {{ p }}
          </button>

          <button
            class="px-3 py-2 rounded bg-neutral-900 border border-neutral-700 text-sm disabled:opacity-50"
            :disabled="pageNumber === totalPages"
            @click="goToPage(pageNumber + 1)"
          >
            Próxima
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
