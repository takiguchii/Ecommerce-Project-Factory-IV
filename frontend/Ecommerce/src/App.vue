<template>
  <div>
    <NavbarComponent />
    <main>
      <router-view />
    </main>
  </div>
</template>

<script setup>
import { RouterView } from 'vue-router'
import NavbarComponent from './components/NavbarComponent.vue'
</script>
