<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/authStore';

const router = useRouter();
const authStore = useAuthStore();

// Referencias para los campos del formulario
const userField = ref('');
const passField = ref('');
const errorMsg = ref('');

const handleLogin = async () => {
  try {
    // 1. Llamamos a tu API de usuarios (ajusta la URL a tu controlador)
    const response = await fetch('http://localhost:3000/api/Usuario');
    const usuarios = await response.json();

    // 2. Buscamos coincidencia (esto es una simulación de auth simple para el PDF)
    const userFound = usuarios.find((u: any) => 
      u.usuario === userField.value && u.contraseña === passField.value
    );

    if (userFound) {
      // 3. Guardamos en el store global de Pinia
      authStore.login(userFound.usuario, userFound.rol);
      
      // 4. Redirigimos según el éxito
      router.push('/');
    } else {
      errorMsg.value = "Usuario o contraseña incorrectos";
    }
  } catch (error) {
    console.error("Error en el login:", error);
  }
};

const goBack = () => router.back();
</script>