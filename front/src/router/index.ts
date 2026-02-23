import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', name: 'home', component: HomeView },
    { 
      path: '/login', 
      name: 'login', 
      component: () => import('../views/LoginView.vue'),
      meta: { hideLayout: true } // Marcador para ocultar Header/Footer
    },
    { path: '/admin', name: 'admin', component: () => import('../views/AdminView.vue') }
  ]
})

export default router