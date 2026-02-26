<template>
  <v-container>
    <h1 class="text-white text-h4 mb-6 font-weight-bold">Panel de Control</h1>
    
    <v-table theme="dark" class="elevation-2 rounded-lg" style="background-color: #1e1e1e;">
      <thead>
        <tr>
          <th class="text-grey-lighten-1">NOMBRE</th>
          <th class="text-grey-lighten-1 text-center">DEPORTE</th>
          <th class="text-grey-lighten-1 text-center">PRECIO/H</th>
          <th class="text-grey-lighten-1 text-center">ACCIONES</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="pista in pistaStore.pistas" :key="pista.idPista" border>
          <td class="text-white font-weight-bold">{{ pista.nombre }}</td>
          <td class="text-center">{{ pista.tipoPista }}</td>
          <td class="text-center text-blue font-weight-bold">{{ pista.precioHora }}€</td>
          <td class="text-center pa-2">
            <v-btn 
              color="blue-darken-2" 
              size="small" 
              class="mr-2 px-4"
              @click="abrirEditor(pista)"
            >
              <span class="mr-1">✎</span> EDITAR
            </v-btn>
            
            <v-btn 
              color="red-darken-3" 
              size="small" 
              class="px-4"
              @click="pistaStore.borrarPista(pista.idPista)"
            >
              <span class="mr-1">🗑</span> BORRAR
            </v-btn>
          </td>
        </tr>
      </tbody>
    </v-table>

    <v-dialog v-model="dialogo" max-width="450px">
      <v-card theme="dark" border class="rounded-xl">
        <v-card-title class="bg-blue text-white pa-4">
          ✎ Modificar Pista: {{ form.nombre }}
        </v-card-title>
        <v-card-text class="pt-6">
          <v-text-field v-model="form.nombre" label="Nombre" variant="outlined"></v-text-field>
          <v-text-field v-model="form.tipoPista" label="Deporte" variant="outlined"></v-text-field>
          <v-text-field v-model.number="form.precioHora" label="Precio" type="number" variant="outlined" suffix="€"></v-text-field>
        </v-card-text>
        <v-card-actions class="pa-4">
          <v-spacer></v-spacer>
          <v-btn variant="text" @click="dialogo = false">Cancelar</v-btn>
          <v-btn color="blue" variant="flat" :loading="pistaStore.isLoading" @click="confirmarGuardado">
            Guardar Cambios
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { usePistaStore } from '../stores/pistaStore';

const pistaStore = usePistaStore();
const dialogo = ref(false);
const form = ref<any>({});

onMounted(() => {
  pistaStore.fetchPistas();
});

const abrirEditor = (pista: any) => {
  form.value = { ...pista };
  dialogo.value = true;
};

const confirmarGuardado = async () => {
  const exito = await pistaStore.editarPista(form.value);
  if (exito) dialogo.value = false;
};
</script>

<style scoped>
/* Estilo para que las filas tengan separación visual */
tr:hover {
  background-color: rgba(255, 255, 255, 0.05) !important;
}
</style>