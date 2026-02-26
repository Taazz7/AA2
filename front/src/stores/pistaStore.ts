import { defineStore } from 'pinia';
import { ref } from 'vue';

export const usePistaStore = defineStore('pista', () => {
  const pistas = ref<any[]>([]);
  const isLoading = ref(false);

  // --- ACCIÓN: Cargar Pistas ---
  const fetchPistas = async () => {
    isLoading.value = true;
    try {
      const response = await fetch('http://localhost:3000/api/Pista');
      if (response.ok) pistas.value = await response.json();
    } catch (err) {
      console.error("Error al cargar:", err);
    } finally {
      isLoading.value = false;
    }
  };

  // --- ACCIÓN: Crear Nueva Pista (POST) ---
  const crearPista = async (nuevaPista: any) => {
    isLoading.value = true;
    try {
      const response = await fetch('http://localhost:3000/api/Pista', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(nuevaPista)
      });
      if (response.ok) {
        const creada = await response.json();
        pistas.value.push(creada); // Actualización reactiva instantánea
        return true;
      }
    } catch (err) {
      console.error("Error al crear:", err);
    } finally {
      isLoading.value = false;
    }
    return false;
  };

  // --- ACCIÓN: Editar Pista (PUT) ---
  const editarPista = async (pistaEditada: any) => {
    isLoading.value = true;
    try {
      const response = await fetch(`http://localhost:3000/api/Pista/${pistaEditada.idPista}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(pistaEditada)
      });
      if (response.ok) {
        // Buscamos y actualizamos en la lista local
        const index = pistas.value.findIndex(p => p.idPista === pistaEditada.idPista);
        if (index !== -1) pistas.value[index] = { ...pistaEditada };
        return true;
      }
    } catch (err) {
      console.error("Error al editar:", err);
    } finally {
      isLoading.value = false;
    }
    return false;
  };

  // --- ACCIÓN: Borrar Pista (DELETE) ---
  const borrarPista = async (id: number) => {
    if (!confirm("⚠️ ¿Estás seguro de eliminar esta pista definitivamente?")) return;
    isLoading.value = true;
    try {
      const response = await fetch(`http://localhost:3000/api/Pista/${id}`, { method: 'DELETE' });
      if (response.ok) {
        pistas.value = pistas.value.filter(p => p.idPista !== id);
      }
    } catch (err) {
      console.error("Error al borrar:", err);
    } finally {
      isLoading.value = false;
    }
  };

  return { pistas, isLoading, fetchPistas, crearPista, editarPista, borrarPista };
});