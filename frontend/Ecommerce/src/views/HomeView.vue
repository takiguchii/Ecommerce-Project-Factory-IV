<!-- src/views/HomeView.vue -->
<template>
  <div class="min-h-screen bg-gray-100">
    <!-- HERO (mantido) -->
    <section class="relative w-full h-[320px] md:h-[420px] lg:h-[520px] overflow-hidden">
      <div class="absolute inset-0 bg-cover bg-center"
        style="background-image: url('https://img.freepik.com/fotos-gratis/luminaria-de-neon-para-vendas-de-sexta-feira-negra_23-2151833076.jpg?semt=ais_hybrid&w=1280');">
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
      <h2 class="text-2xl md:text-3xl font-bold text-gray-800 text-center mb-8">Nossos Produtos</h2>

      <div class="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
        <article v-for="p in products" :key="p.id"
                 class="bg-white rounded-2xl shadow hover:shadow-lg transition overflow-hidden relative">
          <img :src="p.image" :alt="p.title" class="w-full h-44 object-cover" />

          <div class="p-4 space-y-2">
            <h3 class="text-base font-semibold text-gray-800 line-clamp-2">{{ p.title }}</h3>
            <p class="text-sm text-gray-600 line-clamp-2">{{ p.subtitle }}</p>

            <div class="flex items-center justify-between mt-2">
              <span class="text-blue-600 font-bold text-lg">{{ formatPrice(p.price) }}</span>
              <span class="text-xs text-gray-500">Estoque: {{ p.stock }}</span>
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

    <!-- TOAST (popup verde) -->
    <transition name="toast-fade">
      <div
        v-if="toast.visible"
        role="status"
        aria-live="polite"
        class="fixed bottom-6 right-6 z-50"
      >
        <div class="flex items-center gap-3 bg-green-600 text-white px-4 py-3 rounded-xl shadow-lg">
          <!-- ícone check -->
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
import { ref } from 'vue'

const products = ref([
  { id: 1, title: 'Fone Razer', subtitle: 'Cancelamento de ruído e 30h de bateria', price: 199.9, stock: 12, image: 'https://m.media-amazon.com/images/I/51TzGmowjSL._AC_SL1500_.jpg'},
  { id: 2, title: 'Notebook Gamer', subtitle: 'RTX, tela 144Hz e 16GB RAM', price: 4999.0, stock: 5, image: 'https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fimages2.kabum.com.br%2Fprodutos%2Ffotos%2Fsync_mirakl%2F872622%2Fxlarge%2FNotebook-Gamer-Asus-Rog-Strix-Scar-18-Intel-Core-Ultra-9-275hx-64GB-RAM-Nvidia-RTX5090-SSD-4TB-Tela-18-240Hz-Windows-11-Home-Black-G835lx_1755889506.jpg&w=640&q=75' },
  { id: 3, title: 'Smartwatch Fit', subtitle: 'Monitor de saúde, GPS e notificações', price: 499.9, stock: 0, image: 'https://m.media-amazon.com/images/I/51-XHYBPO1L.__AC_SX300_SY300_QL70_ML2_.jpg' },
  { id: 4, title: 'Teclado Mecânico RGB', subtitle: 'Switches táteis e construção em alumínio', price: 349.9, stock: 20, image: 'https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fimages8.kabum.com.br%2Fprodutos%2Ffotos%2F627458%2Fteclado-mecanico-gamer-machenike-k500-b61-rgb-switch-brown-layout-61-teclas-abnt2-cinza-k500-b61_1746534964_gg.jpg&w=640&q=75' }
])

// estado do toast
const toast = ref({ visible: false, message: '' })
let toastTimer = null

function showToast(message) {
  toast.value.message = message
  toast.value.visible = true
  if (toastTimer) clearTimeout(toastTimer)
  toastTimer = setTimeout(() => (toast.value.visible = false), 1400)
}

function addToCart(product) {
  if (product.stock === 0) return
  const cart = JSON.parse(localStorage.getItem('cart') || '[]')
  const idx = cart.findIndex(i => i.id === product.id)
  if (idx >= 0) {
    cart[idx].qty += 1
  } else {
    // salva apenas campos necessários
    cart.push({ id: product.id, title: product.title, price: product.price, image: product.image, qty: 1 })
  }
  localStorage.setItem('cart', JSON.stringify(cart))
  showToast('Adicionado ao carrinho')
}

function formatPrice(n) {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(n)
}
</script>

<style scoped>
.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

/* animação do toast */
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
