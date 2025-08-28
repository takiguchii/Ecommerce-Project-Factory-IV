<template>
  <div class="min-h-screen flex flex-col relative">
      <Navbar />
      <main class="flex-grow">
        <router-view />
      </main>
  </div>
</template>

<script setup>
import Navbar from './components/NavbarComponent.vue'
</script>
