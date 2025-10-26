<script setup>
import { ref, onMounted, computed } from 'vue';
import { useProducts } from '../composables/useProducts.js';
import ProductCardComponent from './ProductCardComponent.vue';

const props = defineProps({
  title: { type: String, default: 'Produtos' },
  products: { type: Array, default: null },
  loading: { type: Boolean, default: false },
  error: { type: String, default: null },

  categoryId: { type: [String, Number], default: null },
  subCategoryId: { type: [String, Number], default: null },
  brandId: { type: [String, Number], default: null },
  limit: { type: Number, default: 12 }, 
});

const emit = defineEmits(['fetch-needed']);

const localProducts = useProducts();

onMounted(async () => {
  if (props.products === null) {
    await localProducts.fetchProductsGridHomePage({
      categoryId: props.categoryId,
      subCategoryId: props.subCategoryId,
      brandId: props.brandId,
      limit: props.limit,
    });
  } else {
    emit('fetch-needed');
  }
});

const displayProducts = computed(() => props.products || localProducts.products.value);
const displayLoading = computed(() => props.loading || localProducts.loading.value);
const displayError = computed(() => props.error || localProducts.error.value);

// --- LÓGICA DO SCROLL ---
const scrollContainer = ref(null);
function scrollLeft() { scrollContainer.value?.scrollBy({ left: -300, behavior: 'smooth' }); }
function scrollRight() { scrollContainer.value?.scrollBy({ left: 300, behavior: 'smooth' }); }

function handleAddToCart(product) {
  console.log('Adicionado ao carrinho:', product.name);
}
</script>

<template>
  <section class="py-12 bg-black">
    <div>
      <div class="flex items-center justify-between mb-8">
        <h2 class="text-2xl md:text-3xl font-semibold text-orange-400">{{ title }}</h2>
        <div class="flex items-center gap-2">
          <div v-if="displayLoading" class="text-sm text-orange-200 mr-4">Carregando...</div>
          <button @click="scrollLeft" class="bg-neutral-800 hover:bg-neutral-700 text-white p-2 rounded-full transition-colors disabled:opacity-50" :disabled="displayLoading">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" /></svg>
          </button>
          <button @click="scrollRight" class="bg-neutral-800 hover:bg-neutral-700 text-white p-2 rounded-full transition-colors disabled:opacity-50" :disabled="displayLoading">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" /></svg>
          </button>
        </div>
      </div>

      <div v-if="displayError" class="text-red-400 mb-4">Erro ao carregar produtos.</div>

      <div v-if="!displayLoading && displayProducts.length"
           ref="scrollContainer"
           class="flex gap-6 overflow-x-auto scrollbar-hide pb-4 -mb-4">

        <div v-for="product in displayProducts" :key="product.id" class="w-[250px] flex-shrink-0">
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
.scrollbar-hide::-webkit-scrollbar { display: none; }
.scrollbar-hide { -ms-overflow-style: none; scrollbar-width: none; }
</style>
