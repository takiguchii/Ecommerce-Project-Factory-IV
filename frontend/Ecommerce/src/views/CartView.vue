<template>
  <div class="cart-container">
    <h1>Meu Carrinho</h1>

    <div v-if="!cart.items || cart.items.length === 0" class="cart-empty">
      <p>Seu carrinho está vazio.</p>
      <router-link to="/">Voltar para a loja</router-link>
    </div>

    <div v-else class="cart-content">
      
      <div class="cart-items">
        <div v-for="item in cart.items" :key="item.productId" class="cart-item">
          <img :src="item.imageUrl" :alt="item.name" class="item-image" />
          <div class="item-details">
            <h3 class="item-name">{{ item.name }}</h3>
            <p class="item-price">R$ {{ formatPrice(item.price) }}</p>
            <div class="item-quantity">
              <label>Qtd:</label>
              <input 
                type="number" 
                :value="item.quantity"
                @change="handleQuantityChange(item.productId, $event.target.value)"
                min="0" 
              />
            </div>
            <button @click="removeFromCart(item.productId)" class="remove-button">Remover</button>
          </div>
        </div>
      </div>

      <div class="cart-summary">
        <h2>Resumo do Pedido</h2>
        <div class="summary-total">
          <span>Total</span>
          <span class="total-value">R$ {{ cart.totalValue.toFixed(2) }}</span>
        </div>
        <button class="checkout-button">Finalizar Compra</button>
      </div>
    </div>

  </div>
</template>

<script setup>
import { onMounted } from 'vue';
import { useCart } from '@/composables/useCart';

const { cart, fetchCart, removeFromCart, updateItemQuantity } = useCart();

onMounted(() => {
  fetchCart();
});

function formatPrice(priceString) {
  const price = parseFloat(priceString);
  return price.toFixed(2);
}

function handleQuantityChange(productId, newQuantity) {
  updateItemQuantity(productId, parseInt(newQuantity, 10));
}
</script>

<style scoped>
.cart-container {
  max-width: 1200px;
  margin: 20px auto;
  padding: 20px;
  font-family: Arial, sans-serif;
}

.cart-empty {
  text-align: center;
  padding: 50px;
}

.cart-content {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
}

.cart-items {
  flex: 3; 
  min-width: 300px;
}

.cart-summary {
  flex: 1;
  min-width: 250px;
  border: 1px solid #ccc;
  border-radius: 8px;
  padding: 20px;
  height: fit-content; 
}

.cart-item {
  display: flex;
  border-bottom: 1px solid #eee;
  padding-bottom: 15px;
  margin-bottom: 15px;
}

.item-image {
  width: 100px;
  height: 100px;
  object-fit: cover;
  margin-right: 15px;
}

.item-details {
  flex: 1;
}

.item-name {
  font-size: 1.1em;
  margin: 0;
}

.item-price {
  font-size: 1em;
  color: #333;
  font-weight: bold;
}

.item-quantity {
  margin: 10px 0;
}

.item-quantity input {
  width: 50px;
  margin-left: 5px;
}

.remove-button {
  background: none;
  border: none;
  color: red;
  cursor: pointer;
  text-decoration: underline;
  padding: 0;
}

.summary-total {
  display: flex;
  justify-content: space-between;
  font-size: 1.2em;
  font-weight: bold;
  margin-bottom: 20px;
}

.checkout-button {
  width: 100%;
  padding: 15px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 5px;
  font-size: 1.1em;
  cursor: pointer;
}

.checkout-button:hover {
  background-color: #0056b3;
}
</style>