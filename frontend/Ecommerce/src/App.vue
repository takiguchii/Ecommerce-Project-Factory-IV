
<template>
  <NavbarComponent />
  <RouterView />
  </template>

<script setup>
import { onMounted } from 'vue';
import { RouterView } from 'vue-router';
import NavbarComponent from './components/NavbarComponent.vue'; // Verifique o caminho
// 1. Importe o composable
import { useAuth } from './composables/useAuth';

const { checkAuthStatus } = useAuth();

onMounted(() => {
  checkAuthStatus();
});
</script>

<style scoped>
</style>