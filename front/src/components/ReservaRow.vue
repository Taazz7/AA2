<template>
  <tr>
    <td>{{ new Date(reserva.fecha).toLocaleDateString() }}</td>
    <td class="text-blue-lighten-4">
      {{ reserva.idUsuario?.usuarioNombre || 'ID: ' + (reserva.idUsuario?.idUsuario || reserva.idUsuario) }}
    </td>
    <td class="text-green-lighten-4">
      Pista #{{ reserva.idPista?.idPista || reserva.idPista }}
    </td>
    <td class="text-center">{{ reserva.horas }} h</td>
    <td class="text-right text-green font-weight-bold">{{ reserva.precio }}€</td>
  </tr>
</template>

<script setup lang="ts">
defineProps<{
  reserva: any
}>();
</script>