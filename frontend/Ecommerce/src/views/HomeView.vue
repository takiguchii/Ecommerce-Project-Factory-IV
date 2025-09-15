<template>
  <div class="min-h-screen bg-gray-100">
    
    <section class="relative w-full h-[320px] md:h-[420px] lg:h-[520px] overflow-hidden">
      <div class="absolute inset-0 bg-cover bg-center"
        style="background-image: url('https://img.freepik.com/fotos-gratis/luminaria-de-neon-para-vendas-de-sexta-feira-negra_23-2151833076.jpg?semt=ais_hybrid&w=1280');">
      </div>
      <div class="absolute inset-0 bg-gradient-to-r from-black/80 via-black/50 to-transparent"></div>

      <div class="relative h-full px-6 md:px-10 flex flex-col justify-center items-start">
        <h1 class="text-4xl md:text-6xl font-extrabold text-white">TechMart</h1>
        <p class="text-white/90 text-lg md:text-2xl italic mt-2">
          Aqui você encontra aquilo que busca.
        </p>
      </div>
    </section>

    <!-- PRODUTOS: estrutura base (placeholder) -->
    <section class="py-12 px-6 md:px-12">
      <h2 class="text-2xl md:text-3xl font-bold text-gray-800 text-center mb-8">Nossos Produtos</h2>

      <div class="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
        <!-- Card modelo (estático) -->
        <article class="bg-white rounded-2xl shadow hover:shadow-lg transition overflow-hidden relative">
          <div class="w-full h-44 bg-gray-200" />
          <div class="p-4 space-y-2">
            <h3 class="text-base font-semibold text-gray-800">Produto</h3>
            <p class="text-sm text-gray-600">Subtítulo do produto</p>
            <div class="flex items-center justify-between mt-2">
              <span class="text-blue-600 font-bold text-lg">R$ 0,00</span>
              <span class="text-xs text-gray-500">Estoque: 0</span>
            </div>
            <button class="w-full mt-3 rounded-xl px-4 py-2.5 text-sm font-semibold text-white bg-indigo-600">
              Adicionar ao carrinho
            </button>
          </div>
        </article>
      
      </div>
    </section>
  </div>
</template>

<script setup>

</script>

<style scoped>

</style>
