<script setup>
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useCategories } from '@/composables/useCategories'
import { useProducts } from '@/composables/useProducts'
import hardwareImage from '@/assets/images/hardwareimage.webp'
import perfericos from '@/assets/images/perfericos.webp'
import celulares from '@/assets/images/image.webp'
import computadores from '@/assets/images/computadores.webp'
import casaInteligente from '@/assets/images/casaInteligente.webp'

const { categories, loading, error, fetchCategories } = useCategories()
const { fetchProductsGridHomePage } = useProducts()
const images = {
  id: {
    1: hardwareImage,
    2: computadores,
    3: casaInteligente,
    4: celulares,
    5: perfericos,
  },
}
const router = useRouter()

onMounted(() => {
  fetchCategories()
})

async function goToCategory(category_id) {
  await fetchProductsGridHomePage(category_id)
  router.push(`/category/${category_id}`)
}
</script>

<template>
  <section class="py-8">
    <div class="container mx-auto px-4">
      <h2 class="text-xl md:text-2xl font-bold text-orange-500 uppercase tracking-wide border-b-2 border-orange-500/20 pb-1 mb-8">
        Navegue por Departamentos
      </h2>

      <div v-if="loading" class="text-center text-orange-200 animate-pulse">Carregando departamentos...</div>
      <div v-else-if="error" class="text-center text-red-400 bg-red-900/20 p-2 rounded">
        Erro ao carregar departamentos.
      </div>

      <div
        v-else-if="categories.length"
        class="w-full"
      >
        <div
          class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-6 justify-items-center"
        >
          <div
            v-for="category in categories"
            :key="category.id"
            @click="goToCategory(category.id)"
            class="group cursor-pointer flex flex-col items-center gap-3 w-full max-w-[160px]"
          >
            <div
              class="w-28 h-28 sm:w-32 sm:h-32 bg-white rounded-2xl shadow-lg flex items-center justify-center p-4 transition-all duration-300 group-hover:-translate-y-2 group-hover:shadow-2xl group-hover:shadow-orange-500/40 border-4 border-transparent group-hover:border-orange-500 relative overflow-hidden"
            >
              <div class="absolute inset-0 bg-orange-50 opacity-0 group-hover:opacity-100 transition-opacity duration-300"></div>

              <img
                :src="images.id[category.id]"
                :alt="category.name"
                class="w-full h-full object-contain relative z-10 transition-transform duration-300 group-hover:scale-110"
              />
            </div>

            <span
              class="text-sm md:text-base font-bold text-gray-400 group-hover:text-white transition-colors text-center uppercase tracking-wider"
            >
              {{ category.name }}
            </span>
          </div>
        </div>
      </div>

      <div v-else class="text-center text-neutral-400 py-10">
        Nenhum departamento encontrado.
      </div>
    </div>
  </section>
</template>