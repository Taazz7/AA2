import { defineStore } from 'pinia';
import { ref } from 'vue';
import Swal from 'sweetalert2'; // 1. Importar SweetAlert2

export const usePistaStore = defineStore('pista', () => {
  const pistas = ref<any[]>([]);
  const isLoading = ref(false);

  // --- ACCIÓN: Cargar Pistas ---
  const fetchPistas = async () => {
    isLoading.value = true;
    try {
      const response = await fetch('http://localhost:3000/api/Pista');
      if (response.ok) {
        pistas.value = await response.json();
      } else {
        throw new Error("Error al obtener datos");
      }
    } catch (err) {
      console.error("Error al cargar:", err);
      Swal.fire('Error', 'No se pudieron cargar las pistas', 'error');
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
        pistas.value.push(creada);
        
        Swal.fire({
          icon: 'success',
          title: '¡Pista creada!',
          timer: 1500,
          showConfirmButton: false
        });
        return true;
      }
    } catch (err) {
      Swal.fire('Error', 'No se pudo crear la pista', 'error');
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
        const index = pistas.value.findIndex(p => p.idPista === pistaEditada.idPista);
        if (index !== -1) pistas.value[index] = { ...pistaEditada };
        
        Swal.fire('Actualizado', 'La pista se ha modificado correctamente', 'success');
        return true;
      }
    } catch (err) {
      Swal.fire('Error', 'Error al actualizar la pista', 'error');
    } finally {
      isLoading.value = false;
    }
    return false;
  };

  // --- ACCIÓN: Borrar Pista (DELETE) ---
  const borrarPista = async (id: number) => {
    // 2. Sustituimos confirm() por una alerta de confirmación asíncrona
    const result = await Swal.fire({
      title: '¿Estás seguro?',
      text: "Esta acción eliminará la pista definitivamente",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    });

    if (result.isConfirmed) {
      isLoading.value = true;
      try {
        const response = await fetch(`http://localhost:3000/api/Pista/${id}`, { method: 'DELETE' });
        if (response.ok) {
          pistas.value = pistas.value.filter(p => p.idPista !== id);
          Swal.fire('Eliminado', 'La pista ha sido borrada.', 'success');
        } else {
          Swal.fire('Error', 'No se pudo eliminar de la base de datos', 'error');
        }
      } catch (err) {
        Swal.fire('Error', 'Hubo un fallo en la conexión', 'error');
      } finally {
        isLoading.value = false;
      }
    }
  };

  return { pistas, isLoading, fetchPistas, crearPista, editarPista, borrarPista };
});