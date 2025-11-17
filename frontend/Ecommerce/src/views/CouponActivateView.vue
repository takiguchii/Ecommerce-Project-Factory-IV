<template>
  <div class="bg-neutral-900 min-h-screen p-4 md:p-8 pt-20">
    <div class="max-w-6xl mx-auto">
      <h1 class="text-3xl font-bold text-gray-100 mb-6">Cupons Ativos</h1>

      <div class="bg-neutral-800 shadow-xl rounded-lg overflow-hidden">
        <div class="flex items-center justify-between p-6 border-b border-neutral-700">
          <h2 class="text-2xl font-semibold text-neutral-200">Lista de Cupons Ativos</h2>
          <button
            @click="fetchCoupons"
            class="px-4 py-2 bg-orange-600 hover:bg-orange-700 text-white rounded-md font-medium transition"
          >
            Atualizar
          </button>
        </div>

        <div v-if="loading" class="text-center p-10 text-neutral-400">Carregando cupons...</div>
        <div v-else-if="error" class="text-center p-10 text-red-500">{{ error }}</div>

        <template v-else>
          <div v-if="displayed.length === 0" class="text-center p-10 text-gray-500">
            Nenhum cupom ativo encontrado.
          </div>

          <ul v-else class="divide-y divide-neutral-700">
            <li
              v-for="coupon in displayed"
              :key="coupon.id"
              class="p-4 flex flex-wrap items-center justify-between gap-3 hover:bg-neutral-700/90 transition"
            >
              <div class="min-w-0">
                <p class="text-gray-100 font-medium truncate">{{ coupon.code }}</p>
                <p class="text-gray-400 text-sm">
                  {{ coupon.description || '—' }}
                  <span class="ml-2">
                    • {{ coupon.isPercentage ? coupon.discountValue + '%' : 'R$ ' + formatNumber(coupon.discountValue) }}
                  </span>
                  <span class="ml-2">
                    • Expira: {{ coupon.expiryDate ? formatDate(coupon.expiryDate) : 'Sem expiração' }}
                  </span>
                </p>
              </div>
              <span class="px-3 py-1 bg-green-600/30 text-green-400 text-sm font-medium rounded-md border border-green-600">
                Ativo
              </span>
            </li>
          </ul>

          <div v-if="totalPages > 1" class="mt-8 flex flex-wrap items-center justify-center gap-2">
            <button
              v-for="page in totalPages"
              :key="page"
              @click="goToPage(page)"
              class="px-3 py-2 rounded border text-sm"
              :class="page === pageNumber ? 'bg-orange-500 border-orange-500 text-black' : 'bg-neutral-900 border-neutral-700 text-white'"
            >
              {{ page }}
            </button>
          </div>
        </template>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { apiGet } from '@/services/api'

const coupons = ref([])
const loading = ref(false)
const error = ref('')
const pageNumber = ref(1)
const pageSize = ref(10)

function formatNumber(value) {
  return Number(value).toFixed(2)
}

function formatDate(date) {
  return date ? new Date(date).toISOString().split('T')[0] : ''
}

const activeCoupons = computed(() => coupons.value.filter(coupon => coupon.isActive))

const totalPages = computed(() =>
  Math.max(1, Math.ceil(activeCoupons.value.length / pageSize.value))
)

const displayed = computed(() => {
  const start = (pageNumber.value - 1) * pageSize.value
  return activeCoupons.value.slice(start, start + pageSize.value)
})

function goToPage(page) {
  if (page < 1 || page > totalPages.value) return
  pageNumber.value = page
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

async function fetchCoupons() {
  loading.value = true
  error.value = ''
  try {
    const data = await apiGet('/Coupon')
    coupons.value = Array.isArray(data) ? data : [data]
  } catch {
    error.value = 'Falha ao carregar cupons.'
  } finally {
    loading.value = false
  }
}

onMounted(fetchCoupons)
</script>
