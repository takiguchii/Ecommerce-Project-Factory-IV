<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { apiGet } from '@/services/api'
import { useBrands } from '@/composables/useBrands'
import { useProducts } from '@/composables/useProducts'
import ProductListing from '@/components/ProductListing.vue'

const route = useRoute()
const { fetchBrandById } = useBrands()
const { products, loading: loadingLegacy, error: errorLegacy, fetchProductsByBrand } = useProducts()

const brandName = ref('Marca')
const brandLogo = ref(null)
const loading = ref(false)
const error = ref(null)
const pagedItems = ref([])

const pageNumber = ref(1)
const pageSize = ref(16)
const totalPages = ref(1)
const totalCount = ref(0)

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

async function fetchFromFilter() {
  loading.value = true
  error.value = null
  const qs = new URLSearchParams({
    pageNumber: String(pageNumber.value),
    pageSize: String(pageSize.value),
    brandId: String(route.params.id),
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
    await fetchProductsByBrand(route.params.id)
    totalCount.value = products.value.length
    totalPages.value = Math.max(1, Math.ceil(products.value.length / pageSize.value))
  } catch (_) {}
}

async function loadAll() {
  pageNumber.value = 1
  await Promise.all([loadBrand(), fetchFromFilter()])
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
          <div class="flex flex-col items-center gap-3 text-center mb-6">
            <div
              v-if="brandLogo"
              class="w-full max-w-[280px] bg-neutral-900/80 border border-neutral-800 rounded-2xl p-4 flex justify-center items-center"
            >
              <img :src="brandLogo" :alt="brandName" class="max-h-24 w-auto object-contain" />
            </div>
            <h2 class="text-3xl font-bold text-orange-400">{{ brandName }}</h2>
          </div>
        </template>
      </ProductListing>
    </div>
  </div>
</template>
