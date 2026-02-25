import { defineStore } from 'pinia';
import { ref } from 'vue';

export const usePistaStore = defineStore('pista', () => {
  const pistas = ref<any[]>([]);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Acción para leer pistas
  const fetchPistas = async () => {
    isLoading.value = true;
    try {
      const response = await fetch('http://localhost:3000/api/Pista');
      if (response.ok) pistas.value = await response.json();
    } catch (err) {
      error.value = "Error al cargar datos";
    } finally {
      isLoading.value = false;
    }
  };

  // Acción para EDITAR (PUT)
  const editarPista = async (pistaEditada: any) => {
    isLoading.value = true;
    try {
      const response = await fetch(`http://localhost:3000/api/Pista/${pistaEditada.idPista}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(pistaEditada)
      });
      if (response.ok) {
        // Actualizamos la lista local para que se vea el cambio sin refrescar
        const index = pistas.value.findIndex(p => p.idPista === pistaEditada.idPista);
        if (index !== -1) pistas.value[index] = pistaEditada;
        return true;
      }
    } catch (err) {
      console.error(err);
    } finally {
      isLoading.value = false;
    }
    return false;
  };

  return { pistas, isLoading, error, fetchPistas, editarPista };
});