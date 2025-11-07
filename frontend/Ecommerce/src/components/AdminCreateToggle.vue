<script setup>
import { computed } from 'vue'

const props = defineProps({
  modelValue: { type: Boolean, default: false },

  entity: { type: String, default: 'Cadastro' },

  forceOpen: { type: Boolean, default: false },

  openLabel: { type: String, default: 'Adicionar' },
  closeLabel: { type: String, default: 'Fechar' },

  icon: { type: String, default: '' },
})

const emit = defineEmits(['update:modelValue', 'open', 'close'])

const isOpen = computed(() => props.forceOpen || props.modelValue)

function toggle() {
  if (props.forceOpen) return
  const next = !props.modelValue
  emit('update:modelValue', next)
  next ? emit('open') : emit('close')
}
</script>

<template>
  <button
    v-if="!forceOpen"
    type="button"
    @click="toggle"
    :aria-expanded="isOpen"
    class="inline-flex items-center gap-2 rounded-md bg-blue-600 px-4 py-2 text-white
           font-medium shadow-sm hover:bg-green-500 transition
           focus:outline-none focus:ring-2 "
  >
    <span class="text-sm/none">{{ icon }}</span>
    <span class="whitespace-nowrap">
      {{ isOpen ? `${closeLabel} ${entity}` : `${openLabel} ${entity}` }}
    </span>
  </button>
</template>
