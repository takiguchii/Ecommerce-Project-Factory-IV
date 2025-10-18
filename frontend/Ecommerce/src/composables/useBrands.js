import { ref } from 'vue'
import { apiGet } from '@/services/api.js'

export function useBrands() {
  const brands = ref([])
  const loading = ref(false)
  const error = ref(null)

  function normalizeBrand(raw) {
    if (!raw) return null
    return {
      id: raw.id ?? raw.ID ?? null,
      name: raw.name ?? raw.Nome ?? '',
      imageUrl: raw.imageUrl ?? raw.imageurl ?? raw.logoUrl ?? raw.logoURL ?? raw.image ?? null,
    }
  }

  async function fetchBrands() {
    loading.value = true
    error.value = null
    try {
      const data = await apiGet('/brands')
      brands.value = (Array.isArray(data) ? data : []).map(normalizeBrand)
    } catch (err) {
      console.error('Erro ao buscar marcas:', err)
      error.value = err
    } finally {
      loading.value = false
    }
  }

  async function fetchBrandById(id) {
    if (!brands.value.length) await fetchBrands()
    return brands.value.find(b => String(b.id) === String(id)) || null
  }

  return { brands, loading, error, fetchBrands, fetchBrandById }
}
