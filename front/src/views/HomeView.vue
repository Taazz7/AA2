<template>
  <v-container>
    <h1 class="text-h4 mb-6">Nuestras Pistas</h1>
    
    <PistaList :isAdmin="false" />
    
    <v-alert v-if="store.pistas.length === 0" type="info">
      No hay pistas disponibles o cargando...
    </v-alert>
  </v-container>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { usePistaStore } from '../stores/pistaStore';
import PistaList from '../components/PistaList.vue';

const store = usePistaStore();

onMounted(() => {
  store.cargarPistas();
});
</script>