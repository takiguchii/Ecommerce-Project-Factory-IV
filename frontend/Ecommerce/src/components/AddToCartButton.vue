<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useCart } from '@/composables/useCart'

const props = defineProps({
  product: { type: Object, required: true }
})

const router = useRouter()
const showToast = ref(false)
const toastMessage = ref('')
const toastType = ref('success') // success | error | info

const { addToCart: addToCartGlobal } = useCart()

function triggerToast(message, type = 'success') {
  toastMessage.value = message
  toastType.value = type
  showToast.value = true
  setTimeout(() => (showToast.value = false), 2500)
}

async function addToCart() {
  try {
    const token = localStorage.getItem('authToken')
    if (!token) {
      triggerToast('⚠️ Faça login para adicionar produtos ao carrinho.', 'error')
      router.push({ name: 'Login' })
      return
    }

    await addToCartGlobal(props.product.id, 1) // usa o composable que atualiza o navbar

    triggerToast('✅ Produto adicionado ao carrinho!')
  } catch (err) {
    console.error('Erro ao adicionar ao carrinho:', err)
    triggerToast('❌ Erro ao adicionar produto ao carrinho.', 'error')
  }
}
</script>

<template>
  <div class="relative">
    <!-- Botão -->
    <button
      @click.stop="addToCart"
      class="w-full mt-3 bg-orange-500 hover:bg-orange-600 text-black font-bold py-2 px-4 rounded-md transition-colors duration-300"
    >
      Adicionar ao Carrinho
    </button>

    <!-- Toast -->
    <transition name="fade">
      <div
        v-if="showToast"
        class="fixed bottom-6 right-6 px-5 py-3 rounded-lg shadow-lg text-sm font-medium z-[100] border
          transition-all duration-500"
        :class="{
          'bg-green-500 text-black border-green-400': toastType === 'success',
          'bg-red-600 text-white border-red-500': toastType === 'error',
          'bg-yellow-500 text-black border-yellow-400': toastType === 'info'
        }"
      >
        {{ toastMessage }}
      </div>
    </transition>
  </div>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.4s ease, transform 0.4s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: translateY(20px);
}
</style>
