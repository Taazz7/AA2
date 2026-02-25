import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Pista } from '../interfaces/Pista';

export const usePistaStore = defineStore('pistas', () => {
  const pistas = ref<Pista[]>([]);

  const cargarPistas = async () => {
  try {
    // Asegúrate de que la URL coincide exactamente con Swagger
    const response = await fetch('http://localhost:3000/api/Pista'); 
    
    if (!response.ok) {
        throw new Error(`Error HTTP: ${response.status}`);
    }

    const data = await response.json();
    console.log("Datos recibidos:", data); // Mira esto en la consola
    pistas.value = data;
  } catch (error) {
    console.error("Error en el fetch:", error);
  }
};

  return { pistas, cargarPistas };
});