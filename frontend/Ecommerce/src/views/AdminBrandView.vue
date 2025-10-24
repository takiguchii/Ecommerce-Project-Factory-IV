<template>
  <div class="admin-brands-container">
    <h1>Gerenciar Marcas (Admin)</h1>

    <form @submit.prevent="createBrandHandler" class="brand-form">
      <h3>Nova Marca</h3>
      <div>
        <label for="brandName">Nome:</label>
        <input type="text" id="brandName" v-model="newBrand.name" required>
      </div>
      <div>
        <label for="brandImageUrl">URL da Imagem:</label>
        <input type="text" id="brandImageUrl" v-model="newBrand.imageUrl">
      </div>
      <button type="submit" :disabled="loadingCreate">
        {{ loadingCreate ? 'Criando...' : 'Criar Marca' }}
      </button>
      <p v-if="errorCreate" class="error-message">{{ errorCreate }}</p>
       <p v-if="successCreate" class="success-message">Marca criada com sucesso!</p>
    </form>

    <hr>

    <h2>Marcas Existentes</h2>
    <div v-if="loadingList">Carregando marcas...</div>
    <div v-else-if="errorList">{{ errorList }}</div>
    <ul v-else-if="brands.length > 0" class="brand-list">
      <li v-for="brand in brands" :key="brand.id">
        <span>{{ brand.name }} (ID: {{ brand.id }})</span>
        <button @click="deleteBrandHandler(brand.id)" :disabled="loadingDelete[brand.id]">
           {{ loadingDelete[brand.id] ? 'Apagando...' : 'Apagar' }}
        </button>
         <p v-if="errorDelete[brand.id]" class="error-message">{{ errorDelete[brand.id] }}</p>
      </li>
    </ul>
    <p v-else>Nenhuma marca encontrada.</p>
     <button @click="logout" class="logout-button">Sair (Logout)</button>
  </div>
</template>

<script setup>
import { ref, onMounted, reactive } from 'vue';
import { useRouter } from 'vue-router';
// Importa as funções específicas do seu api.js
import { getBrands, createBrand, deleteBrand } from '@/services/api';

const router = useRouter();

// Estado para Criar Marca
const newBrand = ref({ name: '', imageUrl: '' });
const loadingCreate = ref(false);
const errorCreate = ref('');
const successCreate = ref(false);

// Estado para Listar Marcas
const brands = ref([]);
const loadingList = ref(false);
const errorList = ref('');

// Estado para Apagar Marca (objeto para rastrear por ID)
const loadingDelete = reactive({});
const errorDelete = reactive({});

// Função para buscar marcas
const fetchBrands = async () => {
  loadingList.value = true;
  errorList.value = '';
  try {
    // Chama a função getBrands do seu api.js
    brands.value = await getBrands();
  } catch (err) {
    console.error("Erro ao buscar marcas:", err);
    errorList.value = 'Falha ao carregar marcas.';
     // Se der erro 401/403, pode ser token expirado/inválido
     if (err.response && (err.response.status === 401 || err.response.status === 403)) {
         logout(); // Desloga o usuário se não autorizado
     }
  } finally {
    loadingList.value = false;
  }
};

// Função handler para criar marca
const createBrandHandler = async () => {
  loadingCreate.value = true;
  errorCreate.value = '';
  successCreate.value = false;
  try {
    // Chama a função createBrand do seu api.js
    await createBrand({
        name: newBrand.value.name,
        imageUrl: newBrand.value.imageUrl || null // Envia null se vazio
    });
    successCreate.value = true;
    newBrand.value = { name: '', imageUrl: '' }; // Limpa form
    await fetchBrands(); // Recarrega lista
     setTimeout(() => successCreate.value = false, 3000); // Esconde msg sucesso
  } catch (err) {
    console.error("Erro ao criar marca:", err);
    errorCreate.value = 'Falha ao criar marca.';
     if (err.response && (err.response.status === 401 || err.response.status === 403)) {
         errorCreate.value = 'Acesso negado. Você precisa ser Admin.';
         // logout(); // Opcional: deslogar se tentar criar sem ser admin
     } else if (err.response && err.response.data && err.response.data.errors) {
         errorCreate.value = Object.values(err.response.data.errors).flat().join(' ');
     }
  } finally {
    loadingCreate.value = false;
  }
};

