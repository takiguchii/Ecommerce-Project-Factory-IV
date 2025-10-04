<template>
  <div class="py-12 bg-black min-h-screen">
    <h2 class="text-3xl font-bold text-orange-400 mb-8">Produtos da Categoria</h2>
    <ProductSectionComponent
      :title="categoryName"
      :products="products"
      :loading="loading"
      :error="error"
    />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import ProductSectionComponent from '../components/ProductSectionComponent.vue';
import { useProducts } from '../composables/useProducts.js';
import { useCategories } from '../composables/useCategories.js';

const route = useRoute();
const { products, loading, error, fetchProductsByCategory } = useProducts();
const categoryName = ref('Categoria');

onMounted(async () => {
  await fetchProductsByCategory(route.params.id);

  // Buscar nome da categoria (opcional)
  const { fetchCategoryById } = useCategories();
  const category = await fetchCategoryById(route.params.id);
  categoryName.value = category?.name || 'Categoria';
});
</script>