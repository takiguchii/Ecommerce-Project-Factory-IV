import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { jwtDecode } from 'jwt-decode';

// --- Estado Reativo (Global) ---
// Qualquer componente que usar este composable compartilhará estas variáveis
const isLoggedIn = ref(false); // Começa como deslogado
const user = ref(null); // Guarda informações do usuário (nome, perfil)

export function useAuth() {
  const router = useRouter();

  // --- Função para checar o status ao carregar a página ---
  const checkAuthStatus = () => {
    const token = localStorage.getItem('authToken');
    if (token) {
      try {
        const decoded = jwtDecode(token);
        // Verifica se o token tem uma data de expiração (exp) e se ela é no futuro
        if (decoded.exp * 1000 > Date.now()) {
          isLoggedIn.value = true;
          // Guarda informações úteis do token (ajuste os nomes das claims se necessário)
          user.value = {
            username: decoded.sub || decoded.name || decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'], // Pega o nome/username
            role: decoded.role || decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] // Pega o perfil
          };
        } else {
          // Token expirado
          logout(); // Limpa token expirado
        }
      } catch (error) {
        console.error("Erro ao decodificar token:", error);
        logout(); // Limpa token inválido
      }
    } else {
      // Se não tem token, garante que está deslogado
      isLoggedIn.value = false;
      user.value = null;
    }
  };

  // --- Função chamada após o login ---
  // (Recebe o token da LoginView)
  const setLoggedIn = (token) => {
    localStorage.setItem('authToken', token);
    checkAuthStatus(); // Atualiza o estado global
  };

  // --- Função de Logout ---
  const logout = () => {
    localStorage.removeItem('authToken'); // Limpa o token
    isLoggedIn.value = false; // Atualiza estado
    user.value = null; // Limpa usuário
    router.push('/login'); // Redireciona para login
  };

  // --- Expõe o estado e as funções para os componentes ---
  return {
    isLoggedIn, // O estado (true/false)
    user,       // Informações do usuário logado (objeto)
    checkAuthStatus, // Para verificar no início
    setLoggedIn, // Para usar na LoginView
    logout      // Para usar na Navbar
  };
}