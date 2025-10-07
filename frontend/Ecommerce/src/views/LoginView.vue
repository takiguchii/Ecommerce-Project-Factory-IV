<template>
  <main class="min-h-[calc(100vh-5rem)] pt-24 bg-gradient-to-b from-neutral-950 via-neutral-900 to-black text-white flex items-center justify-center px-4">
    <section class="w-full max-w-md">
      <div
        class="rounded-2xl bg-neutral-900/70 border border-neutral-800 ring-1 ring-black/5 shadow-xl backdrop-blur p-6 sm:p-8
               transition-all duration-300 hover:shadow-orange-500/10 hover:border-neutral-700"
      >
        <!-- Título -->
        <h1 class="text-3xl font-extrabold text-orange-400 tracking-tight mb-6">Login</h1>

        <!-- Formulário -->
        <form @submit.prevent="onSubmit" class="space-y-4">
          <!-- E-mail -->
          <div class="space-y-1.5">
            <label class="block text-sm text-neutral-300">E-mail</label>
            <div class="relative">
              <span class="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2">
                <!-- ícone mail -->
                <svg class="w-5 h-5 text-neutral-400" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.8" d="M3 8l9 6 9-6"/>
                  <rect x="3" y="6" width="18" height="12" rx="2" ry="2" stroke-width="1.8"/>
                </svg>
              </span>
              <input
                v-model="form.email"
                type="email"
                placeholder="seu@email.com"
                class="w-full rounded-xl bg-neutral-800/80 border border-neutral-700 pl-10 pr-4 py-3 text-white placeholder-neutral-400
                       focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 transition-all"
              />
            </div>
          </div>

          <!-- Senha -->
          <div class="space-y-1.5">
            <label class="block text-sm text-neutral-300">Senha</label>
            <div class="relative">
              <span class="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2">
                <!-- ícone lock -->
                <svg class="w-5 h-5 text-neutral-400" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                  <rect x="4" y="11" width="16" height="9" rx="2" stroke-width="1.8"/>
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.8" d="M8 11V8a4 4 0 118 0v3"/>
                </svg>
              </span>
              <input
                v-model="form.password"
                type="password"
                placeholder="********"
                class="w-full rounded-xl bg-neutral-800/80 border border-neutral-700 pl-10 pr-4 py-3 text-white placeholder-neutral-400
                       focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 transition-all"
              />
            </div>
          </div>

          <!-- Botão -->
          <button
            type="submit"
            class="w-full bg-orange-500 hover:bg-orange-600 active:scale-[0.99] text-black font-semibold py-3 rounded-xl
                   shadow-md ring-1 ring-orange-500/20 hover:ring-orange-500/40 transition-all mt-2"
          >
            Entrar
          </button>
        </form>

        <!-- Link para cadastro -->
        <div class="text-center mt-6">
          <RouterLink to="/register" class="text-sm text-neutral-400 hover:text-orange-300 transition-colors">
            Não possui conta? <span class="text-orange-400">Cadastre-se</span>
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
