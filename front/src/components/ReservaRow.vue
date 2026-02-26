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
        @click="$emit('delete', reserva.idReserva)"
      ></v-btn>
    </td>
  </tr>
</template>

<script setup lang="ts">
defineProps<{ reserva: any }>();
defineEmits(['delete']);

const formatearFecha = (fecha: string) => {
  if (!fecha) return '---';
  return new Date(fecha).toLocaleDateString('es-ES', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
  });
};
</script>