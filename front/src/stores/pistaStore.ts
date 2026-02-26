import { defineStore } from 'pinia';
import { ref } from 'vue';

export const usePistaStore = defineStore('pista', () => {
  const pistas = ref<any[]>([]);
  const isLoading = ref(false);

  const fetchPistas = async () => {
    isLoading.value = true;
    try {
      const response = await fetch('http://localhost:3000/api/Pista');
      if (response.ok) pistas.value = await response.json();
    } catch (e) { console.error(e); }
    finally { isLoading.value = false; }
  };

  const editarPista = async (pista: any) => {
    isLoading.value = true;
    try {
      const response = await fetch(`http://localhost:3000/api/Pista/${pista.idPista}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(pista)
      });
      if (response.ok) {
        const i = pistas.value.findIndex(p => p.idPista === pista.idPista);
        if (i !== -1) pistas.value[i] = { ...pista };
        return true;
      }
    } catch (e) { console.error(e); }
    finally { isLoading.value = false; }
    return false;
  };

  const borrarPista = async (id: number) => {
    if (!confirm("⚠️ ¿Estás seguro de eliminar esta pista definitivamente?")) return;
    
    isLoading.value = true;
    try {
      const res = await fetch(`http://localhost:3000/api/Pista/${id}`, { method: 'DELETE' });
      if (res.ok) {
        pistas.value = pistas.value.filter(p => p.idPista !== id);
      } else {
        alert("No se pudo borrar la pista. Comprueba la conexión.");
      }
    } catch (e) { alert("Error de servidor al intentar borrar."); }
    finally { isLoading.value = false; }
  };

  return { pistas, isLoading, fetchPistas, editarPista, borrarPista };
});