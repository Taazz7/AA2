<template>
  <v-app>
    <v-app-bar color="primary" density="compact">
      <v-app-bar-title class="font-weight-bold">
        Gestión de Pistas
      </v-app-bar-title>

      <v-spacer></v-spacer>

      <v-btn v-if="!authStore.isLogged" to="/login" variant="text">
        Iniciar Sesión
      </v-btn>

      <template v-else>
        <span class="mr-4 text-body-2">
          Bienvenido, <strong>{{ authStore.userName }}</strong>
        </span>
        
        <v-btn v-if="authStore.isAdmin" to="/admin" variant="tonal" class="mr-2" color="yellow-lighten-3">
          Panel Admin
        </v-btn>

        <v-btn @click="handleLogout" variant="outlined" size="small" color="white">
          Cerrar Sesión
        </v-btn>
      </template>
    </v-app-bar>

    <v-main>
      <v-container>
        <router-view></router-view>
      </v-container>
    </v-main>

    <v-footer app class="justify-center text-caption grey--text">
      © 2024 Centro Deportivo — Proyecto AA2
    </v-footer>
  </v-app>
</template>

<script setup lang="ts">
import { useAuthStore } from './stores/authStore';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();

const handleLogout = () => {
  authStore.logout();
  router.push('/login'); // Redirigimos al login tras cerrar sesión
};
</script>

<style scoped>
/* Puedes añadir estilos personalizados aquí */
</style>