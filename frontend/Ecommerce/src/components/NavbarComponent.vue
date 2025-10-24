<template>
  <nav class="fixed inset-x-0 top-0 z-[60] bg-neutral-900/95 text-white shadow-lg backdrop-blur supports-backdrop-blur:bg-neutral-900/80 border-b border-neutral-800">
    <div class="container mx-auto max-w-7xl px-4">
      <div class="flex items-center justify-between h-20">
        <!-- LOGO (Seu código original) -->
        <div class="flex items-center gap-6">
          <RouterLink to="/" class="text-2xl font-bold text-orange-400 hover:text-orange-300 tracking-tight transition-colors">
            TechMart
          </RouterLink>

          <!-- DEPARTAMENTOS -->
          <div class="relative hidden md:block" @mouseenter="showDepartments" @mouseleave="hideDepartments">
            <button class="inline-flex items-center gap-2 rounded-xl bg-neutral-800/80 px-4 py-2 ring-1 ring-neutral-700/60 hover:bg-neutral-700/80 hover:ring-neutral-600 transition-all duration-200 hover:-translate-y-0.5 focus:outline-none focus:ring-2 focus:ring-orange-500">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-200" viewBox="0 0 20 20" fill="currentColor"><path fill-rule="evenodd" d="M3 5a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 10a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 15a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1z" clip-rule="evenodd" /></svg>
              <span class="font-semibold">Departamentos</span>
            </button>
            <transition name="fade">
              <div v-if="isDepartmentsOpen" class="absolute mt-2 w-64 rounded-xl border border-neutral-700/40 bg-neutral-900/95 shadow-xl ring-1 ring-black/5 z-[70] backdrop-blur">
                <div class="py-1" role="menu" aria-orientation="vertical">
                  <div v-for="category in categories" :key="category.id" class="relative group">
                    <RouterLink :to="`/category/${category.id}`" class="block px-3 py-2 text-sm text-gray-300 rounded-lg hover:bg-neutral-800/60 hover:text-orange-400 transition-colors" role="menuitem" @mouseenter="showSubcategories(category.id)" @mouseleave="hideSubcategories">
                      {{ category.name }}
                    </RouterLink>
                    <div v-if="hoveredCategoryId === category.id" class="absolute left-full top-0 ml-2 w-64 rounded-xl border border-neutral-700/40 bg-neutral-900/95 shadow-xl ring-1 ring-black/5 z-[80]" @mouseenter="showSubcategories(category.id)" @mouseleave="hideSubcategories">
                      <div v-for="sub in subcategories.filter(s => s.parentCategoryid === category.id)" :key="sub.id" class="px-3 py-2 text-sm text-gray-300 rounded-lg hover:bg-neutral-800/60 hover:text-orange-400 transition-colors">
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
           <button class="flex items-center justify-center gap-2 bg-gradient-to-r from-neutral-800 to-neutral-800/70 hover:from-green-600 hover:to-green-600 px-3 py-2 rounded-xl shadow-md ring-4 ring-neutral-700/50 hover:ring-orange-500 transition-all duration-200 mx-4 hover:-translate-y-0.5" title="Cupom de desconto">
            <img width="28" height="28" src="https://img.icons8.com/color/48/ticket.png" alt="ticket" class="w-7 h-7" />
            <p class="font-semibold hidden lg:block">Cupom</p> <!-- Oculto em telas menores -->
          </button>
        </div>

        <!-- BARRA DE BUSCA -->
        <div class="flex-1 flex justify-center px-4 sm:px-8">
          <form ref="searchRoot" class="relative w-full max-w-lg" @submit.prevent="submitSearch">
            <input v-model="searchTerm" @input="onInput" @keydown="onKeydown" @focus="onFocus" @blur="onBlur" type="text" placeholder="Busque seu produto..." class="w-full bg-neutral-800/80 border border-neutral-700 rounded-full py-2 pl-11 pr-10 text-white placeholder-gray-400 focus:bg-neutral-900 focus:ring-2 focus:ring-orange-500 focus:border-orange-500 focus:outline-none transition-all"/>
            <button type="submit" class="absolute inset-y-0 right-0 pr-3 flex items-center text-gray-300 hover:text-orange-400" title="Buscar">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="currentColor"><path d="M10 2a8 8 0 105.293 14.293l4.707 4.707 1.414-1.414-4.707-4.707A8 8 0 0010 2zm0 2a6 6 0 110 12A6 6 0 0110 4z"/></svg>
            </button>
            <transition name="fade">
              <div v-if="showSuggestions" class="absolute mt-2 w-full rounded-xl border border-neutral-700/60 bg-neutral-900/95 shadow-2xl ring-1 ring-black/5 z-[80] overflow-hidden">
                <div v-if="loadingSuggestions" class="px-4 py-3 text-sm text-neutral-300">Buscando…</div>
                <ul v-else class="max-h-96 overflow-auto">
                  <li v-for="(item, idx) in suggestions" :key="item.id" class="flex items-center gap-3 px-3 py-2 cursor-pointer" :class="idx === highlighted ? 'bg-neutral-800/80 text-orange-300' : 'hover:bg-neutral-800/60'" @mouseenter="highlighted = idx" @mouseleave="highlighted = -1" @mousedown.prevent="goToSuggestion(item)">
                    <img :src="item.coverImageUrl || 'https://placehold.co/32x32/7f7f7f/333333?text=N/A'" :alt="item.name" class="w-8 h-8 rounded object-cover border border-neutral-700" onerror="this.src='https://placehold.co/32x32/7f7f7f/333333?text=N/A'"/>
                    <span class="text-sm text-gray-200 line-clamp-2">{{ item.name }}</span>
                  </li>
                  <li v-if="suggestions.length" class="border-t border-neutral-800">
                    <button type="button" class="w-full text-left px-3 py-2 text-sm text-orange-300 hover:bg-neutral-800/60" @mousedown.prevent="submitSearch">
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

        <!-- === LOGIN / CARRINHO  -->
        <div class="flex items-center gap-2 sm:gap-4">
          <!-- SE NÃO LOGADO: Mostra o link original -->
          <RouterLink v-if="!isLoggedIn" to="/login" class="flex items-center gap-2 text-gray-300 hover:text-orange-400 rounded-lg px-3 py-2 hover:bg-neutral-800/60 transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke-linejoin="round" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" /></svg>
            <span class="text-sm hidden lg:block">Login ou Cadastre-se</span>
          </RouterLink>

          <!-- SE LOGADO: Mostra o menu dropdown -->
          <div v-else class="relative">
            <!-- Botão com Ícone e Nome -->
            <button @click="toggleUserMenu" class="flex items-center gap-2 text-gray-300 hover:text-orange-400 rounded-lg px-3 py-2 hover:bg-neutral-800/60 transition-colors focus:outline-none" id="user-menu-button" aria-expanded="isUserMenuOpen" aria-haspopup="true">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke-linejoin="round" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" /></svg>
              <span class="text-sm hidden lg:block">{{ user?.username || 'Usuário' }}</span>
              <!-- Seta opcional -->
               <svg class="ml-1 h-4 w-4 text-neutral-400 hidden lg:block" fill="currentColor" viewBox="0 0 20 20"><path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd"></path></svg>
            </button>
            <!-- Dropdown Menu -->
            <transition name="fade">
              <div v-show="isUserMenuOpen" ref="userMenuDropdown" class="absolute right-0 mt-2 w-48 origin-top-right rounded-xl border border-neutral-700/40 bg-neutral-900/95 shadow-xl ring-1 ring-black/5 z-[70] backdrop-blur focus:outline-none" role="menu" aria-orientation="vertical" aria-labelledby="user-menu-button" tabindex="-1">
                <div class="py-1" role="none">
                  <a href="#" class="block px-4 py-2 text-sm text-gray-300 hover:bg-neutral-800/60 hover:text-orange-400 rounded-lg transition-colors" role="menuitem" tabindex="-1">Meu Perfil</a>
                  <RouterLink v-if="user?.role === 'Admin'" to="/admin/brands" class="block px-4 py-2 text-sm text-gray-300 hover:bg-neutral-800/60 hover:text-orange-400 rounded-lg transition-colors" role="menuitem" tabindex="-1" @click="isUserMenuOpen = false">Gerenciar Marcas</RouterLink>
                  <button @click="handleLogout" class="w-full text-left block px-4 py-2 text-sm text-red-400 hover:bg-neutral-800/60 hover:text-red-300 rounded-lg transition-colors" role="menuitem" tabindex="-1">
                    Sair
                  </button>
                </div>
              </div>
            </transition>
          </div>

          <RouterLink to="/cart" class="relative flex items-center text-gray-300 hover:text-orange-400 rounded-lg px-2 py-2 hover:bg-neutral-800/60 transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-7 w-7" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke-linejoin="round" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" /></svg>
            <span class="absolute -top-2 -right-2 bg-orange-500 text-white text-xs font-bold rounded-full h-5 w-5 flex items-center justify-center ring-2 ring-neutral-900">
            </span>
          </RouterLink>
        </div>
      </div>
    </div>
  </nav>
  <!--Teste-->
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount, watch } from 'vue';
import { RouterLink, useRouter } from 'vue-router';
import { useAuth } from '@/composables/useAuth';
import { useCategories } from '@/composables/useCategories';
import { apiGet } from '@/services/api'; 

