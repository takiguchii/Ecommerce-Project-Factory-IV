<template>
    <div class="bg-gray-900 min-h-screen p-4 md:p-8 pt-20">
        <div class="max-w-6xl mx-auto">
            <h1 class="text-3xl font-bold text-gray-100 mb-6">
                {{ editingId ? 'Editar Cupom' : 'Gerenciador de Cupons' }}
            </h1>

            <!-- FORM -->
            <div v-show="editingId || showCreate" class="bg-gray-800 shadow-xl rounded-lg p-6 md:p-8 mb-8">
                <h3 class="text-2xl font-semibold text-gray-200 mb-6 border-b border-gray-700 pb-3">
                    {{ editingId ? `Editando Cupom (ID: ${editingId})` : 'Adicionar Novo Cupom' }}
                </h3>

                <form @submit.prevent="saveHandler" class="grid grid-cols-1 md:grid-cols-2 gap-5">
                    <div>
                        <label class="block text-sm font-medium text-gray-400 mb-1">Código *</label>
                        <input v-model.trim="form.code" type="text" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md
                     focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" required />
                    </div>

                    <div>
                        <label class="block text-sm font-medium text-gray-400 mb-1">Descrição</label>
                        <input v-model.trim="form.description" type="text" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md
                     focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" />
                    </div>

                    <div>
                        <label class="block text-sm font-medium text-gray-400 mb-1">Valor do Desconto *</label>
                        <input v-model.number="form.discountValue" type="number" step="0.01" min="0" class="w-full px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md
                     focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" required />
                        <p class="text-xs text-gray-400 mt-1" v-if="form.isPercentage">Em % (ex.: 10 = 10%)</p>
                        <p class="text-xs text-gray-400 mt-1" v-else>Valor fixo (ex.: 25 = R$ 25,00)</p>
                    </div>

                    <div class="flex items-center gap-10 mt-2">
                        <div class="flex items-center gap-3">
                            <span class="text-sm text-gray-300">Percentual:</span>
                            <button type="button" @click="form.isPercentage = !form.isPercentage"
                                class="relative w-12 h-6 rounded-full transition-colors duration-300"
                                :class="form.isPercentage ? 'bg-orange-500' : 'bg-gray-600'">
                                <span
                                    class="absolute top-0.5 left-0.5 w-5 h-5 bg-white rounded-full transition-transform duration-300"
                                    :class="form.isPercentage ? 'translate-x-6' : 'translate-x-0'"></span>
                            </button>
                        </div>

                        <!-- Toggle: Ativo -->
                        <div class="flex items-center gap-3">
                            <span class="text-sm text-gray-300">Ativo:</span>
                            <button type="button" @click="form.isActive = !form.isActive"
                                class="relative w-12 h-6 rounded-full transition-colors duration-300"
                                :class="form.isActive ? 'bg-green-500' : 'bg-gray-600'">
                                <span
                                    class="absolute top-0.5 left-0.5 w-5 h-5 bg-white rounded-full transition-transform duration-300"
                                    :class="form.isActive ? 'translate-x-6' : 'translate-x-0'"></span>
                            </button>
                        </div>
                    </div>


                    <div class="md:col-span-2">
                        <label class="block text-sm font-medium text-gray-400 mb-1">Data de Expiração</label>
                        <input v-model="form.expiryDate" type="date" class="w-full md:w-1/2 px-4 py-2 border border-gray-600 bg-gray-700 text-gray-100 rounded-md
                     focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500" />
                    </div>

                    <div class="md:col-span-2 flex items-center gap-4 pt-2">
                        <button type="submit" :disabled="loadingSubmit"
                            class="px-5 py-2 bg-orange-600 text-white font-medium rounded-md hover:bg-orange-700 transition disabled:bg-gray-500">
                            {{ loadingSubmit ? 'Salvando...' : (editingId ? 'Atualizar Cupom' : 'Criar Cupom') }}
                        </button>
                        <button v-if="editingId" type="button" @click="resetForm"
                            class="px-5 py-2 bg-gray-600 text-white font-medium rounded-md hover:bg-gray-500 transition">
                            Cancelar
                        </button>
                    </div>

                    <p v-if="errorSubmit" class="md:col-span-2 text-red-500 text-sm font-medium">{{ errorSubmit }}</p>
                    <p v-if="successSubmit" class="md:col-span-2 text-green-500 text-sm font-medium">{{ successMessage
                        }}</p>
                </form>
            </div>

            <!-- LISTA -->
            <div class="bg-gray-800 shadow-xl rounded-lg overflow-hidden">
                <div class="flex items-center justify-between p-6 border-b border-gray-700 flex-wrap gap-3">
                    <h2 class="text-2xl font-semibold text-gray-200">Cupons</h2>
                    <AdminCreateToggle v-model="showCreate" entity="Cupom" :forceOpen="!!editingId" />
                </div>

                <div v-if="loadingList" class="text-center p-10 text-gray-400">Carregando cupons...</div>
                <div v-else-if="errorList" class="text-center p-10 text-red-500">{{ errorList }}</div>

                <div v-else>
                    <div v-if="displayed.length === 0" class="text-center p-10 text-gray-500">Nenhum cupom encontrado.
                    </div>

                    <ul v-else class="divide-y divide-gray-700">
                        <li v-for="item in displayed" :key="item.id || item.code"
                            class="p-4 flex flex-wrap items-center justify-between gap-3 hover:bg-gray-700 transition">
                            <div class="min-w-0">
                                <p class="text-gray-100 font-medium truncate">{{ item.code }}</p>
                                <p class="text-gray-400 text-sm">
                                    {{ item.description || '—' }}
                                    <span class="ml-2">• {{ item.isPercentage ? item.discountValue + '%' : 'R$ ' +
                                        number(item.discountValue) }}</span>
                                    <span class="ml-2">• {{ item.expiryDate ? toInputDate(item.expiryDate) : 'Sem expiração' }}</span>
                                    <span class="ml-2">• {{ item.isActive ? 'Ativo' : 'Inativo' }}</span>
                                </p>
                            </div>
                            <div class="flex-shrink-0 flex gap-2">
                                <button @click="startEdit(item)"
                                    class="px-3 py-1 bg-yellow-500 text-white text-sm font-medium rounded-md hover:bg-yellow-600 transition">
                                    Editar
                                </button>
                                <button @click="deleteHandler(item.id)" :disabled="loadingDelete[item.id]"
                                    class="px-3 py-1 bg-red-600 text-white text-sm font-medium rounded-md hover:bg-red-700 transition disabled:bg-gray-500">
                                    {{ loadingDelete[item.id] ? '...' : 'Apagar' }}
                                </button>
                            </div>
                        </li>
                    </ul>

                    <div v-if="totalPages > 1" class="mt-8 flex flex-wrap items-center justify-center gap-2">
                        <button v-for="p in totalPages" :key="p" @click="goToPage(p)"
                            class="px-3 py-2 rounded border text-sm"
                            :class="p === pageNumber ? 'bg-orange-500 border-orange-500 text-black' : 'bg-neutral-900 border-neutral-700 text-white'">
                            {{ p }}
                        </button>
                    </div>
                </div>
            </div>

            <button @click="logout"
                class="mt-8 px-5 py-2 bg-gray-700 text-white font-medium rounded-md hover:bg-gray-600 transition">
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

