<script setup>
import { RouterLink, RouterView } from 'vue-router'

const navLinkClass = "block py-1.5 px-2 rounded-md no-underline text-gray-700 font-semibold hover:bg-gray-200 hover:text-gray-900 aria-[current=page]:text-blue-600 aria-[current=page]:bg-blue-50";
</script>

<template>
  <div class="flex min-h-screen bg-gray-50">
    
    <aside class="w-56 flex-shrink-0 bg-white p-4 border-r border-gray-200">
      
      <h2 class="text-xl font-bold mb-5 px-2 text-gray-900">
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
    
    <main class="flex-1 p-6">
      <router-view />
    </main>
    
  </div>
</template>

<style scoped>
</style>