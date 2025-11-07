<template>
  <div class="bg-gray-900 min-h-screen p-4 md:p-8 pt-20">
    <div class="max-w-4xl mx-auto">

      <h1 class="text-3xl font-bold text-gray-100 mb-6">
        {{ editingCategoryId ? 'Editar Categoria' : 'Gerenciador de Categorias' }}
      </h1>

      <!-- FORM: controlado por toggle OU edição -->
      <div
        v-show="editingCategoryId || showCreate"
        class="bg-gray-800 shadow-xl rounded-lg p-6 md:p-8 mb-8"
      >
        <h3 class="text-2xl font-semibold text-gray-200 mb-6 border-b border-gray-700 pb-3">
          {{ editingCategoryId ? `Editando Categoria (ID: ${editingCategoryId})` : 'Adicionar Nova Categoria' }}
        </h3>

        <form @submit.prevent="saveCategoryHandler" class="space-y-5">
          <div>
            <label for="categoryName" class="block text-sm font-medium text-gray-400 mb-1">
              Nome da Categoria
            </label>
            <input
              type="text"
              id="categoryName"
              v-model.trim="categoryForm.name"
              class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md shadow-sm
                     focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 placeholder-gray-500"
              placeholder="Ex.: Notebooks"
              required
            />
          </div>

          <div class="flex items-center gap-4 pt-3">
            <button
              type="submit"
              :disabled="loadingSubmit"
              class="px-5 py-2 bg-orange-600 text-white font-medium rounded-md shadow-sm
                     hover:bg-orange-700 transition duration-200
                     disabled:bg-gray-500 disabled:cursor-not-allowed"
            >
              {{ loadingSubmit ? 'Salvando...' : (editingCategoryId ? 'Atualizar Categoria' : 'Criar Categoria') }}
            </button>

            <button
              v-if="editingCategoryId"
              type="button"
              @click="cancelEditHandler"
              class="px-5 py-2 bg-gray-600 text-white font-medium rounded-md shadow-sm hover:bg-gray-500 transition duration-200"
            >
              Cancelar
            </button>
          </div>

          <p v-if="errorSubmit" class="text-red-500 text-sm font-medium pt-2">{{ errorSubmit }}</p>
          <p v-if="successSubmit" class="text-green-500 text-sm font-medium pt-2">{{ successMessage }}</p>
        </form>
      </div>

      <!-- LISTA -->
      <div class="bg-gray-800 shadow-xl rounded-lg overflow-hidden">
        <!-- Cabeçalho com botão à direita -->
        <div class="flex items-center justify-between p-6 border-b border-gray-700 flex-wrap gap-3">
          <h2 class="text-2xl font-semibold text-gray-200">
            Categorias Existentes
          </h2>

          <AdminCreateToggle
            v-model="showCreate"
            entity="Categoria"
            :forceOpen="!!editingCategoryId"
          />
        </div>

        <div v-if="loadingList" class="text-center p-10 text-gray-400">
          Carregando categorias...
        </div>
        <div v-else-if="errorList" class="text-center p-10 text-red-500">
          {{ errorList }}
        </div>

        <ul v-else-if="categories.length > 0" class="divide-y divide-gray-700">
          <li
            v-for="cat in categories"
            :key="cat.id"
            class="p-4 flex flex-wrap items-center justify-between gap-3 hover:bg-gray-700 transition duration-150"
          >
            <span class="text-lg text-gray-100">
              {{ cat.name }}
              <span class="text-sm text-gray-500 ml-2">(ID: {{ cat.id }})</span>
            </span>

            <div class="flex-shrink-0 flex gap-2">
              <button
                @click="startEditHandler(cat)"
                class="px-3 py-1 bg-yellow-500 text-white text-sm font-medium rounded-md shadow-sm hover:bg-yellow-600 transition duration-200"
              >
                Editar
              </button>
              <button
                @click="deleteCategoryHandler(cat.id)"
                :disabled="loadingDelete[cat.id]"
                class="px-3 py-1 bg-red-600 text-white text-sm font-medium rounded-md shadow-sm hover:bg-red-700 transition duration-200 disabled:bg-gray-500"
              >
                {{ loadingDelete[cat.id] ? '...' : 'Apagar' }}
              </button>
            </div>
          </li>
        </ul>

        <p v-else class="text-center p-10 text-gray-500">
          Nenhuma categoria encontrada.
        </p>
      </div>

      <button
        @click="logout"
        class="mt-8 px-5 py-2 bg-gray-700 text-white font-medium rounded-md shadow-sm
               hover:bg-gray-600 transition duration-200"
      >
        Sair (Logout)
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { getCategories, createCategory, updateCategory, deleteCategory } from '@/services/api'
import AdminCreateToggle from '@/components/AdminCreateToggle.vue' // ✅

