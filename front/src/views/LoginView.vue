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
                v-bind="usuarioAttrs"
                label="Usuario o Email"
                placeholder="admin o admin@pistas.com"
                prepend-inner-icon="mdi-account"
                variant="outlined"
                :error-messages="errors.usuario"
                class="mb-2"
              ></v-text-field>

              <v-text-field
                v-model="contraseña"
                v-bind="contraseñaAttrs"
                label="Contraseña"
                prepend-inner-icon="mdi-lock"
                type="password"
                variant="outlined"
                :error-messages="errors.contraseña"
              ></v-text-field>

              <v-alert v-if="serverError" type="error" variant="tonal" closable class="mt-4">
                {{ serverError }}
              </v-alert>
            </v-form>
          </v-card-text>

          <v-divider></v-divider>

          <v-card-actions class="pa-4">
            <v-btn color="grey-darken-1" variant="text" @click="router.push('/')">
              Volver
            </v-btn>
            <v-spacer></v-spacer>
            <v-btn 
              color="primary" 
              variant="elevated" 
              size="large"
              :loading="isSubmitting"
              @click="onSubmit"
            >
              Entrar
            </v-btn>
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
import { useForm } from 'vee-validate';
import * as yup from 'yup';

const router = useRouter();
const authStore = useAuthStore();
const serverError = ref('');

// 1. Esquema de validación Yup
const schema = yup.object({
  usuario: yup.string().required('Introduce tu usuario o email'),
  contraseña: yup.string().min(4, 'La contraseña es demasiado corta').required('Introduce tu contraseña'),
});

const { errors, defineField, handleSubmit, isSubmitting } = useForm({
  validationSchema: schema,
});

const [usuario, usuarioAttrs] = defineField('usuario');
const [contraseña, contraseñaAttrs] = defineField('contraseña');

// Lógica de envío con soporte para Usuario/Email
const onSubmit = handleSubmit(async (values) => {
  serverError.value = '';
  try {
    const response = await fetch('http://localhost:3000/api/Usuario');
    if (!response.ok) throw new Error("Fallo de red");
    
    const usuarios = await response.json();

    /*
       Nota: Usamos nombres en minúscula (camelCase) porque es como los envía .NET por defecto.
    */
    const userFound = usuarios.find((u: any) => 
      (u.usuarioNombre === values.usuario || u.email === values.usuario) && 
      u.contraseña === values.contraseña
    );

    if (userFound) {
      // Guardamos en el Store de Pinia
      authStore.login(userFound.usuarioNombre, userFound.rol);
      
      // Redirección exitosa
      router.push('/');
    } else {
      serverError.value = "Los datos no coinciden con ningún registro";
    }
  } catch (error) {
    serverError.value = "No se pudo conectar con el servidor. Verifica Docker.";
    console.error("Error Login:", error);
  }
});
</script>