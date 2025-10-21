<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { useProducts } from '../composables/useProducts.js'
import ProductCardComponent from './ProductCardComponent.vue'

const props = defineProps({
  title: { type: String, default: 'Produtos' },
  products: { type: Array, default: null },
  loading: { type: Boolean, default: false },
  error: { type: String, default: null },
})
const emit = defineEmits(['fetch-needed'])

const localProducts = useProducts()
onMounted(() => {
  if (props.products === null) {
    localProducts.fetchProducts()
  } else {
    emit('fetch-needed')
  }
})

const displayProducts = computed(() => props.products || localProducts.products.value)
const displayLoading  = computed(() => props.loading  || localProducts.loading.value)
const displayError    = computed(() => props.error    || localProducts.error.value)

// pool aleatório (máx 15)
const MAX_POOL = 15
const WINDOW = 5
const pool = ref([])
function shuffleOnce(list) {
  const arr = list ? [...list] : []
  for (let i = arr.length - 1; i > 0; i--) {
    const j = Math.floor(Math.random() * (i + 1))
    ;[arr[i], arr[j]] = [arr[j], arr[i]]
  }
  return arr.slice(0, MAX_POOL)
}
watch(
  displayProducts,
  (list) => { pool.value = Array.isArray(list) ? shuffleOnce(list) : [] },
  { immediate: true }
)

// janela deslizante (5 visíveis)
const start = ref(0)
const visibleProducts = computed(() => {
  const n = Math.min(WINDOW, pool.value.length)
  return Array.from({ length: n }, (_, i) => pool.value[(start.value + i) % pool.value.length])
})
function next() {
  if (pool.value.length > 1) start.value = (start.value + 1) % pool.value.length
}
function prev() {
  if (pool.value.length > 1) start.value = (start.value - 1 + pool.value.length) % pool.value.length
}

function handleAddToCart(product) {
  console.log('Adicionado ao carrinho:', product?.name)
}
</script>

<template>
  <section class="py-12 bg-black">
    <div>
      <div class="flex items-center justify-between mb-8">
        <h2 class="text-2xl md:text-3xl font-semibold text-orange-400">{{ title }}</h2>

        <div class="flex items-center gap-2">
          <div v-if="displayLoading" class="text-sm text-orange-200 mr-2">Carregando...</div>
          <button
            @click="prev"
            class="bg-neutral-800 hover:bg-neutral-700 text-white p-2 rounded-full transition-colors disabled:opacity-50"
            :disabled="displayLoading || pool.length <= 1"
            aria-label="Anterior"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7"/></svg>
          </button>
          <button
            @click="next"
            class="bg-neutral-800 hover:bg-neutral-700 text-white p-2 rounded-full transition-colors disabled:opacity-50"
            :disabled="displayLoading || pool.length <= 1"
            aria-label="Próximo"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7"/></svg>
          </button>
        </div>
      </div>

      <div v-if="displayError" class="text-red-400 text-sm mb-4">
        {{ displayError }}
      </div>

      <!-- sempre 5 slots (ou menos se não houver) -->
      <div v-if="!displayLoading && visibleProducts.length" class="grid grid-cols-5 gap-6">
        <div
          v-for="product in visibleProducts"
          :key="product.id"
          class="flex-shrink-0"
        >
          <ProductCardComponent
            :product="product"
            @add-to-cart="handleAddToCart"
          />
        </div>
      </div>

      <div v-else-if="!displayLoading" class="text-orange-200">Nenhum produto encontrado.</div>
    </div>
  </section>
</template>

<style scoped>
/* mantendo se precisar em algum lugar com overflow */
.scrollbar-hide::-webkit-scrollbar { display: none; }
.scrollbar-hide { -ms-overflow-style: none; scrollbar-width: none; }
</style>
