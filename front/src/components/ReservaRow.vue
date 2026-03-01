<template>
  <tr>
    <td class="text-white">{{ formatearFecha(reserva.fecha) }}</td>
    <td class="text-blue-lighten-4">
      {{ reserva.idUsuario?.usuarioNombre || 'Usuario #' + (reserva.idUsuario?.idUsuario || reserva.idUsuario) }}
    </td>
    <td class="text-green-lighten-4 text-center">
      {{ reserva.idPista?.nombre || 'Pista #' + (reserva.idPista?.idPista || reserva.idPista) }}
    </td>
    <td class="text-center">{{ reserva.horas }} h</td>
    <td class="text-right text-green font-weight-bold">{{ reserva.precio }}€</td>
    
    <td class="text-center">
      <v-btn 
        icon="mdi-delete-outline" 
        variant="text" 
        color="red-lighten-2" 
        size="small"
        @click="confirmarBorrado"
      >BORRAR</v-btn>
    </td>
  </tr>
</template>

<script setup lang="ts">
import Swal from 'sweetalert2'; // 1. Importar la librería

const props = defineProps<{ reserva: any }>();
const emit = defineEmits(['delete']);

// 2. Función para manejar el clic con confirmación estética
const confirmarBorrado = async () => {
  const result = await Swal.fire({
    title: '¿Eliminar reserva?',
    text: `Se borrará la reserva del día ${formatearFecha(props.reserva.fecha)}`,
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#ff5252', // Rojo de Vuetify (error)
    cancelButtonColor: '#90a4ae',
    confirmButtonText: 'Sí, borrar',
    cancelButtonText: 'Cancelar'
  });

  if (result.isConfirmed) {
    // Solo emitimos el evento si el usuario confirma en el SweetAlert
    emit('delete', props.reserva.idReserva);
  }
};

const formatearFecha = (fecha: string) => {
  if (!fecha) return '---';
  return new Date(fecha).toLocaleDateString('es-ES', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
  });
};
</script>