import { ref } from 'vue';
import api from '@/services/api'; // Seu 'api.js' que já usa o Axios

// --- Estado Global Simples ---
// Definido fora da função, então é compartilhado por todos que usarem o "useCart"
const cart = ref({
  items: [],
  totalValue: 0
});
// -----------------------------

export function useCart() {

  // Função para buscar o carrinho da API
  // (Usado quando a página carrega)
  async function fetchCart() {
    try {
      const response = await api.get('/api/cart');
      cart.value = response.data;
    } catch (error) {
      console.error("Erro ao buscar carrinho:", error);
      // Se der erro (ex: 401 não logado), limpa o carrinho local
      cart.value = { items: [], totalValue: 0 };
    }
  }

  // Função para adicionar um item
  async function addToCart(productId, quantity) {
    try {
      const response = await api.post('/api/cart/add', { productId, quantity });
      cart.value = response.data; // Atualiza o carrinho local com a resposta
      alert('Produto adicionado ao carrinho!');
    } catch (error) {
      console.error("Erro ao adicionar ao carrinho:", error);
      alert('Erro ao adicionar produto. Tente fazer o login.');
    }
  }

  // Função para remover um item
  async function removeFromCart(productId) {
    try {
      const response = await api.delete(`/api/cart/remove/${productId}`);
      cart.value = response.data; // Atualiza o carrinho
    } catch (error) {
      console.error("Erro ao remover do carrinho:", error);
    }
  }

  // Função para atualizar a quantidade
  async function updateItemQuantity(productId, quantity) {
    // Se a quantidade for 0 ou menos, remove
    if (quantity <= 0) {
      await removeFromCart(productId);
      return;
    }

    try {
      const response = await api.put(`/api/cart/update/${productId}?quantity=${quantity}`);
      cart.value = response.data; // Atualiza o carrinho
    } catch (error) {
      console.error("Erro ao atualizar quantidade:", error);
    }
  }

  // Expõe o estado (cart) e as funções para os componentes
  return {
    cart,
    fetchCart,
    addToCart,
    removeFromCart,
    updateItemQuantity
  };
}