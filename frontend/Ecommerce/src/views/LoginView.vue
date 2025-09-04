<template>
  <main class="min-h-screen bg-gray-100 flex items-center justify-center p-4">
    <section class="w-full max-w-sm">
      <div class="bg-white shadow rounded-xl p-6">
        <h1 class="text-2xl font-semibold mb-4">Login / Cadastro</h1>

        <form>
          <div class="mb-3">
            <label class="block text-sm mb-1">Nome</label>
            <input type="text" placeholder="Seu nome" class="w-full border rounded px-3 py-2"/>
          </div>

          <div class="mb-3">
            <label class="block text-sm mb-1">E-mail</label>
            <input type="email" placeholder="seu@email.com" class="w-full border rounded px-3 py-2"/>
          </div>

          <div class="mb-3">
            <label class="block text-sm mb-1">Senha</label>
            <input type="password" placeholder="********" class="w-full border rounded px-3 py-2"/>
          </div>

          <div class="mb-3">
            <label class="block text-sm mb-1">Confirmar Senha</label>
            <input type="password" placeholder="********" class="w-full border rounded px-3 py-2"/>
          </div>

          <button type="submit" class="w-full bg-gray-900 text-white rounded py-2 mt-2">
            Entrar
          </button>
        </form>
      </div>
    </section>
  </main>
</template>

<script setup>
</script>
