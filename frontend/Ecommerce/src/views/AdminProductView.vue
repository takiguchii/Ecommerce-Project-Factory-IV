<template>
  <div class="bg-gray-900 min-h-screen p-4 md:p-8 pt-20">
    <div class="max-w-6xl mx-auto">
      <h1 class="text-3xl font-bold text-gray-100 mb-6">
        {{ editingId ? 'Editar Produto' : 'Gerenciador de Produtos' }}
      </h1>

      <div class="bg-gray-800 shadow-xl rounded-lg p-6 md:p-8 mb-8">
        <h3 class="text-2xl font-semibold text-gray-200 mb-6 border-b border-gray-700 pb-3">
          {{ editingId ? `Editando Produto (ID: ${editingId})` : 'Adicionar Novo Produto' }}
        </h3>

        <form @submit.prevent="saveHandler" class="grid grid-cols-1 md:grid-cols-2 gap-5">
          <div>
            <label class="block text-sm font-medium text-gray-400 mb-1">Código</label>
            <input v-model.trim="form.code" type="text" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" required />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-400 mb-1">Nome</label>
            <input v-model.trim="form.name" type="text" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" required />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-400 mb-1">Preço Original</label>
            <input v-model.trim="form.original_price" type="text" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-400 mb-1">Preço com Desconto</label>
            <input v-model.trim="form.discount_price" type="text" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" />
          </div>
          <div class="md:col-span-2">
            <label class="block text-sm font-medium text-gray-400 mb-1">Descrição</label>
            <textarea v-model.trim="form.description" rows="4" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500"></textarea>
          </div>
          <div class="md:col-span-2">
            <label class="block text-sm font-medium text-gray-400 mb-1">Informações Técnicas</label>
            <textarea v-model.trim="form.technical_info" rows="4" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500"></textarea>
          </div>

          <div class="md:col-span-2 grid grid-cols-1 md:grid-cols-2 gap-5">
            <div>
              <label class="block text-sm font-medium text-gray-400 mb-1">Imagem URL 0</label>
              <input v-model.trim="form.image_url0" type="text" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-400 mb-1">Imagem URL 1</label>
              <input v-model.trim="form.image_url1" type="text" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-400 mb-1">Imagem URL 2</label>
              <input v-model.trim="form.image_url2" type="text" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-400 mb-1">Imagem URL 3</label>
              <input v-model.trim="form.image_url3" type="text" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" />
            </div>
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-400 mb-1">Categoria</label>
            <select v-model="form.category_id" @change="syncSubcategories" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500">
              <option :value="null">Selecione</option>
              <option v-for="c in categories" :key="c.id" :value="c.id">{{ c.name }}</option>
            </select>
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-400 mb-1">Subcategoria</label>
            <select v-model="form.sub_category_id" :disabled="filteredSubcategories.length===0" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500">
              <option :value="null">Selecione</option>
              <option v-for="s in filteredSubcategories" :key="s.id" :value="s.id">{{ s.name }}</option>
            </select>
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-400 mb-1">Marca</label>
            <select v-model="form.brand_id" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500">
              <option :value="null">Selecione</option>
              <option v-for="b in brands" :key="b.id" :value="b.id">{{ b.name }}</option>
            </select>
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-400 mb-1">Fornecedor</label>
            <select v-model="form.provider_id" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500">
              <option :value="null">Selecione</option>
              <option v-for="p in providers" :key="p.id" :value="p.id">{{ p.name }}</option>
            </select>
          </div>

          <div class="md:col-span-2 flex items-center gap-4 pt-2">
            <button type="submit" :disabled="loadingSubmit" class="px-5 py-2 bg-orange-600 text-white font-medium rounded-md hover:bg-orange-700 transition disabled:bg-gray-500">
              {{ loadingSubmit ? 'Salvando...' : (editingId ? 'Atualizar Produto' : 'Criar Produto') }}
            </button>
            <button v-if="editingId" type="button" @click="resetForm" class="px-5 py-2 bg-gray-600 text-white font-medium rounded-md hover:bg-gray-500 transition">
              Cancelar
            </button>
          </div>

          <p v-if="errorSubmit" class="md:col-span-2 text-red-500 text-sm font-medium">{{ errorSubmit }}</p>
          <p v-if="successSubmit" class="md:col-span-2 text-green-500 text-sm font-medium">{{ successMessage }}</p>
        </form>
      </div>

      <div class="bg-gray-800 shadow-xl rounded-lg overflow-hidden">
        <h2 class="text-2xl font-semibold text-gray-200 p-6 border-b border-gray-700">Produtos</h2>

        <div v-if="loadingList" class="text-center p-10 text-gray-400">Carregando produtos...</div>
        <div v-else-if="errorList" class="text-center p-10 text-red-500">{{ errorList }}</div>

        <div v-else>
          <div v-if="displayedProducts.length===0" class="text-center p-10 text-gray-500">Nenhum produto encontrado.</div>

          <ul v-else class="divide-y divide-gray-700">
            <li v-for="item in displayedProducts" :key="item.id" class="p-4 flex flex-wrap items-center justify-between gap-3 hover:bg-gray-700 transition">
              <div class="min-w-0">
                <p class="text-gray-100 font-medium truncate">{{ item.name }}</p>
                <p class="text-gray-400 text-sm">
                  ID: {{ item.id }} • Código: {{ item.code }}
                  <span v-if="item.discount_price" class="ml-2 text-green-400">Desconto: {{ item.discount_price }}</span>
                </p>
              </div>
              <div class="flex-shrink-0 flex gap-2">
                <button @click="startEdit(item)" class="px-3 py-1 bg-yellow-500 text-white text-sm font-medium rounded-md hover:bg-yellow-600 transition">Editar</button>
                <button @click="deleteHandler(item.id)" :disabled="loadingDelete[item.id]" class="px-3 py-1 bg-red-600 text-white text-sm font-medium rounded-md hover:bg-red-700 transition disabled:bg-gray-500">
                  {{ loadingDelete[item.id] ? '...' : 'Apagar' }}
                </button>
              </div>
            </li>
          </ul>

          <div v-if="totalPages > 1" class="mt-8 flex flex-wrap items-center justify-center gap-2">
            <button
              class="px-3 py-2 rounded text-white bg-neutral-900 border border-neutral-700 text-sm disabled:opacity-50"
              :disabled="pageNumber === 1"
              @click="goToPage(pageNumber - 1)"
            >
              Anterior
            </button>

            <template v-for="(p, idx) in visiblePages" :key="idx">
              <button
                v-if="p !== '...'"
                class="px-3 py-2 rounded border text-sm text-white"
                :class="p === pageNumber ? 'bg-orange-500 border-orange-500 text-black' : 'bg-neutral-900 border-neutral-700'"
                @click="goToPage(p)"
              >
                {{ p }}
              </button>
              <span v-else class="px-2 text-neutral-400">…</span>
            </template>

            <button
              class="px-3 py-2 rounded text-white bg-neutral-900 border border-neutral-700 text-sm disabled:opacity-50"
              :disabled="pageNumber === totalPages"
              @click="goToPage(pageNumber + 1)"
            >
              Próxima
            </button>
          </div>
        </div>
      </div>

      <button @click="logout" class="mt-8 px-5 py-2 bg-gray-700 text-white font-medium rounded-md hover:bg-gray-600 transition">
        Sair (Logout)
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, reactive, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { apiGet, apiPost, apiPut, apiDelete, getCategories, getBrands } from '@/services/api'

