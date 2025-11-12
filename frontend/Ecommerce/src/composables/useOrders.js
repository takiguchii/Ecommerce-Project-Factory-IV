import { ref } from 'vue';
import api from '@/services/api'; 

export function useOrders() {
  const isLoading = ref(false);
  const error = ref(null);

  const checkout = async () => {
    isLoading.value = true;
    error.value = null;
    
    try {
      const response = await api.post('/orders/checkout');
      
      return response.data; 
      
    } catch (err) {
      const message = err.response?.data?.message || 'Erro ao finalizar a compra.';
      error.value = message;
      
      throw new Error(message); 
    } finally {
      isLoading.value = false;
    }
  };

  return { 
    checkout, 
    isLoading, 
    error 
  };
}