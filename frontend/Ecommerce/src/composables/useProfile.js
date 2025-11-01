import { ref } from 'vue';
import api from '../services/api'; 

export function useProfile() {
  
  const profile = ref({});
  const isLoading = ref(false);
  const error = ref(null);

  const fetchProfile = async () => {
    isLoading.value = true;
    error.value = null;
    try {
      // CORREÇÃO AQUI
      const response = await api.get('/user/profile'); // Removido o /api
      
      profile.value = response.data;
      
    } catch (err) {
      console.error('Erro ao buscar perfil:', err);
      error.value = 'Não foi possível carregar os dados do perfil.';
    } finally {
      isLoading.value = false;
    }
  };

  const updateProfile = async () => {
    isLoading.value = true;
    error.value = null;
    try {
      // E CORREÇÃO AQUI
      const response = await api.put('/user/profile', profile.value); // Removido o /api
      
      profile.value = response.data;
      return true; 
      
    } catch (err) {
      console.error('Erro ao atualizar perfil:', err);
      error.value = 'Erro ao salvar. Verifique os dados e tente novamente.';
      return false; 
    } finally {
      isLoading.value = false;
    }
  };

  return {
    profile,
    isLoading,
    error,
    fetchProfile,
    updateProfile
  };
}