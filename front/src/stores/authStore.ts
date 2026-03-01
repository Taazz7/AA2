import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import Swal from 'sweetalert2';

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

  // 2. Login con alerta de éxito
  const login = (id: number, name: string, role: string) => {
    isLogged.value = true;
    userId.value = id;
    userName.value = name;
    userRole.value = role;
    localStorage.setItem('user_session', JSON.stringify({ id, name, role }));

    // Alerta estética de bienvenida
    Swal.fire({
      title: `¡Bienvenido, ${name}!`,
      text: 'Has iniciado sesión correctamente',
      icon: 'success',
      timer: 2000,
      showConfirmButton: false,
      toast: true, // Esto la hace pequeña y elegante arriba a la derecha
      position: 'top-end'
    });
  };

  // 3. Logout con confirmación
  const logout = () => {
    Swal.fire({
      title: '¿Cerrar sesión?',
      text: "Tendrás que volver a introducir tus credenciales",
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#42b883', // Color verde de Vue
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí, salir',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        // Limpiamos el estado
        isLogged.value = false;
        userId.value = null;
        userName.value = null;
        userRole.value = null;
        localStorage.removeItem('user_session');
        
        // Redirigimos después de que el usuario vea la confirmación
        window.location.href = '/login';
      }
    });
  };

  return { isLogged, userId, userName, userRole, isAdmin, login, logout };
});