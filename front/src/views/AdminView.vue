<template>
  <v-container>
    <v-row class="mb-5 align-center">
      <v-col>
        <h1 class="text-h4 font-weight-bold">
          <v-icon icon="mdi-cog" class="mr-2"></v-icon>
          Panel de Administración
        </h1>
        <p class="text-subtitle-1 text-grey">Gestión de pistas y recursos del club</p>
      </v-col>
      <v-col class="text-right">
        <v-btn 
          color="primary" 
          prepend-icon="mdi-plus" 
          size="large"
          @click="pistaStore.fetchPistas()"
        >
          Refrescar Datos
        </v-btn>
      </v-col>
    </v-row>

    <v-divider class="mb-8"></v-divider>

    <v-row v-if="pistaStore.loading" justify="center" class="py-10">
      <v-col cols="12" class="text-center">
        <v-progress-circular indeterminate color="primary" size="64"></v-progress-circular>
        <p class="mt-4">Conectando con la base de datos...</p>
      </v-col>
    </v-row>

    <v-row v-else-if="pistaStore.pistas.length > 0">
      <v-col 
        v-for="pista in pistaStore.pistas" 
        :key="pista.idPista" 
        cols="12" 
        sm="6" 
        lg="4"
      >
        <PistaCard 
          :pista="pista" 
          @delete="handleDelete" 
          @edit="handleEdit"
        />
      </v-col>
    </v-row>

    <v-row v-else justify="center">
      <v-col cols="12" md="6">
        <v-alert
          type="info"
          title="Sin registros"
          text="No se han encontrado pistas. Asegúrate de que el Backend esté corriendo y la base de datos AA1 tenga datos."
          variant="tonal"
          icon="mdi-database-off"
        ></v-alert>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
// Usamos rutas relativas para evitar fallos de configuración de alias (@)
import { usePistaStore } from '../stores/pistaStore'
import PistaCard from '../components/PistaCard.vue'

// Inicializamos el store
const pistaStore = usePistaStore()

// Al cargar la vista, llamamos a la API
onMounted(async () => {
  await pistaStore.fetchPistas()
})

// Funciones para gestionar eventos del componente hijo
const handleDelete = async (id: number) => {
  if (confirm('¿Estás seguro de que deseas eliminar esta pista?')) {
    await pistaStore.deletePista(id)
  }
}

const handleEdit = (pista: any) => {
  console.log('Editar pista:', pista)
  alert('Función de edición en desarrollo para el siguiente punto del PDF')
}
</script>

<style scoped>
.v-container {
  max-width: 1200px;
}
</style>