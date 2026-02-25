<template>
  <v-row>
    <v-col v-for="pista in store.pistas" :key="pista.idPista" cols="12" sm="6" md="4">
      <PistaCard 
        :pista="pista" 
        @edit="handleEdit" 
        @delete="handleDelete" 
      />
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { usePistaStore } from '../stores/pistaStore';
import PistaCard from './PistaCard.vue';

const store = usePistaStore();

const handleEdit = (pista: any) => console.log('Editar:', pista);
const handleDelete = (id: number) => console.log('Borrar ID:', id);
</script>