<script setup>
import { onMounted } from 'vue';
import { RouterLink, useRouter } from 'vue-router';
import { useCategories } from '@/composables/useCategories';

const { categories, loading, error, fetchCategories } = useCategories();
const router = useRouter();

onMounted(() => {
  fetchCategories();
});

function goToCategory(categoryId) {
  router.push(`/category/${categoryId}`);
}
</script>
<template>
    <section class="py-12 bg-black">
        <div class="container mx-auto px-4">
            <h2 class="text-2xl md:text-3xl font-semibold text-orange-400 mb-8 text-center">
                Navegue por Departamentos
            </h2>

            <div v-if="loading" class="text-center text-orange-200">Carregando...</div>
            <div v-if="error" class="text-center text-red-400">Erro ao carregar departamentos.</div>

            <div v-if="!loading && categories.length" 
                     class="grid grid-cols-2 sm:grid-cols-4 lg:grid-cols-8 gap-4">
                
                <RouterLink v-for="category in categories" :key="category.id" 
                                        :to="`/category/${category.id}`"
                                        class="group flex flex-col items-center gap-2 p-4 rounded-lg hover:bg-neutral-800 transition-colors">
                    
                    <div class="w-24 h-24 rounded-full bg-neutral-800 group-hover:bg-neutral-700 border-2 border-transparent group-hover:border-orange-500 transition-all duration-300 flex items-center justify-center overflow-hidden">
                        <img :src="category.imageUrlCategory" :alt="category.name" 
                                 class="w-24 h-24 object-contain">
                    </div>

                    <span class="text-sm font-medium text-gray-300 group-hover:text-orange-400 transition-colors">{{ category.name }}</span>
                </RouterLink>

            </div>
        </div>
    </section>
</template>