<template>
  <div class=" max-w-5xl mx-auto p-6 flex flex-col md:flex-row gap-8">
    <div class="flex-1 flex flex-col items-center">
      <img :src="product?.imageUrl" :alt="product?.name" class="w-96 h-96 object-contain bg-white rounded-lg shadow mb-4" />
      <!-- Galeria de imagens pode ser adicionada aqui futuramente -->
    </div>
    <div class="flex-1">
      <h1 class="text-3xl font-bold text-orange-400 mb-2">{{ product?.name }}</h1>
      <div class="mb-4">
        <span v-if="product?.discountPrice" class="text-orange-200/60 line-through mr-2">
          {{ formatPrice(product?.originalPrice) }}
        </span>
        <span class="text-2xl font-bold text-orange-500">
          {{ formatPrice(product?.discountPrice || product?.originalPrice) }}
        </span>
      </div>
      <div class="mb-4">
        <span class="bg-green-700 text-white px-2 py-1 rounded text-xs">Em estoque</span>
      </div>
      <button class="bg-orange-500 hover:bg-orange-600 text-black font-bold py-2 px-4 rounded transition-colors mb-4 w-full">
        Adicionar ao Carrinho
      </button>
      <div class="mb-4">
        <h2 class="text-lg font-semibold text-neutral-200 mb-1">Descrição</h2>
        <p class="text-neutral-200">{{ product?.description }}</p>
      </div>
      <div v-if="product?.technicalInfo" class="mb-4">
        <h2 class="text-lg font-semibold text-neutral-200 mb-1">Informações Técnicas</h2>
        <p class="text-neutral-200 whitespace-pre-line">{{ product?.technicalInfo }}</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { getProductById } from '../services/api';

const route = useRoute();
const product = ref(null);

const formatPrice = (price) => {
  if (price === null || price === undefined) return '';
  return price.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
};

onMounted(async () => {
  const id = route.params.id;
  product.value = await getProductById(id);
});
</script>