import { defineStore } from 'pinia';

export const usePistaStore = defineStore('pista', {
  state: () => ({
    pistas: [] as any[],
    loading: false
  }),
  actions: {
    async fetchPistas() {
      this.loading = true;
      try {
        // Usamos el puerto 3000 que vemos en Swagger
        const response = await fetch('http://localhost:3000/api/Pista');
        if (response.ok) {
          this.pistas = await response.json();
          console.log("Datos recibidos:", this.pistas);
        }
      } catch (error) {
        console.error("Error al conectar con el puerto 3000:", error);
      } finally {
        this.loading = false;
      }
    }
  }
});