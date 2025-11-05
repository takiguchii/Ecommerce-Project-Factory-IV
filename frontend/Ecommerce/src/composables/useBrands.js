import { ref } from 'vue'
import { apiGet } from '@/services/api'

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

  async function fetchBrands() {
    loading.value = true
    error.value = null
    try {
      const data = await apiGet('/brands')
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

  async function fetchBrandById(id) {
    if (!brands.value.length) await fetchBrands()
    return brands.value.find(b => String(b.id) === String(id)) ?? null
  }

  return { brands, loading, error, fetchBrands, fetchBrandById }
}
