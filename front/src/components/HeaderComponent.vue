<template>
  <v-app-bar 
    :color="route.path.startsWith('/admin') ? 'green-darken-3' : 'blue'" 
    density="compact" 
    elevation="0"
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

        <v-btn to="/" variant="text" color="white" class="mr-2">
          INICIO
        </v-btn>

        <v-btn 
          v-if="authStore.isAdmin && !route.path.startsWith('/admin')" 
          to="/admin" 
          variant="tonal" 
          color="white" 
          class="mr-2"
        >
          PANEL ADMIN
        </v-btn>

        <v-btn 
          v-if="route.path.startsWith('/admin')" 
          to="/" 
          variant="outlined" 
          color="white" 
          class="mr-2"
        >
          VOLVER A WEB
        </v-btn>

        <v-btn 
          @click="handleLogout" 
          variant="flat" 
          color="red-lighten-1" 
          size="small"
        >
          CERRAR SESIÓN
        </v-btn>
      </template>

      <template v-else>
        <v-btn to="/" variant="text" color="white" class="mr-2">INICIO</v-btn>
        <v-btn to="/login" variant="elevated" color="white" class="text-blue">LOGIN</v-btn>
      </template>
      
    </div>
  </v-app-bar>
</template>

<script setup lang="ts">
import { useAuthStore } from '../stores/authStore';
import { useRouter, useRoute } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();
const route = useRoute();

const handleLogout = () => {
  authStore.logout();
  router.push('/');
};
</script>

<style scoped>
.v-btn {
  font-weight: 500;
  text-transform: none;
}
</style>