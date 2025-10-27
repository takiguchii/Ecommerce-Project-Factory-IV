<template>
  <nav class="fixed inset-x-0 top-0 z-[60] bg-neutral-900/95 text-white shadow-lg backdrop-blur supports-backdrop-blur:bg-neutral-900/80 border-b border-neutral-800">
    <div class="container mx-auto max-w-7xl px-4">
      <div class="flex items-center justify-between h-20">
        <!-- LOGO -->
        <div class="flex items-center gap-6">
          <RouterLink to="/" class="text-2xl font-bold text-orange-400 hover:text-orange-300 tracking-tight transition-colors">
            TechMart
          </RouterLink>

          <!-- DEPARTAMENTOS -->
          <div class="relative hidden md:block" @mouseenter="showDepartments" @mouseleave="hideDepartments">
            <button
              class="inline-flex items-center gap-2 rounded-xl bg-neutral-800/80 px-4 py-2 ring-1 ring-neutral-700/60 hover:bg-neutral-700/80 hover:ring-neutral-600 transition-all duration-200 hover:-translate-y-0.5 focus:outline-none focus:ring-2 focus:ring-orange-500">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-200" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M3 5a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 10a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 15a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1z" clip-rule="evenodd" /></svg>
              <span class="font-semibold">Departamentos</span>
            </button>

            <transition name="fade">
              <div v-if="isDepartmentsOpen" class="absolute mt-2 w-64 rounded-xl border border-neutral-700/40 bg-neutral-900/95 shadow-xl ring-1 ring-black/5 z-[70] backdrop-blur">
                <div class="py-1" role="menu" aria-orientation="vertical">
                  <div v-for="category in categories" :key="category.id" class="relative group">
                    <RouterLink
                      :to="`/category/${category.id}`"
                      class="block px-3 py-2 text-sm text-gray-300 rounded-lg hover:bg-neutral-800/60 hover:text-orange-400 transition-colors"
                      role="menuitem"
                      @mouseenter="showSubcategories(category.id)"
                      @mouseleave="hideSubcategories">
                      {{ category.name }}
                    </RouterLink>

                    <!-- subcategorias (corrigido: usa category_id) -->
                    <div
                      v-if="hoveredCategoryId === category.id"
                      class="absolute left-full top-0 ml-2 w-64 rounded-xl border border-neutral-700/40 bg-neutral-900/95 shadow-xl ring-1 ring-black/5 z-[80]"
                      @mouseenter="showSubcategories(category.id)"
                      @mouseleave="hideSubcategories">
                      <div
                        v-for="sub in subcategories.filter(s => s.category_id === category.id)"
                        :key="sub.id"
                        class="px-3 py-2 text-sm text-gray-300 rounded-lg hover:bg-neutral-800/60 hover:text-orange-400 transition-colors">
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

        <!-- CUPOM -->
        <div>
          <button
            class="flex items-center justify-center gap-2 bg-gradient-to-r from-neutral-800 to-neutral-800/70 hover:from-green-600 hover:to-green-600 px-3 py-2 rounded-xl shadow-md ring-4 ring-neutral-700/50 hover:ring-orange-500 transition-all duration-200 mx-4 hover:-translate-y-0.5"
            title="Cupom de desconto">
            <img width="28" height="28" src="https://img.icons8.com/color/48/ticket.png" alt="ticket" class="w-7 h-7" />
            <p class="font-semibold hidden lg:block">Cupom</p>
          </button>
        </div>

        <!-- BUSCA -->
        <div class="flex-1 flex justify-center px-4 sm:px-8">
          <form ref="searchRoot" class="relative w-full max-w-lg" @submit.prevent="submitSearch">
            <input
              v-model="searchTerm"
              @input="onInput"
              @keydown="onKeydown"
              @focus="onFocus"
              @blur="onBlur"
              type="text"
              placeholder="Busque seu produto..."
              class="w-full bg-neutral-800/80 border border-neutral-700 rounded-full py-2 pl-11 pr-10 text-white placeholder-gray-400 focus:bg-neutral-900 focus:ring-2 focus:ring-orange-500 focus:border-orange-500 focus:outline-none transition-all"/>

            <button type="submit" class="absolute inset-y-0 right-0 pr-3 flex items-center text-gray-300 hover:text-orange-400" title="Buscar">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="currentColor"><path d="M10 2a8 8 0 105.293 14.293l4.707 4.707 1.414-1.414-4.707-4.707A8 8 0 0010 2zm0 2a6 6 0 110 12A6 6 0 0110 4z"/></svg>
            </button>

            <transition name="fade">
              <div
                v-if="showSuggestions"
                class="absolute mt-2 w-full rounded-xl border border-neutral-700/60 bg-neutral-900/95 shadow-2xl ring-1 ring-black/5 z-[80] overflow-hidden">
                <div v-if="loadingSuggestions" class="px-4 py-3 text-sm text-neutral-300">Buscando…</div>

                <ul v-else class="max-h-96 overflow-auto">
                  <li
                    v-for="(item, idx) in suggestions"
                    :key="item.id ?? item.code ?? idx"
                    class="flex items-center gap-3 px-3 py-2 cursor-pointer"
                    :class="idx === highlighted ? 'bg-neutral-800/80 text-orange-300' : 'hover:bg-neutral-800/60'"
                    @mouseenter="highlighted = idx"
                    @mouseleave="highlighted = -1"
                    @mousedown.prevent="goToSuggestion(item)">
                    <img
                      :src="item.coverImageUrl || 'https://placehold.co/32x32/7f7f7f/333333?text=N/A'"
                      :alt="item.name"
                      class="w-8 h-8 rounded object-cover border border-neutral-700"
                      onerror="this.src='https://placehold.co/32x32/7f7f7f/333333?text=N/A'"/>
                    <span class="text-sm text-gray-200 line-clamp-2">{{ item.name }}</span>
                  </li>

                  <li v-if="suggestions.length" class="border-t border-neutral-800">
                    <button
                      type="button"
                      class="w-full text-left px-3 py-2 text-sm text-orange-300 hover:bg-neutral-800/60"
                      @mousedown.prevent="submitSearch">
                      Ver todos os resultados para “{{ searchTerm }}”
                    </button>
                  </li>
                </ul>

                <div v-if="!loadingSuggestions && !suggestions.length && searchTerm.length > 1" class="px-4 py-3 text-sm text-neutral-300">
                  Sem resultados para "{{ searchTerm }}"
                </div>
              </div>
            </transition>
          </form>
        </div>

        <!-- LOGIN / CARRINHO -->
        <div class="flex items-center gap-2 sm:gap-4">
          <RouterLink
            v-if="!isLoggedIn"
            to="/login"
            class="flex items-center gap-2 text-gray-300 hover:text-orange-400 rounded-lg px-3 py-2 hover:bg-neutral-800/60 transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke-linejoin="round" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" /></svg>
            <span class="text-sm hidden lg:block">Login ou Cadastre-se</span>
          </RouterLink>

          <div v-else class="relative">
            <button
              @click="toggleUserMenu"
              class="flex items-center gap-2 text-gray-300 hover:text-orange-400 rounded-lg px-3 py-2 hover:bg-neutral-800/60 transition-colors focus:outline-none"
              id="user-menu-button" aria-expanded="isUserMenuOpen" aria-haspopup="true">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke-linejoin="round" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" /></svg>
              <span class="text-sm hidden lg:block">{{ user?.username || 'Usuário' }}</span>
              <svg class="ml-1 h-4 w-4 text-neutral-400 hidden lg:block" fill="currentColor" viewBox="0 0 20 20"><path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd"/></svg>
            </button>

            <transition name="fade">
              <div
                v-show="isUserMenuOpen"
                ref="userMenuDropdown"
                class="absolute right-0 mt-2 w-48 origin-top-right rounded-xl border border-neutral-700/40 bg-neutral-900/95 shadow-xl ring-1 ring-black/5 z-[70] backdrop-blur focus:outline-none"
                role="menu" aria-orientation="vertical" aria-labelledby="user-menu-button" tabindex="-1">
                <div class="py-1" role="none">
                  <a href="#" class="block px-4 py-2 text-sm text-gray-300 hover:bg-neutral-800/60 hover:text-orange-400 rounded-lg transition-colors" role="menuitem" tabindex="-1">Meu Perfil</a>
                  <RouterLink
                    v-if="user?.role === 'Admin'"
                    to="/admin/brands"
                    class="block px-4 py-2 text-sm text-gray-300 hover:bg-neutral-800/60 hover:text-orange-400 rounded-lg transition-colors"
                    role="menuitem" tabindex="-1" @click="isUserMenuOpen = false">
                    Painel Administrador
                  </RouterLink>
                  <button @click="handleLogout" class="w-full text-left block px-4 py-2 text-sm text-red-400 hover:bg-neutral-800/60 hover:text-red-300 rounded-lg transition-colors" role="menuitem" tabindex="-1">
                    Sair
                  </button>
                </div>
              </div>
            </transition>
          </div>

          <RouterLink to="/cart" class="relative flex items-center text-gray-300 hover:text-orange-400 rounded-lg px-2 py-2 hover:bg-neutral-800/60 transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-7 w-7" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke-linejoin="round" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" /></svg>
            <span class="absolute -top-2 -right-2 bg-orange-500 text-white text-xs font-bold rounded-full h-5 w-5 flex items-center justify-center ring-2 ring-neutral-900"></span>
          </RouterLink>
        </div>
      </div>
    </div>
  </nav>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount, watch } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { useAuth } from '@/composables/useAuth'
