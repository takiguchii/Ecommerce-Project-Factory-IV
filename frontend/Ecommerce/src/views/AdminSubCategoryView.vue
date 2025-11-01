<template>
  <div class="bg-gray-900 min-h-screen p-4 md:p-8 pt-20">
    <div class="max-w-5xl mx-auto">
      <h1 class="text-3xl font-bold text-gray-100 mb-6">
        {{ editingId ? 'Editar Subcategoria' : 'Gerenciador de Subcategorias' }}
      </h1>

      <div class="bg-gray-800 shadow-xl rounded-lg p-6 md:p-8 mb-8">
        <h3 class="text-2xl font-semibold text-gray-200 mb-6 border-b border-gray-700 pb-3">
          {{ editingId ? `Editando Subcategoria (ID: ${editingId})` : 'Adicionar Nova Subcategoria' }}
        </h3>

        <form @submit.prevent="saveHandler" class="grid grid-cols-1 md:grid-cols-2 gap-5">
          <div class="md:col-span-1">
            <label class="block text-sm font-medium text-gray-400 mb-1">Nome</label>
            <input
              v-model.trim="form.name"
              type="text"
              class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500"
              required
            />
          </div>

          <div class="md:col-span-1">
            <label class="block text-sm font-medium text-gray-400 mb-1">Categoria</label>
            <select
              v-model="form.category_id"
              class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500"
              required
            >
              <option :value="null">Selecione</option>
              <option v-for="c in categories" :key="c.id" :value="c.id">{{ c.name }}</option>
            </select>
          </div>

          <div class="md:col-span-2 flex items-center gap-4 pt-2">
            <button
              type="submit"
              :disabled="loadingSubmit"
              class="px-5 py-2 bg-orange-600 text-white font-medium rounded-md hover:bg-orange-700 transition disabled:bg-gray-500"
            >
              {{ loadingSubmit ? 'Salvando...' : (editingId ? 'Atualizar Subcategoria' : 'Criar Subcategoria') }}
            </button>
            <button
              v-if="editingId"
              type="button"
              @click="resetForm"
              class="px-5 py-2 bg-gray-600 text-white font-medium rounded-md hover:bg-gray-500 transition"
            >
              Cancelar
            </button>
          </div>

          <p v-if="errorSubmit" class="md:col-span-2 text-red-500 text-sm font-medium">{{ errorSubmit }}</p>
          <p v-if="successSubmit" class="md:col-span-2 text-green-500 text-sm font-medium">{{ successMessage }}</p>
        </form>
      </div>

      <div class="bg-gray-800 shadow-xl rounded-lg overflow-hidden">
        <h2 class="text-2xl font-semibold text-gray-200 p-6 border-b border-gray-700">Subcategorias</h2>

        <div v-if="loadingList" class="text-center p-10 text-gray-400">Carregando subcategorias...</div>
        <div v-else-if="errorList" class="text-center p-10 text-red-500">{{ errorList }}</div>

        <div v-else>
          <div v-if="displayedItems.length===0" class="text-center p-10 text-gray-500">Nenhuma subcategoria encontrada.</div>

          <ul v-else class="divide-y divide-gray-700">
            <li
              v-for="item in displayedItems"
              :key="item.id"
              class="p-4 flex flex-wrap items-center justify-between gap-3 hover:bg-gray-700 transition"
            >
              <div class="min-w-0">
                <p class="text-gray-100 font-medium truncate">{{ item.name }}</p>
                <p class="text-gray-400 text-sm">
                  ID: {{ item.id }} • Categoria: {{ categoryName(item.category_id) }}
                </p>
              </div>
              <div class="flex-shrink-0 flex gap-2">
                <button
                  @click="startEdit(item)"
                  class="px-3 py-1 bg-yellow-500 text-white text-sm font-medium rounded-md hover:bg-yellow-600 transition"
                >
                  Editar
                </button>
                <button
                  @click="deleteHandler(item.id)"
                  :disabled="loadingDelete[item.id]"
                  class="px-3 py-1 bg-red-600 text-white text-sm font-medium rounded-md hover:bg-red-700 transition disabled:bg-gray-500"
                >
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
import { apiGet, apiPost, apiPut, apiDelete, getCategories } from '@/services/api'

const router = useRouter()

const items = ref([])
const categories = ref([])

const pageNumber = ref(1)
const pageSize = ref(12)

const totalCount = computed(() => items.value.length)
const totalPages = computed(() => Math.max(1, Math.ceil(totalCount.value / pageSize.value)))

const displayedItems = computed(() => {
  const start = (pageNumber.value - 1) * pageSize.value
  return items.value.slice(start, start + pageSize.value)
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

const form = ref({
  name: '',
  category_id: null
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

const fetchItems = async () => {
  loadingList.value = true
  errorList.value = ''
  try {
    const data = await apiGet('/subcategories')
    items.value = Array.isArray(data) ? data : []
    if (pageNumber.value > totalPages.value) pageNumber.value = totalPages.value
  } catch (e) {
    errorList.value = 'Falha ao carregar subcategorias.'
    if (e?.response && (e.response.status === 401 || e.response.status === 403)) logout()
  } finally {
    loadingList.value = false
  }
}

const fetchCategories = async () => {
  try {
    const cats = await getCategories()
    categories.value = cats ?? []
  } catch {}
}

const categoryName = (id) => categories.value.find(c => String(c.id) === String(id))?.name ?? '-'

const resetForm = () => {
  form.value = { name: '', category_id: null }
  editingId.value = null
  loadingSubmit.value = false
  errorSubmit.value = ''
  successSubmit.value = false
  successMessage.value = ''
}

const startEdit = (item) => {
  editingId.value = item.id
  form.value = {
    name: item.name ?? '',
    category_id: item.category_id ?? null
  }
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

const saveHandler = async () => {
  if (!form.value.name.trim()) { errorSubmit.value = 'Informe o nome.'; return }
  if (form.value.category_id == null) { errorSubmit.value = 'Selecione a categoria.'; return }

  loadingSubmit.value = true
  errorSubmit.value = ''
  successSubmit.value = false

  try {
    const payload = { name: form.value.name, category_id: form.value.category_id }
    if (editingId.value) {
      await apiPut(`/subcategories/${editingId.value}`, payload)
      successMessage.value = 'Subcategoria atualizada com sucesso!'
    } else {
      await apiPost('/subcategories', payload)
      successMessage.value = 'Subcategoria criada com sucesso!'
    }
    successSubmit.value = true
    await fetchItems()
    resetForm()
    setTimeout(() => { successSubmit.value = false; successMessage.value = '' }, 3000)
  } catch (e) {
    errorSubmit.value = 'Falha ao salvar subcategoria.'
    if (e?.response && (e.response.status === 401 || e.response.status === 403)) {
      errorSubmit.value = 'Acesso negado. Você precisa ser Admin.'
    } else if (e?.response?.data?.errors) {
      errorSubmit.value = Object.values(e.response.data.errors).flat().join(' ')
    }
  } finally {
    loadingSubmit.value = false
  }
}

const deleteHandler = async (id) => {
  if (!confirm(`Apagar subcategoria ID ${id}?`)) return
  loadingDelete[id] = true
  try {
    await apiDelete(`/subcategories/${id}`)
    if (editingId.value === id) resetForm()
    await fetchItems()
  } catch (e) {
  } finally {
    const t = { ...loadingDelete }
    delete t[id]
    Object.assign(loadingDelete, t)
  }
}

onMounted(async () => {
  await Promise.all([fetchItems(), fetchCategories()])
})
</script>
