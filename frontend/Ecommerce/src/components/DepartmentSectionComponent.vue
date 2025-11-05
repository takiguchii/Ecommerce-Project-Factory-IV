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
  <section class="py-12 bg-black">
    <div class="container mx-auto px-4">
      <h2 class="text-2xl md:text-3xl font-semibold text-orange-400 mb-8 text-center">
        Navegue por Departamentos
      </h2>

      <!-- Estados -->
      <div v-if="loading" class="text-center text-orange-200">Carregando...</div>
      <div v-else-if="error" class="text-center text-red-400">
        Erro ao carregar departamentos.
      </div>

      <!-- Grid Centralizado -->
      <div
        v-else-if="categories.length"
        class="mx-auto max-w-[1100px]"
      >
        <div
          class="grid [grid-template-columns:repeat(auto-fit,minmax(140px,1fr))] gap-6 place-items-center"
        >
          <div
            v-for="category in categories"
            :key="category.id"
            @click="goToCategory(category.id)"
            class="cursor-pointer group flex flex-col items-center gap-2 p-4 rounded-lg hover:bg-neutral-800 transition-colors"
          >
            <div
              class="w-24 h-24 rounded-full bg-neutral-800 group-hover:bg-neutral-700 border-2 border-transparent group-hover:border-orange-500 transition-all duration-300 flex items-center justify-center overflow-hidden"
            >
              <img
                :src="images.id[category.id]"
                :alt="category.name"
                class="w-24 h-24 object-contain"
              />
            </div>

            <span
              class="text-sm font-medium text-gray-300 group-hover:text-orange-400 transition-colors text-center"
            >
              {{ category.name }}
            </span>
          </div>
        </div>
      </div>

      <div v-else class="text-center text-neutral-400">
        Nenhum departamento encontrado.
      </div>
    </div>
  </section>
</template>
