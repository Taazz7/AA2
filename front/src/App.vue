<template>
  <v-app shadow>
    <HeaderComponent v-if="!$route.meta.hideLayout" />

    <v-main>
      <router-view />
    </v-main>

    <FooterComponent v-if="!$route.meta.hideLayout" />
  </v-app>
</template>

<script setup lang="ts">
import HeaderComponent from './components/HeaderComponent.vue'
import FooterComponent from './components/FooterComponent.vue'
</script>