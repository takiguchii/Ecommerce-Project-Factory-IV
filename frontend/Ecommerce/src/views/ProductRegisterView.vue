<template>
  <main class="min-h-screen bg-gray-100 flex items-center justify-center p-4">
    <section class="w-full max-w-lg">
      <ProductRegisterForm @product-registered="onProductRegistered" />
    </section>
  </main>
</template>

<script setup>
import ProductRegisterForm from '../components/ProductRegisterForm.vue'

function onProductRegistered() {
  alert('Produto registrado com sucesso!')
}
</script>

<style scoped>
</style>