const router = useRouter()

// Toggle de criação
const showCreate = ref(false)

// ----- STATE
const categoryForm = ref({ name: '' })
const editingCategoryId = ref(null)

const loadingSubmit = ref(false)
const errorSubmit = ref('')
const successSubmit = ref(false)
const successMessage = ref('')

const categories = ref([])
const loadingList = ref(false)
const errorList = ref('')

const loadingDelete = reactive({})
const errorDelete = reactive({})

// ----- HELPERS
const resetForm = () => {
  categoryForm.value = { name: '' }
  editingCategoryId.value = null
  loadingSubmit.value = false
  errorSubmit.value = ''
  successSubmit.value = false
  successMessage.value = ''
  showCreate.value = false // ✅ fecha o create após salvar/cancelar
}

const logout = () => {
  localStorage.removeItem('authToken')
  router.push('/login')
}

// ----- API
const fetchCategories = async () => {
  loadingList.value = true
  errorList.value = ''
  try {
    categories.value = await getCategories()
  } catch (err) {
    console.error('Erro ao buscar categorias:', err)
    errorList.value = 'Falha ao carregar categorias.'
    if (err?.response && (err.response.status === 401 || err.response.status === 403)) {
      logout()
    }
  } finally {
    loadingList.value = false
  }
}

const startEditHandler = (cat) => {
  editingCategoryId.value = cat.id
  categoryForm.value.name = cat.name ?? ''
  errorSubmit.value = ''
  successSubmit.value = false
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

const cancelEditHandler = () => resetForm()

const saveCategoryHandler = async () => {
  loadingSubmit.value = true
  errorSubmit.value = ''
  successSubmit.value = false

  try {
    const payload = { name: categoryForm.value.name }

    if (editingCategoryId.value) {
      await updateCategory(editingCategoryId.value, payload)
      successMessage.value = 'Categoria atualizada com sucesso!'
    } else {
      await createCategory(payload)
      successMessage.value = 'Categoria criada com sucesso!'
    }

    successSubmit.value = true
    resetForm()
    await fetchCategories()

    setTimeout(() => {
      successSubmit.value = false
      successMessage.value = ''
    }, 3000)
  } catch (err) {
    console.error('Erro ao salvar categoria:', err)
    errorSubmit.value = 'Falha ao salvar categoria.'
    if (err?.response && (err.response.status === 401 || err.response.status === 403)) {
      errorSubmit.value = 'Acesso negado. Você precisa ser Admin.'
    } else if (err?.response?.data?.errors) {
      errorSubmit.value = Object.values(err.response.data.errors).flat().join(' ')
    }
  } finally {
    loadingSubmit.value = false
  }
}

const deleteCategoryHandler = async (id) => {
  if (!confirm(`Tem certeza que deseja apagar a categoria com ID ${id}?`)) return
  loadingDelete[id] = true
  errorDelete[id] = ''
  try {
    await deleteCategory(id)
    if (editingCategoryId.value === id) resetForm()
    await fetchCategories()
  } catch (err) {
    console.error(`Erro ao apagar categoria ${id}:`, err)
    errorDelete[id] = 'Falha ao apagar.'
    if (err?.response && (err.response.status === 401 || err.response.status === 403)) {
      errorDelete[id] = 'Acesso negado.'
    }
  } finally {
    const temp = { ...loadingDelete }
    delete temp[id]
    Object.assign(loadingDelete, temp)
  }
}

onMounted(fetchCategories)
</script>
