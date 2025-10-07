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
  <div class="bg-gradient-to-b from-neutral-950 via-neutral-900 to-black text-white pt-20 md:pt-24">
    <HeroCarouselComponent />

    <main class="container mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 space-y-12 sm:space-y-16">

      <!-- Promoções -->
      <section
        class="rounded-2xl bg-neutral-900/60 border border-neutral-800/60 shadow-xl ring-1 ring-black/5 backdrop-blur-sm p-4 sm:p-6"
      >
        <ProductSectionComponent 
          title="🔥 Hora de Dar Play!"
          :products="promotions"
          :loading="promotionsLoading"
          :error="promotionsError"
          @fetch-needed="fetchPromotions"
        />
      </section>

      <!-- Departamentos -->
      <section
        class="rounded-2xl bg-neutral-900/40 border border-neutral-800/60 shadow-lg ring-1 ring-black/5 backdrop-blur-sm p-4 sm:p-6"
        id="departamentos"
      >
        <DepartmentSectionComponent />
      </section>

      <!-- Lista de produtos -->
      <section
        class="rounded-2xl bg-neutral-900/60 border border-neutral-800/60 shadow-xl ring-1 ring-black/5 backdrop-blur-sm p-4 sm:p-6"
      >
        <ProductSectionComponent 
          title="Nossos Produtos"
          :products="products"
          :loading="productsLoading"
          :error="productsError"
          @fetch-needed="fetchProducts"
        />
      </section>

    </main>
  </div>
</template>
