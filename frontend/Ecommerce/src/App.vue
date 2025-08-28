<template>
  <div class="min-h-screen flex flex-col relative">
    <Navbar />
    <main class="flex-grow">
      <router-view />
    </main>
  </div>
</template>

<script setup>
import { RouterView } from 'vue-router'
import NavbarComponent from './components/NavbarComponent.vue'
</script>


