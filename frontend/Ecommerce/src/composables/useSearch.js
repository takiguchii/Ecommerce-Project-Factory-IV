import { ref } from 'vue'
import { apiGet } from '@/services/api'

export function useSearch() {
  const results = ref([])
  const loading = ref(false)
  const error = ref(null)

  async function fetchSuggestions(query) {
    if (!query || !query.trim()) {
      results.value = []
      return
    }
    loading.value = true
    error.value = null
    try {
      const data = await apiGet(`/products/search-suggestions?query=${encodeURIComponent(query)}`)
      results.value = (Array.isArray(data) ? data : []).map(p => ({
        ...p,
        coverImageUrl:
          p.coverImageUrl ||
          p.image_url0 ||
          'https://placehold.co/400x400?text=Sem+Imagem'
      }))
    } catch (e) {
      error.value = e?.message || 'Erro ao buscar produtos'
      results.value = []
    } finally {
      loading.value = false
    }
  }

  return { results, loading, error, fetchSuggestions }
}
