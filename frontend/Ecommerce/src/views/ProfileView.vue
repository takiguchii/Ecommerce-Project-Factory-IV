<template>
  <div class="min-h-screen bg-neutral-950 text-white">
    <div class="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8 pt-24 pb-10">
      <!-- Header -->
      <header class="mb-8">
        <h2 class="text-3xl font-semibold tracking-tight text-orange-400">
          Meu Perfil
        </h2>
        <p class="text-sm text-neutral-100 mt-2">
          Complete seus dados para agilizar suas futuras compras.
        </p>
      </header>

      <!-- Alerts  -->
      <div
        v-if="isLoading"
        class="mb-4 rounded-xl border border-blue-500/30 bg-blue-500/10 text-blue-200 px-4 py-3 shadow"
        role="alert"
      >
        <span class="font-medium">Carregando dados…</span>
      </div>
      <div
        v-if="error"
        class="mb-4 rounded-xl border border-red-500/30 bg-red-500/10 text-red-200 px-4 py-3 shadow"
        role="alert"
      >
        {{ error }}
      </div>
      <transition name="fade-in-out">
        <div
          v-if="successMessage"
          class="mb-4 rounded-xl border border-green-500/30 bg-green-500/10 text-green-200 px-4 py-3 shadow"
          role="alert"
        >
          {{ successMessage }}
        </div>
      </transition>

      <!-- Form -->
      <form v-if="profile" @submit.prevent="handleSubmit" class="flex flex-col gap-8">
        <!-- Tabs -->
        <nav class="rounded-2xl bg-neutral-900/50 border border-neutral-800 p-1.5 shadow-lg">
          <ul class="grid grid-cols-4 gap-1">
            <li>
              <button
                type="button"
                :class="tabBtnClass('conta')"
                @click="activeTab = 'conta'"
                aria-controls="tab-conta"
                :aria-selected="activeTab === 'conta'"
                role="tab"
              >
                Dados da Conta
              </button>
            </li>
            <li>
              <button
                type="button"
                :class="tabBtnClass('pessoais')"
                @click="activeTab = 'pessoais'"
                aria-controls="tab-pessoais"
                :aria-selected="activeTab === 'pessoais'"
                role="tab"
              >
                Dados Pessoais
              </button>
            </li>
            <li>
              <button
                type="button"
                :class="tabBtnClass('endereco')"
                @click="activeTab = 'endereco'"
                aria-controls="tab-endereco"
                :aria-selected="activeTab === 'endereco'"
                role="tab"
              >
                Endereço de Entrega
              </button>
            </li>
            <li>
              <button
                type="button"
                :class="tabBtnClass('orders')"
                @click="activeTab = 'orders'"
                aria-controls="tab-orders"
                :aria-selected="activeTab === 'orders'"
                role="tab"
              >
                Pedidos
              </button>
            </li>
          </ul>
        </nav>

        <!-- Panel: Conta -->
        <section
          id="tab-conta"
          role="tabpanel"
          v-show="activeTab === 'conta'"
          class="relative overflow-hidden rounded-2xl border border-neutral-800 bg-gradient-to-b from-neutral-900/70 to-neutral-950/60 backdrop-blur-md shadow-xl p-8 transition-all duration-300"
        >
          <div class="mb-6 flex items-center gap-3">
            <div class="flex h-10 w-10 items-center justify-center rounded-xl bg-orange-500/10 text-orange-400">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-5 w-5"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M5.121 17.804A13.937 13.937 0 0112 15c2.489 0 4.823.607 6.879 1.804M15 10a3 3 0 11-6 0 3 3 0 016 0zM19.5 21a9.5 9.5 0 10-15 0"
                />
              </svg>
            </div>
            <h3 class="text-xl font-semibold text-white tracking-tight">
              Informações da Conta
            </h3>
          </div>

          <div class="grid grid-cols-1 sm:grid-cols-2 gap-8">
            <!-- Username -->
            <div class="field group">
              <label for="username" class="form-label text-neutral-300">
                Username
              </label>
              <div
                class="flex items-center gap-2 bg-neutral-900/60 border border-neutral-800 rounded-xl px-3 py-2.5 shadow-inner transition group-hover:border-orange-500/40"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4 text-neutral-500 group-hover:text-orange-400"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M5.121 17.804A13.937 13.937 0 0112 15c2.489 0 4.823.607 6.879 1.804M15 10a3 3 0 11-6 0 3 3 0 016 0zM19.5 21a9.5 9.5 0 10-15 0"
                  />
                </svg>
                <input
                  type="text"
                  id="username"
                  :value="profile.username"
                  class="w-full bg-transparent border-none focus:ring-0 text-neutral-100 placeholder-neutral-500 cursor-not-allowed"
                  disabled
                />
              </div>
            </div>

            <div class="field group">
              <label for="email" class="form-label text-neutral-300">
                Email
              </label>
              <div
                class="flex items-center gap-2 bg-neutral-900/60 border border-neutral-800 rounded-xl px-3 py-2.5 shadow-inner transition group-hover:border-orange-500/40"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4 text-neutral-500 group-hover:text-orange-400"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                >
                  <path
                    d="M2.94 6.94A2 2 0 014.414 6h11.172a2 2 0 011.475.94L10 11 2.94 6.94z"
                  />
                  <path
                    d="M18 8.118l-8 4.882-8-4.882V14a2 2 0 002 2h12a2 2 0 002-2V8.118z"
                  />
                </svg>
                <input
                  type="email"
                  id="email"
                  :value="profile.email"
                  class="w-full bg-transparent border-none focus:ring-0 text-neutral-100 placeholder-neutral-500 cursor-not-allowed"
                  disabled
                />
              </div>
            </div>
          </div>
        </section>

        <!-- Panel: Dados Pessoais -->
        <section
          id="tab-pessoais"
          role="tabpanel"
          v-show="activeTab === 'pessoais'"
          class="relative overflow-hidden rounded-2xl border border-neutral-800 bg-gradient-to-b from-neutral-900/70 to-neutral-950/60 backdrop-blur-md shadow-xl p-8 transition-all duration-300"
        >
          <div class="mb-6 flex items-center gap-3">
            <div class="flex h-10 w-10 items-center justify-center rounded-xl bg-orange-500/10 text-orange-400">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-5 w-5"
                viewBox="0 0 24 24"
                fill="currentColor"
              >
                <path d="M2 7a2 2 0 012-2h16a2 2 0 012 2v10a2 2 0 01-2 2H4a2 2 0 01-2-2V7z" />
                <path
                  class="opacity-70"
                  d="M7 10.5h6a1 1 0 000-2H7a1 1 0 000 2zM7 14h4a1 1 0 100-2H7a1 1 0 100 2z"
                />
              </svg>
            </div>
            <h3 class="text-xl font-semibold text-white tracking-tight">
              Dados Pessoais
            </h3>
          </div>

          <div class="grid grid-cols-1 sm:grid-cols-2 gap-8">
            <div class="sm:col-span-2 field group">
              <label for="fullName" class="form-label text-neutral-300">
                Nome Completo
              </label>
              <div
                class="flex items-center gap-2 bg-neutral-900/60 border border-neutral-800 rounded-xl px-3 py-2.5 shadow-inner transition group-hover:border-orange-500/40"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4 text-neutral-500 group-hover:text-orange-400"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M5.121 17.804A13.937 13.937 0 0112 15c2.489 0 4.823.607 6.879 1.804M15 10a3 3 0 11-6 0 3 3 0 016 0zM19.5 21a9.5 9.5 0 10-15 0"
                  />
                </svg>
                <input
                  type="text"
                  id="fullName"
                  v-model="profile.fullName"
                  placeholder="Digite seu nome completo"
                  class="w-full bg-transparent border-none focus:ring-0 text-neutral-100 placeholder-neutral-500"
                />
              </div>
            </div>

            <div class="field group">
              <label for="document" class="form-label text-neutral-300">
                CPF/CNPJ
              </label>
              <div
                class="flex items-center gap-2 bg-neutral-900/60 border border-neutral-800 rounded-xl px-3 py-2.5 shadow-inner transition group-hover:border-orange-500/40"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4 text-neutral-500 group-hover:text-orange-400"
                  viewBox="0 0 24 24"
                  fill="currentColor"
                >
                  <path d="M6 2a2 2 0 00-2 2v16l4-2 4 2 4-2 4 2V4a2 2 0 00-2-2H6z" />
                </svg>
                <input
                  type="text"
                  id="document"
                  v-model="profile.documentCpfCnpj"
                  placeholder="Digite seu CPF ou CNPJ"
                  class="w-full bg-transparent border-none focus:ring-0 text-neutral-100 placeholder-neutral-500"
                />
              </div>
            </div>

            <div class="field group">
              <label for="phone" class="form-label text-neutral-300">
                Telefone
              </label>
              <div
                class="flex items-center gap-2 bg-neutral-900/60 border border-neutral-800 rounded-xl px-3 py-2.5 shadow-inner transition group-hover:border-orange-500/40"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4 text-neutral-500 group-hover:text-orange-400"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                >
                  <path
                    d="M2 3a1 1 0 011-1h3l2 4-2 1a11 11 0 006 6l1-2 4 2v3a1 1 0 01-1 1h-1C8.82 17 3 11.18 3 4V3z"
                  />
                </svg>
                <input
                  type="tel"
                  id="phone"
                  v-model="profile.phone"
                  placeholder="(XX) XXXXX-XXXX"
                  class="w-full bg-transparent border-none focus:ring-0 text-neutral-100 placeholder-neutral-500"
                />
              </div>
            </div>
          </div>
        </section>

        <!-- Panel: Endereço -->
        <section
          id="tab-endereco"
          role="tabpanel"
          v-show="activeTab === 'endereco'"
          class="relative overflow-hidden rounded-2xl border border-neutral-800 bg-gradient-to-b from-neutral-900/70 to-neutral-950/60 backdrop-blur-md shadow-xl p-8 transition-all duration-300"
        >
          <div class="mb-6 flex items-center gap-3">
            <div class="flex h-10 w-10 items-center justify-center rounded-xl bg-orange-500/10 text-orange-400">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-5 w-5"
                viewBox="0 0 24 24"
                fill="currentColor"
              >
                <path
                  d="M12 2a7 7 0 00-7 7c0 5.25 7 13 7 13s7-7.75 7-13a7 7 0 00-7-7zm0 9.5a2.5 2.5 0 110-5 2.5 2.5 0 010 5z"
                />
              </svg>
            </div>
            <h3 class="text-xl font-semibold text-white tracking-tight">
              Endereço de Entrega
            </h3>
          </div>

          <div class="grid grid-cols-1 sm:grid-cols-12 gap-8">
            <div class="sm:col-span-3 field group">
              <label for="postalCode" class="form-label text-neutral-300">
                CEP
              </label>
              <div
                class="flex items-center gap-2 bg-neutral-900/60 border border-neutral-800 rounded-xl px-3 py-2.5 shadow-inner transition group-hover:border-orange-500/40"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4 text-neutral-500 group-hover:text-orange-400"
                  viewBox="0 0 24 24"
                  fill="currentColor"
                >
                  <path
                    d="M9 3h2l-1 6h4l1-6h2l-1 6h4v2h-4l-1 6h4v2h-4l-1 6h-2l1-6H9l-1 6H6l1-6H3v-2h4l1-6H4V9h4l1-6zM10 15h4l1-6h-4l-1 6z"
                  />
                </svg>
                <input
                  type="text"
                  id="postalCode"
                  v-model="profile.postalCode"
                  placeholder="XXXXX-XXX"
                  class="w-full bg-transparent border-none focus:ring-0 text-neutral-100 placeholder-neutral-500"
                />
              </div>
            </div>

            <div class="sm:col-span-9 field group">
              <label for="addressLine" class="form-label text-neutral-300">
                Endereço (Rua e Nº)
              </label>
              <div
                class="flex items-center gap-2 bg-neutral-900/60 border border-neutral-800 rounded-xl px-3 py-2.5 shadow-inner transition group-hover:border-orange-500/40"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4 text-neutral-500 group-hover:text-orange-400"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                >
                  <path
                    d="M10.707 2.293a1 1 0 00-1.414 0L2 9.586V18a2 2 0 002 2h4v-6h4v6h4a2 2 0 002-2V9.586l-7.293-7.293z"
                  />
                </svg>
                <input
                  type="text"
                  id="addressLine"
                  v-model="profile.addressLine"
                  placeholder="Ex: Rua Brasil, 123"
                  class="w-full bg-transparent border-none focus:ring-0 text-neutral-100 placeholder-neutral-500"
                />
              </div>
            </div>

            <div class="sm:col-span-6 field group">
              <label for="complement" class="form-label text-neutral-300">
                Complemento
              </label>
              <div
                class="flex items-center gap-2 bg-neutral-900/60 border border-neutral-800 rounded-xl px-3 py-2.5 shadow-inner transition group-hover:border-orange-500/40"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4 text-neutral-500 group-hover:text-orange-400"
                  viewBox="0 0 24 24"
                  fill="currentColor"
                >
                  <path
                    d="M12 2L1 7l11 5 11-5-11-5zm0 9L1 6v2l11 5 11-5V6l-11 5zm0 4L1 11v2l11 5 11-5v-2l-11 4z"
                  />
                </svg>
                <input
                  type="text"
                  id="complement"
                  v-model="profile.addressComplement"
                  placeholder="Ex: Apto 101, Bloco B"
                  class="w-full bg-transparent border-none focus:ring-0 text-neutral-100 placeholder-neutral-500"
                />
              </div>
            </div>

            <div class="sm:col-span-6 field group">
              <label for="neighborhood" class="form-label text-neutral-300">
                Bairro
              </label>
              <div
                class="flex items-center gap-2 bg-neutral-900/60 border border-neutral-800 rounded-xl px-3 py-2.5 shadow-inner transition group-hover:border-orange-500/40"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4 text-neutral-500 group-hover:text-orange-400"
                  viewBox="0 0 24 24"
                  fill="currentColor"
                >
                  <path
                    d="M3 22h18V2H3v20zm4-2H5v-2h2v2zm0-4H5v-2h2v2zm0-4H5V8h2v4zm4 8H9v-2h2v2zm0-4H9v-2h2v2zm0-4H9V8h2v4zm4 8h-2v-6h2v6z"
                  />
                </svg>
                <input
                  type="text"
                  id="neighborhood"
                  v-model="profile.neighborhood"
                  placeholder="Digite seu bairro"
                  class="w-full bg-transparent border-none focus:ring-0 text-neutral-100 placeholder-neutral-500"
                />
              </div>
            </div>

            <div class="sm:col-span-8 field group">
              <label for="city" class="form-label text-neutral-300">
                Cidade
              </label>
              <div
                class="flex items-center gap-2 bg-neutral-900/60 border border-neutral-800 rounded-xl px-3 py-2.5 shadow-inner transition group-hover:border-orange-500/40"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4 text-neutral-500 group-hover:text-orange-400"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                >
                  <path d="M9 2l-5 2v12l5-2 6 2 5-2V2l-5 2-6-2z" />
                </svg>
                <input
                  type="text"
                  id="city"
                  v-model="profile.city"
                  placeholder="Digite sua cidade"
                  class="w-full bg-transparent border-none focus:ring-0 text-neutral-100 placeholder-neutral-500"
                />
              </div>
            </div>

            <div class="sm:col-span-4 field group">
              <label for="state" class="form-label text-neutral-300">
                Estado (UF)
              </label>
              <div
                class="flex items-center gap-2 bg-neutral-900/60 border border-neutral-800 rounded-xl px-3 py-2.5 shadow-inner transition group-hover:border-orange-500/40"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4 text-neutral-500 group-hover:text-orange-400"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                >
                  <path d="M9 2l-5 2v12l5-2 6 2 5-2V2l-5 2-6-2z" />
                </svg>
                <input
                  type="text"
                  id="state"
                  v-model="profile.state"
                  maxlength="2"
                  placeholder="Ex: SP"
                  class="w-full bg-transparent border-none focus:ring-0 text-neutral-100 placeholder-neutral-500 uppercase"
                />
              </div>
            </div>
          </div>
        </section>

        <!-- Panel: Pedidos -->
        <section
          id="tab-orders"
          role="tabpanel"
          v-show="activeTab === 'orders'"
          class="relative overflow-hidden rounded-2xl border border-neutral-800 bg-gradient-to-b from-neutral-900/70 to-neutral-950/60 backdrop-blur-md shadow-xl p-8 transition-all duration-300"
        >
          <div class="mb-6 flex items-center gap-3">
            <div class="flex h-10 w-10 items-center justify-center rounded-xl bg-orange-500/10 text-orange-400">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-5 w-5"
                viewBox="0 0 24 24"
                fill="currentColor"
              >
                <path
                  d="M3 4a1 1 0 011-1h2.2a1 1 0 01.98.804L7.8 6H20a1 1 0 01.96 1.274l-2 7A1 1 0 0118 15H9a1 1 0 01-.98-.804L6.2 5H4a1 1 0 01-1-1z"
                />
                <circle cx="9" cy="19" r="1.5" />
                <circle cx="17" cy="19" r="1.5" />
              </svg>
            </div>
            <div>
              <h3 class="text-xl font-semibold text-white tracking-tight">
                Pedidos
              </h3>
              <p class="text-sm text-neutral-300">
                Histórico das compras realizadas neste dispositivo.
              </p>
            </div>
          </div>

          <div v-if="sortedOrders.length === 0" class="text-neutral-400 text-sm">
            Nenhum pedido encontrado. Finalize uma compra para ver o histórico aqui.
          </div>

          <div v-else class="space-y-5">
            <div
              v-for="order in sortedOrders"
              :key="order.id"
              class="rounded-2xl border border-neutral-800 bg-neutral-900/70 p-4 md:p-5 space-y-4"
            >
              <!-- Cabeçalho do pedido -->
              <div class="flex flex-wrap items-center justify-between gap-3">
                <div>
                  <p class="text-xs text-neutral-400 uppercase tracking-wide">
                    Pedido
                  </p>
                  <p class="text-sm font-semibold text-neutral-100">
                    #{{ order.id }}
                  </p>
                </div>
                <div>
                  <p class="text-xs text-neutral-400 uppercase tracking-wide">
                    Data
                  </p>
                  <p class="text-sm text-neutral-100">
                    {{ formatDateTime(order.createdAt) }}
                  </p>
                </div>
                <div class="text-right">
                  <p class="text-xs text-neutral-400 uppercase tracking-wide">
                    Total
                  </p>
                  <p class="text-sm font-semibold text-orange-400">
                    {{ formatCurrency(order.total) }}
                  </p>
                </div>
              </div>

              <!-- Info pagamento / frete / cupom -->
              <div class="grid grid-cols-1 md:grid-cols-3 gap-3">
                <div class="text-sm">
                  <p class="text-xs text-neutral-400 uppercase tracking-wide">
                    Forma de Pagamento
                  </p>
                  <p class="text-neutral-100">
                    {{ order.paymentMethod }}
                  </p>
                </div>

                <div class="text-sm">
                  <p class="text-xs text-neutral-400 uppercase tracking-wide">
                    Frete / Transportadora
                  </p>
                  <p class="text-neutral-100">
                    {{ order.shippingOption?.name }}
                    <span
                      v-if="order.shippingOption?.carrier"
                      class="text-xs text-neutral-400"
                    >
                      ({{ order.shippingOption.carrier }})
                    </span>
                  </p>
                  <p class="text-xs text-neutral-400">
                    {{ formatCurrency(order.shippingPrice) }}
                    <span v-if="order.shippingOption?.estimatedDeliveryDays">
                      • Entrega em
                      {{ order.shippingOption.estimatedDeliveryDays }} dias úteis
                    </span>
                  </p>
                </div>

                <div class="text-sm">
                  <p class="text-xs text-neutral-400 uppercase tracking-wide">
                    Cupom
                  </p>
                  <p
                    class="text-sm font-semibold"
                    :class="getCouponCode(order) ? 'text-emerald-300' : 'text-neutral-500'"
                  >
                    {{ getCouponCode(order) || 'Sem cupom aplicado' }}
                  </p>
                </div>
              </div>

              <!-- Itens -->
              <div class="border-t border-neutral-800 pt-3 mt-2">
                <h4 class="text-xs font-semibold text-neutral-300 uppercase tracking-wide mb-2">
                  Itens
                </h4>
                <div class="space-y-2">
                  <div
                    v-for="item in order.items"
                    :key="item.id"
                    class="flex items-center justify-between text-sm"
                  >
                    <div class="flex-1 min-w-0">
                      <p class="text-neutral-100 truncate">
                        {{ item.name }}
                      </p>
                      <p class="text-xs text-neutral-400">
                        Qtd: {{ item.quantity }}
                      </p>
                    </div>
                    <div class="text-right">
                      <p class="text-xs text-neutral-400">
                        Unitário
                      </p>
                      <p class="text-neutral-100">
                        {{ formatCurrency(item.price) }}
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>

        <!-- Actions -->
        <div class="flex items-center justify-end">
          <button
            type="submit"
            class="inline-flex items-center justify-center gap-2 rounded-xl px-6 py-3 text-sm font-medium
                   text-white bg-orange-600 hover:bg-orange-500 active:bg-orange-700
                   focus:outline-none focus:ring-2 focus:ring-orange-500/60
                   disabled:bg-neutral-700 disabled:text-neutral-400 disabled:cursor-not-allowed
                   shadow-md hover:shadow-orange-500/20 transition-all"
            :disabled="isLoading"
          >
            <svg
              v-if="isLoading"
              class="h-4 w-4 animate-spin"
              viewBox="0 0 24 24"
              fill="none"
            >
              <circle
                class="opacity-25"
                cx="12"
                cy="12"
                r="10"
                stroke="currentColor"
                stroke-width="4"
              />
              <path
                class="opacity-75"
                d="M4 12a8 8 0 018-8"
                stroke="currentColor"
                stroke-width="4"
                stroke-linecap="round"
              />
            </svg>
            <span>{{ isLoading ? 'Salvando...' : 'Salvar Alterações' }}</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref, computed } from 'vue'
