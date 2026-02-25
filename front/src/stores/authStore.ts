import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useAuthStore = defineStore('auth', () => {
  const isLogged = ref(false);
  const userRole = ref<'admin' | 'user' | null>(null);

  const login = (role: 'admin' | 'user') => {
    isLogged.value = true;
    userRole.value = role;
  };

  const logout = () => {
    isLogged.value = false;
    userRole.value = null;
  };

  return { isLogged, userRole, login, logout };
});