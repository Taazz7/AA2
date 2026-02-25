<template>
  <v-row>
    <v-col v-for="pista in store.pistas" :key="pista.idPista" cols="12" sm="6" md="4">
      <PistaCard 
        :pista="pista" 
        :isAdmin="isAdmin"
        @edit="(p) => $emit('edit', p)"
        @delete="(id) => $emit('delete', id)"
      />
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { usePistaStore } from '../stores/pistaStore';
import PistaCard from './PistaCard.vue';

const store = usePistaStore();

defineProps<{
  isAdmin?: boolean
}>();

defineEmits(['edit', 'delete']);
</script>