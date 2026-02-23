import { defineStore } from 'pinia'

export const useUiStore = defineStore('ui', {
  state: () => ({
    isDarkMode: false,
    loading: false,
    notifications: [] as string[]
  }),
  actions: {
    toggleTheme() {
      this.isDarkMode = !this.isDarkMode
    },
    addNotification(message: string) {
      this.notifications.push(message)
    }
  }
})