<template>
  <v-container class="fill-height" fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="4">
        <v-card class="elevation-12">
          <v-toolbar color="primary" dark flat>
            <v-toolbar-title>Acceso al Centro Deportivo</v-toolbar-title>
          </v-toolbar>
          <v-card-text>
            <v-form>
              <v-text-field
                label="Usuario"
                prepend-icon="mdi-account"
                type="text"
                variant="outlined"
              ></v-text-field>

              <v-text-field
                label="Contraseña"
                prepend-icon="mdi-lock"
                type="password"
                variant="outlined"
              ></v-text-field>
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-btn color="grey" variant="text" @click="goBack">
              Volver
            </v-btn>
            <v-spacer></v-spacer>
            <v-btn color="secondary" variant="text">Registrarse</v-btn>
            <v-btn color="primary" variant="elevated" @click="handleLogin">
              Entrar
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';

const router = useRouter();

const goBack = () => {
  router.back(); // Vuelve a la página anterior en el historial
};

const handleLogin = () => {
  // Aquí irá la validación con Yup/VeeValidate próximamente
  router.push('/'); 
};
</script>