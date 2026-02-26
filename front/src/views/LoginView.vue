<template>
  <v-container class="fill-height" fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="4">
        <v-card class="elevation-12" rounded="lg">
          <v-toolbar color="primary" dark flat>
            <v-toolbar-title class="text-h5">Acceso al Centro</v-toolbar-title>
          </v-toolbar>
          
          <v-card-text class="pt-6">
            <v-form @submit.prevent="onSubmit">
              <v-text-field
                v-model="usuario"
                label="Usuario o Email"
                prepend-inner-icon="mdi-account"
                variant="outlined"
              ></v-text-field>

              <v-text-field
                v-model="contraseña"
                label="Contraseña"
                prepend-inner-icon="mdi-lock"
                type="password"
                variant="outlined"
              ></v-text-field>

              <v-alert v-if="serverError" type="error" variant="tonal" class="mt-4">
                {{ serverError }}
              </v-alert>
            </v-form>
          </v-card-text>

          <v-card-actions class="pa-4">
            <v-btn color="grey" variant="text" @click="router.push('/')">Volver</v-btn>
            <v-spacer></v-spacer>
            <v-btn color="primary" :loading="loading" @click="onSubmit">Entrar</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/authStore';

const router = useRouter();
const authStore = useAuthStore();
const usuario = ref('');
const contraseña = ref('');
const serverError = ref('');
const loading = ref(false);

const onSubmit = async () => {
  serverError.value = '';
  loading.value = true;
  try {
    const response = await fetch('http://localhost:3000/api/Usuario');
    const usuarios = await response.json();

    const userFound = usuarios.find((u: any) => 
      (u.usuarioNombre === usuario.value || u.email === usuario.value) && 
      u.contraseña === contraseña.value
    );

    if (userFound) {
      // Pasamos ID, Nombre y Rol al Store
      authStore.login(userFound.idUsuario, userFound.usuarioNombre, userFound.rol);
      router.push('/');
    } else {
      serverError.value = "Credenciales incorrectas";
    }
  } catch (error) {
    serverError.value = "Error de conexión con el servidor";
  } finally {
    loading.value = false;
  }
};
</script>