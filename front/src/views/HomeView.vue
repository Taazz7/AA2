<template>
  <v-container>
    <v-row class="text-center mt-5">
      <v-col cols="12">
        <h1 class="text-h4 mb-4">Nuestras Pistas Disponibles</h1>
        <v-divider class="mb-6"></v-divider>
      </v-col>
    </v-row>

    <v-row v-if="pistaStore.loading" justify="center">
      <v-progress-circular indeterminate color="primary"></v-progress-circular>
    </v-row>

    <v-row v-else-if="pistaStore.pistas.length > 0">
      <v-col v-for="pista in pistaStore.pistas" :key="pista.idPista" cols="12" sm="6" md="4">
        <PistaCard :pista="pista" />
      </v-col>
    </v-row>

    <v-row v-else justify="center">
      <v-col cols="12" md="6">
        <v-alert type="info" variant="tonal">
          No hay pistas disponibles en este momento. Revisa la conexión al puerto 3000.
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
  await pistaStore.fetchPistas()
})
</script>