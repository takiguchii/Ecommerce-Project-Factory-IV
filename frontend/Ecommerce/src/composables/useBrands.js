import { ref } from 'vue'
import { apiGet } from '@/services/api.js'

export function useBrands() {
  const brands = ref([])
  const loading = ref(false)
  const error = ref(null)

  async function fetchBrands() {
    loading.value = true
    error.value = null
    try {
      const data = await apiGet('/brands')
      brands.value = Array.isArray(data) ? data : []
    } catch (err) {
      error.value = err
    } finally {
      loading.value = false
    }
  }

  return { brands, loading, error, fetchBrands }
}
