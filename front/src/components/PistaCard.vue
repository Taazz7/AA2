<template>
  <v-card class="mb-4" variant="outlined">
    <v-card-item>
      <v-card-title>{{ pista.nombre }}</v-card-title>
      <v-card-subtitle>{{ pista.tipo }} - {{ pista.precioHora }}€/h</v-card-subtitle>
    </v-card-item>

    <v-card-text>
      {{ pista.direccion }}
      <v-chip :color="pista.activa ? 'success' : 'error'" size="small" class="ml-2">
        {{ pista.activa ? 'Activa' : 'Mantenimiento' }}
      </v-chip>
    </v-card-text>

    <v-card-actions>
      <v-btn color="primary" variant="text" @click="$emit('edit', pista)">Editar</v-btn>
      <v-btn color="error" variant="text" @click="$emit('delete', pista.idPista)">Borrar</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script setup lang="ts">
defineProps<{ pista: any }>()
defineEmits(['edit', 'delete'])
</script>