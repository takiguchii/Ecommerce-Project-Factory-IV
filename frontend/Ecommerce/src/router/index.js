import { createRouter, createWebHistory } from 'vue-router';
// Importações existentes
import HomeView from '../views/HomeView.vue';
import LoginView from '../views/LoginView.vue';
import RegisterView from '../views/RegisterView.vue';
import ProductRegisterView from '../views/ProductRegisterView.vue';
import ProductDetailView from '../views/ProductDetailView.vue';
import SearchResultsView from '../views/SearchResultsView.vue';
import AdminBrandView from '../views/AdminBrandView.vue';
import { jwtDecode } from 'jwt-decode';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView,
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterView,
    },
    {
      path: '/product-register',
      name: 'product-register',
      component: ProductRegisterView,
    },
    {
      path: '/about',
      name: 'about',
      component: () => import('../views/AboutView.vue'),
    },
    {
      path: '/produto/:id',
      name: 'ProductDetail',
      component: ProductDetailView
    },
    {
      path: '/category/:id',
      name: 'Category',
      component: () => import('../views/CategoryView.vue')
    },
    { path: '/brand/:id',
      name: 'Brand',
      component: () => import('../views/BrandView.vue') },
    { path: '/search',
      name: 'search',
      component: SearchResultsView
    },

    // --- Rota dos adms  ---
    {
      path: '/admin/brands',
      name: 'admin-brands',
      component: AdminBrandView,
      meta: { requiresAdmin: true }
    }
    // --- FIM DA NOVA ROTA ADMIN ---
  ],
});

// INÍCIO DA PROTEÇÃO DE ROTA (Navigation Guard)
// Este código será executado ANTES de cada mudança de rota
router.beforeEach((to, from, next) => {
  // Verifica se a rota destino (to) tem o meta campo 'requiresAdmin'
  const requiresAdmin = to.matched.some(record => record.meta.requiresAdmin);
  // Pega o token do localStorage
  const token = localStorage.getItem('authToken');

  // Se a rota PRECISA de Admin...
  if (requiresAdmin) {
    // ...e não tem token
    if (!token) {
      // redireciona para o login.
      next({ name: 'login' });
    } else {
      // tem token
      try {
        // decodifica o token
        const decodedToken = jwtDecode(token);
        // ...pega o perfil (role)...
        const userRole = decodedToken.role || decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

        // verifica se é 'Admin'.
        if (userRole === 'Admin') {
          // Se for Admin, permite continuar para a rota /admin/brands.
          next();
        } else {
          // Se NÃO for Admin, redireciona para a Home.
          next({ name: 'home' });
        }
      } catch (e) {
        // Se o token for inválido/expirado (erro no decode)...
        console.error("Erro ao decodificar token:", e);
        localStorage.removeItem('authToken'); // Limpa o token inválido
        // ...redireciona para o login.
        next({ name: 'login' });
      }
    }
  } else {
    // Se a rota NÃO precisa de Admin, permite continuar.
    next();
  }
});

export default router;