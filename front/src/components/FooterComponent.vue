<template>
  <v-footer 
    :color="route.path.startsWith('/admin') ? 'green-darken-3' : 'blue'" 
    class="text-white d-flex align-center justify-center px-4"
    style="min-height: 56px; max-height: 56px;"
    elevation="4"
  >
    <div class="d-flex align-center justify-space-between w-100">
      <div class="text-caption font-weight-medium">
        {{ new Date().getFullYear() }} — <strong>Gestión de Pistas</strong>
      </div>

      <div v-if="route.path.startsWith('/admin')" class="text-caption font-weight-bold d-none d-sm-flex align-center">
        <v-icon start size="small" class="mr-1">mdi-shield-check</v-icon>
        MODO ADMIN
      </div>

      <div class="d-flex">
        <v-btn icon="mdi-facebook" variant="text" size="x-small" color="white" class="mx-1"></v-btn>
        <v-btn icon="mdi-twitter" variant="text" size="x-small" color="white" class="mx-1"></v-btn>
        <v-btn icon="mdi-instagram" variant="text" size="x-small" color="white" class="mx-1"></v-btn>
      </div>
    </div>
  </v-footer>
</template>

<script setup lang="ts">
import { useRoute } from 'vue-router';
const route = useRoute();
</script>

<style scoped>
.v-footer {
  /* Evita que el footer se estire más de la cuenta */
  position: relative; 
  bottom: 0;
  width: 100%;
  padding: 0 16px !important;
  transition: background-color 0.3s ease;
}
</style>