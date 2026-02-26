import { defineStore } from 'pinia';
import { ref, computed } from 'vue';

export const useAuthStore = defineStore('auth', () => {
  const isLogged = ref(false);
  const userId = ref<number | null>(null);
  const userName = ref<string | null>(null);
  const userRole = ref<string | null>(null);

  // Recuperar sesión al cargar la app
  try {
    const savedUser = localStorage.getItem('user_session');
    if (savedUser) {
      const data = JSON.parse(savedUser);
      isLogged.value = true;
      userId.value = data.id;
      userName.value = data.name;
      userRole.value = data.role;
    }
  } catch (e) {
    console.error("Error recuperando sesión:", e);
    localStorage.removeItem('user_session');
  }

  const isAdmin = computed(() => userRole.value === 'admin');

  const login = (id: number, name: string, role: string) => {
    isLogged.value = true;
    userId.value = id;
    userName.value = name;
    userRole.value = role;
    localStorage.setItem('user_session', JSON.stringify({ id, name, role }));
  };

  const logout = () => {
    isLogged.value = false;
    userId.value = null;
    userName.value = null;
    userRole.value = null;
    localStorage.removeItem('user_session');
    window.location.href = '/login';
  };

  return { isLogged, userId, userName, userRole, isAdmin, login, logout };
});