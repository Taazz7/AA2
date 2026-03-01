import { defineStore } from 'pinia';
import { ref } from 'vue';
import Swal from 'sweetalert2'; // 1. Importar la librería

export const useReservaStore = defineStore('reservaStore', () => {
  const reservas = ref<any[]>([]);
  const isLoading = ref(false);

  // --- ACCIÓN: Cargar Reservas (GET) ---
  const fetchReservas = async () => {
    isLoading.value = true;
    try {
      const response = await fetch('http://localhost:3000/api/Reserva');
      if (response.ok) {
        reservas.value = await response.json();
      } else {
        throw new Error("No se pudieron obtener las reservas");
      }
    } catch (err) {
      console.error("Error al cargar reservas:", err);
      // Alerta de error si falla la conexión con el backend de C#
      Swal.fire({
        icon: 'error',
        title: 'Error de conexión',
        text: 'No se pudieron cargar las reservas del servidor.',
      });
    } finally {
      isLoading.value = false;
    }
  };

  // --- ACCIÓN: Borrar Reserva (DELETE) ---
  const borrarReserva = async (id: number) => {
    // 2. Reemplazamos el confirm() nativo por la versión estética
    const result = await Swal.fire({
      title: '¿Confirmar cancelación?',
      text: "Esta acción no se puede deshacer.",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, cancelar reserva',
      cancelButtonText: 'Mantener reserva'
    });

    // Si el usuario confirma, procedemos con el borrado
    if (result.isConfirmed) {
      isLoading.value = true;
      try {
        const response = await fetch(`http://localhost:3000/api/Reserva/${id}`, { 
          method: 'DELETE' 
        });
        
        if (response.ok) {
          reservas.value = reservas.value.filter(r => r.idReserva !== id);
          
          // Alerta de éxito tras borrar
          Swal.fire(
            '¡Eliminada!',
            'La reserva ha sido cancelada correctamente.',
            'success'
          );
          return true;
        } else {
          Swal.fire('Error', 'El servidor no permitió borrar la reserva.', 'error');
        }
      } catch (err) {
        console.error("Error al borrar reserva:", err);
        Swal.fire('Error', 'Hubo un fallo al intentar contactar con el servidor.', 'error');
      } finally {
        isLoading.value = false;
      }
    }
    return false;
  };

  return { 
    reservas, 
    isLoading, 
    fetchReservas, 
    borrarReserva 
  };
});