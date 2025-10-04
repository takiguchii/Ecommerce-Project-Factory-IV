<script setup>
import HeroCarouselComponent from '@/components/HeroCarouselComponent.vue';
import ProductSectionComponent from '@/components/ProductSectionComponent.vue';

import { usePromotions } from '@/composables/usePromotions.js';

const { promotions, loading: promotionsLoading, error: promotionsError, fetchPromotions } = usePromotions();
</script>

<template>
  <div>
    <HeroCarouselComponent />

    <ProductSectionComponent title="Nossos Produtos" />

    <ProductSectionComponent 
      title="Hora de Dar Play!"
      :products="promotions"
      :loading="promotionsLoading"
      :error="promotionsError"
      @fetch-needed="fetchPromotions"
    />
    
    </div>
</template>