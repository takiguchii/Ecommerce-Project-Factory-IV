<script setup>
import { onMounted, computed } from 'vue'
import { RouterLink } from 'vue-router'

const props = defineProps({
  title: { type: String, default: 'Navegue por Marcas' },
  brands: { type: Array, default: () => [] },
  loading: Boolean,
  error: [String, Object],
  brandRouteBase: { type: String, default: '/brand' },
})
const emit = defineEmits(['fetch-needed'])
onMounted(() => emit('fetch-needed'))

const MAX_DISPLAY_SLOTS = 12; // Define o número máximo de slots na grade (ex: 2 linhas de 6)

const displayedBrands = computed(() => props.brands?.slice(0, MAX_DISPLAY_SLOTS) ?? [])

// Lógica para preencher os buracos da grade com blocos vazios
const placeholders = computed(() => {
  const count = displayedBrands.value.length
  // Calcula o número de placeholders necessários para preencher o total de slots
  const remaining = MAX_DISPLAY_SLOTS > count ? MAX_DISPLAY_SLOTS - count : 0
  return Array(remaining).fill(null) // Array de N elementos vazios
})

const placeholderSvg =
  'data:image/svg+xml;utf8,' +
  encodeURIComponent('<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 400 225"><rect width="400" height="225" fill="#18181b"/><text x="50%" y="50%" dominant-baseline="middle" text-anchor="middle" fill="#3f3f46" font-size="18" font-family="sans-serif">Espaço Reservado</text></svg>')
</script>

<template>
  <section class="mt-8">
    <h2 class="text-xl md:text-2xl font-bold text-orange-500 uppercase tracking-wide border-b-2 border-orange-500/20 pb-1 mb-8">
      {{ title }}
    </h2>

    <div v-if="loading" class="text-center text-orange-200">Carregando marcas…</div>
    <div v-else-if="error" class="text-center text-red-400 bg-red-900/20 p-2 rounded">
      Erro ao carregar marcas.
    </div>

    <div v-else class="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 2xl:grid-cols-8 -mx-4 sm:-mx-6">
      <RouterLink
        v-for="brand in displayedBrands"
        :key="brand.id"
        :to="`${brandRouteBase}/${brand.id}`"
        class="group block rounded-xl bg-neutral-900/70 shadow-lg border border-neutral-800 transition-all duration-300 relative overflow-hidden
               hover:shadow-orange-500/30 hover:border-orange-500/50 hover:bg-neutral-800/80 hover:-translate-y-1"
      >
        <div class="aspect-[4/3] w-full overflow-hidden bg-white/5 flex items-center justify-center p-4">
          <img
            :src="brand.imageUrl || placeholderSvg"
            :alt="brand.name"
            class="max-h-full max-w-full object-contain transition-transform duration-300 group-hover:scale-[1.05]"
            loading="lazy"
          />
        </div>

        <div class="px-3 py-2 text-center">
          <p class="text-xs font-semibold text-neutral-300 group-hover:text-white transition-colors">
            {{ (brand.name || '').toUpperCase() }}
          </p>
          <span class="mt-1 inline-flex items-center gap-1 text-[10px] font-bold text-orange-400 group-hover:text-orange-300 transition-colors">
            VER OFERTAS
          </span>
        </div>
      </RouterLink>

      <div 
        v-for="(placeholder, index) in placeholders"
        :key="`placeholder-${index}`"
        class="block rounded-xl bg-neutral-900/50 border border-neutral-800 relative overflow-hidden flex flex-col items-center justify-center aspect-[4/3] w-full cursor-default"
      >
        <svg xmlns="http://www.w3.org/2000/svg" class="w-1/4 h-1/4 text-neutral-700" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" d="M9 13h6m-3-3v6m-9 1V7a2 2 0 012-2h6l2 2h6a2 2 0 012 2v8a2 2 0 01-2 2H5a2 2 0 01-2-2z" />
        </svg>
        <span class="text-xs text-neutral-600 mt-2">Próxima Marca</span>
      </div>
    </div>
  </section>
</template>