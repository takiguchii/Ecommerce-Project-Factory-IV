<template>
  <section class="min-h-[60vh] bg-neutral-950 text-neutral-100">
    <div class="max-w-6xl mx-auto p-4 md:p-8">
      <h1 class="text-2xl md:text-3xl font-bold text-orange-400 mb-6">Meu Carrinho</h1>

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

      <div v-else class="grid grid-cols-1 lg:grid-cols-3 gap-6">
        
        <div class="lg:col-span-2 space-y-6">
          
          <div class="space-y-4">
             <div
              v-for="item in cart.items"
              :key="item.productId"
              class="bg-neutral-900 border border-neutral-800 rounded-xl p-4 md:p-5 flex gap-4 md:gap-5 items-start"
            >
              <div class="w-24 h-24 md:w-28 md:h-28 bg-white rounded-lg flex items-center justify-center overflow-hidden shrink-0">
                <img :src="getImageUrl(item)" :alt="item.name" class="max-w-full max-h-full object-contain" />
              </div>

              <div class="flex-1 min-w-0">
                <h3 class="font-semibold text-orange-300 truncate">{{ item.name }}</h3>
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
                </div>
                <button @click="removeFromCart(item.productId)" class="mt-3 text-sm text-red-400 hover:text-red-300">Remover</button>
              </div>
            </div>
          </div>

          <div class="bg-neutral-900 border border-neutral-800 rounded-xl p-6">
            <h2 class="text-lg font-semibold text-neutral-200 mb-4">Calcular Frete</h2>
            
            <div class="flex gap-3 mb-4">
              <input 
                type="text" 
                v-model="cepInput"
                @keyup.enter="handleCalculateShipping"
                placeholder="Digite seu CEP"
                class="flex-1 rounded-lg bg-neutral-800 border border-neutral-700 px-4 py-2 text-neutral-100 focus:outline-none focus:border-orange-500 transition-colors"
              />
              <button 
                @click="handleCalculateShipping"
                :disabled="loadingShipping"
                class="px-6 py-2 rounded-lg border border-orange-500 text-orange-500 font-semibold hover:bg-orange-500 hover:text-black transition-all disabled:opacity-50"
              >
                {{ loadingShipping ? '...' : 'OK' }}
              </button>
            </div>

            <p v-if="shippingError" class="text-red-400 text-sm mb-3">{{ shippingError }}</p>

            <div v-if="shippingOptions.length > 0" class="space-y-2">
              <div 
                v-for="option in shippingOptions" 
                :key="option.name"
                @click="selectedShipping = option"
                class="flex items-center justify-between p-3 rounded-lg border cursor-pointer transition-all hover:bg-neutral-800"
                :class="selectedShipping?.name === option.name ? 'border-orange-500 bg-neutral-800' : 'border-neutral-700'"
              >
                <div class="flex items-center gap-3">
                  <div class="w-4 h-4 rounded-full border flex items-center justify-center"
                      :class="selectedShipping?.name === option.name ? 'border-orange-500' : 'border-neutral-500'">
                      <div v-if="selectedShipping?.name === option.name" class="w-2 h-2 rounded-full bg-orange-500"></div>
                  </div>
                  <div>
                    <p class="font-semibold text-neutral-200">{{ option.name }} <span class="text-xs font-normal text-neutral-400">({{ option.carrier }})</span></p>
                    <p class="text-xs text-neutral-400">Entrega em {{ option.estimatedDeliveryDays }} dias úteis</p>
                  </div>
                </div>
                <p class="font-bold text-orange-400">R$ {{ toFixed2(option.price) }}</p>
              </div>
            </div>
          </div>

          <div class="bg-neutral-900 border border-neutral-800 rounded-xl p-6">
            <h2 class="text-lg font-semibold text-neutral-200 mb-4">Forma de Pagamento</h2>
            
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <button
                @click="selectedPayment = 'PIX'"
                class="flex items-center gap-3 p-4 rounded-lg border-2 transition-all hover:bg-neutral-800"
                :class="selectedPayment === 'PIX' ? 'border-orange-500 bg-neutral-800' : 'border-neutral-700'"
              >
                <span class="text-2xl">💠</span> 
                <div class="text-left">
                  <p class="font-bold text-neutral-100">Pix</p>
                  <p class="text-xs text-neutral-400">Aprovação imediata</p>
                </div>
              </button>

              <button
                @click="selectedPayment = 'CREDITCARD'"
                class="flex items-center gap-3 p-4 rounded-lg border-2 transition-all hover:bg-neutral-800"
                :class="selectedPayment === 'CREDITCARD' ? 'border-orange-500 bg-neutral-800' : 'border-neutral-700'"
              >
                 <span class="text-2xl">💳</span>
                <div class="text-left">
                  <p class="font-bold text-neutral-100">Cartão de Crédito</p>
                  <p class="text-xs text-neutral-400">Até 12x sem juros</p>
                </div>
              </button>
            </div>
          </div>

        </div>

        <aside class="bg-neutral-900 border border-neutral-800 rounded-xl p-5 h-fit sticky top-6">
          <h2 class="text-lg font-semibold text-neutral-200 mb-4">Resumo do Pedido</h2>

          <div class="flex justify-between text-neutral-300 mb-2">
            <span>Itens</span>
            <span>R$ {{ toFixed2(cartTotal) }}</span>
          </div>
          
          <div class="flex justify-between text-neutral-300 mb-2">
             <span>Frete</span>
             <span :class="selectedShipping ? 'text-neutral-200' : 'text-neutral-500'">
               {{ selectedShipping ? `R$ ${toFixed2(selectedShipping.price)}` : '--' }}
             </span>
          </div>

          <div class="flex justify-between text-neutral-300 mb-2 text-sm">
             <span>Método</span>
             <span class="text-orange-200">{{ selectedPayment === 'PIX' ? 'Pix' : (selectedPayment === 'CREDITCARD' ? 'Cartão' : '-') }}</span>
          </div>

          <div class="flex justify-between text-neutral-300 mb-4 mt-4 border-t border-neutral-800 pt-4">
            <span class="text-lg">Total</span>
            <span class="font-bold text-orange-400 text-xl">
              R$ {{ toFixed2(cartTotal + (selectedShipping?.price || 0)) }}
            </span>
          </div>

          <button
            @click="handleCheckout"
            :disabled="isCheckoutLoading || !selectedPayment || !selectedShipping"
            class="w-full rounded-lg bg-orange-500 hover:bg-orange-600 text-black font-bold py-3 transition-colors disabled:bg-neutral-600 disabled:cursor-not-allowed disabled:text-neutral-400"
          >
            {{ isCheckoutLoading ? 'Processando...' : 'Finalizar Compra' }}
          </button>
          
          <div class="mt-3 text-center text-xs text-neutral-500 space-y-1">
            <p v-if="!selectedShipping" class="text-orange-400/70">* Calcule e selecione o frete</p>
            <p v-if="!selectedPayment" class="text-orange-400/70">* Selecione o pagamento</p>
          </div>

          <p v-if="checkoutError" class="mt-3 text-sm text-red-400 text-center">
            {{ checkoutError }}
          </p>
        </aside>
      </div>
    </div>
  </section>