// Função handler para apagar marca
const deleteBrandHandler = async (id) => {
  if (!confirm(`Tem certeza que deseja apagar a marca com ID ${id}?`)) {
    return;
  }
  loadingDelete[id] = true;
  errorDelete[id] = '';
  try {
    // Chama a função deleteBrand do seu api.js
    await deleteBrand(id);
    await fetchBrands(); // Recarrega a lista
  } catch (err) {
    console.error(`Erro ao apagar marca ${id}:`, err);
    errorDelete[id] = 'Falha ao apagar.';
    if (err.response && (err.response.status === 401 || err.response.status === 403)) {
         errorDelete[id] = 'Acesso negado.';
         // logout(); // Opcional: deslogar se tentar apagar sem ser admin
     }
  } finally {
    // Garante que o estado de loading seja removido, mesmo se houver erro
    // Vue 3.3+ pode usar delete loadingDelete[id];
    // Para compatibilidade:
    const tempLoading = { ...loadingDelete };
    delete tempLoading[id];
    Object.assign(loadingDelete, tempLoading);
    //delete loadingDelete[id];
  }
};

// Função de Logout
const logout = () => {
    localStorage.removeItem('authToken');
    router.push('/login');
};

// Carrega as marcas quando o componente é montado
onMounted(fetchBrands);

</script>

<style scoped>
.admin-brands-container {
  max-width: 600px;
  margin: 30px auto;
  padding: 20px;
  border: 1px solid #ccc;
  border-radius: 8px;
  background-color: #f9f9f9; /* Fundo claro para contraste */
  color: #333; /* Texto escuro */
}
.brand-form {
  margin-bottom: 20px;
  padding: 15px;
  border: 1px solid #ddd; /* Borda mais suave */
  border-radius: 4px;
  background-color: #fff; /* Fundo branco */
}
.brand-list {
  list-style: none; /* Remove marcadores */
  padding: 0;
}
.brand-list li {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 5px; /* Mais preenchimento */
  border-bottom: 1px solid #eee;
}
.brand-list li:last-child {
  border-bottom: none; /* Remove borda do último item */
}
.brand-list li span {
  flex-grow: 1; /* O nome ocupa o espaço disponível */
  margin-right: 10px;
}
.brand-list li button {
  padding: 4px 10px; /* Botão um pouco maior */
  background-color: #dc3545;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.2s; /* Efeito suave */
}
.brand-list li button:hover {
  background-color: #c82333; /* Cor mais escura no hover */
}
.brand-list li button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}
.error-message {
  color: #dc3545; /* Vermelho erro */
  font-size: 0.9em;
  margin-top: 5px;
}
.success-message {
    color: #28a745; /* Verde sucesso */
    margin-top: 5px;
}
label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold; /* Label em negrito */
}
input[type="text"] {
  width: 95%;
  padding: 10px; /* Input maior */
  margin-bottom: 15px; /* Mais espaço */
  border: 1px solid #ccc;
  border-radius: 4px;
}
button {
  cursor: pointer;
  padding: 10px 18px; /* Botão maior */
  border-radius: 4px;
  border: none;
  background-color: #007bff; /* Azul primário */
  color: white;
  transition: background-color 0.2s;
}
button:hover {
  background-color: #0056b3;
}
button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}
hr {
  margin: 30px 0; /* Mais espaço na linha */
  border: 0;
  border-top: 1px solid #eee;
}
.logout-button {
    margin-top: 20px;
    background-color: #6c757d; /* Cinza */
}
.logout-button:hover {
    background-color: #5a6268;
}
</style>