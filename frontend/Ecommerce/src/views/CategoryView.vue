<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { apiGet } from '@/services/api'
import { useCategories } from '@/composables/useCategories'
import { useProducts } from '@/composables/useProducts'
import ProductListing from '@/components/ProductListing.vue'

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
    let data = await apiGet(`/products/filter?${qs}`)
    if (typeof data === 'string') {
      try { data = JSON.parse(data) } catch {}
    }
    const list = Array.isArray(data) ? data : (data?.items ?? [])
    pagedItems.value = list
    totalPages.value = Number(data?.totalPages ?? Math.max(1, Math.ceil((data?.totalCount ?? list.length) / pageSize.value)))
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
      p.imageUrl ||
      p.additionalImageUrl1 ||
      p.additionalImageUrl2 ||
      p.additionalImageUrl3 ||
      p.additionalImageUrl4 ||
      'https://placehold.co/400x400?text=Sem+Imagem'
  }))
})

function handleAddToCart(prod) {
  console.log('Adicionar ao carrinho:', prod)
}
</script>

<template>
  <div class="py-12 bg-black min-h-screen text-white">
    <div class="max-w-7xl mx-auto px-4">
      <ProductListing
        :items="displayed"
        :loading="loading || loadingLegacy"
        :error="error || errorLegacy"
        :page-number="pageNumber"
        :page-size="pageSize"
        :total-pages="totalPages"
        :total-count="totalCount"
        @update:pageNumber="val => (pageNumber = val)"
        @update:pageSize="val => (pageSize = val)"
        @addToCart="handleAddToCart"
      >
        <template #header>
          <h2 class="text-3xl font-bold text-orange-400 mb-6">{{ categoryName }}</h2>
        </template>
      </ProductListing>
    </div>
  </div>
</template>
