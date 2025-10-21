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

const topBrands = computed(() => props.brands?.slice(0, 8) ?? [])
const placeholder =
  'data:image/svg+xml;utf8,' +
  encodeURIComponent('<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 400 225"><rect width="400" height="225" fill="#111"/><text x="50%" y="50%" dominant-baseline="middle" text-anchor="middle" fill="#666" font-size="18" font-family="sans-serif">Marca</text></svg>')
</script>

<template>
  <section>
    <h2 class="text-2xl md:text-3xl font-semibold text-orange-400 mb-4">
      {{ title }}
    </h2>

    <div v-if="loading" class="text-center text-orange-200">Carregando marcas…</div>
    <div v-else-if="error" class="text-center text-red-400">Erro ao carregar marcas.</div>

    <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
      <RouterLink
        v-for="brand in topBrands"
        :key="brand.id"
        :to="`${brandRouteBase}/${brand.id}`"
        class="group block rounded-xl bg-neutral-900/70 border border-neutral-800 shadow-lg hover:shadow-2xl hover:-translate-y-0.5 transition-all duration-300"
      >
        <div class="aspect-video w-full overflow-hidden rounded-t-xl bg-neutral-950 flex items-center justify-center">
          <img
            :src="brand.imageUrl || placeholder"
            :alt="brand.name"
            class="max-h-full max-w-full object-contain transition-transform duration-300 group-hover:scale-[1.02]"
            loading="lazy"
          />
        </div>

        <!-- rodapé -->
        <div class="px-4 py-3">
          <p class="text-xs text-neutral-300">{{ (brand.name || '').toUpperCase() }}</p>
          <span class="mt-1 inline-flex items-center gap-1 text-xs font-semibold text-orange-400 group-hover:text-orange-300 transition">
            VER PRODUTOS &gt;
          </span>
        </div>
      </RouterLink>
    </div>
  </section>
</template>
