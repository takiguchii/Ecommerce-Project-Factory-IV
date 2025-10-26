<script setup>
import HeroCarouselComponent from '@/components/HeroCarouselComponent.vue';
import ProductSectionComponent from '@/components/ProductSectionComponent.vue';
import DepartmentSectionComponent from '@/components/DepartmentSectionComponent.vue';
import BrandSectionComponent from '@/components/BrandSectionComponent.vue'; 
import { usePromotions } from '@/composables/usePromotions.js';
import { useProducts } from '@/composables/useProducts.js';
import { useBrands } from '@/composables/useBrands.js'; 

const { promotions, loading: promotionsLoading, error: promotionsError, fetchPromotions } = usePromotions();
const { products, loading: productsLoading, error: productsError, fetchProducts } = useProducts();
const { brands, loading: brandsLoading, error: brandsError, fetchBrands } = useBrands(); 
</script>

<template>
  <div class="bg-gradient-to-b from-neutral-950 via-neutral-900 to-black text-white pt-20 md:pt-24">
    <HeroCarouselComponent />

    <main class="container mx-auto space-y-10 md:space-y-12">
      
      <!-- Promoções -->
      <section
        class="rounded-2xl bg-neutral-900/60 border-[1.5px] md:border-4 border-neutral-700/70 shadow-xl ring-1 ring-black/5 backdrop-blur-sm p-4 sm:p-6 hover:border-orange-500/40 transition-all duration-300"
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
        id="departamentos"
        class="rounded-2xl bg-neutral-900/50 border-[1.5px] md:border-4 border-neutral-700/70 shadow-lg ring-1 ring-black/5 backdrop-blur-sm p-4 sm:p-6 hover:border-orange-500/40 transition-all duration-300"
      >
        <DepartmentSectionComponent />
      </section>

      <!-- Marcas -->
      <section
        id="marcas"
        class="rounded-2xl bg-neutral-900/50 border-[1.5px] md:border-4 border-neutral-700/70 shadow-lg ring-1 ring-black/5 backdrop-blur-sm p-4 sm:p-6 hover:border-orange-500/40 transition-all duration-300"
      >
       <BrandSectionComponent 
          title="Navegue por Marcas"
          :brands="brands"
          :loading="brandsLoading"
          :error="brandsError"
          @fetch-needed="fetchBrands"
        />
      </section> 

      <!-- Lista de produtos -->
      <section
        class="rounded-2xl bg-neutral-900/60 border-[1.5px] md:border-4 border-neutral-700/70 shadow-xl ring-1 ring-black/5 backdrop-blur-sm p-4 sm:p-6 hover:border-orange-500/40 transition-all duration-300"
      >
        <ProductSectionComponent 
          title="Nossos Produtos"
          :products="promotions"
          :loading="promotionsLoading"
          :error="promotionsError"
          @fetch-needed="fetchPromotions"
        />
      </section>
    </main>
  </div>
</template>
