<script setup>
import { computed, watchEffect } from 'vue'
import { useRouter } from 'vue-router'
import AddToCartButton from '@/components/AddToCartButton.vue'

const props = defineProps({ product: { type: Object, required: true } })

const router = useRouter()
const goToDetail = () => {
  router.push({ name: 'ProductDetail', params: { id: props.product?.id } })
}

const mainUrl = 'https://www.kabum.com.br/'
const name = computed(() => props.product?.name ?? '')
const imgSrc = computed(() => {
  const p = props.product ?? {}
  const rel =
    p.image_url0 || p.image_url1 || p.image_url2 || p.image_url3 || p.image_url4 || ''
  return rel ? mainUrl + rel : ''
})
const originalPrice = computed(() => props.product?.original_price ?? props.product?.originalPrice ?? null)
const discountPrice = computed(() => props.product?.discount_price ?? props.product?.discountPrice ?? null)

watchEffect(() => {})
</script>

<template>
  <article
    @click="goToDetail"
    class="cursor-pointer bg-neutral-900 rounded-lg shadow-lg hover:shadow-orange-400/20 transition-shadow duration-300 flex flex-col border border-neutral-800 overflow-hidden"
  >
    <div class="h-48 flex items-center justify-center p-4 bg-white">
      <img :src="imgSrc" :alt="name" class="max-h-full max-w-full object-contain" />
    </div>

    <div class="p-4 flex flex-col flex-grow">
      <h3 class="text-sm font-semibold text-orange-300 h-10 line-clamp-2 mb-2">
        {{ name }}
      </h3>

      <div class="mt-auto">
        <div class="h-12">
          <p v-if="discountPrice" class="text-xs text-orange-200/60 line-through">
            {{ (originalPrice) }}
          </p>
          <p class="text-2xl font-bold text-orange-400">
            {{ (discountPrice || originalPrice) }}
          </p>
        </div>

        
        <div @click.stop>
          <AddToCartButton :product="props.product" />
        </div>
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