const router = useRouter()

const products = ref([])
const categories = ref([])
const subcategories = ref([])
const providers = ref([])
const brands = ref([])

const pageNumber = ref(1)
const pageSize = ref(10)

const totalCount = computed(() => products.value.length)
const totalPages = computed(() => Math.max(1, Math.ceil(totalCount.value / pageSize.value)))

const displayedProducts = computed(() => {
  const start = (pageNumber.value - 1) * pageSize.value
  return products.value.slice(start, start + pageSize.value)
})

const visiblePages = computed(() => {
  const max = totalPages.value
  const current = pageNumber.value
  const windowSize = 5
  if (max <= windowSize) return Array.from({ length: max }, (_, i) => i + 1)
  const pages = []
  const add = (x) => pages.push(x)
  add(1)
  let start = Math.max(2, current - 1)
  let end = Math.min(max - 1, current + 1)
  if (current <= 3) { start = 2; end = 4 }
  if (current >= max - 2) { start = max - 3; end = max - 1 }
  if (start > 2) add('...')
  for (let p = start; p <= end; p++) add(p)
  if (end < max - 1) add('...')
  add(max)
  return pages
})

function goToPage(p) {
  if (p < 1 || p > totalPages.value || p === pageNumber.value) return
  pageNumber.value = p
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

const filteredSubcategories = computed(() => {
  if (!form.value.category_id) return []
  return subcategories.value.filter(s => String(s.category_id) === String(form.value.category_id))
})

const form = ref({
  code: '',
  name: '',
  original_price: '',
  discount_price: '',
  description: '',
  technical_info: '',
  image_url0: '',
  image_url1: '',
  image_url2: '',
  image_url3: '',
  category_id: null,
  sub_category_id: null,
  brand_id: null,
  provider_id: null
})

const editingId = ref(null)
const loadingSubmit = ref(false)
const errorSubmit = ref('')
const successSubmit = ref(false)
const successMessage = ref('')

const loadingList = ref(false)
const errorList = ref('')

const loadingDelete = reactive({})

const logout = () => {
  localStorage.removeItem('authToken')
  router.push('/login')
}

const fetchProducts = async () => {
  loadingList.value = true
  errorList.value = ''
  try {
    const data = await apiGet('/products')
    products.value = Array.isArray(data) ? data : []
    if (pageNumber.value > totalPages.value) pageNumber.value = totalPages.value
  } catch (e) {
    errorList.value = 'Falha ao carregar produtos.'
    if (e?.response && (e.response.status === 401 || e.response.status === 403)) logout()
  } finally {
    loadingList.value = false
  }
}

const fetchLookups = async () => {
  try {
    const [cats, subs, provs, brs] = await Promise.all([
      getCategories(),
      apiGet('/subcategories'),
      apiGet('/providers'),
      getBrands()
    ])
    categories.value = cats ?? []
    subcategories.value = Array.isArray(subs) ? subs : []
    providers.value = Array.isArray(provs) ? provs : []
    brands.value = brs ?? []
  } catch (e) {}
}

const syncSubcategories = () => {
  if (!form.value.category_id) form.value.sub_category_id = null
  else if (!filteredSubcategories.value.find(s => String(s.id) === String(form.value.sub_category_id))) {
    form.value.sub_category_id = null
  }
}

const resetForm = () => {
  form.value = {
    code: '',
    name: '',
    original_price: '',
    discount_price: '',
    description: '',
    technical_info: '',
    image_url0: '',
    image_url1: '',
    image_url2: '',
    image_url3: '',
    category_id: null,
    sub_category_id: null,
    brand_id: null,
    provider_id: null
  }
  editingId.value = null
  loadingSubmit.value = false
  errorSubmit.value = ''
  successSubmit.value = false
  successMessage.value = ''
}

const startEdit = (item) => {
  editingId.value = item.id
  form.value = {
    code: item.code ?? '',
    name: item.name ?? '',
    original_price: item.original_price ?? '',
    discount_price: item.discount_price ?? '',
    description: item.description ?? '',
    technical_info: item.technical_info ?? '',
    image_url0: item.image_url0 ?? '',
    image_url1: item.image_url1 ?? '',
    image_url2: item.image_url2 ?? '',
    image_url3: item.image_url3 ?? '',
    category_id: item.category_id ?? null,
    sub_category_id: item.sub_category_id ?? null,
    brand_id: item.brand_id ?? null,
    provider_id: item.provider_id ?? null
  }
  syncSubcategories()
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

function normalizePrice(v) {
  if (v == null) return null
  const s = String(v).trim()
  if (s === '') return null
  const num = s.replace(/[R$\s]/g, '').replace(/\./g, '').replace(',', '.')
  return num === '' ? null : num
}

function toInt(v) {
  if (v === '' || v == null) return null
  const n = Number(v)
  return Number.isFinite(n) ? n : null
}

function toNullableInt(v) {
  if (v === '' || v == null) return null
  const n = Number(v)
  return Number.isFinite(n) ? n : null
}

const saveHandler = async () => {
  loadingSubmit.value = true
  errorSubmit.value = ''
  successSubmit.value = false

  try {
    const payload = {
      code: form.value.code,
      name: form.value.name,
      original_price: normalizePrice(form.value.original_price),
      discount_price: normalizePrice(form.value.discount_price),
      description: form.value.description ?? '',
      technical_info: form.value.technical_info ?? '',
      image_url0: form.value.image_url0 ?? '',
      image_url1: form.value.image_url1 ?? '',
      image_url2: form.value.image_url2 ?? '',
      image_url3: form.value.image_url3 ?? '',
      category_id: toInt(form.value.category_id),
      sub_category_id: toNullableInt(form.value.sub_category_id),
      brand_id: toInt(form.value.brand_id),
      provider_id: toInt(form.value.provider_id)
    }

    if (!payload.category_id || !payload.brand_id || !payload.provider_id) {
      throw new Error('Categoria, Marca e Fornecedor são obrigatórios.')
    }

    if (editingId.value) {
      await apiPut(`/products/${editingId.value}`, payload)
      successMessage.value = 'Produto atualizado com sucesso!'
    } else {
      await apiPost('/products', payload)
      successMessage.value = 'Produto criado com sucesso!'
    }

    successSubmit.value = true
    await fetchProducts()
    resetForm()
    setTimeout(() => {
      successSubmit.value = false
      successMessage.value = ''
    }, 3000)
  } catch (e) {
    if (e?.response?.data?.errors) {
      errorSubmit.value = Object.values(e.response.data.errors).flat().join(' ')
    } else if (e?.response?.data?.title) {
      errorSubmit.value = e.response.data.title
    } else {
      errorSubmit.value = e.message || 'Falha ao salvar produto.'
    }
  } finally {
    loadingSubmit.value = false
  }
}

const deleteHandler = async (id) => {
  if (!confirm(`Apagar produto ID ${id}?`)) return
  loadingDelete[id] = true
  try {
    await apiDelete(`/products/${id}`)
    if (editingId.value === id) resetForm()
    await fetchProducts()
  } catch (e) {
  } finally {
    const t = { ...loadingDelete }
    delete t[id]
    Object.assign(loadingDelete, t)
  }
}

onMounted(async () => {
  await Promise.all([fetchProducts(), fetchLookups()])
})
</script>
