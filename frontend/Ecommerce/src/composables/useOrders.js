import { ref } from 'vue';
import api from '@/services/api'; 

export function useOrders() {
  const isLoading = ref(false);
  const error = ref(null);

  const checkout = async (paymentMethod, shippingOption, couponCode = null) => {
    isLoading.value = true;
    error.value = null;
    
    try {
      const payload = {
        paymentMethod: paymentMethod,
        shippingCost: shippingOption.price,
        carrier: shippingOption.name + ' (' + shippingOption.carrier + ')',
        couponCode: couponCode
      };

      const response = await api.post('/orders/checkout', payload);
      return response.data; 
      
    } catch (err) {
      const message = err.response?.data?.message || 'Erro ao finalizar a compra.';
      error.value = message;
      throw new Error(message); 
    } finally {
      isLoading.value = false;
    }
  };

  return { checkout, isLoading, error };
}