import { useRoute } from 'vue-router'
import { useProfile } from '../composables/useProfile'

const { profile, isLoading, error, fetchProfile, updateProfile } = useProfile()
const successMessage = ref('')

const route = useRoute()

// histórico completo de pedidos
const orders = ref([])

// tab inicial: suporta /profile?tab=orders
const activeTab = ref(route.query.tab || 'conta')

const tabBtnClass = (key) =>
  [
    'w-full rounded-xl px-4 py-2 text-sm font-medium transition-all',
    'focus:outline-none focus:ring-2 focus:ring-orange-500/60',
    activeTab.value === key
      ? 'bg-neutral-800 text-white shadow border border-neutral-700'
      : 'bg-transparent text-neutral-400 hover:text-neutral-200 hover:bg-neutral-800/40 border border-transparent',
  ].join(' ')

onMounted(() => {
  fetchProfile()

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

// pedidos ordenados da compra mais recente para a mais antiga
const sortedOrders = computed(() =>
  [...orders.value].sort(
    (a, b) => new Date(b.createdAt) - new Date(a.createdAt),
  ),
)

const handleSubmit = async () => {
  successMessage.value = ''
  const success = await updateProfile()
  if (success) {
    successMessage.value = 'Perfil atualizado com sucesso!'
    setTimeout(() => (successMessage.value = ''), 3000)
  }
}

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

function getCouponCode(order) {
  return (
    order.couponCode ||
    order.coupon?.code ||
    order.appliedCouponCode ||
    order.appliedCoupon?.code ||
    order.coupon ||
    ''
  )
}
</script>

<style scoped>
.field {
  @apply space-y-2;
}

.form-label {
  @apply block text-sm font-medium text-neutral-200;
}

.form-input {
  @apply block w-full rounded-xl border border-neutral-800 bg-neutral-900/60 text-neutral-100 placeholder-neutral-500 px-3 py-2.5 shadow-inner focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 transition;
}

.form-input.disabled {
  @apply bg-neutral-900/40 text-neutral-500 cursor-not-allowed;
}

.fade-in-out-enter-active,
.fade-in-out-leave-active {
  transition: opacity 0.35s ease, transform 0.35s ease;
}

.fade-in-out-enter-from,
.fade-in-out-leave-to {
  opacity: 0;
  transform: translateY(-6px);
}

input:-webkit-autofill {
  -webkit-text-fill-color: #e5e7eb;
  transition: background-color 9999s ease-in-out 0s;
}
</style>
