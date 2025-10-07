<script setup>
import { ref, onMounted } from 'vue';
import { RouterLink } from 'vue-router';
import { useCategories } from '@/composables/useCategories';

const isDepartmentsOpen = ref(false);
const hoveredCategoryId = ref(null);

const { categories, fetchCategories, subcategories, fetchSubCategories } = useCategories();

onMounted(async () => {
  await fetchCategories();
  await fetchSubCategories();
});

function toggleDepartments() {
  isDepartmentsOpen.value = !isDepartmentsOpen.value;
}

function showSubcategories(categoryId) {
  hoveredCategoryId.value = categoryId;
}

function hideSubcategories() {
  hoveredCategoryId.value = null;
}
</script>

<template>
  <nav class="bg-neutral-900 text-white shadow-md">
    <div class="container mx-auto px-4">
      <div class="flex items-center justify-between h-20">

        <div class="flex items-center gap-6">
          <RouterLink to="/" class="text-2xl font-bold text-orange-400">
            TechMart
          </RouterLink>

          <div class="relative hidden md:block">
            <button @click="toggleDepartments" class="flex items-center gap-2 bg-neutral-800 hover:bg-neutral-700 px-4 py-2 rounded-md transition-colors">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M3 5a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 10a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 15a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1z" clip-rule="evenodd" /></svg>
              <span class="font-semibold">Departamentos</span>
            </button>
            <transition name="fade">
              <div v-if="isDepartmentsOpen" class="absolute mt-2 w-56 rounded-md shadow-lg bg-neutral-800 ring-1 ring-black ring-opacity-5 z-10">
                <div class="py-1" role="menu" aria-orientation="vertical">
                  <div v-for="category in categories" :key="category.id" class="relative group">
                    <RouterLink
                      :to="`/category/${category.id}`"
                      class="block px-4 py-2 text-sm text-gray-300 hover:bg-neutral-700 hover:text-orange-400"
                      role="menuitem"n-600 
                      @mouseenter="showSubcategories(category.id)"
                      @mouseleave="hideSubcategories"
                    >
                      {{ category.name }}
                    </RouterLink>
                    <!-- Subcategorias -->
                    <div
                      v-if="hoveredCategoryId === category.id"
                      class="absolute left-full top-0 w-56 bg-neutral-900 shadow-lg rounded-md z-20"
                      @mouseenter="showSubcategories(category.id)"
                      @mouseleave="hideSubcategories"
                    >
                      <div v-for="sub in subcategories.filter(s => s.parentCategoryid === category.id)" :key="sub.id"
                        class="px-4 py-2 text-sm text-gray-300 hover:bg-neutral-700 hover:text-orange-400">
                        <RouterLink :to="`/subcategory/${sub.id}`">
                          {{ sub.name }}
                        </RouterLink>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </transition>
          </div>
        </div>
        <!--Botao de cupom-->
        <div>
          <button
            class="flex items-center justify-center bg-neutral-800 hover:bg-green-600 p-2 rounded-md shadow-md mx-4 transition-colors"
            title="Cupom de desconto"
          >
            <img
              width="28"
              height="28"
              src="https://img.icons8.com/color/48/ticket.png"
              alt="ticket"
              class="w-7 h-7"
            />
            <p class="mx-2 font-semibold" >Cupom</p>
          </button>
        </div>

        <div class="flex-1 flex justify-center px-8">
          <div class="relative w-full max-w-lg">
            <input type="text" placeholder="Busque seu produto..."
                   class="w-full bg-neutral-800 border border-neutral-700 rounded-full py-2 pl-10 pr-4 text-white focus:outline-none focus:ring-2 focus:ring-orange-500" />
            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <svg class="h-5 w-5 text-gray-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd" /></svg>
            </div>
          </div>
        </div>

        <div class="flex items-center gap-6">
          <RouterLink to="/login" class="flex items-center gap-2 text-gray-300 hover:text-orange-400 transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke-linejoin="round" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" /></svg>
            <span class="text-sm hidden lg:block">Login ou Cadastre-se</span>
          </RouterLink>

          <RouterLink to="/cart" class="relative flex items-center text-gray-300 hover:text-orange-400 transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-7 w-7" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke-linejoin="round" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" /></svg>
            <span class="absolute -top-2 -right-2 bg-orange-500 text-white text-xs font-bold rounded-full h-5 w-5 flex items-center justify-center">0</span>
          </RouterLink>
        </div>

      </div>
    </div>
  </nav>
</template>

<style scoped>
/* Estilos para a animação do dropdown */
.fade-enter-active, .fade-leave-active {
  transition: opacity 0.2s ease;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}
</style>