import { defineStore } from 'pinia'

// Definimos la interfaz según tu SQL
export interface Pista {
  idPista?: number
  nombre: string
  tipo: string
  direccion: string
  activa: boolean
  precioHora: number
}

export const usePistaStore = defineStore('pista', {
  state: () => ({
    pistas: [] as Pista[],
    loading: false
  }),
  actions: {
    async fetchPistas() {
      this.loading = true
      try {
        const response = await fetch('http://localhost:8080/api/pistas')
        this.pistas = await response.json()
      } catch (error) {
        console.error("Error cargando pistas:", error)
      } finally {
        this.loading = false
      }
    },
    async deletePista(id: number) {
        await fetch(`http://localhost:8080/api/pistas/${id}`, { method: 'DELETE' })
        this.pistas = this.pistas.filter(p => p.idPista !== id)
    }
  }
})