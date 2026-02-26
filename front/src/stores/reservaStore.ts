import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useReservaStore = defineStore('reserva', () => {
  const isLoading = ref(false);
  const reservas = ref<any[]>([]);

  // Función para obtener todas las reservas (para el admin)
  const fetchReservas = async () => {
    isLoading.value = true;
    try {
      const response = await fetch('http://localhost:3000/api/Reserva');
      if (response.ok) {
        reservas.value = await response.json();
      }
    } catch (error) {
      console.error("Error al obtener reservas:", error);
    } finally {
      isLoading.value = false;
    }
  };

  const crearReserva = async (reserva: any) => {
    isLoading.value = true;
    try {
      const response = await fetch('http://localhost:3000/api/Reserva', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(reserva)
      });
      return response.ok;
    } catch (error) {
      console.error("Error al crear reserva:", error);
      return false;
    } finally {
      isLoading.value = false;
    }
  };

  return { reservas, fetchReservas, crearReserva, isLoading };
});