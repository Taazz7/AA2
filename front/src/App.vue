<template>
  <v-app>
    
    <v-app-bar color="primary">
      <v-toolbar-title>Gestión de Pistas</v-toolbar-title>
      <v-spacer></v-spacer>
      
      <v-btn v-if="!authStore.isLogged" to="/login">Login</v-btn>
      <v-btn v-if="authStore.isLogged" @click="authStore.logout">Cerrar Sesión</v-btn>
    </v-app-bar>

    <v-main>
      <router-view></router-view>
    </v-main>

    <v-footer app>
      <span>&copy; 2024 Centro Deportivo</span>
    </v-footer>

  </v-app>
</template>

<script setup lang="ts">
import { useAuthStore } from './stores/authStore';
const authStore = useAuthStore();
</script>

<style>
/* Estilos globales si los necesitas */
</style>