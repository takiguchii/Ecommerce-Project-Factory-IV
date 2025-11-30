import { ref } from 'vue'
import { apiGet } from '@/services/api'

const MAX_BRANDS_HOME = 12

export function useBrands() {
  const brands = ref([])
  const loading = ref(false)
  const error = ref(null)

  function fixImageUrl(u) {
    if (!u) return null
    if (/^https?:\/\//.test(u)) return u
    if (u.startsWith('/_next/image')) return `https://www.kabum.com.br${u}`
    return u
  }

  function normalizeBrand(raw) {
    if (!raw) return null
    return {
      id: raw.id,
      name: raw.name ?? '',
      imageUrl: fixImageUrl(
        raw.brand_image_url ?? raw.brandImageUrl ?? raw.imageUrl ?? null
      ),
      products: Array.isArray(raw.products) ? raw.products : [],
    }
  }

  // Busca marcas aleatórias
  async function fetchBrands(limit = MAX_BRANDS_HOME) {
    loading.value = true
    error.value = null

    try {
      const data = await apiGet('/brands/random', {
        params: { limit },
      })

      brands.value = (Array.isArray(data) ? data : [])
        .map(normalizeBrand)
        .filter(Boolean)
    } catch (err) {
      console.error('Erro ao buscar marcas:', err)
      error.value = err?.message ?? 'Erro ao buscar marcas'
    } finally {
      loading.value = false
    }
  }

  // Busca uma marca específica pelo ID                               
  async function fetchBrandById(id) {
    try {
      const data = await apiGet(`/brands/${id}`)
      return normalizeBrand(data)
    } catch (err) {
      console.error('Erro ao buscar marca por ID:', err)
      return null
    }
  }

  return { brands, loading, error, fetchBrands, fetchBrandById }
}
