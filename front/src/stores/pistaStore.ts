import { defineStore } from 'pinia'

export const usePistaStore = defineStore('pista', {
  state: () => ({
    pistas: [] as any[],
    loading: false
  }),
  actions: {
    async fetchPistas() {
      this.loading = true
      try {
        // Usamos el puerto 3000 de tu docker-compose
        const response = await fetch('http://localhost:3000/api/pistas')
        if (!response.ok) throw new Error('Error en la red')
        
        // CORRECCIÓN: .json() en lugar de .data()
        this.pistas = await response.json() 
      } catch (error) {
        console.error("Error cargando pistas:", error)
      } finally {
        this.loading = false
      }
    }
  }
})