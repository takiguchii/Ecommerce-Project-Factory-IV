<template>
  <section class="min-h-[60vh] bg-neutral-950 text-neutral-100">
    <div class="max-w-6xl mx-auto p-4 md:p-8">
      <h1 class="text-2xl md:text-3xl font-bold text-orange-400 mb-6">Meu Carrinho</h1>

      <!-- vazio -->
      <div
        v-if="!cart.items || cart.items.length === 0"
        class="bg-neutral-900 border border-neutral-800 rounded-xl p-10 text-center"
      >
        <p class="text-neutral-300 mb-6">Seu carrinho está vazio.</p>
        <router-link
          to="/"
          class="inline-flex items-center justify-center rounded-lg px-5 py-2 font-semibold bg-orange-500 hover:bg-orange-600 text-black transition-colors"
        >
          Voltar para a loja
        </router-link>
      </div>

      <!-- conteúdo -->
      <div v-else class="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <!-- itens -->
        <div class="lg:col-span-2 space-y-4">
          <div
            v-for="item in cart.items"
            :key="item.productId"
            class="bg-neutral-900 border border-neutral-800 rounded-xl p-4 md:p-5 flex gap-4 md:gap-5 items-start"
          >
            <!-- imagem -->
            <div
              class="w-24 h-24 md:w-28 md:h-28 bg-white rounded-lg flex items-center justify-center overflow-hidden shrink-0"
            >
              <img :src="getImageUrl(item)" :alt="item.name" class="max-w-full max-h-full object-contain" />
            </div>

            <!-- detalhes -->
            <div class="flex-1 min-w-0">
              <h3 class="font-semibold text-orange-300 truncate">{{ item.name }}</h3>
              <p class="text-sm text-neutral-400 mt-1">ID: {{ item.productId }}</p>

              <div class="mt-2 flex flex-wrap items-center gap-4">
                <p class="text-lg font-bold text-orange-400">R$ {{ toFixed2(item.price) }}</p>

                <div class="flex items-center gap-2">
                  <label class="text-sm text-neutral-400">Qtd:</label>
                  <input
                    type="number"
                    :value="item.quantity"
                    min="0"
                    @change="handleQuantityChange(item.productId, $event.target.value)"
                    class="w-16 rounded-md bg-neutral-800 border border-neutral-700 px-2 py-1 text-neutral-100 focus:outline-none focus:ring-2 focus:ring-orange-500"
                  />
                </div>

                <p class="text-sm md:text-base text-neutral-300">
                  Subtotal: <span class="font-semibold text-orange-300">R$ {{ toFixed2(itemSubtotal(item)) }}</span>
                </p>
              </div>

              <button
                @click="removeFromCart(item.productId)"
                class="mt-3 text-sm text-red-400 hover:text-red-300 transition-colors"
              >
                Remover
              </button>
            </div>
          </div>
        </div>

        <!-- resumo -->
        <aside class="bg-neutral-900 border border-neutral-800 rounded-xl p-5 h-fit sticky top-6">
          <h2 class="text-lg font-semibold text-neutral-200 mb-4">Resumo do Pedido</h2>

          <div class="flex justify-between text-neutral-300 mb-2">
            <span>Itens</span>
            <span>{{ (cart.items || []).reduce((s,i)=>s + Number(i.quantity||0), 0) }}</span>
          </div>

          <div class="flex justify-between text-neutral-300 mb-4">
            <span>Total</span>
            <span class="font-bold text-orange-300">R$ {{ toFixed2(cartTotal) }}</span>
          </div>

          <button
            class="w-full rounded-lg bg-orange-500 hover:bg-orange-600 text-black font-semibold py-2 transition-colors"
          >
            Finalizar Compra
          </button>
        </aside>
      </div>
    </div>
  </section>
</template>


<script setup>
import { onMounted, computed } from 'vue'
import { useCart } from '@/composables/useCart'

const { cart, fetchCart, removeFromCart, updateItemQuantity } = useCart()
const mainUrl = 'https://www.kabum.com.br/'

onMounted(() => {
  fetchCart()
})

/* ---------- helpers numéricos ---------- */
function toNum(v) {
  if (typeof v === 'number') return v
  if (!v) return 0
  const s = String(v).replace(/\s/g, '').replace('R$', '')
  const norm = s.replace(/\./g, '').replace(',', '.')
  const n = parseFloat(norm)
  return Number.isFinite(n) ? n : 0
}
function toFixed2(v) {
  return Number(toNum(v)).toFixed(2)
}

/* ---------- imagens (padrão Kabum) ---------- */
function getImageUrl(item) {
  // Se já veio completo da API
  if (item.imageUrl && /^https?:\/\//i.test(item.imageUrl)) return item.imageUrl

  // Se veio caminho relativo ou campos image_urlN
  const path =
    item.imageUrl ||
    item.image_url0 ||
    item.image_url1 ||
    item.image_url2 ||
    item.image_url3 ||
    item.image_url4 ||
    ''

  return /^https?:\/\//i.test(path) ? path : mainUrl + path
}

/* ---------- totais ---------- */
function itemSubtotal(item) {
  return toNum(item.price) * Number(item.quantity || 1)
}
const cartTotal = computed(() =>
  (cart.value?.items || []).reduce((sum, i) => sum + itemSubtotal(i), 0)
)

/* ---------- handlers ---------- */
function handleQuantityChange(productId, newQuantity) {
  const q = parseInt(newQuantity, 10)
  updateItemQuantity(productId, q)
}
</script>