import { useCategories } from '@/composables/useCategories'
import { apiGet } from '@/services/api'

const router = useRouter()

/* --- auth --- */
const { isLoggedIn, user, logout } = useAuth()
const isUserMenuOpen = ref(false)
const userMenuDropdown = ref(null)
const toggleUserMenu = () => { isUserMenuOpen.value = !isUserMenuOpen.value }
const handleLogout = () => { isUserMenuOpen.value = false; logout() }

/* --- departamentos --- */
const isDepartmentsOpen = ref(false)
const hoveredCategoryId = ref(null)
let departmentTimeout = null
let subcategoryTimeout = null

const showDepartments = () => { clearTimeout(departmentTimeout); isDepartmentsOpen.value = true }
const hideDepartments = () => {
  departmentTimeout = setTimeout(() => { isDepartmentsOpen.value = false; hoveredCategoryId.value = null }, 200)
}
const showSubcategories = (categoryId) => { clearTimeout(subcategoryTimeout); clearTimeout(departmentTimeout); hoveredCategoryId.value = categoryId }
const hideSubcategories = () => {
  subcategoryTimeout = setTimeout(() => { hoveredCategoryId.value = null; hideDepartments() }, 200)
}

/* --- categorias --- */
const { categories, fetchCategories, subcategories, fetchSubCategories } = useCategories()

