<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import ProductCardComponent from '@/components/ProductCardComponent.vue'
import { apiGet } from '@/services/api'

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

function goToPage(p) {
  if (p < 1 || p > totalPages.value || p === pageNumber.value) return
  pageNumber.value = p
}

async function fetchSearch() {
  if (!q.value) {
    results.value = []
    return
  }
  loading.value = true
  error.value = null
  try {
    const data = await apiGet(`/products/search-suggestions?query=${encodeURIComponent(q.value)}`)
    results.value = (Array.isArray(data) ? data : []).map(p => ({
      ...p,
      coverImageUrl:
        p.coverImageUrl ||
        p.imageUrl ||
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

function changePageSize(val) {
  pageSize.value = Number(val)
  pageNumber.value = 1
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
