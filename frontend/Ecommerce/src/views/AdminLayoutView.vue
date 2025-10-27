<script setup>
import { RouterLink, RouterView } from 'vue-router'

const navLinkClass = "block py-2 px-3 rounded-md no-underline text-gray-300 font-medium hover:bg-gray-700 hover:text-white transition-colors duration-150 aria-[current=page]:text-white aria-[current=page]:bg-orange-600";
</script>

<template>
  <div class="flex min-h-screen bg-gray-900">
    
    <aside class="w-56 flex-shrink-0 bg-gray-800 p-4 pt-20 border-r border-gray-700">
      
      <h2 class="text-xl font-bold mb-5 px-2 text-gray-100">
        Painel Admin
      </h2>

      <nav>
        <ul class="list-none p-0 space-y-1">
          <li>
            <router-link to="/admin/brands" :class="navLinkClass">
              Marcas
            </router-link>
          </li>
          <li>
            <router-link to="/admin/products" :class="navLinkClass">
              Produtos
            </router-link>
          </li>
          <li>
            <router-link to="/admin/categories" :class="navLinkClass">
              Categorias
            </router-link>
          </li>
          <li>
            <router-link to="/admin/subcategories" :class="navLinkClass">
              Subcategorias
            </router-link>
          </li>
          <li>
            <router-link to="/admin/providers" :class="navLinkClass">
              Fornecedores
            </router-link>
          </li>
        </ul>
      </nav>
    </aside>
    
    <main class="flex-1">
      <router-view />
    </main>
    
  </div>
</template>
