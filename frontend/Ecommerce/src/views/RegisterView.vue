<template>
  <main class="min-h-[calc(100vh-5rem)] pt-24 bg-gradient-to-b from-neutral-950 via-neutral-900 to-black text-white flex items-center justify-center px-4">
    <section class="w-full max-w-md">
      <div
        class="rounded-2xl bg-neutral-900/70 border border-neutral-800 ring-1 ring-black/5 shadow-xl backdrop-blur p-6 sm:p-8
               transition-all duration-300 hover:shadow-orange-500/10 hover:border-neutral-700"
      >
        <!-- Título -->
        <header class="mb-6 text-center">
          <h1 class="text-3xl font-extrabold text-orange-400 tracking-tight">Cadastro</h1>
          <p class="text-sm text-neutral-400 mt-1">Crie sua conta na <span class="text-orange-400/90">TechMart</span></p>
        </header>

        <!-- Formulário (somente visual) -->
        <form class="space-y-4" @submit.prevent>
          <!-- Nome -->
          <div class="space-y-1.5">
            <label for="name" class="block text-sm text-neutral-300">Nome</label>
            <div class="relative">
              <span class="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2">
                <!-- ícone user -->
                <svg class="w-5 h-5 text-neutral-400" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.8" d="M16 7a4 4 0 11-8 0 4 4 0 018 0z"/>
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.8" d="M12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"/>
                </svg>
              </span>
              <input
                id="name"
                type="text"
                placeholder="Seu nome"
                class="w-full rounded-xl bg-neutral-800/80 border border-neutral-700 pl-10 pr-4 py-3 text-white placeholder-neutral-400
                       focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 transition-all"
              />
            </div>
          </div>

          <!-- E-mail -->
          <div class="space-y-1.5">
            <label for="email" class="block text-sm text-neutral-300">E-mail</label>
            <div class="relative">
              <span class="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2">
                <!-- ícone mail -->
                <svg class="w-5 h-5 text-neutral-400" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.8" d="M3 8l9 6 9-6"/>
                  <rect x="3" y="6" width="18" height="12" rx="2" ry="2" stroke-width="1.8"/>
                </svg>
              </span>
              <input
                id="email"
                type="email"
                placeholder="seu@email.com"
                class="w-full rounded-xl bg-neutral-800/80 border border-neutral-700 pl-10 pr-4 py-3 text-white placeholder-neutral-400
                       focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 transition-all"
              />
            </div>
          </div>

          <!-- Senha -->
          <div class="space-y-1.5">
            <label for="password" class="block text-sm text-neutral-300">Senha</label>
            <div class="relative">
              <span class="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2">
                <!-- ícone lock -->
                <svg class="w-5 h-5 text-neutral-400" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                  <rect x="4" y="11" width="16" height="9" rx="2" stroke-width="1.8"/>
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.8" d="M8 11V8a4 4 0 118 0v3"/>
                </svg>
              </span>
              <input
                id="password"
                type="password"
                placeholder="********"
                class="w-full rounded-xl bg-neutral-800/80 border border-neutral-700 pl-10 pr-4 py-3 text-white placeholder-neutral-400
                       focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 transition-all"
              />
            </div>
            <p class="text-xs text-neutral-400">Mínimo de 8 caracteres.</p>
          </div>

          <!-- Confirmar Senha -->
          <div class="space-y-1.5">
            <label for="confirm" class="block text-sm text-neutral-300">Confirmar senha</label>
            <div class="relative">
              <span class="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2">
                <!-- ícone check-lock -->
                <svg class="w-5 h-5 text-neutral-400" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                  <rect x="4" y="11" width="16" height="9" rx="2" stroke-width="1.8"/>
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.8" d="M8 11V8a4 4 0 118 0v3"/>
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.8" d="M9.5 15.5l1.5 1.5 3.5-3.5"/>
                </svg>
              </span>
              <input
                id="confirm"
                type="password"
                placeholder="********"
                class="w-full rounded-xl bg-neutral-800/80 border border-neutral-700 pl-10 pr-4 py-3 text-white placeholder-neutral-400
                       focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 transition-all"
              />
            </div>
          </div>

          <!-- Botão -->
          <button
            type="button"
            class="w-full bg-orange-500 hover:bg-orange-600 active:scale-[0.99] text-black font-semibold py-3 rounded-xl
                   shadow-md ring-1 ring-orange-500/20 hover:ring-orange-500/40 transition-all mt-2"
          >
            Cadastrar
          </button>
        </form>

        <!-- Link para login -->
        <div class="text-center mt-6">
          <RouterLink to="/login" class="text-sm text-neutral-400 hover:text-orange-300 transition-colors">
            Já possui conta? <span class="text-orange-400">Entrar</span>
          </RouterLink>
        </div>
      </div>
    </section>
  </main>
</template>

<script setup>
// nenhum estado/lógica por enquanto
</script>
