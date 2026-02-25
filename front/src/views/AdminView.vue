<template>
  <v-container>
    <h1 class="text-white text-h4 mb-6">Panel de Control</h1>
    
    <v-table theme="dark" class="elevation-0" style="background: transparent;">
      <thead>
        <tr>
          <th class="text-grey">Nombre</th>
          <th class="text-grey">Deporte</th>
          <th class="text-grey">Precio/h</th>
          <th class="text-grey">Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="pista in pistaStore.pistas" :key="pista.idPista" style="border-bottom: 1px solid #333;">
          <td>{{ pista.nombre }}</td>
          <td>{{ pista.tipoPista }}</td>
          <td>{{ pista.precioHora }}€</td>
          <td>
            <v-btn icon color="blue" variant="text" @click="abrirModal(pista)">
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
          </td>
        </tr>
      </tbody>
    </v-table>

    <v-dialog v-model="dialogo" max-width="400">
      <v-card theme="dark" border>
        <v-card-title class="bg-blue">Editar Pista</v-card-title>
        <v-card-text class="pt-4">
          <v-text-field v-model="form.nombre" label="Nombre" variant="outlined" density="compact"></v-text-field>
          <v-text-field v-model="form.tipoPista" label="Deporte" variant="outlined" density="compact"></v-text-field>
          <v-text-field v-model.number="form.precioHora" label="Precio" type="number" variant="outlined" density="compact" suffix="€"></v-text-field>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn variant="text" @click="dialogo = false">Cancelar</v-btn>
          <v-btn color="blue" variant="flat" :loading="pistaStore.isLoading" @click="guardar">Guardar</v-btn>
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

onMounted(() => pistaStore.fetchPistas());

const abrirModal = (pista: any) => {
  form.value = { ...pista }; // Copia para editar
  dialogo.value = true;
};

const guardar = async () => {
  const ok = await pistaStore.editarPista(form.value);
  if (ok) dialogo.value = false;
};
</script>