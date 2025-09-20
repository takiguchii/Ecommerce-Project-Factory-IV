<template>
  <div class="min-h-screen bg-gray-100 flex items-center justify-center p-4">
    <section class="w-full max-w-md">
      <div class="bg-white shadow rounded-xl p-6">
        <header>
          <h1 class="text-2xl font-bold text-zinc-900">Registrar Produto</h1>
          <p class="text-sm text-zinc-500 mt-1">Cadastre um item no catálogo.</p>
        </header>

        <form @submit.prevent="onSubmit" class="space-y-4 mt-4">
          <div>
            <label class="block text-sm font-medium text-zinc-700 mb-1">Nome do Produto</label>
            <input v-model="form.name" type="text" class="w-full rounded-lg border border-zinc-300 px-3 py-2" required/>
          </div>

          <div>
            <label class="block text-sm font-medium text-zinc-700 mb-1">Descrição</label>
            <textarea v-model="form.description" class="w-full rounded-lg border border-zinc-300 px-3 py-2 min-h-[110px]" required></textarea>
          </div>

          <div>
            <label class="block text-sm font-medium text-zinc-700 mb-1">Preço</label>
            <input v-model="form.price" type="number" step="0.01" class="w-full rounded-lg border border-zinc-300 px-3 py-2" required/>
          </div>

          <div>
            <label class="block text-sm font-medium text-zinc-700 mb-1">Estoque</label>
            <input v-model="form.stock" type="number" min="0" class="w-full rounded-lg border border-zinc-300 px-3 py-2" required/>
          </div>

          <button type="submit" class="w-full rounded-lg py-2.5 font-semibold text-white bg-zinc-900">
            Registrar Produto
          </button>
        </form>
      </div>
    </section>
  </div>
</template>

<script setup>
import { reactive } from 'vue'
import { apiPost } from '../services/api'

const form = reactive({ name:'', description:'', price:'', stock:'' })

async function onSubmit() {
  try {
    await apiPost('/products', form)
    alert('Produto registrado com sucesso!')
    form.name=''; form.description=''; form.price=''; form.stock=''
  } catch (e) {
    alert(`Erro ao registrar produto: ${e.message}`)
  }
}
</script>

<style scoped>
</style>
