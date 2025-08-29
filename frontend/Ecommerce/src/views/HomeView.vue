<template>
  <div class="min-h-screen bg-gray-100">
    <!-- HERO -->
    <section class="relative w-full h-[320px] md:h-[420px] lg:h-[520px] overflow-hidden">
      <!-- imagem de fundo -->
      <div class="absolute inset-0 bg-cover bg-center"
        style="background-image: url('https://img.freepik.com/fotos-gratis/luminaria-de-neon-para-vendas-de-sexta-feira-negra_23-2151833076.jpg?semt=ais_hybrid&w=1280');">
      </div>
      <!-- overlay com degradê (esquerda -> transparente) -->
      <div class="absolute inset-0 bg-gradient-to-r from-black/80 via-black/50 to-transparent"></div>

      <!-- texto alinhado à esquerda dentro de um container -->
      <div class="relative h-full px-6 md:px-10 flex flex-col justify-center items-start">
        <h1 class="text-4xl md:text-6xl font-extrabold text-white">TechMart</h1>
        <p class="text-white/90 text-lg md:text-2xl italic mt-2">
          Aqui você encontra aquilo que busca.
        </p>
      </div>
    </section>
  </div>
</template>
