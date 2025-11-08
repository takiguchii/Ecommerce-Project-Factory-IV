<template>
  <div class="bg-gray-900 min-h-screen p-4 md:p-8 pt-20">
    <div class="max-w-6xl mx-auto">
      <h1 class="text-3xl font-bold text-gray-100 mb-6">
        {{ editingId ? 'Editar Fornecedor' : 'Gerenciador de Fornecedores' }}
      </h1>

      <!-- FORM: controlado por toggle OU edição -->
      <div
        v-show="editingId || showCreate"
        class="bg-gray-800 shadow-xl rounded-lg p-6 md:p-8 mb-8"
      >
        <h3 class="text-2xl font-semibold text-gray-200 mb-6 border-b border-gray-700 pb-3">
          {{ editingId ? `Editando Fornecedor (ID: ${editingId})` : 'Adicionar Novo Fornecedor' }}
        </h3>

        <form @submit.prevent="saveHandler" class="grid grid-cols-1 md:grid-cols-2 gap-5">
          <div>
            <label class="block text-sm font-medium text-gray-400 mb-1">Nome</label>
            <input v-model.trim="form.name" type="text" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" required />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-400 mb-1">CNPJ</label>
            <input v-model.trim="form.cnpj" type="text" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" required />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-400 mb-1">E-mail</label>
            <input v-model.trim="form.email" type="email" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-400 mb-1">Telefone</label>
            <input v-model.trim="form.phone_number" type="text" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" />
          </div>
          <div class="md:col-span-2">
            <label class="block text-sm font-medium text-gray-400 mb-1">Endereço</label>
            <input v-model.trim="form.address" type="text" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" />
          </div>

          <div class="md:col-span-2 flex items-center gap-4 pt-2">
            <button type="submit" :disabled="loadingSubmit" class="px-5 py-2 bg-orange-600 text-white font-medium rounded-md hover:bg-orange-700 transition disabled:bg-gray-500">
              {{ loadingSubmit ? 'Salvando...' : (editingId ? 'Atualizar Fornecedor' : 'Criar Fornecedor') }}
            </button>
            <button v-if="editingId" type="button" @click="resetForm" class="px-5 py-2 bg-gray-600 text-white font-medium rounded-md hover:bg-gray-500 transition">
              Cancelar
            </button>
          </div>

          <p v-if="errorSubmit" class="md:col-span-2 text-red-500 text-sm font-medium">{{ errorSubmit }}</p>
          <p v-if="successSubmit" class="md:col-span-2 text-green-500 text-sm font-medium">{{ successMessage }}</p>
        </form>
      </div>

      <!-- LISTA -->
      <div class="bg-gray-800 shadow-xl rounded-lg overflow-hidden">
        <!-- Cabeçalho com botão à direita -->
        <div class="flex items-center justify-between p-6 border-b border-gray-700 flex-wrap gap-3">
          <h2 class="text-2xl font-semibold text-gray-200">Fornecedores</h2>

          <AdminCreateToggle
            v-model="showCreate"
            entity="Fornecedor"
            :forceOpen="!!editingId"
          />
        </div>

        <div v-if="loadingList" class="text-center p-10 text-gray-400">Carregando fornecedores...</div>
        <div v-else-if="errorList" class="text-center p-10 text-red-500">{{ errorList }}</div>

        <div v-else>
          <div v-if="displayed.length===0" class="text-center p-10 text-gray-500">Nenhum fornecedor encontrado.</div>

          <ul v-else class="divide-y divide-gray-700">
            <li v-for="item in displayed" :key="item.id" class="p-4 flex flex-wrap items-center justify-between gap-3 hover:bg-gray-700 transition">
              <div class="min-w-0">
                <p class="text-gray-100 font-medium truncate">{{ item.name }}</p>
                <p class="text-gray-400 text-sm">
                  ID: {{ item.id }} • CNPJ: {{ item.cnpj }}
                  <span v-if="item.email" class="ml-2">• {{ item.email }}</span>
                </p>
                <p class="text-gray-500 text-sm truncate" v-if="item.address">{{ item.address }}</p>
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
            <button class="px-3 py-2 rounded text-white bg-neutral-900 border border-neutral-700 text-sm disabled:opacity-50" :disabled="pageNumber === 1" @click="goToPage(pageNumber - 1)">Anterior</button>
            <template v-for="(p, idx) in visiblePages" :key="idx">
              <button v-if="p !== '...'" class="px-3 py-2 rounded border text-sm text-white" :class="p === pageNumber ? 'bg-orange-500 border-orange-500 text-black' : 'bg-neutral-900 border-neutral-700'" @click="goToPage(p)">{{ p }}</button>
              <span v-else class="px-2 text-neutral-400">…</span>
            </template>
            <button class="px-3 py-2 rounded text-white bg-neutral-900 border border-neutral-700 text-sm disabled:opacity-50" :disabled="pageNumber === totalPages" @click="goToPage(pageNumber + 1)">Próxima</button>
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
import { apiGet, apiPost, apiPut, apiDelete } from '@/services/api'
import AdminCreateToggle from '@/components/AdminCreateToggle.vue'

