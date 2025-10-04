import { ref } from 'vue';
import { apiGet } from '../services/api';

export function useCategories() {
  const categories = ref([]);
  const loading = ref(false);
  const error = ref(null);

  const fetchCategories = async () => {
    loading.value = true;
    error.value = null;
    try {
      categories.value = await apiGet('/categories');
    } catch (e) {
      error.value = e.message;
    } finally {
      loading.value = false;
    }
  };

  return { categories, loading, error, fetchCategories };
}