/* --- busca/sugestões --- */
const searchRoot = ref(null)
const searchTerm = ref('')
const suggestions = ref([])
const loadingSuggestions = ref(false)
const highlighted = ref(-1)
const showSuggestions = ref(false)

const KABUM_HOST = 'https://www.kabum.com.br'
const fixUrl = (u) => !u ? null : (u.startsWith('http') ? u : (u.startsWith('/') ? `${KABUM_HOST}${u}` : u))
const normalizeItem = (p = {}) => {
  const imgs = [p.coverImageUrl, p.imageUrl, p.image_url, p.image_url0, p.image_url1, p.image_url2, p.image_url3, p.image_url4]
    .map(fixUrl).filter(Boolean)
  return {
    id: p.id ?? p.code ?? p.productId,
    name: p.name ?? p.nome ?? '',
    coverImageUrl: imgs[0] || 'https://placehold.co/32x32/7f7f7f/333333?text=N/A'
  }
}

let debounceId = null
async function fetchSuggestions() {
  const term = searchTerm.value.trim()
  if (term.length < 2) {
    suggestions.value = []
    showSuggestions.value = false
    highlighted.value = -1
    return
  }
  loadingSuggestions.value = true
  highlighted.value = -1
  try {
    // endpoint corrigido (seguindo o projeto): /products/search-suggestions?query=
    const res = await apiGet(`/products/search-suggestions?query=${encodeURIComponent(term)}`)
    const arr = Array.isArray(res) ? res : []
    suggestions.value = arr.map(normalizeItem).slice(0, 10)
    showSuggestions.value = true
  } catch (e) {
    // fallback opcional: tenta rota antiga /products/suggestions?q=
    try {
      const res2 = await apiGet(`/products/suggestions?q=${encodeURIComponent(term)}`)
      const arr2 = Array.isArray(res2) ? res2 : []
      suggestions.value = arr2.map(normalizeItem).slice(0, 10)
      showSuggestions.value = suggestions.value.length > 0
    } catch {
      suggestions.value = []
      showSuggestions.value = false
    }
  } finally {
    loadingSuggestions.value = false
  }
}

