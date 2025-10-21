import { ref } from 'vue';
import { apiGet } from '../services/api';

export function useProducts() {
  const products = ref([]);
  const error = ref(null);
  const loading = ref(false);

  // Todos os produtos
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

  // Produtos por categoria
  const fetchProductsByCategory = async (categoryId) => {
    loading.value = true;
    error.value = null;
    try {
      const allProducts = await apiGet('/products');
      products.value = (Array.isArray(allProducts) ? allProducts : [])
        .filter(p => String(p.categoryId) === String(categoryId));
    } catch (err) {
      console.error('Error fetching products by category:', err);
      error.value = 'Erro ao carregar produtos da categoria.';
    } finally {
      loading.value = false;
    }
  };

  // Produtos por marca
  const fetchProductsByBrand = async (brandId) => {
    loading.value = true;
    error.value = null;
    try {
      const allProducts = await apiGet('/products');
      products.value = (Array.isArray(allProducts) ? allProducts : [])
        .filter(p => String(p.brandId) === String(brandId));
      console.log(`Produtos filtrados por marca ${brandId}:`, products.value);
    } catch (err) {
      console.error('Error fetching products by brand:', err);
      error.value = 'Erro ao carregar produtos da marca.';
    } finally {
      loading.value = false;
    }
  };

  return {
    products,
    error,
    loading,
    fetchProducts,
    fetchProductsByCategory,
    fetchProductsByBrand,
  };
}
