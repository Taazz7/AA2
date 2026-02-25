<template>
  <v-app-bar color="blue" density="compact" elevation="0">
    <v-app-bar-title class="text-white font-weight-bold">
      Gestión de Pistas
    </v-app-bar-title>
    
    <v-spacer></v-spacer>

    <div class="d-flex align-center">
      
      <template v-if="authStore.isLogged">
        <span class="text-white mr-4 text-body-2">
          Bienvenido, <strong>{{ authStore.userName }}</strong>
        </span>

        <v-btn to="/" variant="text" color="white" class="mr-2">
          INICIO
        </v-btn>

        <v-btn 
          v-if="authStore.isAdmin" 
          to="/admin" 
          variant="tonal" 
          color="white" 
          class="mr-2"
        >
          PANEL ADMIN
        </v-btn>

        <v-btn 
          @click="handleLogout" 
          variant="outlined" 
          color="white" 
          size="small"
        >
          CERRAR SESIÓN
        </v-btn>
      </template>

      <template v-else>
        <v-btn to="/" variant="text" color="white" class="mr-2">INICIO</v-btn>
        <v-btn to="/login" variant="text" color="white">LOGIN</v-btn>
      </template>
      
    </div>
  </v-app-bar>
</template>

<script setup lang="ts">
import { useAuthStore } from '../stores/authStore';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();

// Función para cerrar sesión y volver a la raíz
const handleLogout = () => {
  authStore.logout();
  router.push('/');
};
</script>

<style scoped>
/* Forzamos que los botones de Vuetify no tengan sombras pesadas si no quieres */
.v-btn {
  font-weight: 500;
}
</style>