<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { apiGet } from '@/services/api'
import { useCategories } from '@/composables/useCategories'
import ProductCardComponent from '@/components/ProductCardComponent.vue'

const route = useRoute()
// Importamos a nova função e o estado subCategories
const { fetchCategoryById, fetchSubCategoriesByCategoryId, subCategories } = useCategories()

const categoryName = ref('Categoria')
const loading = ref(false)
const error = ref(null)
const allProducts = ref([]) // Armazena todos os produtos buscados
const selectedSubCategoryId = ref(null) // Controla o filtro ativo

// Paginação local
const pageNumber = ref(1)
const pageSize = ref(16)

/* --- 1. Carregar Dados Iniciais --- */
async function loadData() {
  loading.value = true
  error.value = null
  selectedSubCategoryId.value = null // Reseta filtro ao mudar de categoria
  pageNumber.value = 1
  
  const catId = route.params.id

  try {
    // 1. Busca nome da categoria
    const c = await fetchCategoryById(catId)
    categoryName.value = c?.name || 'Departamento'

    // 2. Busca as subcategorias para o menu lateral
    await fetchSubCategoriesByCategoryId(catId)

    // 3. Busca os produtos iniciais (sem filtro)
    await fetchProducts(catId)

  } catch (e) {
    console.error(e)
    error.value = 'Erro ao carregar dados.'
  } finally {
    loading.value = false
  }
}

/* --- 2. Buscar Produtos (Com ou Sem Filtro) --- */
async function fetchProducts(categoryId, subId = null) {
  loading.value = true
  try {
    // Monta a URL com o filtro opcional
    let url = `/products/category/${categoryId}`
    if (subId) {
      url += `?subCategoryId=${subId}`
    }

    const data = await apiGet(url)
    allProducts.value = Array.isArray(data) ? data : []
    pageNumber.value = 1 // Volta para página 1 ao filtrar
  } catch (e) {
    error.value = 'Erro ao buscar produtos.'
    allProducts.value = []
  } finally {
    loading.value = false
  }
}

/* --- 3. Ação do Filtro --- */
function toggleSubCategory(subId) {
  // Se clicar no mesmo que já está ativo, remove o filtro (toggle)
  if (selectedSubCategoryId.value === subId) {
    selectedSubCategoryId.value = null
  } else {
    selectedSubCategoryId.value = subId
  }
  // Recarrega os produtos com o novo estado do filtro
  fetchProducts(route.params.id, selectedSubCategoryId.value)
}

/* --- 4. Hooks e Watchers --- */
onMounted(loadData)

watch(() => route.params.id, () => {
  loadData()
  window.scrollTo({ top: 0, behavior: 'smooth' })
})

// Paginação computada (Client-side)
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
  <div class="py-8 bg-black min-h-screen text-white">
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
            <h3 class="text-lg font-bold text-white mb-4 flex items-center gap-2">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-orange-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 4a1 1 0 011-1h16a1 1 0 011 1v2.586a1 1 0 01-.293.707l-6.414 6.414a1 1 0 00-.293.707V17l-4 4v-6.586a1 1 0 00-.293-.707L3.293 7.293A1 1 0 013 6.586V4z" />
              </svg>
              Filtrar por
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
              <button @click="selectedSubCategoryId = null; fetchProducts(route.params.id)" class="mt-4 text-orange-400 hover:underline text-sm">
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