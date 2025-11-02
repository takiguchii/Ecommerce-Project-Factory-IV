import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { jwtDecode } from 'jwt-decode';


const isLoggedIn = ref(false);
const user = ref(null);

export function useAuth() {
  const router = useRouter();

  const checkAuthStatus = () => {
    const token = localStorage.getItem('authToken');
    if (token) {
      try {
        const decoded = jwtDecode(token);
        if (decoded.exp * 1000 > Date.now()) {
          isLoggedIn.value = true;
          user.value = {
            username: decoded.sub || decoded.name || decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'], // Pega o nome/username
            role: decoded.role || decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] // Pega o perfil
          };
        } else {
          logout(); 
        }
      } catch (error) {
        console.error("Erro ao decodificar token:", error);
        logout();
      }
    } else {
      isLoggedIn.value = false;
      user.value = null;
    }
  };

  const setLoggedIn = (token) => {
    localStorage.setItem('authToken', token);
    checkAuthStatus(); // Atualiza o estado global
  };

  const logout = () => {
    localStorage.removeItem('authToken');
    isLoggedIn.value = false;
    user.value = null;
    router.push('/login');
  };

  return {
    isLoggedIn,
    user,
    checkAuthStatus,
    setLoggedIn,
    logout
  };
}