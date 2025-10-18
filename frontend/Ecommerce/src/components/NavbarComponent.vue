<script setup>
import { ref, onMounted } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { useCategories } from '@/composables/useCategories'

const router = useRouter()

const isDepartmentsOpen = ref(false)
const hoveredCategoryId = ref(null)
const searchTerm = ref('')

const { categories, fetchCategories, subcategories, fetchSubCategories } = useCategories()

onMounted(async () => {
  await fetchCategories()
  await fetchSubCategories()
})

function showDepartments() {
  isDepartmentsOpen.value = true
}
function hideDepartments() {
  isDepartmentsOpen.value = false
  hoveredCategoryId.value = null
}
function showSubcategories(categoryId) {
  hoveredCategoryId.value = categoryId
}
function hideSubcategories() {
  hoveredCategoryId.value = null
}

function submitSearch() {
  const q = searchTerm.value.trim()
  if (!q) return
  router.push({ name: 'search', query: { q } })
}
</script>

<template>
  <nav
    class="fixed inset-x-0 top-0 z-[60] bg-neutral-900/95 text-white shadow-lg backdrop-blur supports-backdrop-blur:bg-neutral-900/80"
  >
    <div class="container mx-auto max-w-7xl px-4">
      <div class="flex items-center justify-between h-20">

        <!-- LOGO -->
        <div class="flex items-center gap-6">
          <RouterLink
            to="/"
            class="text-2xl font-bold text-orange-400 hover:text-orange-300 tracking-tight transition-colors"
          >
            TechMart
          </RouterLink>

          <!-- DEPARTAMENTOS -->
          <div
            class="relative hidden md:block"
            @mouseenter="showDepartments"
            @mouseleave="hideDepartments"
          >
            <button
              class="inline-flex items-center gap-2 rounded-xl bg-neutral-800/80 px-4 py-2 ring-1 ring-neutral-700/60 hover:bg-neutral-700/80 hover:ring-neutral-600 transition-all duration-200 hover:-translate-y-0.5 focus:outline-none focus:ring-2 focus:ring-orange-500"
            >
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-200" viewBox="0 0 20 20" fill="currentColor">
                <path fill-rule="evenodd" d="M3 5a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 10a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 15a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1z" clip-rule="evenodd" />
              </svg>
              <span class="font-semibold">Departamentos</span>
            </button>

            <transition name="fade">
              <div
                v-if="isDepartmentsOpen"
                class="absolute mt-2 w-64 rounded-xl border border-neutral-700/40 bg-neutral-900/95 shadow-xl ring-1 ring-black/5 z-[70] backdrop-blur"
              >
                <div class="py-1" role="menu" aria-orientation="vertical">
                  <div v-for="category in categories" :key="category.id" class="relative group">
                    <RouterLink
                      :to="`/category/${category.id}`"
                      class="block px-3 py-2 text-sm text-gray-300 rounded-lg hover:bg-neutral-800/60 hover:text-orange-400 transition-colors"
                      role="menuitem"
                      @mouseenter="showSubcategories(category.id)"
                      @mouseleave="hideSubcategories"
                    >
                      {{ category.name }}
                    </RouterLink>

                    <!-- SUBCATEGORIAS -->
                    <div
                      v-if="hoveredCategoryId === category.id"
                      class="absolute left-full top-0 ml-2 w-64 rounded-xl border border-neutral-700/40 bg-neutral-900/95 shadow-xl ring-1 ring-black/5 z-[80]"
                      @mouseenter="showSubcategories(category.id)"
                      @mouseleave="hideSubcategories"
                    >
                      <div
                        v-for="sub in subcategories.filter(s => s.parentCategoryid === category.id)"
                        :key="sub.id"
                        class="px-3 py-2 text-sm text-gray-300 rounded-lg hover:bg-neutral-800/60 hover:text-orange-400 transition-colors"
                      >
                        <RouterLink :to="`/subcategory/${sub.id}`" class="block">
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

        <!-- BOTÃO CUPOM -->
        <div>
          <button
            class="flex items-center justify-center gap-2 bg-gradient-to-r from-neutral-800 to-neutral-800/70 hover:from-green-600 hover:to-green-600 px-3 py-2 rounded-xl shadow-md ring-4 ring-neutral-700/50 hover:ring-orange-500 transition-all duration-200 mx-4 hover:-translate-y-0.5"
            title="Cupom de desconto"
          >
            <img width="28" height="28" src="https://img.icons8.com/color/48/ticket.png" alt="ticket" class="w-7 h-7" />
            <p class="font-semibold">Cupom</p>
          </button>
        </div>

        <!-- BARRA DE BUSCA -->
        <div class="flex-1 flex justify-center px-4 sm:px-8">
          <form class="relative w-full max-w-lg" @submit.prevent="submitSearch">
            <input
              v-model="searchTerm"
              type="text"
              placeholder="Busque seu produto..."
              class="w-full bg-neutral-800/80 border border-neutral-700 rounded-full py-2 pl-11 pr-10 text-white placeholder-gray-400 focus:bg-neutral-900 focus:ring-2 focus:ring-orange-500 focus:border-orange-500 focus:outline-none transition-all"
            />
            <button
              type="submit"
              class="absolute inset-y-0 right-0 pr-3 flex items-center text-gray-300 hover:text-orange-400"
              title="Buscar"
            >
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="currentColor">
                <path d="M10 2a8 8 0 105.293 14.293l4.707 4.707 1.414-1.414-4.707-4.707A8 8 0 0010 2zm0 2a6 6 0 110 12A6 6 0 0110 4z"/>
              </svg>
            </button>
          </form>
        </div>

        <!-- LOGIN / CARRINHO -->
        <div class="flex items-center gap-2 sm:gap-4">
          <RouterLink
            to="/login"
            class="flex items-center gap-2 text-gray-300 hover:text-orange-400 rounded-lg px-3 py-2 hover:bg-neutral-800/60 transition-colors"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
              <path stroke-linecap="round" stroke-linejoin="round" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
            </svg>
            <span class="text-sm hidden lg:block">Login ou Cadastre-se</span>
          </RouterLink>

          <RouterLink
            to="/cart"
            class="relative flex items-center text-gray-300 hover:text-orange-400 rounded-lg px-2 py-2 hover:bg-neutral-800/60 transition-colors"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="h-7 w-7" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
              <path stroke-linecap="round" stroke-linejoin="round" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
            </svg>
            <span class="absolute -top-2 -right-2 bg-orange-500 text-white text-xs font-bold rounded-full h-5 w-5 flex items-center justify-center ring-2 ring-neutral-900">
              0
            </span>
          </RouterLink>
        </div>

      </div>
    </div>
  </nav>
</template>

<style scoped>
/* animação do dropdown */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.25s ease, transform 0.25s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: translateY(-4px);
}
</style>
