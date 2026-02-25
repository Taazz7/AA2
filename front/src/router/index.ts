import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/authStore'
import HomeView from '../views/HomeView.vue'

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView,
    // Meta layout para mostrar Header/Footer estándar
    meta: { layout: 'public' }
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('../views/LoginView.vue'),
    // Meta layout 'blank' para cumplir requisito de no tener header/footer 
    meta: { layout: 'blank' }
  },
  {
    path: '/admin',
    name: 'admin',
    component: () => import('../views/AdminView.vue'),
    // Requiere admin y tiene layout propio
    meta: { layout: 'admin', requiresAdmin: true }
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

// Guard de navegación para proteger la ruta de Admin 
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()

  // Si la ruta requiere ser administrador y el usuario no lo es
  if (to.meta.requiresAdmin && authStore.userRole !== 'admin') {
    // Bloqueamos acceso y redirigimos al Home (funcionalidad extra de seguridad)
    next({ name: 'home' })
  } else {
    next()
  }
})

export default router