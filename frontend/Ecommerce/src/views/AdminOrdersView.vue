<script setup>
import { ref, computed, onMounted } from 'vue'

const orders = ref([])

onMounted(() => {
    const stored = localStorage.getItem('orderHistory')
    if (stored) {
        try {
            orders.value = JSON.parse(stored)
        } catch (e) {
            console.error('Erro ao ler orderHistory do localStorage', e)
            orders.value = []
        }
    }
})

const sortedOrders = computed(() =>
    [...orders.value].sort(
        (a, b) => new Date(b.createdAt) - new Date(a.createdAt),
    ),
)

const formatCurrency = (value) => {
    if (value == null) return 'R$ 0,00'
    return Number(value).toLocaleString('pt-BR', {
        style: 'currency',
        currency: 'BRL',
    })
}

const formatDateTime = (isoString) => {
    if (!isoString) return '—'
    const d = new Date(isoString)
    return d.toLocaleString('pt-BR')
}

const totalItemsCount = (order) =>
    order.items?.reduce((acc, i) => acc + (i.quantity || 0), 0) ?? 0

function getCustomerName(order) {
    return (
        order.customerName ||
        order.fullName ||
        order.userName ||
        order.profile?.fullName ||
        'Cliente'
    )
}

function getCustomerDocument(order) {
    return (
        order.userCpfCnpj ||
        order.cpfCnpj ||
        order.cpf ||
        order.documentCpfCnpj ||
        order.document ||
        order.profile?.documentCpfCnpj ||
        ''
    )
}
</script>

<template>
    <div class="min-h-screen bg-gray-900 pt-20 pb-10 px-4 sm:px-6 lg:px-8">
        <div class="max-w-5xl mx-auto">

            <header class="flex items-center justify-between mb-6">
                <div>
                    <h1 class="text-2xl md:text-3xl font-bold text-gray-100">
                        Pedidos dos Clientes
                    </h1>
                    <p class="text-sm text-gray-400 mt-1">
                        Histórico de compras registradas nesta loja.
                    </p>
                </div>
            </header>

            <div
                v-if="sortedOrders.length === 0"
                class="bg-gray-800 border border-gray-700 rounded-xl p-8 text-center text-gray-300"
            >
                Nenhum pedido encontrado no histórico local.
            </div>

            <div v-else class="space-y-4">
                <div
                    v-for="order in sortedOrders"
                    :key="order.id"
                    class="bg-gray-800 border border-gray-700 rounded-xl px-4 py-4 md:px-6 md:py-5"
                >

                    <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-3">

                        <div class="space-y-1">
                            <p class="text-sm text-gray-400">
                                Pedido
                                <span class="font-semibold text-gray-100">
                                    #{{ order.id }}
                                </span>
                            </p>

                            <p class="text-sm text-gray-300">
                                {{ getCustomerName(order) }}
                                <span
                                    v-if="order.userEmail"
                                    class="text-xs text-gray-400"
                                >
                                    · {{ order.userEmail }}
                                </span>
                            </p>

                            <p
                                v-if="getCustomerDocument(order)"
                                class="text-xs text-gray-400"
                            >
                                CPF/CNPJ: {{ getCustomerDocument(order) }}
                            </p>

                            <p class="text-xs text-gray-500">
                                {{ formatDateTime(order.createdAt) }}
                            </p>
                        </div>

                        <!-- NOVO BLOCO ALINHADO -->
                        <div
                            class="grid grid-cols-1 sm:grid-cols-3 gap-6 w-full md:w-auto 
                                   bg-gray-900/40 p-5 rounded-lg border border-gray-700 
                                   justify-items-center text-center"
                        >
                            <!-- Pagamento -->
                            <div class="space-y-1">
                                <p class="text-xs text-gray-400 uppercase tracking-wide">
                                    Pagamento
                                </p>
                                <p class="text-sm font-semibold text-gray-100">
                                    {{ order.paymentMethod }}
                                </p>
                            </div>

                            <!-- Frete -->
                            <div class="space-y-1">
                                <p class="text-xs text-gray-400 uppercase tracking-wide">
                                    Frete
                                </p>
                                <p class="text-sm text-gray-100">
                                    {{ order.shippingOption?.name || '—' }}
                                    <span
                                        v-if="order.shippingOption?.carrier"
                                        class="text-xs text-gray-400"
                                    >
                                        ({{ order.shippingOption.carrier }})
                                    </span>
                                </p>
                                <p class="text-xs text-gray-400">
                                    {{ formatCurrency(order.shippingPrice) }}
                                    <span
                                        v-if="order.shippingOption?.estimatedDeliveryDays"
                                    >
                                        · {{ order.shippingOption.estimatedDeliveryDays }} dias úteis
                                    </span>
                                </p>
                            </div>

                            <!-- Total -->
                            <div class="space-y-1">
                                <p class="text-xs text-gray-400 uppercase tracking-wide">
                                    Total
                                </p>
                                <p class="text-xl font-bold text-orange-400">
                                    {{ formatCurrency(order.total) }}
                                </p>
                            </div>
                        </div>
                    </div>

                    <div
                        class="mt-3 flex flex-wrap items-center justify-between gap-2 border-t border-gray-700 pt-3"
                    >
                        <p class="text-xs text-gray-400">
                            {{ totalItemsCount(order) }} itens neste pedido
                        </p>
                    </div>

                    <div
                        class="mt-3 bg-gray-900/40 rounded-lg border border-gray-700/60 px-3 py-3"
                    >
                        <p class="text-xs font-semibold text-gray-300 uppercase mb-2">
                            Itens
                        </p>

                        <div class="space-y-1.5 max-h-40 overflow-auto pr-1">
                            <div
                                v-for="item in order.items"
                                :key="item.id"
                                class="flex items-center justify-between text-xs md:text-sm text-gray-200"
                            >
                                <div class="flex-1 min-w-0">
                                    <p class="truncate">
                                        {{ item.name }}
                                    </p>
                                    <p class="text-[11px] text-gray-400">
                                        Qtd: {{ item.quantity }}
                                    </p>
                                </div>
                                <div class="text-right ml-3">
                                    <p class="text-[11px] text-gray-400">
                                        Unitário
                                    </p>
                                    <p>
                                        {{ formatCurrency(item.price) }}
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</template>
