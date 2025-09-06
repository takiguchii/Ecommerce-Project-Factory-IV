<template>
  <main class="min-h-screen bg-gray-100 flex items-center justify-center p-4">
    <section class="w-full max-w-sm">
      <div class="bg-white shadow rounded-xl p-6">
        <!-- Título -->
        <h1 class="text-2xl font-semibold mb-4">Cadastro</h1>

        <!-- Formulário -->
        <form @submit.prevent="onSubmit">
          <div class="mb-3">
            <label class="block text-sm mb-1">Nome</label>
            <input
              v-model="form.name"
              type="text"
              placeholder="Seu nome"
              class="w-full border rounded px-3 py-2"
            />
          </div>

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

          <div class="mb-3">
            <label class="block text-sm mb-1">Confirmar Senha</label>
            <input
              v-model="form.confirm"
              type="password"
              placeholder="********"
              class="w-full border rounded px-3 py-2"
            />
          </div>

          <!-- Botão -->
          <button type="submit" class="w-full bg-gray-900 text-white rounded py-2 mt-2">
            Cadastrar
          </button>
        </form>

        <!-- Link para login -->
        <div class="text-center mt-4">
          <RouterLink to="/login" class="underline text-sm text-gray-600">
            Já possui conta? Entrar
          </RouterLink>
        </div>
      </div>
    </section>
  </main>
</template>

<script setup>
import { reactive } from 'vue'

const form = reactive({
  name: '',
  email: '',
  password: '',
  confirm: ''
})

function onSubmit() {
  alert('Cadastro simples validado!')
}
</script>
