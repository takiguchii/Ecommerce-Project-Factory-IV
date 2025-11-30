<script setup>
import { RouterLink, RouterView, useRouter } from 'vue-router'

const router = useRouter()

const goHome = () => {
  router.push('/')
}

const navLinkClass =
  'block py-2 px-3 rounded-md no-underline text-gray-300 font-medium hover:bg-gray-700 hover:text-white transition-colors duration-150 aria-[current=page]:text-white aria-[current=page]:bg-orange-600'
</script>

<template>
  <div class="flex min-h-screen bg-gray-900">
    <aside
      class="w-56 flex-shrink-0 bg-gray-800 p-4 pt-10 border-r border-gray-700
             flex flex-col gap-6"
    >

      <!-- BOTÃO VOLTAR PARA HOME -->
      <button
        @click="goHome"
        class="w-full py-2 bg-orange-600 text-white font-medium rounded-md shadow-sm
               hover:bg-orange-700 transition-colors duration-200"
      >
        Ir para Home
      </button>

      <!-- TÍTULO DO PAINEL -->
      <h2 class="text-xl font-bold px-1 text-gray-100 mt-2">
        Painel Admin
      </h2>

      <!-- MENU -->
      <nav class="mt-2">
        <ul class="list-none p-0 space-y-1">
          <li>
            <RouterLink to="/admin/brands" :class="navLinkClass">
              Marcas
            </RouterLink>
          </li>
          <li>
            <RouterLink to="/admin/products" :class="navLinkClass">
              Produtos
            </RouterLink>
          </li>
          <li>
            <RouterLink to="/admin/categories" :class="navLinkClass">
              Categorias
            </RouterLink>
          </li>
          <li>
            <RouterLink to="/admin/subcategories" :class="navLinkClass">
              Subcategorias
            </RouterLink>
          </li>
          <li>
            <RouterLink to="/admin/providers" :class="navLinkClass">
              Fornecedores
            </RouterLink>
          </li>
          <li>
            <RouterLink to="/admin/Coupon" :class="navLinkClass">
              Cupom
            </RouterLink>
          </li>
          <li>
            <RouterLink to="/admin/orders" :class="navLinkClass">
              Pedidos
            </RouterLink>
          </li>
        </ul>
      </nav>
    </aside>

    <main class="flex-1">
      <RouterView />
    </main>
  </div>
</template>
