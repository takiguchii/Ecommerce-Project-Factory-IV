<template>
  <main class="min-h-[calc(100vh-5rem)] pt-24 bg-gradient-to-b from-neutral-950 via-neutral-900 to-black text-white flex items-center justify-center px-4">
    <section class="w-full max-w-md">
      <div
        class="rounded-2xl bg-neutral-900/70 border border-neutral-800 ring-1 ring-black/5 shadow-xl backdrop-blur p-6 sm:p-8
               transition-all duration-300 hover:shadow-orange-500/10 hover:border-neutral-700"
      >
        <h1 class="text-3xl font-extrabold text-orange-400 tracking-tight mb-6">Login</h1>

        <form @submit.prevent="handleLogin" class="space-y-4">
          <div class="space-y-1.5">
            <label class="block text-sm text-neutral-300">Usuário</label> <div class="relative">
              <span class="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2">
                <svg class="w-5 h-5 text-neutral-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                </svg>
              </span>
              <input
                v-model="form.username" type="text"             placeholder="nome.usuario" required                class="w-full rounded-xl bg-neutral-800/80 border border-neutral-700 pl-10 pr-4 py-3 text-white placeholder-neutral-400
                       focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 transition-all"
              />
            </div>
          </div>

          <div class="space-y-1.5">
            <label class="block text-sm text-neutral-300">Senha</label>
            <div class="relative">
              <span class="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2">
                <svg class="w-5 h-5 text-neutral-400" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                  <rect x="4" y="11" width="16" height="9" rx="2" stroke-width="1.8"/>
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.8" d="M8 11V8a4 4 0 118 0v3"/>
                </svg>
              </span>
              <input
                v-model="form.password"
                type="password"
                placeholder="********"
                required                class="w-full rounded-xl bg-neutral-800/80 border border-neutral-700 pl-10 pr-4 py-3 text-white placeholder-neutral-400
                       focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 transition-all"
              />
            </div>
          </div>

          <button
            type="submit"
            :disabled="loading"
            class="w-full bg-orange-500 hover:bg-orange-600 active:scale-[0.99] text-black font-semibold py-3 rounded-xl
                   shadow-md ring-1 ring-orange-500/20 hover:ring-orange-500/40 transition-all mt-2 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {{ loading ? 'Entrando...' : 'Entrar' }}
          </button>
        </form>

        <p v-if="error" class="text-red-500 text-sm mt-4 text-center">{{ error }}</p>

        <div class="text-center mt-6">
          <RouterLink to="/register" class="text-sm text-neutral-400 hover:text-orange-300 transition-colors">
            Não possui conta? <span class="text-orange-400">Cadastre-se</span>
          </RouterLink>
           <p class="text-xs text-neutral-500 mt-2">Admin: admin / Admin@123</p>
        </div>
      </div>
    </section>
  </main>
</template>

<script setup>
import { reactive, ref } from 'vue';
import { useRouter } from 'vue-router';
import { login as apiLogin } from '@/services/api'; // Renomeia 'login' para evitar conflito
// 1. Importa o composable e a função 'setLoggedIn'
import { useAuth } from '@/composables/useAuth';

const { setLoggedIn, user } = useAuth(); // Pega a função para setar o estado

const form = reactive({ username: '', password: '' });
const loading = ref(false);
const error = ref('');
const router = useRouter();

async function handleLogin() {
  loading.value = true;
  error.value = '';

  try {
    const response = await apiLogin({ // Usa a função renomeada da API
      username: form.username,
      password: form.password
    });

    if (response.data && response.data.token) {
      const token = response.data.token;

      // --- INÍCIO DA MODIFICAÇÃO ---
      // 2. Chama a função do composable para guardar token e atualizar estado global
      setLoggedIn(token);

      // 3. Usa o 'user' reativo do composable para decidir o redirecionamento
      //    (checkAuthStatus dentro de setLoggedIn já preencheu 'user')
      if (user.value?.role === 'Admin') { // Usa optional chaining (?.)
        router.push('/admin/brands');
      } else {
        router.push('/');
      }
      // --- FIM DA MODIFICAÇÃO ---

    } else {
      throw new Error('Resposta inválida do servidor');
    }

  } catch (err) {
    console.error("Erro no login:", err);
    localStorage.removeItem('authToken'); // Segurança extra
    // A lógica de erro continua igual
    if (err.response && err.response.status === 401) {
      error.value = 'Usuário ou senha inválidos.';
    } else {
      error.value = 'Ocorreu um erro ao tentar fazer login.';
    }
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
</style>