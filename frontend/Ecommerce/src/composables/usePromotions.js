import { ref } from 'vue';
import { apiGet } from '../services/api';

export function usePromotions() {
  const promotions = ref([]);
  const loading = ref(false);
  const error = ref(null);

  const fetchPromotions = async () => {
    loading.value = true;
    error.value = null;
    try {
      promotions.value = await apiGet('/products/promotions');
    } catch (e) {
      error.value = e.message;
    } finally {
      loading.value = false;
    }
  };

  return { promotions, loading, error, fetchPromotions };
}