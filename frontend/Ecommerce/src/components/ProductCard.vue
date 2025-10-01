<script setup>
import { computed } from 'vue';

const props = defineProps({
  name: {
    type: String,
    required: true
  },
  price: {
    type: Number,
    required: true
  },
  discount: {
    type: Number,
    default: 0
  },
  imageUrl: {
    type: String,
    required: true
  },
  description: {
    type: String,
    required: true
  }
});

const calculateDiscountedPrice = computed(() => {
  if (!props.discount) return props.price;
  return (props.price * (1 - props.discount / 100)).toFixed(2);
});
</script>

<template>
  <div class="bg-white rounded-lg shadow-md overflow-hidden">
    <img :src="imageUrl" :alt="name" class="w-full h-48 object-cover">
    <div class="p-4">
      <h3 class="text-lg font-semibold mb-2">{{ name }}</h3>
      <p class="text-gray-600 text-sm mb-2 line-clamp-2">{{ description }}</p>
      <div class="flex items-center justify-between">
        <div>
          <span v-if="discount" class="text-red-600 font-bold text-lg">
            ${{ calculateDiscountedPrice }}
          </span>
          <span v-if="discount" class="text-gray-400 text-sm line-through ml-2">
            ${{ price }}
          </span>
          <span v-else class="text-gray-900 font-bold text-lg">
            ${{ price }}
          </span>
        </div>
        <span v-if="discount" class="bg-red-100 text-red-600 px-2 py-1 rounded-full text-sm">
          -{{ discount }}%
        </span>
      </div>
    </div>
  </div>
</template>

<style scoped>
.product-card {
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  background-color: #fff;
  transition: box-shadow 0.3s ease;
}

.product-card:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.product-image {
  width: 100%;
  height: 200px;
  object-fit: contain; /* Usar 'contain' para não cortar a imagem */
  padding: 10px;
  background-color: #f9f9f9;
}

.product-info {
  padding: 16px;
  text-align: center;
  flex-grow: 1;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.product-name {
  font-size: 1rem;
  font-weight: 600;
  margin-bottom: 12px;
  min-height: 40px; /* Garante que os cards tenham a mesma altura */
}

.product-prices {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.original-price {
  font-size: 0.85rem;
  color: #888;
  text-decoration: line-through;
}

.current-price {
  font-size: 1.25rem;
  font-weight: bold;
  color: #2c3e50;
}
</style>