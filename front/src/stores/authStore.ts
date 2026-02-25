import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useAuthStore = defineStore('auth', () => {
    // Estado inicial: no logueado
    const isLogged = ref(false);
    const userRole = ref<string | null>(null);
    const userEmail = ref<string | null>(null);

    // Función para simular o realizar el login
    const login = (email: string, role: string) => {
        isLogged.value = true;
        userEmail.value = email;
        userRole.value = role; // 'admin' o 'user'
    };

    const logout = () => {
        isLogged.value = false;
        userRole.value = null;
        userEmail.value = null;
    };

    return { isLogged, userRole, userEmail, login, logout };
});