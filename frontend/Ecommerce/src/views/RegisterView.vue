<template>
  <main class="min-h-[calc(100vh-5rem)] pt-24 bg-gradient-to-b from-neutral-950 via-neutral-900 to-black text-white flex items-center justify-center px-4">
    <section class="w-full max-w-md">
      <div
        class="rounded-2xl bg-neutral-900/70 border border-neutral-800 ring-1 ring-black/5 shadow-xl backdrop-blur p-6 sm:p-8
               transition-all duration-300 hover:shadow-orange-500/10 hover:border-neutral-700"
      >
        <h1 class="text-3xl font-extrabold text-orange-400 tracking-tight mb-6">Cadastro</h1>

        <form @submit.prevent="handleRegister" class="space-y-4">
          <div class="space-y-1.5">
            <label class="block text-sm text-neutral-300">Nome de Usuário</label>
            <div class="relative">
              <span class="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2">
                 <svg class="w-5 h-5 text-neutral-400" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" /></svg>
              </span>
              <input v-model="form.username" type="text" placeholder="nome.de.usuario" required class="w-full rounded-xl bg-neutral-800/80 border border-neutral-700 pl-10 pr-4 py-3 text-white placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 transition-all"/>
            </div>
          </div>

          <div class="space-y-1.5">
            <label class="block text-sm text-neutral-300">E-mail</label>
            <div class="relative">
              <span class="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2">
                <svg class="w-5 h-5 text-neutral-400" viewBox="0 0 24 24" fill="none" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.8" d="M3 8l9 6 9-6"/><rect x="3" y="6" width="18" height="12" rx="2" ry="2" stroke-width="1.8"/></svg>
              </span>
              <input v-model="form.email" type="email" placeholder="seu@email.com" required class="w-full rounded-xl bg-neutral-800/80 border border-neutral-700 pl-10 pr-4 py-3 text-white placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 transition-all"/>
            </div>
          </div>

          <div class="space-y-1.5">
            <label class="block text-sm text-neutral-300">Senha</label>
            <div class="relative">
              <span class="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2">
                <svg class="w-5 h-5 text-neutral-400" viewBox="0 0 24 24" fill="none" stroke="currentColor"><rect x="4" y="11" width="16" height="9" rx="2" stroke-width="1.8"/><path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.8" d="M8 11V8a4 4 0 118 0v3"/></svg>
              </span>
              <input v-model="form.password" type="password" placeholder="********" required class="w-full rounded-xl bg-neutral-800/80 border border-neutral-700 pl-10 pr-4 py-3 text-white placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 transition-all"/>
              <p class="text-xs text-neutral-400 mt-1 pl-1">Mínimo 6 caracteres, maiúscula, minúscula, número e símbolo (ex: !@#$).</p>
            </div>
          </div>

          <div class="space-y-1.5">
            <label class="block text-sm text-neutral-300">Confirmar Senha</label>
            <div class="relative">
              <span class="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2">
                <svg class="w-5 h-5 text-neutral-400" viewBox="0 0 24 24" fill="none" stroke="currentColor"><rect x="4" y="11" width="16" height="9" rx="2" stroke-width="1.8"/><path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.8" d="M8 11V8a4 4 0 118 0v3"/></svg>
              </span>
              <input
                v-model="form.confirmPassword" type="password"
                placeholder="********"
                required
                class="w-full rounded-xl bg-neutral-800/80 border border-neutral-700 pl-10 pr-4 py-3 text-white placeholder-neutral-400
                       focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 transition-all"
              />
            </div>
          </div>
          <button type="submit" :disabled="loading" class="w-full bg-orange-500 hover:bg-orange-600 active:scale-[0.99] text-black font-semibold py-3 rounded-xl shadow-md ring-1 ring-orange-500/20 hover:ring-orange-500/40 transition-all mt-2 disabled:opacity-50 disabled:cursor-not-allowed">
             {{ loading ? 'Cadastrando...' : 'Cadastrar' }}
          </button>
        </form>

        <p v-if="error && !success" class="text-red-500 text-sm mt-4 text-center">{{ error }}</p>
        <p v-if="success" class="text-green-500 text-sm mt-4 text-center">Cadastro realizado com sucesso! Redirecionando para login...</p>

        <div class="text-center mt-6">
          <RouterLink to="/login" class="text-sm text-neutral-400 hover:text-orange-300 transition-colors">
            Já possui conta? <span class="text-orange-400">Faça login</span>
          </RouterLink>
        </div>
      </div>
    </section>
  </main>
</template>

<script setup>
import { reactive, ref } from 'vue';
import { useRouter } from 'vue-router';
import { register } from '@/services/api';

const form = reactive({
  username: '',
  email: '',
  password: '',
  confirmPassword: '' 
});

const loading = ref(false);
const error = ref('');
const success = ref(false);
const router = useRouter();

async function handleRegister() {
  loading.value = true;
  error.value = '';
  success.value = false;

  if (form.password !== form.confirmPassword) {
      error.value = 'As senhas não coincidem.';
      loading.value = false;
      return;
  }
  try {
      await register({
          username: form.username,
          email: form.email,
          password: form.password,
          confirmPassword: form.confirmPassword 
      });
      // === FIM DA ADIÇÃO ===

      success.value = true;
      setTimeout(() => {
          router.push('/login');
      }, 2000);

  } catch (err) {
      console.error("Erro no cadastro:", err);
      success.value = false;
      if (err.response && err.response.data) {
          if (err.response.data.errors && Array.isArray(err.response.data.errors)) {

              error.value = err.response.data.errors.map(e => {

                  return e.description; // Usa a descrição padrão do Identity
              }).join(' ');
              // === FIM DA MODIFICAÇÃO ===
          } else if (err.response.data.message) {
              error.value = err.response.data.message;
          } else {
              error.value = `Erro ${err.response.status}: ${err.response.statusText || 'Erro desconhecido.'}`;
          }
      } else {
          error.value = 'Não foi possível conectar ao servidor.';
      }
  } finally {
      loading.value = false;
  }
}
</script>

<style scoped></style>