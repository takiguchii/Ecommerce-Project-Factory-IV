<script setup>
import { ref, onMounted, onUnmounted } from 'vue';
import promoImage1 from '@/assets/images/promo1.webp';
import promoImage2 from '@/assets/images/promo2.webp';
import promoImage3 from '@/assets/images/promo3.webp';

const promotions = ref([
  { id: 1, src: promoImage1, alt: 'Promoção de Hardware' },
  { id: 2, src: promoImage2, alt: 'Ofertas em Periféricos' },
  { id: 3, src: promoImage3, alt: 'Descontos em Notebooks' },
]);

const activeIndex = ref(0);
let intervalId = null;

function next() {
  activeIndex.value = (activeIndex.value + 1) % promotions.value.length;
  resetInterval();
}

function prev() {
  activeIndex.value = (activeIndex.value - 1 + promotions.value.length) % promotions.value.length;
  resetInterval();
}

function goToSlide(index) {
  activeIndex.value = index;
  resetInterval();
}

function resetInterval() {
  clearInterval(intervalId);
  startInterval();
}

function startInterval() {
  intervalId = setInterval(() => {
    next();
  }, 5000);
}

onMounted(startInterval);
onUnmounted(() => {
  clearInterval(intervalId);
});
</script>

<template>
  <section class="bg-black py-8">
    <div class="container mx-auto relative">

      <div class="relative w-full h-[250px] md:h-[350px] lg:h-[350px] overflow-hidden rounded-2xl">
        <transition-group name="fade">
          <img 
            v-for="(promo, index) in promotions" 
            :key="promo.id"
            v-show="index === activeIndex"
            :src="promo.src" 
            :alt="promo.alt"
            class="absolute top-0 left-0 w-full h-full object-cover"
          />
        </transition-group>
      </div>

      <button @click="prev" class="absolute left-6 top-1/2 -translate-y-1/2 bg-black/40 hover:bg-black/70 text-white p-3 rounded-full transition-colors z-10">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" /></svg>
      </button>

      <button @click="next" class="absolute right-6 top-1/2 -translate-y-1/2 bg-black/40 hover:bg-black/70 text-white p-3 rounded-full transition-colors z-10">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" /></svg>
      </button>

      <div class="absolute bottom-4 left-1/2 -translate-x-1/2 flex gap-2 z-10">
        <button v-for="(promo, index) in promotions" :key="`dot-${promo.id}`" @click="goToSlide(index)"
                class="w-3 h-3 rounded-full transition-colors"
                :class="index === activeIndex ? 'bg-orange-500' : 'bg-white/40 hover:bg-white/70'">
        </button>
      </div>
    </div>
  </section>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.8s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>