<template>
  <v-container>
    <h1 class="text-white text-h4 mb-6">Nuestras Pistas</h1>

    <v-alert v-if="pistaStore.isLoading" color="blue" class="text-white mb-4">
      No hay pistas disponibles o cargando...
    </v-alert>

    <v-row v-else-if="pistaStore.pistas.length > 0">
      <v-col v-for="pista in pistaStore.pistas" :key="pista.idPista" cols="12" md="4">
        <v-card theme="dark" variant="outlined" class="pa-4" style="border-color: rgba(255,255,255,0.5);">
          <v-card-title class="pa-0 text-h6 font-weight-bold">{{ pista.nombre }}</v-card-title>
          <v-card-subtitle class="pa-0 mt-1">
            {{ pista.tipoPista }} - {{ pista.precioHora }}€/h
          </v-card-subtitle>
          
          <v-card-text class="pa-0 mt-4 mb-4">
            <p class="text-caption">Pista de alta calidad</p>
            <v-chip size="x-small" color="green" class="mt-2">Activa</v-chip>
          </v-card-text>

          <v-card-actions class="pa-0">
            <v-btn block color="green-darken-1" variant="flat" class="text-white">
              RESERVAR PISTA
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>

    <v-alert v-else color="orange-darken-4" theme="dark">
      No hay pistas registradas en el servidor.
    </v-alert>
  </v-container>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { usePistaStore } from '../stores/pistaStore';
const pistaStore = usePistaStore();

onMounted(() => {
  pistaStore.fetchPistas();
});
</script>