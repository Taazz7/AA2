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
// Definimos la interfaz para cumplir con TypeScript [cite: 25]
import type { Pista } from '../interfaces/Pista';

defineProps<{
  pista: Pista,
  isAdmin?: boolean
}>();

const emit = defineEmits(['edit', 'delete', 'reserve']);

const handleReserva = () => {
  console.log("Reserva iniciada");
  // Aquí irá la lógica de reserva [cite: 49]
};
</script>