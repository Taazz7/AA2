<template>
  <v-app-bar 
    :color="route.path.startsWith('/admin') ? 'green-darken-3' : 'blue'" 
    density="compact" 
    elevation="2"
  >
    <v-app-bar-title class="text-white font-weight-bold">
      {{ route.path.startsWith('/admin') ? 'Panel Administración' : 'Gestión de Pistas' }}
    </v-app-bar-title>
    
    <v-spacer></v-spacer>

    <div class="d-flex align-center px-4">
      
      <template v-if="authStore.isLogged">
        <span class="text-white mr-4 text-body-2 d-none d-sm-inline">
          Bienvenido, <strong>{{ authStore.userName }}</strong>
        </span>

        <v-btn 
          to="/" 
          variant="text" 
          :class="{ 'bg-blue-darken-4': route.path === '/' && !route.path.startsWith('/admin') }"
          color="white" 
          class="mr-2"
        >
          INICIO
        </v-btn>

        <v-btn 
          v-if="authStore.isAdmin" 
          to="/admin" 
          variant="tonal" 
          :color="route.path.startsWith('/admin') ? 'yellow-accent-2' : 'white'"
          class="mr-2"
        >
          <v-icon start v-if="route.path.startsWith('/admin')">mdi-view-dashboard</v-icon>
          PANEL ADMIN
        </v-btn>

        <v-btn 
          @click="handleLogout" 
          variant="flat" 
          color="red-lighten-1" 
          size="small"
          class="ml-2"
        >
          SALIR
        </v-btn>
      </template>

      <template v-else>
        <v-btn 
          to="/" 
          variant="text" 
          :class="{ 'border-b-sm': route.path === '/' }"
          color="white" 
          class="mr-2"
        >
          INICIO
        </v-btn>
        
        <v-btn 
          to="/login" 
          variant="elevated" 
          :color="route.path === '/login' ? 'yellow-accent-1' : 'white'" 
          class="text-blue"
        >
          LOGIN
        </v-btn>
      </template>
      
    </div>
  </v-app-bar>
</template>

<script setup lang="ts">
import { useAuthStore } from '../stores/authStore';
import { useRouter, useRoute } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();
const route = useRoute(); // IMPORTANTE: Detecta la ruta actual para el resaltado

const handleLogout = () => {
  authStore.logout();
  router.push('/');
};
</script>

<style scoped>
.v-btn {
  font-weight: 600;
  text-transform: none;
  transition: all 0.3s ease;
}

/* Clase para crear un subrayado simple o resaltar el botón activo */
.border-b-sm {
  border-bottom: 3px solid white !important;
  border-radius: 0;
}
</style>