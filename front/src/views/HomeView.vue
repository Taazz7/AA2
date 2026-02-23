<template>
  <v-container>
    <v-row class="text-center mb-10">
      <v-col cols="12">
        <h1 class="text-h3 font-weight-bold color-primary">Nuestras Pistas</h1>
        <p class="text-subtitle-1">Consulta la disponibilidad y precios en tiempo real</p>
      </v-col>
    </v-row>

    <v-row v-if="pistaStore.loading" justify="center">
      <v-progress-circular indeterminate color="primary"></v-progress-circular>
    </v-row>

    <v-row v-else-if="pistaStore.pistas.length > 0">
      <v-col 
        v-for="pista in pistaStore.pistas" 
        :key="pista.idPista" 
        cols="12" 
        sm="6" 
        md="4"
      >
        <PistaCard :pista="pista" />
      </v-col>
    </v-row>

    <v-row v-else justify="center">
      <v-col cols="12" md="6">
        <v-alert type="warning" variant="tonal" icon="mdi-alert-circle">
          No se han podido cargar las pistas. Revisa la conexión con el servidor.
        </v-alert>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { usePistaStore } from '../stores/pistaStore'
import PistaCard from '../components/PistaCard.vue'

const pistaStore = usePistaStore()

onMounted(async () => {
  // Forzamos la carga al entrar en Inicio
  await pistaStore.fetchPistas()
})
</script>