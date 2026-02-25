<template>
  <v-app>
    <HeaderComponent />

    <v-main style="background-color: #121212; min-height: 100vh;">
      <router-view></router-view>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
// Importamos el componente desde la carpeta components
import HeaderComponent from './components/HeaderComponent.vue';
</script>

<style>
/* Estilos globales para asegurar que no haya márgenes extraños */
html, body {
  margin: 0;
  padding: 0;
  overflow-x: hidden;
}
</style>