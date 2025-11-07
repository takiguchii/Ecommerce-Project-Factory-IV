import { ref } from 'vue';
import api from '@/services/api'; 

const cart = ref({
  items: [],
  totalValue: 0
});

export function useCart() {

  async function fetchCart() {
    try {
      const response = await api.get('/api/cart');
      cart.value = response.data;
    } catch (error) {
      console.error("Erro ao buscar carrinho:", error);
      cart.value = { items: [], totalValue: 0 };
    }
  }

  async function addToCart(productId, quantity) {
    try {
      const response = await api.post('/api/cart/add', { productId, quantity });
      cart.value = response.data; 
      alert('Produto adicionado ao carrinho!');
    } catch (error) {
      console.error("Erro ao adicionar ao carrinho:", error);
      alert('Erro ao adicionar produto. Tente fazer o login.');
    }
  }

  async function removeFromCart(productId) {
    try {
      const response = await api.delete(`/api/cart/remove/${productId}`);
      cart.value = response.data; 
    } catch (error) {
      console.error("Erro ao remover do carrinho:", error);
    }
  }

  async function updateItemQuantity(productId, quantity) {
    if (quantity <= 0) {
      await removeFromCart(productId);
      return;
    }

    try {
      const response = await api.put(`/api/cart/update/${productId}?quantity=${quantity}`);
      cart.value = response.data; 
    } catch (error) {
      console.error("Erro ao atualizar quantidade:", error);
    }
  }

  return {
    cart,
    fetchCart,
    addToCart,
    removeFromCart,
    updateItemQuantity
  };
}