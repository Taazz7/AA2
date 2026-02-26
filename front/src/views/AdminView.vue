<template>
  <v-container>
    <div class="d-flex justify-space-between align-center mb-6">
      <h1 class="text-white text-h4 font-weight-bold">Panel de Control</h1>
      <v-btn color="green-darken-2" @click="abrirFormularioCrear" class="px-6">
        <v-icon start>mdi-plus</v-icon> AÑADIR PISTA
      </v-btn>
    </div>

    <v-card theme="dark" class="mb-12 rounded-lg" variant="outlined" style="border-color: #333;">
      <v-card-title class="text-subtitle-1 text-grey-lighten-1">LISTADO DE PISTAS</v-card-title>
      <v-table theme="dark">
        <thead>
          <tr>
            <th class="text-blue">ID</th>
            <th>NOMBRE</th>
            <th class="text-center">ESTADO</th>
            <th class="text-center">PRECIO/H</th>
            <th class="text-center">ACCIONES</th>
          </tr>
        </thead>
        <tbody>
          <PistaRow 
            v-for="pista in pistaStore.pistas" 
            :key="pista.idPista" 
            :pista="pista"
            @edit="abrirFormularioEditar"
            @delete="pistaStore.borrarPista"
          />
        </tbody>
      </v-table>
    </v-card>

    <div class="mb-6">
      <h2 class="text-white text-h5 font-weight-bold">Historial de Reservas</h2>
    </div>

    <v-card theme="dark" class="rounded-lg" variant="outlined" style="border-color: #333;">
      <v-progress-linear v-if="reservaStore.isLoading" indeterminate color="green"></v-progress-linear>
      <v-table theme="dark">
        <thead>
          <tr>
            <th>FECHA</th>
            <th>USUARIO</th>
            <th>ID PISTA</th>
            <th class="text-center">DURACIÓN</th>
            <th class="text-right">TOTAL</th>
          </tr>
        </thead>
        <tbody>
          <ReservaRow 
            v-for="reserva in reservaStore.reservas" 
            :key="reserva.idReserva" 
            :reserva="reserva"
          />
          <tr v-if="reservaStore.reservas.length === 0">
            <td colspan="5" class="text-center py-4 text-grey">No hay reservas registradas.</td>
          </tr>
        </tbody>
      </v-table>
    </v-card>

    <v-dialog v-model="dialogo" max-width="500px">
      <v-card theme="dark" class="rounded-xl">
        <v-card-title class="bg-blue text-white pa-4">
          {{ editando ? '✎ Editar Pista' : '➕ Nueva Pista' }}
        </v-card-title>
        <v-card-text class="pt-6">
          <v-text-field v-model="form.nombre" label="Nombre" variant="outlined" density="compact"></v-text-field>
          <v-text-field v-model="form.tipo" label="Deporte" variant="outlined" density="compact"></v-text-field>
          <v-text-field v-model.number="form.precioHora" label="Precio/Hora" type="number" variant="outlined" density="compact"></v-text-field>
          <v-switch v-model="form.activa" label="Pista Activa" color="green" inset></v-switch>
        </v-card-text>
        <v-card-actions class="pa-4">
          <v-spacer></v-spacer>
          <v-btn variant="text" @click="dialogo = false">Cancelar</v-btn>
          <v-btn color="blue" variant="flat" @click="guardar">Guardar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { usePistaStore } from '../stores/pistaStore';
import { useReservaStore } from '../stores/reservaStore';
// IMPORTAMOS LOS NUEVOS COMPONENTES
import PistaRow from '../components/PistaRow.vue';
import ReservaRow from '../components/ReservaRow.vue';

const pistaStore = usePistaStore();
const reservaStore = useReservaStore();

const dialogo = ref(false);
const editando = ref(false);
const form = ref<any>({ nombre: '', tipo: '', precioHora: 0, activa: true });

onMounted(() => {
  pistaStore.fetchPistas();
  reservaStore.fetchReservas();
});

const abrirFormularioCrear = () => {
  editando.value = false;
  form.value = { nombre: '', tipo: '', precioHora: 0, activa: true };
  dialogo.value = true;
};

const abrirFormularioEditar = (pista: any) => {
  editando.value = true;
  form.value = { ...pista };
  dialogo.value = true;
};

const guardar = async () => {
  const ok = editando.value 
    ? await pistaStore.editarPista(form.value) 
    : await pistaStore.crearPista(form.value);
  if (ok) dialogo.value = false;
};
</script>