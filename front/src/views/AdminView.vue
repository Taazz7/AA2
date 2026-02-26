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
        </tbody>
      </v-table>
    </v-card>

    <v-dialog v-model="dialogo" max-width="500px" persistent>
      <v-card theme="dark" class="rounded-xl">
        <v-card-title class="bg-blue text-white pa-4">
          {{ editando ? '✎ Editar Pista' : '➕ Nueva Pista' }}
        </v-card-title>
        
        <v-card-text class="pt-6">
          <v-text-field
            v-model="nombre"
            label="Nombre"
            :error-messages="errors.nombre"
            variant="outlined"
          ></v-text-field>

          <v-text-field
            v-model="tipo"
            label="Deporte"
            :error-messages="errors.tipo"
            variant="outlined"
          ></v-text-field>

          <v-text-field
            v-model.number="precioHora"
            label="Precio/Hora"
            type="number"
            :error-messages="errors.precioHora"
            variant="outlined"
          ></v-text-field>

          <v-switch v-model="activa" label="Pista Activa" color="green" inset></v-switch>
        </v-card-text>

        <v-card-actions class="pa-4">
          <v-spacer></v-spacer>
          <v-btn variant="text" @click="cerrarDialogo">Cancelar</v-btn>
          <v-btn color="blue" variant="flat" @click="handleGuardar">Guardar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { usePistaStore } from '../stores/pistaStore';
import { useReservaStore } from '../stores/reservaStore';
import PistaRow from '../components/PistaRow.vue';
import ReservaRow from '../components/ReservaRow.vue';

// Validación
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';

const pistaStore = usePistaStore();
const reservaStore = useReservaStore();
const dialogo = ref(false);
const editando = ref(false);
const idEdicion = ref<number | null>(null);

// Esquema de validación
const schema = yup.object({
  nombre: yup.string().required('Requerido').min(3, 'Muy corto'),
  tipo: yup.string().required('Requerido'),
  precioHora: yup.number().required('Requerido').positive('Debe ser > 0'),
  activa: yup.boolean()
});

const { errors, handleSubmit, resetForm, setValues } = useForm({
  validationSchema: schema,
  initialValues: { nombre: '', tipo: '', precioHora: 0, activa: true }
});

const { value: nombre } = useField<string>('nombre');
const { value: tipo } = useField<string>('tipo');
const { value: precioHora } = useField<number>('precioHora');
const { value: activa } = useField<boolean>('activa');

onMounted(async () => {
  await pistaStore.fetchPistas();
  await reservaStore.fetchReservas();
});

const abrirFormularioCrear = () => {
  editando.value = false;
  idEdicion.value = null;
  resetForm({ values: { nombre: '', tipo: '', precioHora: 0, activa: true } });
  dialogo.value = true;
};

const abrirFormularioEditar = (pista: any) => {
  editando.value = true;
  idEdicion.value = pista.idPista;
  setValues({
    nombre: pista.nombre,
    tipo: pista.tipo,
    precioHora: pista.precioHora,
    activa: pista.activa
  });
  dialogo.value = true;
};

const handleGuardar = handleSubmit(async (values) => {
  const payload = { ...values, idPista: idEdicion.value };
  const ok = editando.value 
    ? await pistaStore.editarPista(payload) 
    : await pistaStore.crearPista(payload);
    
  if (ok) cerrarDialogo();
});

const cerrarDialogo = () => {
  dialogo.value = false;
  resetForm();
};
</script>