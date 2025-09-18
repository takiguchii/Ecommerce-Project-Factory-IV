import { ref } from 'vue'
import { apiGet } from '../services/api'

export function useProducts() {
  const products = ref([])
  const loading = ref(false)
  const error = ref(null)

  async function fetchProducts() {
    loading.value = true
    error.value = null
    try {
      // chama GET http://localhost:5295/api/products
      const data = await apiGet('/products')

      // espera [{ name, originalPrice, discountPrice, description, ... }]
      products.value = (Array.isArray(data) ? data : []).map((it, idx) => {
        const price = it.discountPrice && it.discountPrice > 0
          ? it.discountPrice
          : it.originalPrice

        return {
          id: it.id ?? idx + 1,
          name: it.name ?? 'Produto',
          description: it.description ?? '',
          originalPrice: it.originalPrice ?? 0,
          discountPrice: it.discountPrice ?? 0,
        }
      })
    } catch (e) {
      error.value = e.message || String(e)
    } finally {
      loading.value = false
    }
  }

  return { products, loading, error, fetchProducts }
}
