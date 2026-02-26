import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useReservaStore = defineStore('reservaStore', () => {
  const reservas = ref<any[]>([]);
  const isLoading = ref(false);

  // --- ACCIÓN: Cargar Reservas (GET) ---
  const fetchReservas = async () => {
    isLoading.value = true;
    try {
      // Ajusta el puerto 3000 al de tu backend de C#
      const response = await fetch('http://localhost:3000/api/Reserva');
      if (response.ok) {
        reservas.value = await response.json();
      }
    } catch (err) {
      console.error("Error al cargar reservas:", err);
    } finally {
      isLoading.value = false;
    }
  };

  // --- ACCIÓN: Borrar Reserva (DELETE) ---
  const borrarReserva = async (id: number) => {
    if (!confirm("⚠️ ¿Estás seguro de eliminar esta reserva definitivamente?")) return;
    
    isLoading.value = true;
    try {
      // Ajusta el puerto 3000 al de tu backend de C#
      const response = await fetch(`http://localhost:3000/api/Reserva/${id}`, { 
        method: 'DELETE' 
      });
      
      if (response.ok) {
        // Actualización reactiva instantánea filtrando el array
        reservas.value = reservas.value.filter(r => r.idReserva !== id);
        return true;
      }
    } catch (err) {
      console.error("Error al borrar reserva:", err);
    } finally {
      isLoading.value = false;
    }
    return false;
  };

  // Retornamos las variables y funciones para que sean accesibles
  return { 
    reservas, 
    isLoading, 
    fetchReservas, 
    borrarReserva 
  };
});