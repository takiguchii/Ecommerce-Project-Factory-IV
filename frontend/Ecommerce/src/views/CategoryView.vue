<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { apiGet } from '@/services/api' 
import { useCategories } from '@/composables/useCategories'
import ProductCardComponent from '@/components/ProductCardComponent.vue'

const route = useRoute()
const { fetchCategoryById, fetchSubCategoriesByCategoryId, subCategories } = useCategories()

const categoryName = ref('Categoria')
const loading = ref(false)
const error = ref(null)
const allProducts = ref([]) 
const selectedSubCategoryId = ref(null) 
const currentSort = ref('') 

const pageNumber = ref(1)
const pageSize = ref(16)

async function loadData() {
  loading.value = true
  error.value = null
  selectedSubCategoryId.value = null 
  currentSort.value = '' 
  pageNumber.value = 1
  
  const catId = route.params.id

  try {
    const c = await fetchCategoryById(catId)
    categoryName.value = c?.name || 'Departamento'

    await fetchSubCategoriesByCategoryId(catId)
    await fetchProducts(catId)

  } catch (e) {
    console.error(e)
    error.value = 'Erro ao carregar dados.'
  } finally {
    loading.value = false
  }
}

async function fetchProducts(categoryId, subId = null) {
  loading.value = true
  try {
    let url = `/products/category/${categoryId}`
    const params = []

    const activeSubId = subId !== null ? subId : selectedSubCategoryId.value
    if (activeSubId) {
      params.push(`subCategoryId=${activeSubId}`)
    }

    if (currentSort.value) {
      params.push(`sort=${currentSort.value}`)
    }

    if (params.length > 0) {
      url += `?${params.join('&')}`
    }

    const data = await apiGet(url)
    allProducts.value = Array.isArray(data) ? data : []
    pageNumber.value = 1 
  } catch (e) {
    error.value = 'Erro ao buscar produtos.'
    allProducts.value = []
  } finally {
    loading.value = false
  }
}

function toggleSubCategory(subId) {
  if (selectedSubCategoryId.value === subId) {
    selectedSubCategoryId.value = null
  } else {
    selectedSubCategoryId.value = subId
  }
  fetchProducts(route.params.id, selectedSubCategoryId.value)
}

function handleSortChange() {
  fetchProducts(route.params.id)
}

onMounted(loadData)

watch(() => route.params.id, () => {
  loadData()
  window.scrollTo({ top: 0, behavior: 'smooth' })
})

const totalCount = computed(() => allProducts.value.length)
const totalPages = computed(() => Math.ceil(totalCount.value / pageSize.value) || 1)

const displayedProducts = computed(() => {
  const start = (pageNumber.value - 1) * pageSize.value
  const end = start + pageSize.value
  return allProducts.value.slice(start, end).map(p => ({
    ...p,
    coverImageUrl: p.coverImageUrl || p.image_url0 || 'https://placehold.co/400x400?text=Sem+Imagem'
  }))
})

function goToPage(p) {
  if (p >= 1 && p <= totalPages.value) {
    pageNumber.value = p
    window.scrollTo({ top: 0, behavior: 'smooth' })
  }
}

const visiblePages = computed(() => {
  const delta = 2
  const pages = []
  const start = Math.max(1, pageNumber.value - delta)
  const end = Math.min(totalPages.value, pageNumber.value + delta)
  for (let i = start; i <= end; i++) pages.push(i)
  return pages
})
</script>

