import { createRouter, createWebHistory } from 'vue-router';
// Importações existentes
import HomeView from '../views/HomeView.vue';
import LoginView from '../views/LoginView.vue';
import RegisterView from '../views/RegisterView.vue';
import ProductRegisterView from '../views/ProductRegisterView.vue';
import ProductDetailView from '../views/ProductDetailView.vue';
import SearchResultsView from '../views/SearchResultsView.vue';
import AdminLayout from '../views/AdminLayoutView.vue';
import AdminBrandView from '../views/AdminBrandView.vue'; // 
import AdminProductView from '../views/AdminProductView.vue';
import AdminCategoryView from '../views/AdminCategoryView.vue';
import AdminSubCategoryView from '../views/AdminSubCategoryView.vue';
import AdminProviderView from '../views/AdminProviderView.vue';
import ProfileView from '../views/ProfileView.vue';
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
    {
      path: '/profile',
      name: 'Profile',
      component: ProfileView,
      meta: { requiresAuth: true } 
    },
    {
      path: '/admin',
      component: AdminLayout, 
      children: [
        {
          path: '', 
          redirect: '/admin/brands' 
        },
        {
          path: 'brands', 
          name: 'admin-brands',
          component: AdminBrandView 
        },
        {
          path: 'products', 
          name: 'admin-products',
          component: AdminProductView 
        },
        {
          path: 'categories', 
          name: 'admin-categories',
          component: AdminCategoryView 
        },
        {
          path: 'subcategories', 
          name: 'admin-subcategories',
          component: AdminSubCategoryView 
        },
        {
          path: 'providers', 
          name: 'admin-providers',
          component: AdminProviderView 
        }
      ]
    }
  ],
});


router.beforeEach((to, from, next) => {
  const requiresAdmin = to.matched.some(record => record.meta.requiresAdmin);
  const token = localStorage.getItem('authToken');

  if (requiresAdmin) {
    if (!token) {
      next({ name: 'login' });
    } else {
      try {
        const decodedToken = jwtDecode(token);
        const userRole = decodedToken.role || decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

        if (userRole === 'Admin') {
          next();
        } else {
          next({ name: 'home' });
        }
      } catch (e) {
        console.error("Erro ao decodificar token:", e);
        localStorage.removeItem('authToken');
        next({ name: 'login' });
      }
    }
  } else {
    next();
  }
});

export default router;