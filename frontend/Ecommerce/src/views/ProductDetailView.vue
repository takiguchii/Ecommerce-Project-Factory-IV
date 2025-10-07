<template>
  <!-- fundo e padding pro navbar fixo -->
  <div class="bg-gradient-to-b from-neutral-950 via-neutral-900 to-black text-white min-h-[calc(100vh-5rem)] pt-24 px-4">
    <div class="max-w-6xl mx-auto grid grid-cols-1 lg:grid-cols-2 gap-8 lg:gap-12">

      <!-- COLUNA ESQUERDA: imagem / skeleton -->
      <div class="flex flex-col items-center">
        <!-- Skeleton enquanto não tem produto -->
        <div v-if="!product" class="w-full max-w-md aspect-square rounded-2xl bg-neutral-900/60 border border-neutral-800 ring-1 ring-black/5 shadow-xl animate-pulse" />
        <!-- Card de imagem -->
        <div v-else class="group w-full max-w-md aspect-square bg-neutral-900/60 border border-neutral-800 rounded-2xl shadow-xl ring-1 ring-black/5 backdrop-blur-sm p-4 overflow-hidden">
          <img
            :src="product?.imageUrl"
            :alt="product?.name"
            class="w-full h-full object-contain transition-transform duration-300 group-hover:scale-[1.02]"
          />
        </div>
      </div>

      <!-- COLUNA DIREITA: detalhes / skeleton -->
      <div class="flex flex-col md:sticky md:top-28">
        <!-- Skeleton do título/preço/botão -->
        <div v-if="!product" class="space-y-4">
          <div class="h-8 w-3/4 bg-neutral-800 rounded-md animate-pulse"></div>
          <div class="h-6 w-1/2 bg-neutral-800 rounded-md animate-pulse"></div>
          <div class="h-10 w-full bg-neutral-800 rounded-xl animate-pulse"></div>
          <div class="h-24 w-full bg-neutral-900/70 rounded-xl animate-pulse"></div>
        </div>

        <template v-else>
          <h1 class="text-3xl md:text-4xl font-extrabold text-orange-400 mb-3 leading-tight">
            {{ product?.name }}
          </h1>

          <div class="mb-5">
            <span v-if="product?.discountPrice" class="text-orange-200/60 line-through mr-3 text-lg">
              {{ formatPrice(product?.originalPrice) }}
            </span>
            <span class="text-3xl font-bold text-orange-500">
              {{ formatPrice(product?.discountPrice || product?.originalPrice) }}
            </span>
          </div>

          <div class="mb-6">
            <span class="inline-flex items-center gap-2 bg-green-600/90 text-white px-2.5 py-1 rounded-full text-xs ring-1 ring-green-400/30">
              <span class="inline-block w-2 h-2 rounded-full bg-white/90"></span>
              Em estoque
            </span>
          </div>

          <button
            class="bg-orange-500 hover:bg-orange-600 active:scale-[0.99] text-black font-semibold py-3 px-4 rounded-xl transition-all duration-200 w-full shadow-md ring-1 ring-orange-500/20 hover:ring-orange-500/40 mb-6"
          >
            Adicionar ao Carrinho
          </button>

          <!-- Card de infos -->
          <div class="rounded-2xl bg-neutral-900/60 border border-neutral-800 ring-1 ring-black/5 shadow-lg divide-y divide-neutral-800 overflow-hidden">
            <!-- Descrição -->
            <div class="p-5">
              <h2 class="text-lg font-semibold text-neutral-200 mb-2">Descrição</h2>
              <p class="text-neutral-300 leading-relaxed whitespace-pre-line">
                {{ descriptionDisplay }}
              </p>
              <button
                v-if="isDescLong"
                @click="showDesc = !showDesc"
                class="mt-3 text-sm font-medium text-orange-400 hover:text-orange-300 transition-colors"
                :aria-expanded="showDesc"
              >
                {{ showDesc ? 'Ver menos' : 'Ler mais' }}
              </button>
            </div>

            <!-- Informações Técnicas -->
            <div v-if="product?.technicalInfo" class="p-5">
              <h2 class="text-lg font-semibold text-neutral-200 mb-2">Informações Técnicas</h2>
              <p class="text-neutral-300 whitespace-pre-line leading-relaxed">
                {{ techDisplay }}
              </p>
              <button
                v-if="isTechLong"
                @click="showTech = !showTech"
                class="mt-3 text-sm font-medium text-orange-400 hover:text-orange-300 transition-colors"
                :aria-expanded="showTech"
              >
                {{ showTech ? 'Ver menos' : 'Ler mais' }}
              </button>
            </div>
          </div>
        </template>
      </div>

    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import { useRoute } from 'vue-router';
import { getProductById } from '../services/api';

const route = useRoute();
const product = ref(null);

/* --- formatação de preço --- */
const formatPrice = (price) => {
  if (price === null || price === undefined) return '';
  return price.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
};

/* --- "Ler mais" / "Ver menos" --- */
const DESC_LIMIT = 220;
const TECH_LIMIT = 260;

const showDesc = ref(false);
const showTech = ref(false);

const descriptionText = computed(() => product.value?.description ?? '');
const techText = computed(() => product.value?.technicalInfo ?? '');

const isDescLong = computed(() => descriptionText.value.length > DESC_LIMIT);
const isTechLong = computed(() => techText.value.length > TECH_LIMIT);

const descriptionDisplay = computed(() =>
  showDesc.value || !isDescLong.value
    ? descriptionText.value
    : descriptionText.value.slice(0, DESC_LIMIT).trimEnd() + '…'
);

const techDisplay = computed(() =>
  showTech.value || !isTechLong.value
    ? techText.value
    : techText.value.slice(0, TECH_LIMIT).trimEnd() + '…'
);

/* --- carregar produto --- */
onMounted(async () => {
  const id = route.params.id;
  product.value = await getProductById(id);
});
</script>
