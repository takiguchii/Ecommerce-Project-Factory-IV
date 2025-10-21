<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { apiGet } from '@/services/api'
import ProductListing from '@/components/ProductListing.vue'

const route = useRoute()

const results = ref([])
const loading = ref(false)
const error = ref(null)

const pageNumber = ref(1)
const pageSize = ref(16)

const q = computed(() => String(route.query.q ?? '').trim())

const totalCount = computed(() => results.value.length)
const totalPages = computed(() => Math.max(1, Math.ceil(totalCount.value / pageSize.value)))

const displayed = computed(() => {
  const start = (pageNumber.value - 1) * pageSize.value
  return results.value.slice(start, start + pageSize.value)
})

async function fetchSearch() {
  if (!q.value) {
    results.value = []
    return
  }
  loading.value = true
  error.value = null
  try {
    let data = await apiGet(`/products/search-suggestions?query=${encodeURIComponent(q.value)}`)
    // se o backend devolver text/plain
    if (typeof data === 'string') {
      try { data = JSON.parse(data) } catch {}
    }
    const list = Array.isArray(data) ? data : []
    results.value = list.map(p => ({
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
  } catch (e) {
    error.value = e?.message || 'Erro ao buscar produtos'
    results.value = []
  } finally {
    loading.value = false
  }
}

async function load() {
  pageNumber.value = 1
  await fetchSearch()
  window.scrollTo({ top: 0, behavior: 'auto' })
}

onMounted(load)
watch(() => route.query.q, load)

// handlers vindos do ProductListing
function onUpdatePage(n) {
  pageNumber.value = n
  window.scrollTo({ top: 0, behavior: 'auto' })
}
function onUpdatePageSize(n) {
  pageSize.value = Number(n)
  pageNumber.value = 1
  window.scrollTo({ top: 0, behavior: 'auto' })
}
function handleAddToCart(prod) {
  console.log('Adicionar ao carrinho:', prod)
}
</script>

<template>
  <div class="py-12 bg-black min-h-screen text-white">
    <div class="max-w-7xl mx-auto px-4">
      <ProductListing
        :items="displayed"
        :loading="loading"
        :error="error"
        :page-number="pageNumber"
        :page-size="pageSize"
        :total-pages="totalPages"
        :total-count="totalCount"
        @update:pageNumber="onUpdatePage"
        @update:pageSize="onUpdatePageSize"
        @addToCart="handleAddToCart"
      >
        <template #header>
          <h2 class="text-3xl font-bold text-orange-400 mb-6">
            Resultados para: <span class="text-orange-300">"{{ q }}"</span>
          </h2>
        </template>

        <template #empty>
          Nenhum produto encontrado para "{{ q }}".
        </template>
      </ProductListing>
    </div>
  </div>
</template>
