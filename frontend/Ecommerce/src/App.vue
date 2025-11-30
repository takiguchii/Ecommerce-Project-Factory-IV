<template>
  <NavbarComponent v-if="!isAdminRoute" />
  <RouterView />
</template>

<script setup>
import { onMounted, computed } from 'vue'
import { RouterView, useRoute } from 'vue-router'
import NavbarComponent from './components/NavbarComponent.vue'
import { useAuth } from './composables/useAuth'

const route = useRoute()
const { checkAuthStatus } = useAuth()
const isAdminRoute = computed(() => route.path.startsWith('/admin'))
onMounted(() => {
  checkAuthStatus()
})
</script>
<style scoped>
</style>