</template>

<script setup>
import { onMounted, computed, ref } from 'vue' 
import { useCart } from '@/composables/useCart'
import { useOrders } from '@/composables/useOrders' 
import { useRouter } from 'vue-router'
import { calculateShipping } from '@/services/api' 

const { cart, fetchCart, removeFromCart, updateItemQuantity } = useCart()
const { checkout, isLoading: isCheckoutLoading, error: checkoutError } = useOrders();
const router = useRouter();

const selectedPayment = ref('');
const mainUrl = 'https://www.kabum.com.br/'

const cepInput = ref('');
const shippingOptions = ref([]);
const selectedShipping = ref(null); 
const loadingShipping = ref(false);
const shippingError = ref('');

onMounted(() => {
  fetchCart()
})

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
function getImageUrl(item) {
  if (item.imageUrl && /^https?:\/\//i.test(item.imageUrl)) return item.imageUrl
  const path = item.imageUrl || item.image_url0 || ''
  return /^https?:\/\//i.test(path) ? path : mainUrl + path
}
function itemSubtotal(item) {
  return toNum(item.price) * Number(item.quantity || 1)
}

const cartTotal = computed(() =>
  (cart.value?.items || []).reduce((sum, i) => sum + itemSubtotal(i), 0)
)

function handleQuantityChange(productId, newQuantity) {
  const q = parseInt(newQuantity, 10)
  updateItemQuantity(productId, q)
}

// --- Lógica do Frete ---
const handleCalculateShipping = async () => {
    if (!cepInput.value || cepInput.value.length < 8) {
        shippingError.value = "Digite um CEP válido.";
        return;
    }

    loadingShipping.value = true;
    shippingError.value = '';
    shippingOptions.value = [];
    selectedShipping.value = null;

    try {
        const response = await calculateShipping(cepInput.value);
        shippingOptions.value = response.data; // O axios devolve em .data
        
        if(shippingOptions.value.length === 0) {
           shippingError.value = "Nenhuma opção encontrada para este CEP.";
        }
    } catch (error) {
        console.error("Erro frete:", error);
        shippingError.value = "Erro ao calcular frete. Verifique o CEP.";
    } finally {
        loadingShipping.value = false;
    }
};

const handleCheckout = async () => {
  if (!selectedPayment.value) {
    alert('Por favor, selecione uma forma de pagamento.');
    return;
  }
  
  if (!selectedShipping.value) {
    alert('Por favor, selecione uma opção de frete.');
    return;
  }

  try {

    await checkout(selectedPayment.value);

    alert('Pedido realizado com sucesso!');
    
    selectedPayment.value = '';
    selectedShipping.value = null;
    shippingOptions.value = [];
    cepInput.value = '';
    
    await fetchCart(); 

  } catch (error) {
    console.error(error);
  }
};
</script>