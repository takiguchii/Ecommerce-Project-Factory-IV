<script setup>
import { onMounted } from 'vue';
import useProducts from '../composables/useProducts.js';
import ProductCardComponent from './ProductCardComponent.vue';

defineProps({
  title: {
    type: String,
    default: 'Produtos'
  }
});

const { products, loading, error, fetchProducts } = useProducts();

onMounted(() => {
  fetchProducts();
});

function handleAddToCart(product) {
  console.log('Adicionado ao carrinho:', product.name);
}
</script>

<template>
  <section class="py-12 px-6 md:px-12 bg-black">
    <div class="flex items-center justify-between mb-8">
      <h2 class="text-2xl md:text-3xl font-semibold text-orange-400">{{ title }}</h2>
      <div v-if="loading" class="text-sm text-orange-200">Carregando...</div>
    </div>

    <div v-if="error" class="mb-6 p-3 rounded-lg bg-red-800 text-red-100 border border-red-600">
      Erro ao carregar produtos: {{ error }}
    </div>
    
    <div v-if="!loading && products.length" class="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-5">
      <ProductCardComponent 
        v-for="product in products" 
        :key="product.id"
        :product="product"
        @add-to-cart="handleAddToCart"
      />
    </div>
  </section>
</template>