function onInput() {
  clearTimeout(debounceId)
  debounceId = setTimeout(fetchSuggestions, 200)
}

function onKeydown(e) {
  if (!showSuggestions.value || !suggestions.value.length) {
    if (e.key === 'Enter') submitSearch()
    return
  }
  if (e.key === 'ArrowDown') {
    e.preventDefault()
    highlighted.value = (highlighted.value + 1) % suggestions.value.length
  } else if (e.key === 'ArrowUp') {
    e.preventDefault()
    highlighted.value = (highlighted.value - 1 + suggestions.value.length) % suggestions.value.length
  } else if (e.key === 'Enter') {
    e.preventDefault()
    if (highlighted.value !== -1) goToSuggestion(suggestions.value[highlighted.value])
    else submitSearch()
  } else if (e.key === 'Escape') {
    showSuggestions.value = false
    highlighted.value = -1
  }
}

function onFocus() {
  if (searchTerm.value.trim().length >= 2) {
    fetchSuggestions()
  }
}

function onBlur() {
  setTimeout(() => {
    if (searchRoot.value && !searchRoot.value.contains(document.activeElement)) {
      showSuggestions.value = false
      highlighted.value = -1
    }
  }, 120)
}

function goToSuggestion(suggestion) {
  if (!suggestion) return
  router.push(`/produto/${suggestion.id}`)
  clearSearchState()
}
function submitSearch() {
  const q = searchTerm.value.trim()
  if (!q) return
  router.push(`/search?q=${encodeURIComponent(q)}&page=1&size=16`)
  clearSearchState()
}
function clearSearchState() {
  searchTerm.value = ''
  suggestions.value = []
  showSuggestions.value = false
  highlighted.value = -1
  document.activeElement?.blur()
}

/* --- fechar dropdowns ao clicar fora --- */
function onDocPointerDown(e) {
  // busca
  if (searchRoot.value && !searchRoot.value.contains(e.target)) {
    showSuggestions.value = false
    highlighted.value = -1
  }
  // menu usuário
  const btn = document.getElementById('user-menu-button')
  if (isUserMenuOpen.value && btn && !btn.contains(e.target) && userMenuDropdown.value && !userMenuDropdown.value.contains(e.target)) {
    isUserMenuOpen.value = false
  }
}

const isMobileMenuOpen = ref(false)
onMounted(async () => {
  await fetchCategories()
  await fetchSubCategories()
  document.addEventListener('pointerdown', onDocPointerDown, { capture: true })
})
onBeforeUnmount(() => {
  document.removeEventListener('pointerdown', onDocPointerDown, { capture: true })
  clearTimeout(debounceId)
  clearTimeout(departmentTimeout)
  clearTimeout(subcategoryTimeout)
})
watch(() => router.currentRoute.value, () => {
  isMobileMenuOpen.value = false
  isUserMenuOpen.value = false
})
</script>

<style scoped>
.fade-enter-active, .fade-leave-active { transition: opacity .25s ease, transform .25s ease; }
.fade-enter-from, .fade-leave-to { opacity: 0; transform: translateY(-4px); }
</style>
