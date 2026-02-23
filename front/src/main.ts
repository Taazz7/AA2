import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import router from './router'

// Importar Vuetify
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import { aliases, mdi } from 'vuetify/iconsets/mdi' // Para los iconos

const vuetify = createVuetify({
  components,
  directives,
  theme: { defaultTheme: 'dark' } // O 'light' según prefieras
})

const app = createApp(App)

app.use(createPinia())
app.use(router)
app.use(vuetify) // ¡Esto es vital!

app.mount('#app')