const router = useRouter();

const { isLoggedIn, user, logout } = useAuth();
const isUserMenuOpen = ref(false);
const userMenuDropdown = ref(null); 

const isDepartmentsOpen = ref(false);
const hoveredCategoryId = ref(null);
const searchTerm = ref('');
const suggestions = ref([]);
const showSuggestions = ref(false);
const loadingSuggestions = ref(false);
const highlighted = ref(-1);
let debounceId = null;
const searchRoot = ref(null);
const isMobileMenuOpen = ref(false); 

const toggleUserMenu = () => {
  isUserMenuOpen.value = !isUserMenuOpen.value;
};

const handleLogout = () => {
  isUserMenuOpen.value = false; 
  logout();
};

const handleLogoutMobile = () => {
   isMobileMenuOpen.value = false;
   logout();
};


let departmentTimeout = null;
const showDepartments = () => {
  clearTimeout(departmentTimeout);
  isDepartmentsOpen.value = true;
};
const hideDepartments = () => {
  departmentTimeout = setTimeout(() => {
    isDepartmentsOpen.value = false;
    hoveredCategoryId.value = null;
  }, 200); 
};

let subcategoryTimeout = null;
const showSubcategories = (categoryId) => {
   clearTimeout(subcategoryTimeout);
   clearTimeout(departmentTimeout); 
   hoveredCategoryId.value = categoryId;
};
const hideSubcategories = () => {
   subcategoryTimeout = setTimeout(() => {
       hoveredCategoryId.value = null;
       hideDepartments(); 
   }, 200);
};


