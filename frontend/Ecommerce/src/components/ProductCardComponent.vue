  <script setup>
  import { useRouter } from 'vue-router';

  const props = defineProps({
    product: {
      type: Object,
      required: true
    }
  });

  const emit = defineEmits(['addToCart']);

  const formatPrice = (price) => {
    if (price === null || price === undefined) return '';
    return price.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  };

  const router = useRouter();

  const goToDetail = () => {
    console.log('Navegando para:', props.product.id);
    router.push({ name: 'ProductDetail', params: { id: props.product.id } });
  };
  </script>

  <template>
    <article @click="goToDetail" class="cursor-pointer bg-neutral-900 rounded-lg shadow-lg hover:shadow-orange-400/20 transition-shadow duration-300 flex flex-col border border-neutral-800 overflow-hidden">
      
      <div class="h-48 flex items-center justify-center p-4 bg-white">
        <img :src="props.product.imageUrl" :alt="props.product.name" class="max-h-full max-w-full object-contain" />
      </div>

      <div class="p-4 flex flex-col flex-grow">
        <h3 class="text-sm font-semibold text-orange-300 h-10 line-clamp-2 mb-2">{{ props.product.name }}</h3>
        
        <div class="mt-auto">
          <div class="h-12">
              <p v-if="product.discountPrice" class="text-xs text-orange-200/60 line-through">
                {{ formatPrice(product.originalPrice) }}
              </p>
              <p class="text-2xl font-bold text-orange-400">
                {{ formatPrice(product.discountPrice || product.originalPrice) }}
              </p>
          </div>

          <button @click.stop="emit('addToCart', product)" 
                  class="w-full mt-3 bg-orange-500 hover:bg-orange-600 text-black font-bold py-2 px-4 rounded-md transition-colors duration-300">
            Adicionar ao Carrinho
          </button>
        </div>
      </div>
    </article>
  </template>

  <style scoped>
  .line-clamp-2 {
    display: -webkit-box;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
    overflow: hidden;
  }
  </style>