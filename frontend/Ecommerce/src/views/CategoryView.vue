<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { apiGet } from '@/services/api'
import { useCategories } from '@/composables/useCategories'
import { useProducts } from '@/composables/useProducts'
import ProductCardComponent from '@/components/ProductCardComponent.vue' 

const route = useRoute()
const { fetchCategoryById } = useCategories()
const { products, loading: loadingLegacy, error: errorLegacy, fetchProductsByCategory } = useProducts()

const categoryName = ref('Categoria')
const loading = ref(false)
const error = ref(null)
const pagedItems = ref([])

const pageNumber = ref(1)
const pageSize = ref(16)
const totalPages = ref(1)
const totalCount = ref(0)

async function loadCategoryName() {
  try {
    const c = await fetchCategoryById(route.params.id)
    categoryName.value = c?.name || 'Categoria'
  } catch {
    categoryName.value = 'Categoria'
  }
}

async function fetchFromFilter() {
  loading.value = true
  error.value = null

  const qs = new URLSearchParams({
    pageNumber: String(pageNumber.value),
    pageSize: String(pageSize.value),
    categoryId: String(route.params.id),
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

    if (!list.length) await fetchLegacy()
  } catch (e) {
    await fetchLegacy()
    error.value = e?.message || null
  } finally {
    loading.value = false
  }
}

async function fetchLegacy() {
  try {
    await fetchProductsByCategory(route.params.id)
    totalCount.value = products.value.length
    totalPages.value = Math.max(1, Math.ceil(products.value.length / pageSize.value))
  } catch (_) {}
}

async function loadAll() {
  pageNumber.value = 1
  await Promise.all([loadCategoryName(), fetchFromFilter()])
}

onMounted(loadAll)
watch(() => route.params.id, async () => {
  await loadAll()
  window.scrollTo({ top: 0, behavior: 'auto' })
})
watch([pageNumber, pageSize], async () => {
  await fetchFromFilter()
  window.scrollTo({ top: 0, behavior: 'auto' })
})

const displayed = computed(() => {
  const list = pagedItems.value?.length ? pagedItems.value : (() => {
    const start = (pageNumber.value - 1) * pageSize.value
    return products.value.slice(start, start + pageSize.value)
  })()
  return list.map(p => ({
    ...p,
    coverImageUrl:
      p.coverImageUrl ||
      p.image_url0 ||
      p.additionalImage_url1 ||
      p.additionalImage_url2 ||
      p.additionalImage_url3 ||
      p.additionalImage_url4 ||
      'https://placehold.co/400x400?text=Sem+Imagem'
  }))
})

function goToPage(p) {
  if (p < 1 || p > totalPages.value || p === pageNumber.value) return
  pageNumber.value = p
}

const showingRange = computed(() => {
  const start = (pageNumber.value - 1) * pageSize.value + 1
  const end = Math.min(pageNumber.value * pageSize.value, totalCount.value)
  return { start, end }
})

function handleAddToCart(prod) {
  console.log('Adicionar ao carrinho:', prod)
}

/* --- Paginação dinâmica --- */
const visiblePages = computed(() => {
  const delta = 2 // número de páginas para mostrar antes/depois
  const pages = []
  const start = Math.max(1, pageNumber.value - delta)
  const end = Math.min(totalPages.value, pageNumber.value + delta)

  for (let i = start; i <= end; i++) {
    pages.push(i)
  }

  // Adiciona reticências se necessário
  if (start > 1) pages.unshift('...')
  if (end < totalPages.value) pages.push('...')
  if (!pages.includes(1)) pages.unshift(1)
  if (!pages.includes(totalPages.value)) pages.push(totalPages.value)

  return pages.filter((v, i, a) => a.indexOf(v) === i)
})
</script>

<template>
  <div class="py-12 bg-black min-h-screen text-white">
    <div class="max-w-7xl mx-auto px-4">
      <h2 class="text-3xl font-bold text-orange-400 mb-6">{{ categoryName }}</h2>

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

      <div v-if="loading || loadingLegacy" class="text-orange-200">Carregando...</div>
      <div v-else-if="(error || errorLegacy) && !displayed.length" class="text-red-400">
        {{ error || errorLegacy }}
      </div>

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

          <template v-for="(p, idx) in visiblePages" :key="idx">
            <button
              v-if="p !== '...'"
              class="px-3 py-2 rounded border text-sm"
              :class="p === pageNumber ? 'bg-orange-500 border-orange-500 text-black' : 'bg-neutral-900 border-neutral-700'"
              @click="goToPage(p)"
            >
              {{ p }}
            </button>
            <span v-else class="px-2 text-neutral-400">…</span>
          </template>

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
