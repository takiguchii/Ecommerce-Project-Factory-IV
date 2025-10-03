<script setup>
import { ref, onMounted, onUnmounted } from 'vue';

// Importe suas imagens da pasta assets
import promoImage1 from '@/assets/images/promo1.webp';
import promoImage2 from '@/assets/images/promo2.webp';
import promoImage3 from '@/assets/images/promo3.webp';

// Toda a lógica do carrossel agora vive aqui, isolada.
const promotions = ref([
  { id: 1, src: promoImage1 },
  { id: 2, src: promoImage2 },
  { id: 3, src: promoImage3 },
]);

const activeIndex = ref(0);
let intervalId = null;

onMounted(() => {
  intervalId = setInterval(() => {
    activeIndex.value = (activeIndex.value + 1) % promotions.value.length;
  }, 5000);
});

onUnmounted(() => {
  clearInterval(intervalId);
});
</script>

<template>
  <section class="relative w-full h-[320px] md:h-[420px] lg:h-[520px] overflow-hidden">
    <div class="absolute inset-0">
      <div 
        v-for="(promo, index) in promotions" 
        :key="promo.id"
        class="absolute inset-0 bg-cover bg-center transition-opacity duration-1000 ease-in-out"
        :style="{ backgroundImage: `url(${promo.src})` }"
        :class="{ 'opacity-100': index === activeIndex, 'opacity-0': index !== activeIndex }"
      ></div>
    </div>
    <div class="absolute inset-0 bg-gradient-to-r from-black via-black/70 to-orange-900/30"></div>
    <div class="relative h-full px-6 md:px-10 flex flex-col justify-center items-start">
      <h1 class="text-4xl md:text-6xl font-extrabold text-orange-400 drop-shadow">TechMart</h1>
      <p class="text-orange-100 text-lg md:text-2xl italic mt-2">
        Aqui você encontra aquilo que busca.
      </p>
    </div>
  </section>
</template>