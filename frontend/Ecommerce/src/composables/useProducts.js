import { ref } from 'vue';
import { apiGet } from '../services/api';

export function useProducts() {
  const products = ref([]);
  const error = ref(null);
  const loading = ref(false);

  const fetchProducts = async () => {
    loading.value = true;
    error.value = null;
    
    try {
      products.value = await apiGet('/products');
      console.log('Products loaded:', products.value);
    } catch (err) {
      console.error('Error fetching products:', err);
      error.value = 'Erro ao carregar produtos. Tente novamente mais tarde.';
    } finally {
      loading.value = false;
    }
  };

  return {
    products,
    error,
    loading,
    fetchProducts
  };
}