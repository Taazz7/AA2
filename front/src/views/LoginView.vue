<template>
  <v-container class="fill-height" fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="4">
        <v-card class="elevation-12">
          <v-toolbar color="primary" dark flat>
            <v-toolbar-title>Acceso al Centro Deportivo</v-toolbar-title>
          </v-toolbar>
          
          <v-card-text>
            <v-form @submit.prevent="onSubmit">
              <v-text-field
                v-model="usuario"
                v-bind="usuarioAttrs"
                label="Usuario"
                prepend-icon="mdi-account"
                variant="outlined"
                :error-messages="errors.usuario"
              ></v-text-field>

              <v-text-field
                v-model="contraseña"
                v-bind="contraseñaAttrs"
                label="Contraseña"
                prepend-icon="mdi-lock"
                type="password"
                variant="outlined"
                :error-messages="errors.contraseña"
              ></v-text-field>

              <v-alert v-if="serverError" type="error" variant="tonal" class="mt-3">
                {{ serverError }}
              </v-alert>
            </v-form>
          </v-card-text>

          <v-card-actions>
            <v-btn color="grey" variant="text" @click="router.push('/')">Volver</v-btn>
            <v-spacer></v-spacer>
            <v-btn 
              color="primary" 
              variant="elevated" 
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

// 1. Esquema de validación (Requisito obligatorio del PDF)
const schema = yup.object({
  usuario: yup.string().required('El usuario es obligatorio'),
  contraseña: yup.string().min(4, 'Mínimo 4 caracteres').required('La contraseña es obligatoria'),
});

// 2. Configuración del formulario con VeeValidate
const { errors, defineField, handleSubmit, isSubmitting } = useForm({
  validationSchema: schema,
});

const [usuario, usuarioAttrs] = defineField('usuario');
const [contraseña, contraseñaAttrs] = defineField('contraseña');

// 3. Lógica de envío
const onSubmit = handleSubmit(async (values) => {
  serverError.value = '';
  try {
    const response = await fetch('http://localhost:3000/api/Usuario');
    if (!response.ok) throw new Error("Error de red");
    
    const usuarios = await response.json();

    // Buscamos al usuario en la tabla USUARIOS según tus campos
    const userFound = usuarios.find((u: any) => 
      u.usuario === values.usuario && u.contraseña === values.contraseña
    );

    if (userFound) {
      authStore.login(userFound.usuario, userFound.rol);
      router.push('/');
    } else {
      serverError.value = "Credenciales incorrectas";
    }
  } catch (error) {
    serverError.value = "Error al conectar con el servidor";
    console.error(error);
  }
});
</script>