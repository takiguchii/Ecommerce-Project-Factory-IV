<script setup>
import { ref, onMounted } from 'vue';
import { useProducts } from '../composables/useProducts.js';
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

const scrollContainer = ref(null);

function scrollLeft() {
  if (scrollContainer.value) {
    scrollContainer.value.scrollBy({ left: -300, behavior: 'smooth' });
  }
}

function scrollRight() {
  if (scrollContainer.value) {
    scrollContainer.value.scrollBy({ left: 300, behavior: 'smooth' });
  }
}

function handleAddToCart(product) {
  console.log('Adicionado ao carrinho:', product.name);
}
</script>

<template>
  <section class="py-12 bg-black">
    <div class="container mx-auto px-4">
      
      <div class="flex items-center justify-between mb-8">
        <h2 class="text-2xl md:text-3xl font-semibold text-orange-400">{{ title }}</h2>
        
        <div class="flex items-center gap-2">
          <div v-if="loading" class="text-sm text-orange-200 mr-4">Carregando...</div>
          <button @click="scrollLeft" class="bg-neutral-800 hover:bg-neutral-700 text-white p-2 rounded-full transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" /></svg>
          </button>
          <button @click="scrollRight" class="bg-neutral-800 hover:bg-neutral-700 text-white p-2 rounded-full transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" /></svg>
          </button>
        </div>
      </div>

      <div v-if="error" class="mb-6 p-3 rounded-lg bg-red-800 text-red-100 border border-red-600">
        Erro ao carregar produtos: {{ error }}
      </div>
      
      <div v-if="!loading && products.length" 
           ref="scrollContainer"
           class="flex gap-6 overflow-x-auto scrollbar-hide pb-4 -mb-4">
        
        <div v-for="product in products" :key="product.id" class="w-[250px] flex-shrink-0">
          <ProductCardComponent 
            :product="product"
            @add-to-cart="handleAddToCart"
          />
        </div>

      </div>
    </div>
  </section>
</template>

<style scoped>
.scrollbar-hide::-webkit-scrollbar {
  display: none;
}
.scrollbar-hide {
  -ms-overflow-style: none;  /* IE and Edge */
  scrollbar-width: none;  /* Firefox */
}
</style>