/* Estado */
const showCreate = ref(false)
const coupons = ref([])
const editingId = ref(null)

const form = ref({
    code: '',
    description: '',
    discountValue: 0,
    isPercentage: true,
    expiryDate: '', // YYYY-MM-DD
    isActive: true
})

/* Feedback */
const loadingList = ref(false)
const errorList = ref('')
const loadingSubmit = ref(false)
const errorSubmit = ref('')
const successSubmit = ref(false)
const successMessage = ref('')
const loadingDelete = reactive({})

/* Paginação (simples) */
const pageNumber = ref(1)
const pageSize = ref(10)
const totalPages = computed(() =>
    Math.max(1, Math.ceil(coupons.value.length / pageSize.value))
)
const displayed = computed(() => {
    const start = (pageNumber.value - 1) * pageSize.value
    return coupons.value.slice(start, start + pageSize.value)
})
const goToPage = (p) => {
    if (p < 1 || p > totalPages.value) return
    pageNumber.value = p
    window.scrollTo({ top: 0, behavior: 'smooth' })
}

/* Helpers */
const number = (v) => Number(v).toFixed(2)
const toInputDate = (value) => (value ? new Date(value).toISOString().split('T')[0] : '')
const fromInputDate = (value) => (value ? new Date(`${value}T00:00:00`).toISOString() : null)

/* API */
const fetchCoupons = async () => {
    loadingList.value = true
    errorList.value = ''
    try {
        const data = await apiGet('/Coupon')
        coupons.value = Array.isArray(data) ? data : (data ? [data] : [])
        if (pageNumber.value > totalPages.value) pageNumber.value = totalPages.value
    } catch (e) {
        errorList.value = 'Falha ao carregar cupons.'
        if (e?.response && (e.response.status === 401 || e.response.status === 403)) logout()
    } finally {
        loadingList.value = false
    }
}

const buildPayload = () => ({
    code: form.value.code,
    description: form.value.description,
    discountValue: Number(form.value.discountValue),
    isPercentage: !!form.value.isPercentage,
    expiryDate: fromInputDate(form.value.expiryDate),
    isActive: !!form.value.isActive
})
const saveCoupon = (id, payload) =>
    id ? apiPut(`/Coupon/${id}`, payload) : apiPost('/Coupon', payload)

/* Ações */
const resetForm = () => {
    form.value = { code: '', description: '', discountValue: 0, isPercentage: true, expiryDate: '', isActive: true }
    editingId.value = null
    loadingSubmit.value = false
    errorSubmit.value = ''
    successSubmit.value = false
    successMessage.value = ''
    showCreate.value = false
}

const startEdit = (item) => {
    editingId.value = item.id
    form.value = {
        code: item.code ?? '',
        description: item.description ?? '',
        discountValue: Number(item.discountValue ?? 0),
        isPercentage: !!item.isPercentage,
        expiryDate: toInputDate(item.expiryDate),
        isActive: !!item.isActive
    }
    window.scrollTo({ top: 0, behavior: 'smooth' })
}

const saveHandler = async () => {
    loadingSubmit.value = true
    errorSubmit.value = ''
    successSubmit.value = false
    try {
        await saveCoupon(editingId.value, buildPayload())
        successMessage.value = editingId.value ? 'Cupom atualizado com sucesso!' : 'Cupom criado com sucesso!'
        successSubmit.value = true
        await fetchCoupons()
        resetForm()
        setTimeout(() => { successSubmit.value = false; successMessage.value = '' }, 3000)
    } catch (e) {
        errorSubmit.value = 'Falha ao salvar cupom.'
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
    if (!id || !confirm(`Apagar cupom ID ${id}?`)) return
    loadingDelete[id] = true
    try {
        await apiDelete(`/Coupon/${id}`)
        if (editingId.value === id) resetForm()
        await fetchCoupons()
    } finally {
        const t = { ...loadingDelete }; delete t[id]; Object.assign(loadingDelete, t)
    }
}

const logout = () => {
    localStorage.removeItem('authToken')
    router.push('/login')
}

onMounted(fetchCoupons)
</script>
