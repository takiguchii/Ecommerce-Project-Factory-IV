<template>
  <div class="min-h-screen bg-gray-100">
    <!-- HERO -->
    <section class="relative w-full h-[320px] md:h-[420px] lg:h-[520px] overflow-hidden">
      <div class="absolute inset-0 bg-cover bg-center"
        style="background-image: url('https://sdmntprwestus.oaiusercontent.com/files/00000000-21c0-6230-9222-6bc99992e6b0/raw?se=2025-09-16T02%3A16%3A10Z&sp=r&sv=2024-08-04&sr=b&scid=514e3787-5952-5638-a630-b4070f5b6b47&skoid=ea1de0bc-0467-43d6-873a-9a5cf0a9f835&sktid=a48cca56-e6da-484e-a814-9c849652bcb3&skt=2025-09-16T00%3A12%3A23Z&ske=2025-09-17T00%3A12%3A23Z&sks=b&skv=2024-08-04&sig=kfrvWuNmvd27Hb8Q62qCBdRF%2B6IRWDjfAKsQdgEKiSo%3D');">
      </div>
      <div class="absolute inset-0 bg-gradient-to-r from-black/80 via-black/50 to-transparent"></div>

      <div class="relative h-full px-6 md:px-10 flex flex-col justify-center items-start">
        <h1 class="text-4xl md:text-6xl font-extrabold text-white">TechMart</h1>
        <p class="text-white/90 text-lg md:text-2xl italic mt-2">
          Aqui você encontra aquilo que busca.
        </p>
      </div>
    </section>

    <!-- PRODUTOS -->
    <section class="py-12 px-6 md:px-12">
      <div class="flex items-center justify-between mb-8">
        <h2 class="text-center text-2xl md:text-3xl font-semibold text-orange-500">Nossos Produtos</h2>
        <div v-if="loading" class="text-sm text-gray-500">Carregando...</div>
      </div>

      <div v-if="error" class="mb-6 p-3 rounded-lg bg-red-50 text-red-700 border border-red-200">
        Erro ao carregar produtos: {{ error }}
      </div>

      <div v-if="products.length === 0 && !loading && !error" class="text-gray-600">
        Nenhum produto encontrado.
      </div>

      <div v-else class="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
        <article
          v-for="p in products"
          :key="p.id"
          class="bg-white rounded-2xl shadow hover:shadow-lg transition overflow-hidden relative"
        >
          <img :src="p.image" :alt="p.name" class="w-full h-44 object-cover" />

          <div class="p-4 space-y-2">
            <h3 class="text-base font-semibold text-gray-800 line-clamp-2">{{ p.name }}</h3>
            <p class="text-sm text-gray-600 line-clamp-2">{{ p.description }}</p>

            <div class="flex items-center justify-between mt-2">
              <div class="flex items-baseline gap-2">
                <span class="text-blue-600 font-bold text-lg">
                  {{ formatPrice(p.price) }}
                </span>
                <span
                  v-if="p.discountPrice && p.originalPrice && p.originalPrice > p.discountPrice"
                  class="text-xs text-gray-500 line-through"
                >
                  {{ formatPrice(p.originalPrice) }}
                </span>
              </div>
              <span v-if="p.stock !== null" class="text-xs text-gray-500">
                Estoque: {{ p.stock }}
              </span>
            </div>

            <button
              :disabled="p.stock === 0"
              @click="addToCart(p)"
              class="w-full mt-3 inline-flex items-center justify-center gap-2 rounded-xl px-4 py-2.5 text-sm font-semibold
                     text-white shadow-sm transition
                     disabled:opacity-60 disabled:cursor-not-allowed
                     bg-indigo-600 hover:bg-indigo-700">
              {{ p.stock === 0 ? 'Indisponível' : 'Adicionar ao carrinho' }}
            </button>
          </div>
        </article>
      </div>
    </section>

    <!-- TOAST -->
    <transition name="toast-fade">
      <div
        v-if="toast.visible"
        role="status"
        aria-live="polite"
        class="fixed bottom-6 right-6 z-50"
      >
        <div class="flex items-center gap-3 bg-green-600 text-white px-4 py-3 rounded-xl shadow-lg">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 flex-shrink-0" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-7.25 7.25a1 1 0 01-1.414 0l-3-3a1 1 0 111.414-1.414l2.293 2.293 6.543-6.543a1 1 0 011.414 0z" clip-rule="evenodd"/>
          </svg>
          <span class="font-medium">{{ toast.message }}</span>
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useProducts } from '../composables/useProducts'

const { products, loading, error, fetchProducts } = useProducts()

// toast
const toast = ref({ visible: false, message: '' })
let toastTimer = null

function showToast(message) {
  toast.value.message = message
  toast.value.visible = true
  if (toastTimer) clearTimeout(toastTimer)
  toastTimer = setTimeout(() => (toast.value.visible = false), 1400)
}

function addToCart(product) {
  const cart = JSON.parse(localStorage.getItem('cart') || '[]')
  const idx = cart.findIndex(i => i.id === product.id)
  if (idx >= 0) {
    cart[idx].qty += 1
  } else {
    cart.push({
      id: product.id,
      title: product.name,
      originalPrice: product.price,
      description: product.description,
    })
  }
  localStorage.setItem('cart', JSON.stringify(cart))
  showToast('Adicionado ao carrinho')
}

function formatPrice(n) {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(n || 0)
}

onMounted(fetchProducts)
</script>

<style scoped>
.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
.toast-fade-enter-active,
.toast-fade-leave-active {
  transition: opacity 0.2s ease, transform 0.2s ease;
}
.toast-fade-enter-from,
.toast-fade-leave-to {
  opacity: 0;
  transform: translateY(8px);
}
</style>
