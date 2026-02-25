
import { defineStore } from 'pinia';
import { ref, computed } from 'vue';

export const useAuthStore = defineStore('auth', () => {
  const isLogged = ref(false);
  const userName = ref<string | null>(null);
  const userRole = ref<string | null>(null);

  // Try-catch para evitar que un JSON mal formateado rompa la App
  try {
    const savedUser = localStorage.getItem('user_session');
    if (savedUser) {
      const data = JSON.parse(savedUser);
      isLogged.value = true;
      userName.value = data.name;
      userRole.value = data.role;
    }
  } catch (e) {
    console.error("Error recuperando sesión:", e);
    localStorage.removeItem('user_session');
  }

  const isAdmin = computed(() => userRole.value === 'admin');

  const login = (name: string, role: string) => {
    isLogged.value = true;
    userName.value = name;
    userRole.value = role;
    localStorage.setItem('user_session', JSON.stringify({ name, role }));
  };

  const logout = () => {
    isLogged.value = false;
    userName.value = null;
    userRole.value = null;
    localStorage.removeItem('user_session');
    // CAMBIO: Usamos router si es posible, o recarga limpia
    window.location.href = '/login';
  };

  return { isLogged, userName, userRole, isAdmin, login, logout };
});