<script setup>
import HeroCarouselComponent from '@/components/HeroCarouselComponent.vue';
import ProductSectionComponent from '@/components/ProductSectionComponent.vue';
import DepartmentSectionComponent from '@/components/DepartmentSectionComponent.vue'; 
import { usePromotions } from '@/composables/usePromotions.js';
import { useProducts } from '@/composables/useProducts.js';

const { promotions, loading: promotionsLoading, error: promotionsError, fetchPromotions } = usePromotions();
const { products, loading: productsLoading, error: productsError, fetchProducts } = useProducts();
</script>

<template>
  <!-- padding-top para o navbar fixo -->
  <div class="bg-black pt-20 md:pt-24">
    <HeroCarouselComponent />

    <main class="container mx-auto px-4">

      <ProductSectionComponent 
        title="🔥 Hora de Dar Play!"
        :products="promotions"
        :loading="promotionsLoading"
        :error="promotionsError"
        @fetch-needed="fetchPromotions"
      />

      <DepartmentSectionComponent />

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
