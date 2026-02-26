<template>
  <v-app>
    <HeaderComponent v-if="route.path !== '/login'" />

    <v-main class="bg-grey-darken-4">
      <router-view />
    </v-main>

    <FooterComponent v-if="route.path !== '/login'" />
  </v-app>
</template>

<script setup lang="ts">
import { useRoute } from 'vue-router';
import HeaderComponent from './components/HeaderComponent.vue';
import FooterComponent from './components/FooterComponent.vue';

const route = useRoute();
</script>