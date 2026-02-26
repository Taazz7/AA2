<template>
  <v-container>
    <div class="d-flex justify-space-between align-center mb-6">
      <h1 class="text-white text-h4 font-weight-bold">Panel de Control</h1>
      <v-btn color="green-darken-2" @click="abrirFormularioCrear" class="px-6">
        <span class="mr-2">➕</span> AÑADIR PISTA
      </v-btn>
    </div>

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
        <tr v-for="pista in pistaStore.pistas" :key="pista.idPista" style="border-bottom: 1px solid #333;">
          <td class="text-white font-weight-bold">{{ pista.nombre }}</td>
          <td class="text-center">{{ pista.tipo }}</td> <td class="text-center text-blue font-weight-bold">{{ pista.precioHora }}€</td>
          <td class="text-center pa-2">
            <v-btn color="blue-darken-2" size="small" class="mr-2 px-4" @click="abrirFormularioEditar(pista)">
              <span class="mr-1">✎</span> EDITAR
            </v-btn>
            <v-btn color="red-darken-3" size="small" class="px-4" @click="pistaStore.borrarPista(pista.idPista)">
              <span class="mr-1">🗑</span> BORRAR
            </v-btn>
          </td>
        </tr>
      </tbody>
    </v-table>

    <v-dialog v-model="dialogo" max-width="500px">
      <v-card theme="dark" border class="rounded-xl">
        <v-card-title class="bg-blue text-white pa-4">
          {{ editando ? '✎ Modificar Pista' : '➕ Nueva Pista' }}
        </v-card-title>
        
        <v-card-text class="pt-6">
          <v-text-field v-model="form.nombre" label="Nombre" variant="outlined" density="compact"></v-text-field>
          <v-text-field v-model="form.tipo" label="Deporte (Tipo)" variant="outlined" density="compact"></v-text-field>
          <v-text-field v-model="form.direccion" label="Dirección" variant="outlined" density="compact"></v-text-field>
          <v-text-field v-model.number="form.precioHora" label="Precio por Hora" type="number" variant="outlined" density="compact" suffix="€"></v-text-field>
          <v-switch v-model="form.activa" label="Pista Activa" color="green" inset density="compact"></v-switch>
        </v-card-text>

        <v-card-actions class="pa-4">
          <v-spacer></v-spacer>
          <v-btn variant="text" @click="dialogo = false">Cancelar</v-btn>
          <v-btn color="blue" variant="flat" :loading="pistaStore.isLoading" @click="guardar">
            {{ editando ? 'Guardar Cambios' : 'Crear Pista' }}
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
const editando = ref(false);
const form = ref<any>({ nombre: '', tipo: '', direccion: '', precioHora: 0, activa: true });

onMounted(() => pistaStore.fetchPistas());

const abrirFormularioCrear = () => {
  editando.value = false;
  form.value = { nombre: '', tipo: '', direccion: '', precioHora: 0, activa: true };
  dialogo.value = true;
};

const abrirFormularioEditar = (pista: any) => {
  editando.value = true;
  form.value = { ...pista }; // Copia para editar
  dialogo.value = true;
};

const guardar = async () => {
  const ok = editando.value 
    ? await pistaStore.editarPista(form.value) 
    : await pistaStore.crearPista(form.value);
  if (ok) dialogo.value = false;
};
</script>