<template>
  <tr>
    <td class="text-blue font-weight-bold">#{{ pista.idPista }}</td>
    <td class="text-white">{{ pista.nombre }}</td>
    <td class="text-center">
      <v-chip size="x-small" :color="pista.activa ? 'green' : 'red'">
        {{ pista.activa ? 'SÍ' : 'NO' }}
      </v-chip>
    </td>
    <td class="text-center text-blue font-weight-bold">{{ pista.precioHora }}€</td>
    <td class="text-center">
      <v-btn 
        color="blue" 
        size="small" 
        variant="tonal" 
        class="mr-2" 
        @click="$emit('edit', pista)"
      >EDITAR</v-btn>
      <v-btn 
        color="red" 
        size="small" 
        variant="tonal" 
        @click="$emit('delete', pista.idPista)"
      >BORRAR</v-btn>
    </td>
  </tr>
</template>

<script setup lang="ts">
defineProps<{
  pista: any
}>();

defineEmits(['edit', 'delete']);
</script>