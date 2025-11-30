<script setup>
import { ref, onMounted, computed } from 'vue'
import { useProducts } from '../composables/useProducts.js'
import ProductCardComponent from './ProductCardComponent.vue'

const CELLPHONES_CATEGORY_ID = 4

const props = defineProps({
  title: { type: String, default: 'Celulares e Smartphones' },
  limit: { type: Number, default: 12 },
})

const localProducts = useProducts()

const displayProducts = computed(() => localProducts.products.value)
const displayLoading = computed(() => localProducts.loading.value)
const displayError = computed(() => localProducts.error.value)

onMounted(async () => {
  await localProducts.fetchProductsGridHomePage({
    categoryId: CELLPHONES_CATEGORY_ID,
    limit: props.limit,
  })
})

function handleAddToCart(product) {
  console.log('Adicionado ao carrinho (Celular):', product.name)
}

const scrollContainer = ref(null)

function scrollLeft() {
  if (scrollContainer.value) {
    scrollContainer.value.scrollBy({ left: -300, behavior: 'smooth' })
  }
}

function scrollRight() {
  if (scrollContainer.value) {
    scrollContainer.value.scrollBy({ left: 300, behavior: 'smooth' })
  }
}
</script>

<template>
  <section class="py-4">
    <div>
      <div class="flex items-center justify-between mb-6">
        <h2
          class="text-xl md:text-2xl font-bold text-orange-500 uppercase tracking-wide border-b-2 border-orange-500/20 pb-1"
        >
          {{ title }}
        </h2>

        <div class="flex items-center gap-2">
          <div
            v-if="displayLoading"
            class="text-sm text-orange-400 animate-pulse mr-2 hidden sm:block"
          >
            Carregando...
          </div>

          <button
            @click="scrollLeft"
            class="bg-neutral-800 hover:bg-orange-500 hover:text-white text-neutral-400 p-2 rounded-full transition-all duration-300 shadow-md border border-neutral-700 hover:border-orange-400 group disabled:opacity-50"
            :disabled="displayLoading"
            aria-label="Anterior"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="h-5 w-5 group-hover:-translate-x-0.5 transition-transform"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M15 19l-7-7 7-7"
              />
            </svg>
          </button>

          <button
            @click="scrollRight"
            class="bg-neutral-800 hover:bg-orange-500 hover:text-white text-neutral-400 p-2 rounded-full transition-all duration-300 shadow-md border border-neutral-700 hover:border-orange-400 group disabled:opacity-50"
            :disabled="displayLoading"
            aria-label="Próximo"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="h-5 w-5 group-hover:translate-x-0.5 transition-transform"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M9 5l7 7-7 7"
              />
            </svg>
          </button>
        </div>
      </div>

      <div
        v-if="displayError"
        class="text-red-400 bg-red-900/20 p-4 rounded-lg text-center text-sm mb-4 border border-red-500/30"
      >
        Ops! Não conseguimos carregar estas ofertas agora.
      </div>

      <div
        v-if="!displayLoading && displayProducts.length"
        ref="scrollContainer"
        class="flex gap-4 overflow-x-auto scrollbar-hide pb-4 -mb-4 scroll-smooth snap-x snap-mandatory"
      >
        <div
          v-for="product in displayProducts"
          :key="product.id"
          class="flex-shrink-0 w-[240px] sm:w-[260px] snap-start"
        >
          <ProductCardComponent
            :product="product"
            class="h-full w-full"
            @add-to-cart="handleAddToCart"
          />
        </div>
      </div>

      <div
        v-if="!displayLoading && !displayError && displayProducts.length === 0"
        class="text-neutral-500 text-center py-12"
      >
        Nenhum produto encontrado nesta seção.
      </div>
    </div>
  </section>
</template>

<style scoped>
.scrollbar-hide::-webkit-scrollbar {
  display: none;
}
.scrollbar-hide {
  -ms-overflow-style: none;
  scrollbar-width: none;
}
</style>
