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

  const fetchProductsByCategory = async (categoryId) => {
    loading.value = true;
    error.value = null;

    try {
      const allProducts = await apiGet('/products');
      console.log('Todos os produtos:', allProducts);
      console.log('ID da categoria:', categoryId);
      products.value = allProducts.filter(p => String(p.categoryId) === String(categoryId));
      console.log('Produtos filtrados:', products.value);
    } catch (err) {
      console.error('Error fetching products by category:', err);
      error.value = 'Erro ao carregar produtos da categoria.';
    } finally {
      loading.value = false;
    }
  };

  return {
    products,
    error,
    loading,
    fetchProducts,
    fetchProductsByCategory
  };
}