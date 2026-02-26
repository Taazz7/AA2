import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useReservaStore = defineStore('reserva', () => {
  const isLoading = ref(false);

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
      console.error("Error al conectar con la API de reservas:", error);
      return false;
    } finally {
      isLoading.value = false;
    }
  };

  return { crearReserva, isLoading };
});