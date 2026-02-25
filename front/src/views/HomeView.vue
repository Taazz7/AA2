<template>
  <div class="container">
    <h2>Listado de Pistas Disponibles</h2>

    <p v-if="pistaStore.loading">Consultando base de datos...</p>

    <p v-else-if="pistaStore.error" style="color: red;">
      No se pudo cargar el listado: {{ pistaStore.error }}
    </p>

    <table v-else class="table">
      <thead>
        <tr>
          <th>Nombre</th>
          <th>Deporte</th>
          <th>Precio/Hora</th>
          <th>Estado</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="pista in pistaStore.pistas" :key="pista.idPista">
          <td>{{ pista.nombre }}</td>
          <td>{{ pista.tipo }}</td>
          <td>{{ pista.precioHora }}€</td>
          <td>
            <span v-if="pista.activa">Disponible</span>
            <span v-else>En mantenimiento</span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { onMounted } from 'vue';
import { usePistaStore } from '@/stores/pistaStore';

const pistaStore = usePistaStore();

// Esto es lo que "dispara" la acción al cargar la web
onMounted(() => {
  pistaStore.fetchPistas();
});
</script>