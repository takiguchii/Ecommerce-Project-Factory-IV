import { ref } from 'vue'
import api, { apiGet } from '@/services/api'

const cart = ref({ items: [], totalValue: 0 })

function normalizeCart(raw) {
  if (!raw) return { items: [], totalValue: 0 }

  const items = Array.isArray(raw.items || raw.Itens || raw.Products)
    ? (raw.items || raw.Itens || raw.Products).map((i) => ({
        productId: i.productId ?? i.id ?? i.product_id ?? null,
        name: i.name ?? i.productName ?? i.nome ?? '',
        price: i.price ?? i.unitPrice ?? i.valor ?? 0,
        quantity: i.quantity ?? i.qtd ?? i.amount ?? 1,

        imageUrl: i.imageUrl ?? i.image_url ?? i.image ?? '',
        image_url0: i.image_url0,
        image_url1: i.image_url1,
        image_url2: i.image_url2,
        image_url3: i.image_url3,
        image_url4: i.image_url4,
      }))
    : []

  const totalValue =
    raw.totalValue ?? raw.total_value ?? raw.total ?? items.reduce((s, it) => s + toNum(it.price) * (it.quantity || 1), 0)

  return { items, totalValue }
}

function toNum(v) {
  if (typeof v === 'number') return v
  if (!v) return 0
  const s = String(v).replace(/\s/g, '').replace('R$', '')
  const norm = s.replace(/\./g, '').replace(',', '.')
  const n = parseFloat(norm)
  return Number.isFinite(n) ? n : 0
}

export function useCart() {
  async function fetchCart() {
    try {
      const data = await apiGet('/cart')
      cart.value = normalizeCart(data)
    } catch (error) {
      console.error('Erro ao buscar carrinho:', error)
      cart.value = { items: [], totalValue: 0 }
    }
  }

  async function addToCart(productId, quantity = 1) {
    try {
      await api.post('/cart/add', { productId, quantity })
      await fetchCart() // <-- atualização automática
    } catch (error) {
      console.error('Erro ao adicionar ao carrinho:', error)
      throw error
    }
  }

  async function removeFromCart(productId) {
    try {
      await api.delete(`/cart/remove/${productId}`)
      await fetchCart()
    } catch (error) {
      console.error('Erro ao remover do carrinho:', error)
    }
  }

  async function updateItemQuantity(productId, quantity) {
    try {
      await api.put(`/cart/update/${productId}?quantity=${quantity}`)
      await fetchCart() 
    } catch (error) {
      console.error('Erro ao atualizar quantidade:', error)
    }
  }

  return {
    cart,
    fetchCart,
    addToCart,
    removeFromCart,
    updateItemQuantity,
  }
}
