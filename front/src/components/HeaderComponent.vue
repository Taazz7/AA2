<template>
  <v-app-bar color="primary">
    <v-toolbar-title>Reserva tu pista</v-toolbar-title>
    <v-spacer></v-spacer>
    
    <v-btn to="/" variant="text">Inicio</v-btn>
    
    <v-btn v-if="authStore.userRole === 'admin'" to="/admin" variant="text">
      Admin
    </v-btn>

    <v-btn v-if="!authStore.isLogged" to="/login" variant="text">Login</v-btn>
    <v-btn v-else @click="handleLogout" variant="text">Salir</v-btn>
  </v-app-bar>
</template>

<script setup lang="ts">
import { useAuthStore } from '../stores/authStore';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();

const handleLogout = () => {
  authStore.logout();
  router.push('/');
};
</script>