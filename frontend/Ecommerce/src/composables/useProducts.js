import { ref } from 'vue';
import { apiGet } from '../services/api';

export function useProducts() {
  const products = ref([]);
  const error = ref(null);
  const loading = ref(false);

  const fetchProductsGridHomePage = async ({
    categoryId = null,
    subCategoryId = null,
    brandId = null,
    limit = null, 
  } = {}) => {
    loading.value = true;
    error.value = null;

    try {
      const params = new URLSearchParams();
      if (categoryId != null) params.append('categoryId', String(categoryId));
      if (subCategoryId != null) params.append('subCategoryId', String(subCategoryId));
      if (brandId != null) params.append('brandId', String(brandId));
      if (limit != null) params.append('limit', String(limit)); 

      const qs = params.toString();
      const data = await apiGet(`/products/productsGridHomePage${qs ? `?${qs}` : ''}`);

      products.value = Array.isArray(data) ? data : [];
      
    } catch (err) {
      console.error('Error fetching products grid:', err);
      error.value = 'Erro ao carregar produtos.';
      products.value = [];
    } finally {
      loading.value = false;
    }
  };

  const fetchProducts = async () => {
    loading.value = true;
    error.value = null;
    try {
      products.value = await apiGet('/products');
    } catch (err) {
      console.error('Error fetching products:', err);
      error.value = 'Erro ao carregar produtos. Tente novamente mais tarde.';
      products.value = [];
    } finally {
      loading.value = false;
    }
  };

  const fetchProductsByCategory = async (category_id, limit = null) => {
    return fetchProductsGridHomePage({ categoryId: category_id, limit });
  };

  const fetchProductsByBrand = async (brand_id, limit = null) => {
    return fetchProductsGridHomePage({ brandId: brand_id, limit });
  };

  return {
    products,
    error,
    loading,
    fetchProducts,
    fetchProductsGridHomePage,
    fetchProductsByCategory,
    fetchProductsByBrand,
  };
}
