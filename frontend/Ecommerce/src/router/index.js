//router
import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import ProductRegisterView from '../views/ProductRegisterView.vue'
import ProductDetailView from '../views/ProductDetailView.vue';
import SearchResultsView from '../views/SearchResultsView.vue'

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
    { path: '/search', 
      name: 'search', 
      component: SearchResultsView 
    }
  ],
})

export default router