<template>
  <div class="py-8 bg-black min-h-screen text-white pt-24 pb-10">
    <div class="container mx-auto px-4">
      
      <div class="mb-8 border-b border-neutral-800 pb-4">
        <h2 class="text-3xl font-bold text-orange-500 uppercase tracking-wide">{{ categoryName }}</h2>
        <p class="text-neutral-400 text-sm mt-1" v-if="!loading">
          {{ totalCount }} produtos encontrados
        </p>
      </div>

      <div class="flex flex-col lg:flex-row gap-8">
        
        <aside class="w-full lg:w-1/4">
          <div class="bg-neutral-900/50 rounded-xl p-6 border border-neutral-800 sticky top-24">
            
            <div class="mb-6">
              <h3 class="text-sm font-bold text-white uppercase mb-3 flex items-center gap-2 tracking-wider">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 text-orange-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 4h13M3 8h9m-9 4h6m4 0l4-4m0 0l4 4m-4-4v12" />
                </svg>
                Ordenar
              </h3>
              <select 
                v-model="currentSort" 
                @change="handleSortChange"
                class="w-full bg-neutral-950 text-neutral-300 border border-neutral-800 rounded-lg px-4 py-3 text-sm focus:outline-none focus:border-orange-500 transition-all cursor-pointer hover:border-neutral-600 appearance-none"
              >
                <option value="">Padrão</option>
                <option value="price_asc">Menor Preço</option>
                <option value="price_desc">Maior Preço</option>
              </select>
            </div>

            <div class="h-px bg-neutral-800 w-full mb-6"></div>

            <div>
              <h3 class="text-sm font-bold text-white uppercase mb-3 flex items-center gap-2 tracking-wider">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 text-orange-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 4a1 1 0 011-1h16a1 1 0 011 1v2.586a1 1 0 01-.293.707l-6.414 6.414a1 1 0 00-.293.707V17l-4 4v-6.586a1 1 0 00-.293-.707L3.293 7.293A1 1 0 013 6.586V4z" />
                </svg>
                Filtrar
              </h3>
              
              <div v-if="subCategories.length" class="space-y-2">
                <button 
                  v-for="sub in subCategories" 
                  :key="sub.id"
                  @click="toggleSubCategory(sub.id)"
                  class="w-full text-left px-3 py-2 rounded-lg text-sm transition-all duration-200 flex items-center justify-between group"
                  :class="selectedSubCategoryId === sub.id 
                    ? 'bg-orange-500 text-black font-bold shadow-lg shadow-orange-500/20' 
                    : 'text-neutral-400 hover:bg-neutral-800 hover:text-white'"
                >
                  {{ sub.name }}
                  <span v-if="selectedSubCategoryId === sub.id" class="text-xs bg-black/20 px-2 py-0.5 rounded-full">✓</span>
                </button>
              </div>
              <div v-else class="text-neutral-500 text-sm italic">
                Sem filtros disponíveis.
              </div>
            </div>

          </div>
        </aside>

        <main class="w-full lg:w-3/4">
          
          <div v-if="loading" class="py-20 text-center">
            <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-orange-500 mx-auto mb-4"></div>
            <p class="text-neutral-400">Carregando produtos...</p>
          </div>

          <div v-else-if="error" class="text-red-400 bg-red-900/20 p-4 rounded-lg border border-red-500/30 text-center">
            {{ error }}
          </div>

          <div v-else>
            <div v-if="displayedProducts.length" class="grid grid-cols-2 sm:grid-cols-3 xl:grid-cols-4 gap-4 sm:gap-6">
              <ProductCardComponent
                v-for="product in displayedProducts"
                :key="product.id"
                :product="product"
              />
            </div>
            
            <div v-else class="text-center py-20 bg-neutral-900/30 rounded-xl border border-neutral-800 border-dashed">
              <p class="text-neutral-400 text-lg">Nenhum produto encontrado com este filtro.</p>
              <button @click="selectedSubCategoryId = null; currentSort = ''; fetchProducts(route.params.id)" class="mt-4 text-orange-400 hover:underline text-sm">
                Limpar filtros
              </button>
            </div>

            <div v-if="totalPages > 1" class="mt-10 flex justify-center gap-2">
              <button @click="goToPage(pageNumber - 1)" :disabled="pageNumber === 1" class="px-3 py-1 rounded bg-neutral-800 text-neutral-300 disabled:opacity-50 hover:bg-neutral-700">
                &lt;
              </button>
              <button 
                v-for="p in visiblePages" 
                :key="p" 
                @click="goToPage(p)"
                class="px-3 py-1 rounded text-sm font-medium transition-colors"
                :class="pageNumber === p ? 'bg-orange-500 text-black' : 'bg-neutral-800 text-neutral-300 hover:bg-neutral-700'"
              >
                {{ p }}
              </button>
              <button @click="goToPage(pageNumber + 1)" :disabled="pageNumber === totalPages" class="px-3 py-1 rounded bg-neutral-800 text-neutral-300 disabled:opacity-50 hover:bg-neutral-700">
                &gt;
              </button>
            </div>
          </div>
        </main>
      </div>
    </div>
  </div>
</template>