<template>
  <div
    class="bg-gradient-to-b from-neutral-950 via-neutral-900 to-black text-white min-h-[calc(100vh-5rem)] pt-24 px-4">
    <div class="max-w-6xl mx-auto">

      <!-- LINHA SUPERIOR: IMAGENS + INFO PRINCIPAL -->
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-8 lg:gap-12">

        <!-- COLUNA ESQUERDA: Imagens -->
        <div class="flex flex-col items-center">
          <div v-if="!product"
            class="w-full max-w-md aspect-square rounded-2xl bg-neutral-900/60 border border-neutral-800 ring-1 ring-black/5 shadow-xl animate-pulse" />
          <div v-else
            class="w-full max-w-md bg-neutral-900/60 border border-neutral-800 rounded-2xl shadow-xl ring-1 ring-black/5 backdrop-blur-sm p-4">
            <div class="relative w-full aspect-square overflow-hidden rounded-xl">
              <img :src="currentImage || placeholder" :alt="product?.name"
                class="w-full h-full object-contain transition-transform duration-300 hover:scale-[1.02]"
                @error="onMainImgError" />
            </div>

            <div v-if="thumbs.length" class="mt-4 grid grid-cols-3 gap-3">
              <button v-for="thumb in thumbs" :key="thumb.index" type="button"
                class="rounded-lg border border-neutral-800 overflow-hidden cursor-pointer focus:outline-none focus:ring-2 focus:ring-orange-500/60"
                @click="selectIndex(thumb.index)">
                <img :src="thumb.src || placeholder" :alt="product?.name"
                  class="w-full aspect-square object-contain transition-all duration-200 hover:opacity-90"
                  @error="onThumbImgError" />
              </button>
            </div>
          </div>
        </div>

        <!-- COLUNA DIREITA SUPERIOR -->
        <div class="flex flex-col md:sticky md:top-28">
          <div v-if="!product" class="space-y-4">
            <div class="h-8 w-3/4 bg-neutral-800 rounded-md animate-pulse"></div>
            <div class="h-6 w-1/2 bg-neutral-800 rounded-md animate-pulse"></div>
            <div class="h-10 w-full bg-neutral-800 rounded-xl animate-pulse"></div>
            <div class="h-24 w-full bg-neutral-900/70 rounded-xl animate-pulse"></div>
          </div>

          <template v-else>
            <h1 class="text-3xl md:text-4xl font-extrabold text-orange-400 mb-1 leading-tight">
              {{ product?.name }}
            </h1>

            <div class="mb-4 flex flex-wrap items-center gap-3">
              <span v-if="product?.code || brandName"
                class="inline-flex items-center gap-2 px-2.5 py-1 rounded-full text-xs font-medium bg-neutral-800/80 border border-neutral-700 text-neutral-200">
                <svg viewBox="0 0 24 24" class="w-4 h-4">
                  <path fill="currentColor" d="M3 7h18v2H3V7zm0 4h12v2H3v-2zm0 4h18v2H3v-2z" />
                </svg>
                <template v-if="brandName">{{ brandName }}<span class="mx-1.5">•</span></template>
                {{ product?.code }}
              </span>

              <div class="flex items-center gap-2">
                <div class="flex">
                  <svg v-for="(s, i) in starIcons" :key="i" viewBox="0 0 24 24" class="w-4 h-4 mx-[1px]">
                    <defs>
                      <linearGradient :id="`grad-${i}`" x1="0" y1="0" x2="1" y2="0">
                        <stop offset="0" stop-color="currentColor" />
                        <stop :offset="s === 'half' ? '50%' : '0%'" stop-color="currentColor" />
                        <stop :offset="s === 'half' ? '50%' : '0%'" stop-color="transparent" />
                        <stop offset="100%" stop-color="transparent" />
                      </linearGradient>
                    </defs>
                    <path :fill="s === 'full' ? 'currentColor' : (s === 'half' ? `url(#grad-${i})` : 'none')"
                      stroke="currentColor" stroke-width="1"
                      d="M12 2.25l2.7 5.47 6.04.88-4.37 4.26 1.03 6.01L12 16.96 6.6 18.87l1.03-6.01L3.26 8.6l6.04-.88L12 2.25z" />
                  </svg>
                </div>
                <span class="text-sm text-neutral-300">{{ averageStarsFixed }} • {{ formattedReviews }}</span>
              </div>
            </div>

            <!-- PREÇOS -->
            <div class="mb-5 space-y-1">
              <div class="flex items-center justify-between" v-if="originalPriceBRL">
                <span class="text-sm text-neutral-300">Preço normal</span>
                <span class="text-lg">{{ originalPriceBRL }}</span>
              </div>

              <div class="flex items-center justify-between" v-if="discountPriceBRL">
                <span class="text-sm text-neutral-300">Preço com desconto</span>
                <span class="text-2xl font-bold text-orange-500">{{ discountPriceBRL }}</span>
              </div>

              <div v-if="hasRealDiscount" class="pt-1">
                <span
                  class="inline-flex items-center gap-2 bg-green-600/90 text-white px-2.5 py-1 rounded-full text-xs ring-1 ring-green-400/30">
                  -{{ savingsPercent }}% (você economiza {{ savingsAbsolute }})
                </span>
              </div>
            </div>

            <div class="mb-6">
              <span
                class="inline-flex items-center gap-2 bg-green-600/90 text-white px-2.5 py-1 rounded-full text-xs ring-1 ring-green-400/30">
                <span class="inline-block w-2 h-2 rounded-full bg-white/90"></span>
                Em estoque
              </span>
            </div>

            <div class="mb-6">
              <AddToCartButton v-if="product" class="w-full" :product="{ id: product.id }" />
              <div v-else class="h-12 w-full bg-neutral-800 rounded-xl animate-pulse"></div>
            </div>
          </template>
        </div>

      </div>

      <!-- LINHA INFERIOR: TÉCNICO (ESQ) + DESCRIÇÃO (DIR) -->
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-8 mt-10 items-start">

        <!-- INFORMAÇÕES TÉCNICAS À ESQUERDA -->
        <div v-if="techText"
          class="rounded-2xl bg-neutral-900/60 border border-neutral-800 ring-1 ring-black/5 shadow-lg p-5">
          <h2 class="text-lg font-semibold text-neutral-200 mb-2">Informações Técnicas</h2>

          <p class="text-neutral-300 whitespace-pre-line leading-relaxed">{{ techDisplay }}</p>

          <button v-if="isTechLong" @click="showTech = !showTech"
            class="mt-3 text-sm font-medium text-orange-400 hover:text-orange-300 transition-colors">
            {{ showTech ? 'Ver menos' : 'Ler mais' }}
          </button>
        </div>

        <!-- DESCRIÇÃO À DIREITA (ALTURA = TAMANHO DO TEXTO) -->
        <div class="rounded-2xl bg-neutral-900/60 border border-neutral-800 ring-1 ring-black/5 shadow-lg p-5">
          <h2 class="text-lg font-semibold text-neutral-200 mb-2">Descrição</h2>

          <p :class="[
            'text-neutral-300 leading-relaxed transition-all duration-300',
            showDesc ? 'max-h-none' : 'max-h-40 overflow-hidden'
          ]">
            {{ descriptionDisplay }}
          </p>

          <button v-if="isDescLong" @click="showDesc = !showDesc"
            class="mt-3 text-sm font-medium text-orange-400 hover:text-orange-300 transition-colors">
            {{ showDesc ? 'Ver menos' : 'Ler mais' }}
          </button>
        </div>

      </div>

    </div>


  </div>
