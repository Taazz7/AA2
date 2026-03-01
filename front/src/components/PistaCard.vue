<template>
  <v-card class="mx-auto my-4" max-width="400" variant="outlined">
    <v-card-item>
      <v-card-title>{{ pista.nombre }}</v-card-title>
      <v-card-subtitle>{{ pista.tipo }} - {{ pista.precioHora }}€/h</v-card-subtitle>
    </v-card-item>

    <v-card-text>
      <div>{{ pista.direccion }}</div>
      <v-chip :color="pista.activa ? 'success' : 'error'" size="small" class="mt-2">
        {{ pista.activa ? 'Activa' : 'Inactiva' }}
      </v-chip>
    </v-card-text>

    <v-card-actions>
      <template v-if="isAdmin">
        <v-btn color="primary" variant="text" @click="$emit('edit', pista)">EDITAR</v-btn>
        <v-btn color="error" variant="text" @click="$emit('delete', pista.idPista)">BORRAR</v-btn>
      </template>

      <template v-else>
        <v-btn color="success" variant="elevated" block @click="handleReserva">
          RESERVAR PISTA
        </v-btn>
      </template>
    </v-card-actions>
  </v-card>
</template>

<script setup lang="ts">
import type { Pista } from '../interfaces/Pista';
import Swal from 'sweetalert2'; // 1. Importamos la librería

const props = defineProps<{
  pista: Pista,
  isAdmin?: boolean
}>();

const emit = defineEmits(['edit', 'delete', 'reserve']);

// 2. Modificamos la función para que sea asíncrona y use SweetAlert
const handleReserva = async () => {
  const result = await Swal.fire({
    title: '¿Confirmar reserva?',
    text: `Vas a reservar la pista: ${props.pista.nombre}`,
    icon: 'question',
    showCancelButton: true,
    confirmButtonColor: '#4caf50', // Verde de Vuetify/Success
    cancelButtonColor: '#f44336',
    confirmButtonText: 'Sí, reservar ahora',
    cancelButtonText: 'Cancelar'
  });

  if (result.isConfirmed) {
    // Si el usuario acepta, emitimos el evento al componente padre
    emit('reserve', props.pista);
    
    // Mostramos un pequeño aviso de que la acción se ha procesado
    Swal.fire({
      title: 'Procesando...',
      text: 'Estamos gestionando tu solicitud',
      icon: 'info',
      timer: 1500,
      showConfirmButton: false,
      toast: true,
      position: 'bottom-end'
    });
  }
};
</script>