<template>
  <v-container>
    <h2 class="text-h4 mb-6">Gestión de Pistas</h2>
    
    <v-btn color="success" class="mb-6" prepend-icon="mdi-plus">Nueva Pista</v-btn>

    <v-row v-if="!pistaStore.loading">
      <v-col v-for="pista in pistaStore.pistas" :key="pista.idPista" cols="12" md="6">
        <PistaCard 
          :pista="pista" 
          @delete="eliminar" 
          @edit="prepararEdicion" 
        />
      </v-col>
    </v-row>
    
    <v-progress-circular v-else indeterminate color="primary"></v-progress-circular>
  </v-container>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { usePistaStore } from '../stores/pistaStore'
import PistaCard from '../components/PistaCard.vue'

const pistaStore = usePistaStore()

onMounted(() => {
  pistaStore.fetchPistas()
})

const eliminar = (id: number) => {
  if(confirm('¿Seguro?')) pistaStore.deletePista(id)
}

const prepararEdicion = (pista: any) => {
  console.log("Editando...", pista)
}
</script>