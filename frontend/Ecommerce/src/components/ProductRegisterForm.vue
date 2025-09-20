<template>
  <div class="min-h-screen bg-gradient-to-br from-black via-zinc-900 to-zinc-800 flex items-center justify-center p-4">
    <section class="w-full max-w-md">
      <div class="bg-white/95 backdrop-blur shadow-xl rounded-2xl border border-black/5 p-6">
        <header>
          <h1 class="text-2xl font-bold text-zinc-900">Registrar Produto</h1>
          <p class="text-sm text-zinc-500 mt-1">Cadastre um item no catálogo com nome, descrição e valores.</p>
        </header>

        <form @submit.prevent="onSubmit" class="space-y-4 mt-4">
          <div>
            <label class="block text-sm font-medium text-zinc-700 mb-1">Nome do Produto</label>
            <input v-model="form.name" type="text" placeholder="Ex.: Mouse Gamer RGB"
              class="w-full rounded-lg border border-zinc-300 px-3 py-2 outline-none focus:ring-2 focus:ring-orange-500/70 focus:border-orange-500 transition" required/>
          </div>

          <div>
            <label class="block text-sm font-medium text-zinc-700 mb-1">Descrição</label>
            <textarea v-model="form.description" placeholder="Ex.: Sensor 16.000 DPI…"
              class="w-full rounded-lg border border-zinc-300 px-3 py-2 min-h-[110px] outline-none focus:ring-2 focus:ring-orange-500/70 focus:border-orange-500 transition" required></textarea>
          </div>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label class="block text-sm font-medium text-zinc-700 mb-1">Preço</label>
              <input v-model="form.price" type="number" step="0.01" placeholder="199.90"
                class="w-full rounded-lg border border-zinc-300 px-3 py-2 outline-none focus:ring-2 focus:ring-orange-500/70 focus:border-orange-500 transition" required/>
            </div>
            <div>
              <label class="block text-sm font-medium text-zinc-700 mb-1">Estoque</label>
              <input v-model="form.stock" type="number" min="0" placeholder="Ex.: 25"
                class="w-full rounded-lg border border-zinc-300 px-3 py-2 outline-none focus:ring-2 focus:ring-orange-500/70 focus:border-orange-500 transition" required/>
            </div>
          </div>

          <button type="submit" class="w-full rounded-lg py-2.5 font-semibold text-white bg-gradient-to-r from-orange-600 to-orange-500 hover:from-orange-500 hover:to-orange-400 shadow hover:shadow-md transition">
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
async function onSubmit(){ try{ await apiPost('/products', form); alert('Produto registrado com sucesso!'); form.name=''; form.description=''; form.price=''; form.stock='' }catch(e){ alert(`Erro ao registrar produto: ${e.message}`)}}
</script>

<style scoped>
</style>