const router = useRouter()

/* Toggle de criação */
const showCreate = ref(false)

const providers = ref([])

const pageNumber = ref(1)
const pageSize = ref(10)
const totalCount = computed(() => providers.value.length)
const totalPages = computed(() => Math.max(1, Math.ceil(totalCount.value / pageSize.value)))
const displayed = computed(() => {
  const start = (pageNumber.value - 1) * pageSize.value
  return providers.value.slice(start, start + pageSize.value)
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
  cnpj: '',
  email: '',
  phone_number: '',
  address: ''
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

const fetchProviders = async () => {
  loadingList.value = true
  errorList.value = ''
  try {
    const data = await apiGet('/providers')
    providers.value = Array.isArray(data) ? data : []
    if (pageNumber.value > totalPages.value) pageNumber.value = totalPages.value
  } catch (e) {
    errorList.value = 'Falha ao carregar fornecedores.'
    if (e?.response && (e.response.status === 401 || e.response.status === 403)) logout()
  } finally {
    loadingList.value = false
  }
}

const resetForm = () => {
  form.value = { name: '', cnpj: '', email: '', phone_number: '', address: '' }
  editingId.value = null
  loadingSubmit.value = false
  errorSubmit.value = ''
  successSubmit.value = false
  successMessage.value = ''
  showCreate.value = false // fecha a área de criação após salvar/cancelar
}

const startEdit = (item) => {
  editingId.value = item.id
  form.value = {
    name: item.name ?? '',
    cnpj: item.cnpj ?? '',
    email: item.email ?? '',
    phone_number: item.phone_number ?? '',
    address: item.address ?? ''
  }
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

const saveHandler = async () => {
  loadingSubmit.value = true
  errorSubmit.value = ''
  successSubmit.value = false
  try {
    const payload = { ...form.value }
    if (editingId.value) {
      await apiPut(`/providers/${editingId.value}`, payload)
      successMessage.value = 'Fornecedor atualizado com sucesso!'
    } else {
      await apiPost('/providers', payload)
      successMessage.value = 'Fornecedor criado com sucesso!'
    }
    successSubmit.value = true
    await fetchProviders()
    resetForm()
    setTimeout(() => {
      successSubmit.value = false
      successMessage.value = ''
    }, 3000)
  } catch (e) {
    errorSubmit.value = 'Falha ao salvar fornecedor.'
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
  if (!confirm(`Apagar fornecedor ID ${id}?`)) return
  loadingDelete[id] = true
  try {
    await apiDelete(`/providers/${id}`)
    if (editingId.value === id) resetForm()
    await fetchProviders()
  } catch (e) {
  } finally {
    const t = { ...loadingDelete }
    delete t[id]
    Object.assign(loadingDelete, t)
  }
}

onMounted(fetchProviders)
</script>
