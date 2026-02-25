import { defineStore } from 'pinia'

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
        const response = await fetch('http://localhost:3000/api/Pistas')
        // CORRECCIÓN: Usar .json() en lugar de .data()
        this.pistas = await response.json()
      } catch (error) {
        console.error("Error cargando pistas:", error)
      } finally {
        this.loading = false
      }
    },

    // AÑADE ESTA ACCIÓN PARA ELIMINAR EL ERROR ROJO
    async deletePista(id: number) {
      try {
        const response = await fetch(`http://localhost:3000/api/pistas/${id}`, { 
          method: 'DELETE' 
        })
        if (response.ok) {
          // Actualizamos la lista localmente para que desaparezca de la pantalla
          this.pistas = this.pistas.filter(p => p.idPista !== id)
        }
      } catch (error) {
        console.error("Error al borrar:", error)
      }
    }
  }
})  