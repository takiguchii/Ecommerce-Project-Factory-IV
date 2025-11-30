<template>
  <div class="bg-gray-900 min-h-screen p-4 md:p-8 pt-20">
    <div class="max-w-4xl mx-auto">
      <h1 class="text-3xl font-bold text-gray-100 mb-6">
        {{ editingBrandId ? 'Editar Marca' : 'Gerenciador de Marcas' }}
      </h1>

      <!-- Card de criação/edição controlado pelo toggle OU pela edição -->
      <div
        v-show="editingBrandId || showCreate"
        class="bg-gray-800 shadow-xl rounded-lg p-6 md:p-8 mb-8"
      >
        <h3 class="text-2xl font-semibold text-gray-200 mb-6 border-b border-gray-700 pb-3">
          {{ editingBrandId ? `Editando Marca (ID: ${editingBrandId})` : 'Adicionar Nova Marca' }}
        </h3>

        <form @submit.prevent="saveBrandHandler" class="space-y-5">
          <div>
            <label for="brandName" class="block text-sm font-medium text-gray-400 mb-1">
              Nome da Marca
            </label>
            <input
              type="text"
              id="brandName"
              v-model="brandForm.name"
              class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md shadow-sm
                     focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 placeholder-gray-500"
              required
            />
          </div>

          <div>
            <label for="brandImageUrl" class="block text-sm font-medium text-gray-400 mb-1">
              URL da Imagem (Logo)
            </label>
            <input
              type="text"
              id="brandImageUrl"
              v-model="brandForm.imageUrl"
              class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md shadow-sm
                     focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 placeholder-gray-500"
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
              {{ loadingSubmit ? 'Salvando...' : (editingBrandId ? 'Atualizar Marca' : 'Criar Marca') }}
            </button>

            <button
              type="button"
              v-if="editingBrandId"
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

      <!-- Lista -->
      <div class="bg-gray-800 shadow-xl rounded-lg overflow-hidden">
        <!-- Cabeçalho com botão à direita -->
        <div class="flex items-center justify-between p-6 border-b border-gray-700 flex-wrap gap-3">
          <h2 class="text-2xl font-semibold text-gray-200">
            Marcas Existentes
          </h2>

          <!-- Botão toggle (some em modo edição) -->
          <AdminCreateToggle
            v-model="showCreate"
            entity="Marca"
            :forceOpen="!!editingBrandId"
          />
        </div>

        <div v-if="loadingList" class="text-center p-10 text-gray-400">
          Carregando marcas...
        </div>
        <div v-else-if="errorList" class="text-center p-10 text-red-500">
          {{ errorList }}
        </div>

        <ul v-else-if="brands.length > 0" class="divide-y divide-gray-700">
          <li
            v-for="brand in brands"
            :key="brand.id"
            class="p-4 flex flex-wrap items-center justify-between gap-3 hover:bg-gray-700 transition duration-150"
          >
            <span class="text-lg text-gray-100">
              {{ brand.name }}
              <span class="text-sm text-gray-500 ml-2">(ID: {{ brand.id }})</span>
            </span>

            <div class="flex-shrink-0 flex gap-2">
              <button
                @click="startEditHandler(brand)"
                class="px-3 py-1 bg-yellow-500 text-white text-sm font-medium rounded-md shadow-sm hover:bg-yellow-600 transition duration-200"
              >
                Editar
              </button>
              <button
                @click="deleteBrandHandler(brand.id)"
                :disabled="loadingDelete[brand.id]"
                class="px-3 py-1 bg-red-600 text-white text-sm font-medium rounded-md shadow-sm hover:bg-red-700 transition duration-200 disabled:bg-gray-500"
              >
                {{ loadingDelete[brand.id] ? '...' : 'Apagar' }}
              </button>
            </div>
          </li>
        </ul>

        <p v-else class="text-center p-10 text-gray-500">
          Nenhuma marca encontrada.
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
import { getBrands, createBrand, deleteBrand, updateBrand } from '@/services/api'
import AdminCreateToggle from '@/components/AdminCreateToggle.vue'

const router = useRouter()

const showCreate = ref(false)

const brandForm = ref({ name: '', imageUrl: '' })
const editingBrandId = ref(null)

const loadingSubmit = ref(false)
const errorSubmit = ref('')
const successSubmit = ref(false)
const successMessage = ref('')

const brands = ref([])
const loadingList = ref(false)
const errorList = ref('')

const loadingDelete = reactive({})
const errorDelete = reactive({})

const fetchBrands = async () => {
  loadingList.value = true
  errorList.value = ''
  try {
    brands.value = await getBrands()
  } catch (err) {
    console.error('Erro ao buscar marcas:', err)
    errorList.value = 'Falha ao carregar marcas.'
    if (err.response && (err.response.status === 401 || err.response.status === 403)) {
      logout()
    }
  } finally {
    loadingList.value = false
  }
}

const resetForm = () => {
  brandForm.value = { name: '', imageUrl: '' }
  editingBrandId.value = null
  loadingSubmit.value = false
  errorSubmit.value = ''
  successSubmit.value = false
  successMessage.value = ''
  showCreate.value = false // fecha a área de criação
}

const startEditHandler = (brand) => {
  editingBrandId.value = brand.id
  brandForm.value.name = brand.name
  brandForm.value.imageUrl = brand.brand_image_url || ''
  errorSubmit.value = ''
  successSubmit.value = false
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

const cancelEditHandler = () => {
  resetForm()
}

const saveBrandHandler = async () => {
  loadingSubmit.value = true
  errorSubmit.value = ''
  successSubmit.value = false

  try {
    const payload = {
      name: brandForm.value.name,
      brand_image_url: brandForm.value.imageUrl || null,
    }

    if (editingBrandId.value) {
      await updateBrand(editingBrandId.value, payload)
      successMessage.value = 'Marca atualizada com sucesso!'
    } else {
      await createBrand(payload)
      successMessage.value = 'Marca criada com sucesso!'
    }

    successSubmit.value = true
    resetForm()
    await fetchBrands()

    setTimeout(() => {
      successSubmit.value = false
      successMessage.value = ''
    }, 3000)
  } catch (err) {
    console.error('Erro ao salvar marca:', err)
    errorSubmit.value = 'Falha ao salvar marca.'
    if (err.response && (err.response.status === 401 || err.response.status === 403)) {
      errorSubmit.value = 'Acesso negado. Você precisa ser Admin.'
    } else if (err.response && err.response.data && err.response.data.errors) {
      errorSubmit.value = Object.values(err.response.data.errors).flat().join(' ')
    }
  } finally {
    loadingSubmit.value = false
  }
}

const deleteBrandHandler = async (id) => {
  if (!confirm(`Tem certeza que deseja apagar a marca com ID ${id}?`)) {
    return
  }
  loadingDelete[id] = true
  errorDelete[id] = ''
  try {
    await deleteBrand(id)
    if (editingBrandId.value === id) {
      resetForm()
    }
    await fetchBrands()
  } catch (err) {
    console.error(`Erro ao apagar marca ${id}:`, err)
    errorDelete[id] = 'Falha ao apagar.'
    if (err.response && (err.response.status === 401 || err.response.status === 403)) {
      errorDelete[id] = 'Acesso negado.'
    }
  } finally {
    const tempLoading = { ...loadingDelete }
    delete tempLoading[id]
    Object.assign(loadingDelete, tempLoading)
  }
}

const logout = () => {
  localStorage.removeItem('authToken')
  router.push('/login')
}

onMounted(fetchBrands)
</script>
