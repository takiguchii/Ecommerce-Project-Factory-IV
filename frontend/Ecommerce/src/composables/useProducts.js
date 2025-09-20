import { ref } from 'vue'
import { apiGet, apiPost } from '../services/api'

export function useProducts() {
  const products = ref([])
  const loading = ref(false)
  const error = ref(null)

  async function fetchProducts() {
    loading.value = true
    error.value = null
    try {
      const data = await apiGet('/products')
      products.value = (Array.isArray(data) ? data : []).map((it, idx) => ({
        id: it.id ?? idx + 1,
        name: it.name ?? 'Produto',
        description: it.description ?? '',
        originalPrice: it.originalPrice ?? 0,
        discountPrice: it.discountPrice ?? 0,
      }))
    } catch (e) {
      error.value = e.message || String(e)
    } finally {
      loading.value = false
    }
  }

  async function registerProduct(product) {
    loading.value = true
    error.value = null
    try {
      const response = await apiPost('/products', product)
      return response
    } catch (e) {
      error.value = e.message || String(e)
    } finally {
      loading.value = false
    }
  }

  return { products, loading, error, fetchProducts, registerProduct }
}