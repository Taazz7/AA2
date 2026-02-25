import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import { useAuthStore } from '../stores/authStore'

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('../views/LoginView.vue')
  },
  {
    path: '/admin',
    name: 'admin',
    component: () => import('../views/AdminView.vue'),
    meta: { requiresAdmin: true }
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

// GUARD DE SEGURIDAD CORREGIDO
router.beforeEach((to, from, next) => {
  // Accedemos al store aquí adentro, donde Pinia ya está garantizado
  const authStore = useAuthStore()

  if (to.matched.some(record => record.meta.requiresAdmin)) {
    if (!authStore.isLogged || authStore.userRole !== 'admin') {
      next({ name: 'login' })
    } else {
      next()
    }
  } else {
    next()
  }
})

export default router