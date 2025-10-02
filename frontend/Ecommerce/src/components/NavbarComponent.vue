<template>
  <nav class="bg-blue-600 text-white px-6 py-3 flex items-center justify-between">
    <!-- Left: Logo -->
    <div class="font-bold text-lg flex-1 text-left">
      EcomerceProject
    </div>
    <form class="flex-1 flex justify-center">
      <input type="text" placeholder="Pesquisar..." class="rounded px-2 py-1 text-black"/>
      <button type="submit" class="bg-white text-blue-600 rounded px-3 py-1 ml-2 hover:bg-gray-200">
        Buscar
      </button>
    </form>
    <ul class="flex gap-6 flex-1 justify-end">
      <li>
        <router-link to="/" class="hover:underline">Home</router-link>
      </li>
      <li>
        <router-link to="/sidebar" class="hover:underline">
          <img src="https://cdn-icons-png.flaticon.com/512/5358/5358649.png" alt="Sidebar" class="w-6 h-6 inline filter invert brightness-0 contrast-100"/>
        </router-link>
      </li>
    </ul>
  </nav>
</template>