function onDocClick(e) {
  // Fecha dropdown de busca
  if (searchRoot.value && !searchRoot.value.contains(e.target)) {
    showSuggestions.value = false;
    highlighted.value = -1;
  }
  const userMenuButton = document.getElementById('user-menu-button');
   if (isUserMenuOpen.value && userMenuButton && !userMenuButton.contains(e.target)) {
       if (userMenuDropdown.value && !userMenuDropdown.value.contains(e.target)) {
          isUserMenuOpen.value = false;
       }
   }
}

const { categories, fetchCategories, subcategories, fetchSubCategories } = useCategories();

async function fetchSuggestions() {
  if (!searchTerm.value || searchTerm.value.length < 2) {
    suggestions.value = [];
    showSuggestions.value = false;
    return;
  }
  loadingSuggestions.value = true;
  highlighted.value = -1;
  try {
    const results = await apiGet(`/products/suggestions?q=${encodeURIComponent(searchTerm.value)}`);
     suggestions.value = (results || []).map(item => ({
        ...item,
        coverImageUrl: item.coverImageUrl || 'https://placehold.co/32x32/7f7f7f/333333?text=N/A'
    }));
    showSuggestions.value = suggestions.value.length > 0;
  } catch (error) {
    console.error('Erro ao buscar sugestões:', error);
    suggestions.value = [];
    showSuggestions.value = false;
  } finally {
    loadingSuggestions.value = false;
  }
}

function onInput() {
  clearTimeout(debounceId);
  debounceId = setTimeout(fetchSuggestions, 300);
}

function onKeydown(e) {
    if (!showSuggestions.value || suggestions.value.length === 0) {
        if (e.key === 'Enter') {
            submitSearch(); // Permite submeter busca mesmo sem sugestões
        }
        return;
    }
    switch (e.key) {
        case 'ArrowDown':
            e.preventDefault();
            highlighted.value = (highlighted.value + 1) % suggestions.value.length;
            break;
        case 'ArrowUp':
            e.preventDefault();
            highlighted.value = (highlighted.value - 1 + suggestions.value.length) % suggestions.value.length;
            break;
        case 'Enter':
             e.preventDefault();
            if (highlighted.value !== -1) {
                goToSuggestion(suggestions.value[highlighted.value]);
            } else {
                submitSearch(); // Submete a busca se Enter for pressionado sem highlight
            }
            break;
        case 'Escape':
            showSuggestions.value = false;
            highlighted.value = -1;
            break;
    }
}

function onFocus() {
    if (searchTerm.value.length >= 2 && suggestions.value.length > 0) {
        showSuggestions.value = true;
    }
}
function onBlur() {
    setTimeout(() => {
        if (!searchRoot.value.contains(document.activeElement)) { // Só fecha se o foco saiu da área de busca
             showSuggestions.value = false;
             highlighted.value = -1;
        }
    }, 150); // Delay pequeno
}

function goToSuggestion(suggestion) {
   if (!suggestion) return;
   router.push(`/produto/${suggestion.id}`); 
   clearSearchState();
}

function submitSearch() {
    if(!searchTerm.value) return;
    router.push(`/search?q=${encodeURIComponent(searchTerm.value)}`);
    clearSearchState();
}

function clearSearchState() {
    searchTerm.value = '';
    suggestions.value = [];
    showSuggestions.value = false;
    highlighted.value = -1;
    // Tira o foco do input (opcional)
    document.activeElement?.blur();
}


onMounted(async () => {
  await fetchCategories();
  await fetchSubCategories();
  document.addEventListener('click', onDocClick);
});

onBeforeUnmount(() => {
  document.removeEventListener('click', onDocClick);
  clearTimeout(debounceId);
  clearTimeout(departmentTimeout); // Limpa timeouts
  clearTimeout(subcategoryTimeout);
});

// Watch para fechar menus mobile/desktop na navegação (mantido)
watch(() => router.currentRoute.value, () => {
  isMobileMenuOpen.value = false;
  isUserMenuOpen.value = false; // Fecha menu usuário também
});

</script>

<style scoped>
/* Suas classes de transição 'fade' originais */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.25s ease, transform 0.25s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: translateY(-4px);
}
/* Estilos Tailwind implícitos no template */
</style>