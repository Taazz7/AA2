<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/authStore';
import Swal from 'sweetalert2'; // 1. Importamos SweetAlert2

const router = useRouter();
const authStore = useAuthStore();
const usuario = ref('');
const contraseña = ref('');
const serverError = ref('');
const loading = ref(false);

const onSubmit = async () => {
  // Validación básica antes de consultar al servidor
  if (!usuario.value || !contraseña.value) {
    Swal.fire({
      icon: 'warning',
      title: 'Campos incompletos',
      text: 'Por favor, introduce tu usuario y contraseña',
      confirmButtonColor: '#1976D2' // Azul primario de Vuetify
    });
    return;
  }

  serverError.value = '';
  loading.value = true;
  
  try {
    const response = await fetch('http://localhost:3000/api/Usuario');
    
    if (!response.ok) throw new Error("Error en la respuesta del servidor");
    
    const usuarios = await response.json();

    const userFound = usuarios.find((u: any) => 
      (u.usuarioNombre === usuario.value || u.email === usuario.value) && 
      u.contraseña === contraseña.value
    );

    if (userFound) {
      // El login() del authStore ya disparará la alerta de éxito que configuramos antes
      authStore.login(userFound.idUsuario, userFound.usuarioNombre, userFound.rol);
      router.push('/');
    } else {
      serverError.value = "Credenciales incorrectas";
      Swal.fire({
        icon: 'error',
        title: 'Error de acceso',
        text: 'Usuario o contraseña no válidos',
        confirmButtonColor: '#FF5252'
      });
    }
  } catch (error) {
    serverError.value = "Error de conexión con el servidor";
    Swal.fire({
      icon: 'error',
      title: 'Servidor no disponible',
      text: 'No se pudo conectar con la base de datos. Inténtalo más tarde.',
    });
  } finally {
    loading.value = false;
  }
};
</script>