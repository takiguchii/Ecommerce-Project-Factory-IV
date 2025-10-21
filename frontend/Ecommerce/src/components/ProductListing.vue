<script setup>
import { computed } from 'vue'
import ProductCardComponent from '@/components/ProductCardComponent.vue'

const props = defineProps({
  items: { type: Array, default: () => [] },
  loading: { type: Boolean, default: false },
  error: { type: [String, null], default: null },
  pageNumber: { type: Number, default: 1 },
  pageSize: { type: Number, default: 16 },
  totalPages: { type: Number, default: 1 },
  totalCount: { type: Number, default: 0 },
  showCount: { type: Boolean, default: true },
  pageSizeOptions: {
    type: Array,
    default: () => [8, 12, 16, 24],
  },
})

const emits = defineEmits([
  'update:pageNumber',
  'update:pageSize',
  'addToCart',
])

function goToPage(p) {
  if (p < 1 || p > props.totalPages || p === props.pageNumber) return
  emits('update:pageNumber', p)
}

function changePageSize(val) {
  const n = Number(val)
  if (!Number.isFinite(n) || n <= 0) return
  emits('update:pageSize', n)
  emits('update:pageNumber', 1)
}

const showingRange = computed(() => {
  const start = (props.pageNumber - 1) * props.pageSize + 1
  const end = Math.min(props.pageNumber * props.pageSize, props.totalCount || (props.pageNumber * props.pageSize))
  return { start, end }
})
</script>

<template>
  <div class="w-full">
    <!-- header -->
    <slot name="header" />

    <!-- Top bar -->
    <div class="flex flex-wrap items-center justify-between gap-4 mb-6">
      <div class="text-sm text-orange-200">
        <template v-if="showCount && totalCount">
          Mostrando {{ showingRange.start }}–{{ showingRange.end }} de {{ totalCount }}
        </template>
        <slot name="top-left" />
      </div>

      <div class="flex items-center gap-3">
        <slot name="top-right" />
        <label class="text-sm text-neutral-300">Itens por página</label>
        <select
          :value="pageSize"
          @change="changePageSize($event.target.value)"
          class="bg-neutral-900 border border-neutral-700 rounded px-3 py-2 text-sm"
        >
          <option v-for="opt in pageSizeOptions" :key="`opt-${opt}`" :value="opt">{{ opt }}</option>
        </select>
      </div>
    </div>

    <!-- Estados -->
    <div v-if="loading" class="text-orange-200">Carregando...</div>
    <div v-else-if="error && !items?.length" class="text-red-400">{{ error }}</div>

    <!-- Grid -->
    <div v-else>
      <div v-if="items && items.length" class="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-4 gap-4">
        <ProductCardComponent
          v-for="p in items"
          :key="p.id ?? p.code"
          :product="p"
          @addToCart="$emit('addToCart', p)"
        />
      </div>

      <div v-else class="text-neutral-300">
        <slot name="empty">Nenhum produto encontrado.</slot>
      </div>

      <!-- Paginação -->
      <div v-if="totalPages > 1" class="mt-8 flex flex-wrap items-center justify-center gap-2">
        <button
          class="px-3 py-2 rounded bg-neutral-900 border border-neutral-700 text-sm disabled:opacity-50"
          :disabled="pageNumber === 1"
          @click="goToPage(pageNumber - 1)"
        >
          Anterior
        </button>

        <button
          v-for="p in Math.min(totalPages, 7)"
          :key="`p-${p}`"
          class="px-3 py-2 rounded border text-sm"
          :class="p === pageNumber ? 'bg-orange-500 border-orange-500 text-black' : 'bg-neutral-900 border-neutral-700'"
          @click="goToPage(p)"
        >
          {{ p }}
        </button>

        <button
          class="px-3 py-2 rounded bg-neutral-900 border border-neutral-700 text-sm disabled:opacity-50"
          :disabled="pageNumber === totalPages"
          @click="goToPage(pageNumber + 1)"
        >
          Próxima
        </button>
      </div>
    </div>
  </div>
</template>