</template>

<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { useRoute } from 'vue-router'
import { getProductById, getBrandCached } from '@/services/api'
import AddToCartButton from '@/components/AddToCartButton.vue'

const route = useRoute()
const product = ref(null)
const brand = ref(null)

/* placeholder minimal */
const placeholder =
  'data:image/svg+xml;utf8,' +
  encodeURIComponent(`<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 256 256"><rect width="256" height="256" rx="16" fill="#171717"/><g fill="#f97316"><path d="M40 184l40-56 32 40 24-32 40 48H40z"/><circle cx="172" cy="84" r="14"/></g></svg>`)

/* ---------- Imagens ---------- */
const KABUM_HOST = 'https://www.kabum.com.br'

function fixUrl(u) {
  if (!u || typeof u !== 'string') return null
  if (u.startsWith('http')) return u
  if (u.startsWith('/')) return `${KABUM_HOST}${u}`
  return u
}

const images = computed(() => {
  if (!product.value) return []
  const p = product.value
  const candidates = [
    p.image_url0, p.image_url1, p.image_url2, p.image_url3, p.image_url4,
    p.image_url, p.imageUrl, p.coverImageUrl
  ].map(fixUrl).filter(Boolean)
  return Array.from(new Set(candidates))
})

const currentIndex = ref(0)
const currentImage = computed(() => images.value[currentIndex.value] ?? placeholder)
const thumbs = computed(() =>
  images.value.map((src, index) => ({ src, index }))
    .filter(img => img.index !== currentIndex.value)
    .slice(0, 3)
)
function selectIndex(i) { if (i >= 0 && i < images.value.length) currentIndex.value = i }
function onMainImgError(e) { e.target.src = placeholder }
function onThumbImgError(e) { e.target.src = placeholder }

