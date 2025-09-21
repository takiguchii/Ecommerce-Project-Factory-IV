<template>
  <!-- apenas o formulário -->
  <section class="w-full max-w-md">
    <div class="bg-white shadow-xl rounded-2xl border border-black/5 overflow-hidden">
      <header class="px-6 pt-6 pb-2">
        <h1 class="text-2xl font-bold text-zinc-900">Registrar Produto</h1>
        <p class="text-sm text-zinc-500 mt-1">
          Cadastre um item no catálogo com nome, descrição e valores.
        </p>
      </header>

      <!-- Banners -->
      <transition name="fade">
        <div
          v-if="banner.type === 'success'"
          class="mx-6 mt-4 rounded-lg border border-emerald-300 bg-emerald-50 text-emerald-800 px-4 py-3 text-sm"
        >
          ✅ {{ banner.message }}
        </div>
      </transition>
      <transition name="fade">
        <div
          v-if="banner.type === 'error'"
          class="mx-6 mt-4 rounded-lg border border-rose-300 bg-rose-50 text-rose-800 px-4 py-3 text-sm"
        >
          ⚠️ {{ banner.message }}
        </div>
      </transition>

      <div class="px-6 pb-6 pt-4">
        <form @submit.prevent="onSubmit" class="space-y-4">
          <!-- Nome -->
          <div>
            <label class="block text-sm font-medium text-zinc-700 mb-1">Nome do Produto</label>
            <div class="relative">
              <span
                class="pointer-events-none absolute inset-y-0 left-0 flex items-center pl-3"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4 text-zinc-400"
                  viewBox="0 0 24 24"
                  fill="currentColor"
                >
                  <path
                    d="M3 6h18v2H3V6zm0 5h18v2H3v-2zm0 5h18v2H3v-2z"
                  />
                </svg>
              </span>
              <input
                v-model="form.name"
                type="text"
                placeholder="Ex.: Mouse Gamer RGB"
                :class="inputClass(form.name)"
                required
              />
            </div>
            <p class="mt-1 text-xs text-zinc-500">Use um nome claro e fácil de buscar.</p>
          </div>

          <!-- Descrição -->
          <div>
            <label class="block text-sm font-medium text-zinc-700 mb-1">Descrição</label>
            <div class="relative">
              <span class="pointer-events-none absolute left-0 top-2.5 pl-3">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4 text-zinc-400"
                  viewBox="0 0 24 24"
                  fill="currentColor"
                >
                  <path
                    d="M4 4h16v2H4V4zm0 4h10v2H4V8zm0 4h16v2H4v-2zm0 4h10v2H4v-2z"
                  />
                </svg>
              </span>
              <textarea
                v-model="form.description"
                placeholder="Ex.: Sensor 16.000 DPI, 7 botões programáveis…"
                :class="textareaClass(form.description)"
                required
              ></textarea>
            </div>
            <p class="mt-1 text-xs text-zinc-500">
              Detalhe pontos fortes: material, desempenho, garantia.
            </p>
          </div>

          <!-- Grid preço/estoque -->
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label class="block text-sm font-medium text-zinc-700 mb-1">Preço</label>
              <div class="relative">
                <span
                  class="pointer-events-none absolute inset-y-0 left-0 flex items-center pl-3"
                >
                  <span class="text-zinc-400 text-xs">R$</span>
                </span>
                <input
                  v-model="form.price"
                  type="number"
                  step="0.01"
                  placeholder="199.90"
                  :class="inputClass(form.price)"
                  required
                />
              </div>
              <p class="mt-1 text-xs text-zinc-500">
                Apenas números, use ponto para decimais.
              </p>
            </div>

            <div>
              <label class="block text-sm font-medium text-zinc-700 mb-1">Estoque</label>
              <div class="relative">
                <span
                  class="pointer-events-none absolute inset-y-0 left-0 flex items-center pl-3"
                >
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    class="h-4 w-4 text-zinc-400"
                    viewBox="0 0 24 24"
                    fill="currentColor"
                  >
                    <path d="M3 3h18v2H3V3zm2 4h14v12H5V7zm2 2v8h10V9H7z" />
                  </svg>
                </span>
                <input
                  v-model="form.stock"
                  type="number"
                  min="0"
                  placeholder="Ex.: 25"
                  :class="inputClass(form.stock)"
                  required
                />
              </div>
              <p class="mt-1 text-xs text-zinc-500">
                Quantidade disponível para venda.
              </p>
            </div>
          </div>

          <button
            type="submit"
            :disabled="submitting"
            class="w-full rounded-lg py-2.5 mt-2 font-semibold text-white bg-gradient-to-r from-orange-600 to-orange-500 hover:from-orange-500 hover:to-orange-400 disabled:opacity-70 disabled:cursor-not-allowed active:from-orange-600 active:to-orange-500 shadow hover:shadow-md transition flex items-center justify-center gap-2"
          >
            <svg
              v-if="submitting"
              class="h-4 w-4 animate-spin"
              viewBox="0 0 24 24"
              fill="none"
            >
              <circle
                class="opacity-25"
                cx="12"
                cy="12"
                r="10"
                stroke="currentColor"
                stroke-width="4"
              />
              <path
                class="opacity-75"
                fill="currentColor"
                d="M4 12a8 8 0 018-8v4a4 4 0 00-4 4H4z"
              />
            </svg>
            <span>{{ submitting ? 'Registrando...' : 'Registrar Produto' }}</span>
          </button>
        </form>
      </div>
    </div>
  </section>
</template>

<script setup>
import { reactive, ref } from 'vue'
import { apiPost } from '../services/api'

const form = reactive({ name: '', description: '', price: '', stock: '' })
const submitting = ref(false)
const triedSubmit = ref(false)
const banner = reactive({ type: '', message: '' })

const baseInput =
  'w-full rounded-lg border px-3 py-2 outline-none transition pl-9 pr-3 ' +
  'focus:ring-2 focus:ring-orange-500/70 focus:border-orange-500'
const baseTextarea =
  'w-full rounded-lg border px-3 py-2 outline-none transition min-h-[110px] pl-9 pr-3 ' +
  'focus:ring-2 focus:ring-orange-500/70 focus:border-orange-500'

function inputClass(value) {
  return [
    baseInput,
    value ? 'border-zinc-300' : triedSubmit.value ? 'border-rose-400' : 'border-zinc-300'
  ].join(' ')
}
function textareaClass(value) {
  return [
    baseTextarea,
    value ? 'border-zinc-300' : triedSubmit.value ? 'border-rose-400' : 'border-zinc-300'
  ].join(' ')
}

async function onSubmit() {
  triedSubmit.value = true
  banner.type = ''
  banner.message = ''

  try {
    submitting.value = true
    await apiPost('/products', form)
    banner.type = 'success'
    banner.message = 'Produto registrado com sucesso!'
    form.name = ''
    form.description = ''
    form.price = ''
    form.stock = ''
    triedSubmit.value = false
  } catch (error) {
    banner.type = 'error'
    banner.message = `Erro ao registrar produto: ${error.message}`
  } finally {
    submitting.value = false
    setTimeout(() => {
      banner.type = ''
      banner.message = ''
    }, 4000)
  }
}
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 220ms ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
