<script setup>
import HeroCarouselComponent from '@/components/HeroCarouselComponent.vue';
import ProductSectionComponent from '@/components/ProductSectionComponent.vue';
import { usePromotions } from '@/composables/usePromotions.js';
import { useProducts } from '@/composables/useProducts.js';

const { promotions, loading: promotionsLoading, error: promotionsError, fetchPromotions } = usePromotions();
const { products, loading: productsLoading, error: productsError, fetchProducts } = useProducts();
</script>

<template>
  <div class="bg-black">
    <HeroCarouselComponent />

    <main class="container mx-auto px-4">
    
      <ProductSectionComponent 
        title="Hora de Dar Play!"
        :products="promotions"
        :loading="promotionsLoading"
        :error="promotionsError"
        @fetch-needed="fetchPromotions"
      />

      <ProductSectionComponent 
        title="Nossos Produtos"
        :products="products"
        :loading="productsLoading"
        :error="productsError"
        @fetch-needed="fetchProducts"
      />

    </main>
  </div>
</template>