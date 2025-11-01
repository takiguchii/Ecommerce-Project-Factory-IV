<template>
  <div class="max-w-3xl mx-auto p-4 sm:p-6 lg:p-8">
    <h2 class="text-2xl font-bold text-gray-900 mb-2">Meu Perfil</h2>
    <p class="text-sm text-gray-600 mb-6">
      Complete seus dados para agilizar suas futuras compras.
    </p>

    <div v-if="isLoading" class="bg-blue-100 border border-blue-400 text-blue-700 px-4 py-3 rounded relative mb-4" role="alert">
      Carregando dados...
    </div>
    <div v-if="error" class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative mb-4" role="alert">
      {{ error }}
    </div>
    <div v-if="successMessage" class="bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded relative mb-4" role="alert">
      {{ successMessage }}
    </div>

    <form v-if="profile" @submit.prevent="handleSubmit" class="flex flex-col gap-6">
      
      <fieldset class="border border-gray-200 rounded-lg shadow-sm overflow-hidden">
        <legend class="px-4 py-2 bg-gray-50 text-sm font-medium text-gray-700 w-full">Dados da Conta</legend>
        <div class="p-4 sm:p-6 space-y-4">
          <div>
            <label for="username" class="block text-sm font-medium text-gray-700 mb-1">Username:</label>
            <input type="text" id="username" :value="profile.username" class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm sm:text-sm disabled:bg-gray-100 disabled:text-gray-500" disabled />
          </div>
          <div>
            <label for="email" class="block text-sm font-medium text-gray-700 mb-1">Email:</label>
            <input type="email" id="email" :value="profile.email" class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm sm:text-sm disabled:bg-gray-100 disabled:text-gray-500" disabled />
          </div>
        </div>
      </fieldset>

      <fieldset class="border border-gray-200 rounded-lg shadow-sm overflow-hidden">
        <legend class="px-4 py-2 bg-gray-50 text-sm font-medium text-gray-700 w-full">Dados Pessoais</legend>
        <div class="p-4 sm:p-6 grid grid-cols-1 sm:grid-cols-2 gap-4">
          <div class="sm:col-span-2">
            <label for="fullName" class="block text-sm font-medium text-gray-700 mb-1">Nome Completo:</label>
            <input type="text" id="fullName" v-model="profile.fullName" placeholder="Digite seu nome completo" class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
          <div>
            <label for="document" class="block text-sm font-medium text-gray-700 mb-1">CPF/CNPJ:</label>
            <input type="text" id="document" v-model="profile.documentCpfCnpj" placeholder="Digite seu CPF ou CNPJ" class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
          <div>
            <label for="phone" class="block text-sm font-medium text-gray-700 mb-1">Telefone:</label>
            <input type="tel" id="phone" v-model="profile.phone" placeholder="(XX) XXXXX-XXXX" class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
        </div>
      </fieldset>

      <fieldset class="border border-gray-200 rounded-lg shadow-sm overflow-hidden">
        <legend class="px-4 py-2 bg-gray-50 text-sm font-medium text-gray-700 w-full">Endereço de Entrega</legend>
        <div class="p-4 sm:p-6 grid grid-cols-1 sm:grid-cols-6 gap-4">
          <div class="sm:col-span-2">
            <label for="postalCode" class="block text-sm font-medium text-gray-700 mb-1">CEP:</label>
            <input type="text" id="postalCode" v-model="profile.postalCode" placeholder="XXXXX-XXX" class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
          <div class="sm:col-span-4">
             <label for="addressLine" class="block text-sm font-medium text-gray-700 mb-1">Endereço (Rua e Nº):</label>
            <input type="text" id="addressLine" v-model="profile.addressLine" placeholder="Ex: Rua Brasil, 123" class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
          <div class="sm:col-span-3">
            <label for="complement" class="block text-sm font-medium text-gray-700 mb-1">Complemento:</label>
            <input type="text" id="complement" v-model="profile.addressComplement" placeholder="Ex: Apto 101, Bloco B" class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
          <div class="sm:col-span-3">
            <label for="neighborhood" class="block text-sm font-medium text-gray-700 mb-1">Bairro:</label>
            <input type="text" id="neighborhood" v-model="profile.neighborhood" placeholder="Digite seu bairro" class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
          <div class="sm:col-span-4">
            <label for="city" class="block text-sm font-medium text-gray-700 mb-1">Cidade:</label>
            <input type="text" id="city" v-model="profile.city" placeholder="Digite sua cidade" class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
          <div class="sm:col-span-2">
            <label for="state" class="block text-sm font-medium text-gray-700 mb-1">Estado (UF):</label>
            <input type="text" id="state" v-model="profile.state" placeholder="Ex: SP" maxlength="2" class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
          </div>
        </div>
      </fieldset>
      <div class="flex justify-end">
        <button type="submit" 
                class="inline-flex justify-center py-2 px-6 border border-transparent shadow-sm text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:bg-gray-400 disabled:cursor-not-allowed" 
                :disabled="isLoading">
          {{ isLoading ? 'Salvando...' : 'Salvar Alterações' }}
        </button>
      </div>

    </form>
  </div>
</template>

<script setup>
import { onMounted, ref } from 'vue';
import { useProfile } from '../composables/useProfile';

const { profile, isLoading, error, fetchProfile, updateProfile } = useProfile();

const successMessage = ref('');

onMounted(() => {
  fetchProfile();
});

// Função chamada ao enviar o formulário
const handleSubmit = async () => {
  successMessage.value = '';
  
  const success = await updateProfile(); 
  
  if (success) {
    successMessage.value = 'Perfil atualizado com sucesso!';
    setTimeout(() => {
      successMessage.value = '';
    }, 3000);
  }
};
</script>