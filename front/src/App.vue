<template>
  <v-app>
    <HeaderComponent v-if="route.meta.layout !== 'blank'" :isAdmin="route.meta.layout === 'admin'" />

    <v-main>
      <router-view />
    </v-main>

    <FooterComponent v-if="route.meta.layout !== 'blank'" />
  </v-app>
</template>

<script setup lang="ts">
import { useRoute } from 'vue-router'
import HeaderComponent from './components/HeaderComponent.vue'
import FooterComponent from './components/FooterComponent.vue'

const route = useRoute()
</script>