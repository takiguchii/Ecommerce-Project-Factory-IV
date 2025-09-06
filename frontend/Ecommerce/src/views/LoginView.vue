<template>
  <main class="min-h-screen bg-gray-100 flex items-center justify-center p-4">
    <section class="w-full max-w-sm">
      <div class="bg-white shadow rounded-xl p-6">
        <!-- Título -->
        <h1 class="text-2xl font-semibold mb-4">Login</h1>

        <!-- Formulário -->
        <form @submit.prevent="onSubmit">
          <div class="mb-3">
            <label class="block text-sm mb-1">E-mail</label>
            <input
              v-model="form.email"
              type="email"
              placeholder="seu@email.com"
              class="w-full border rounded px-3 py-2"
            />
          </div>

          <div class="mb-3">
            <label class="block text-sm mb-1">Senha</label>
            <input
              v-model="form.password"
              type="password"
              placeholder="********"
              class="w-full border rounded px-3 py-2"
            />
          </div>

          <!-- Botão -->
          <button type="submit" class="w-full bg-gray-900 text-white rounded py-2 mt-2">
            Entrar
          </button>
        </form>

        <!-- Link para cadastro -->
        <div class="text-center mt-4">
          <RouterLink to="/register" class="underline text-sm text-gray-600">
            Não possui conta? Cadastre-se
          </RouterLink>
        </div>
      </div>
    </section>
  </main>
</template>

<script setup>
import { reactive } from 'vue'

const form = reactive({
  email: '',
  password: ''
})

function onSubmit() {
  alert(`Login com: ${form.email}`)
}
</script>

<style scoped>
</style>
