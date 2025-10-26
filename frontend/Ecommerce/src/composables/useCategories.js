import { ref } from 'vue'
import { apiGet } from '@/services/api'

export function useCategories() {
  const categories = ref([])
  const loading = ref(false)
  const error = ref(null)

  function normalizeCategory(raw) {
    if (!raw) return null
    return {
      id: raw.id,
      name: raw.name ?? '',
      products: Array.isArray(raw.products) ? raw.products : [], 
    }
  }

  async function fetchCategories() {
    loading.value = true
    error.value = null
    try {
      const data = await apiGet('/categories')
      categories.value = (Array.isArray(data) ? data : [])
        .map(normalizeCategory)
        .filter(Boolean)
    } catch (e) {
      console.error('Erro ao buscar categorias:', e)
      error.value = e?.message ?? 'Erro ao buscar categorias'
    } finally {
      loading.value = false
    }
  }

  async function fetchCategoryById(id) {
    if (!categories.value.length) await fetchCategories()
    return categories.value.find(c => String(c.id) === String(id)) ?? null
  }

  return { categories, loading, error, fetchCategories, fetchCategoryById }
}
