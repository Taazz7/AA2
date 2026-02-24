<template>
  <v-container>
    <v-row class="align-center mb-6">
      <v-col>
        <h1 class="text-h4">Panel de Administración</h1>
      </v-col>
      <v-col class="text-right">
        <v-btn color="primary" prepend-icon="mdi-plus" @click="dialog = true">
          Nueva Pista
        </v-btn>
      </v-col>
    </v-row>

    <v-divider class="mb-6"></v-divider>

    <v-row v-if="!pistaStore.loading">
      <v-col v-for="pista in pistaStore.pistas" :key="pista.idPista" cols="12" md="6" lg="4">
        <PistaCard 
          :pista="pista" 
          @delete="eliminar" 
          @edit="abrirEdicion" 
        />
      </v-col>
    </v-row>

    <v-row v-else justify="center">
      <v-progress-circular indeterminate color="primary"></v-progress-circular>
    </v-row>

    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-title class="bg-primary text-white">Detalles de la Pista</v-card-title>
        <v-card-text class="pt-4">
          <v-form @submit.prevent="guardarPista">
            <v-text-field
              v-model="nombre.value"
              label="Nombre de la pista"
              :error-messages="nombre.errorMessage"
              variant="outlined"
            ></v-text-field>

            <v-select
              v-model="tipo.value"
              :items="['Tenis', 'Padel', 'Futbol']"
              label="Tipo de Pista"
              :error-messages="tipo.errorMessage"
              variant="outlined"
            ></v-select>

            <v-text-field
              v-model="precio.value"
              label="Precio por Hora (€)"
              type="number"
              :error-messages="precio.errorMessage"
              variant="outlined"
            ></v-text-field>

            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="grey" variant="text" @click="dialog = false">Cancelar</v-btn>
              <v-btn color="success" type="submit" variant="elevated">Guardar</v-btn>
            </v-card-actions>
          </v-form>
        </v-card-text>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { usePistaStore } from '../stores/pistaStore'
import PistaCard from '../components/PistaCard.vue'
import { useForm, useField } from 'vee-validate'
import * as yup from 'yup'

const pistaStore = usePistaStore()
const dialog = ref(false)

// 1. Esquema de Validación (YUP) - Obligatorio según el PDF
const schema = yup.object({
  nombre: yup.string().required('El nombre es obligatorio').min(3, 'Mínimo 3 letras'),
  tipo: yup.string().required('Selecciona un deporte'),
  precio: yup.number().typeError('Debe ser un número').required('Precio necesario').min(1, 'Mínimo 1€')
})

// 2. Configurar VeeValidate
const { handleSubmit, resetForm } = useForm({
  validationSchema: schema,
})

const nombre = useField<string>('nombre')
const tipo = useField<string>('tipo')
const precio = useField<number>('precio')

// 3. Carga inicial
onMounted(() => {
  pistaStore.fetchPistas()
})

// 4. Lógica del CRUD
const eliminar = (id: number) => {
  if (confirm('¿Seguro que quieres borrar esta pista?')) {
    pistaStore.deletePista(id)
  }
}

const abrirEdicion = (pista: any) => {
  console.log('Editar:', pista)
  // Aquí podrías cargar los datos en el formulario para editar
}

const guardarPista = handleSubmit(async (values) => {
  console.log('Datos validados enviados al backend:', values)
  
  // Aquí llamarías a tu función de crear en el store:
  // await pistaStore.createPista(values)
  
  dialog.value = false
  resetForm()
})
</script>