/* ---------- Preços ---------- */
function parsePrice(raw) {
  if (raw === undefined || raw === null) return null
  if (typeof raw === 'number') return raw
  if (typeof raw === 'string') {
    const norm = raw.replace(/[^\d,.-]/g, '').replace(/\./g, '').replace(',', '.')
    const n = parseFloat(norm)
    return Number.isNaN(n) ? null : n
  }
  return null
}
function formatBRL(n) {
  return n === null ? '' : n.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
}

const originalPriceNum = computed(() => parsePrice(product.value?.original_price ?? product.value?.originalPrice))
const discountPriceNum = computed(() =>
  parsePrice(product.value?.discount_price ?? product.value?.discountPrice ?? product.value?.price)
)

const originalPriceBRL = computed(() => formatBRL(originalPriceNum.value))
const discountPriceBRL = computed(() => formatBRL(discountPriceNum.value))

const hasRealDiscount = computed(() =>
  originalPriceNum.value !== null &&
  discountPriceNum.value !== null &&
  discountPriceNum.value < originalPriceNum.value
)

const savingsAbsolute = computed(() =>
  hasRealDiscount.value ? formatBRL(originalPriceNum.value - discountPriceNum.value) : ''
)
const savingsPercent = computed(() =>
  hasRealDiscount.value ? Math.round((1 - (discountPriceNum.value / originalPriceNum.value)) * 100) : 0
)

/* ---------- Avaliações ---------- */
const averageStars = computed(() => {
  const v = Number(product.value?.stars ?? product.value?.averageStars ?? 0)
  return Number.isNaN(v) ? 0 : Math.max(0, Math.min(5, v))
})
const averageStarsFixed = computed(() => averageStars.value.toFixed(1))
const reviewCount = computed(() => Number(product.value?.rating ?? product.value?.reviewCount ?? 0))
const formattedReviews = computed(() => new Intl.NumberFormat('pt-BR').format(reviewCount.value) + ' avaliações')

const starIcons = computed(() => {
  const stars = averageStars.value
  const full = Math.floor(stars)
  const frac = stars - full
  const hasHalf = frac >= 0.25 && frac < 0.75
  const arr = []
  for (let i = 0; i < full && i < 5; i++) arr.push('full')
  if (hasHalf && arr.length < 5) arr.push('half')
  while (arr.length < 5) arr.push('empty')
  return arr
})

/* ---------- Marca ---------- */
const productBrandName = computed(() =>
  product.value?.brand?.name ?? product.value?.brandName ?? product.value?.brand?.Nome ?? ''
)
const brandName = computed(() => productBrandName.value || brand.value?.name || '')

/* ---------- Descrição / Técnico ---------- */
const DESC_LIMIT = 220
const TECH_LIMIT = 260

const showDesc = ref(false)
const showTech = ref(false)

/* DESCRIÇÃO LIMPA */
const descriptionText = computed(() => {
  const raw = product.value?.description ?? ''
  return raw
    .replace(/\r\n/g, '\n')
    .split('\n')
    .map(line => line.trim())
    .filter(Boolean)
    .join(' ')
})

const techText = computed(() =>
  product.value?.technical_info ?? product.value?.technicalInfo ?? ''
)

const isDescLong = computed(() => descriptionText.value.length > DESC_LIMIT)
const isTechLong = computed(() => techText.value.length > TECH_LIMIT)

/* Agora sempre usamos o texto inteiro; o tamanho visível é controlado pela classe (max-h-40) */
const descriptionDisplay = computed(() => descriptionText.value)

const techDisplay = computed(() =>
  showTech.value || !isTechLong.value
    ? techText.value
    : techText.value.slice(0, TECH_LIMIT).trimEnd() + '…'
)

/* ---------- Carregamento ---------- */
async function loadBrandIfNeeded() {
  if (productBrandName.value) return
  const brandId = product.value?.brand_id ?? product.value?.brandId ?? product.value?.brand?.id ?? product.value?.brand?.ID
  if (!brandId && brandId !== 0) return
  try { brand.value = await getBrandCached(brandId) } catch (e) { console.warn('Erro ao carregar marca:', e) }
}

async function loadProduct(id) {
  if (!id) return
  product.value = null
  brand.value = null
  currentIndex.value = 0
  showDesc.value = false
  showTech.value = false
  try {
    const p = await getProductById(id)
    product.value = p
    await loadBrandIfNeeded()
    window.scrollTo({ top: 0, behavior: 'auto' })
  } catch (e) { console.warn('Erro ao carregar produto:', e) }
}

onMounted(() => loadProduct(route.params.id))
watch(() => route.params.id, (n, o) => { if (n !== o) loadProduct(n) })
watch(() => [product.value?.brand_id, product.value?.brandId, productBrandName.value],
  async () => { await loadBrandIfNeeded() })
watch(images, (arr) => { if (arr.length) currentIndex.value = 0 }, { immediate: true })
</script>
