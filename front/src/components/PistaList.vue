<template>
  <v-container>
    <h1 class="text-white text-h4 mb-6">Nuestras Pistas</h1>

    <v-row v-if="pistaStore.pistas.length > 0">
      <v-col v-for="pista in pistaStore.pistas" :key="pista.idPista" cols="12" md="4">
        <v-card theme="dark" variant="outlined" class="pa-4" style="border-color: rgba(255,255,255,0.5);">
          <v-card-title class="pa-0 text-h6 font-weight-bold">
            {{ pista.nombre }} - {{ pista.tipo }}
          </v-card-title>
          <v-card-subtitle class="pa-0 mt-1">{{ pista.precioHora }}€/hora</v-card-subtitle>
          
          <v-card-text class="pa-0 mt-4 mb-4">
             <v-chip size="x-small" :color="pista.activa ? 'green' : 'red'">
              {{ pista.activa ? 'Activa' : 'Inactiva' }}
            </v-chip>
          </v-card-text>

          <v-card-actions class="pa-0">
            <v-btn 
              block 
              :color="pista.activa ? 'green-darken-1' : 'grey-darken-2'" 
              variant="flat" 
              :disabled="!pista.activa || !authStore.isLogged"
              @click="prepararReserva(pista)"
            >
              {{ authStore.isLogged ? (pista.activa ? 'RESERVAR (Pago en pista)' : 'NO DISPONIBLE') : 'LOGUEATE PARA RESERVAR' }}
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>

    <v-dialog v-model="mostrarModal" max-width="400">
      <v-card theme="dark" class="rounded-xl" border>
        <v-card-title class="bg-green-darken-1 text-white pa-4">Confirmar Reserva</v-card-title>
        <v-card-text class="pt-6">
          <p class="mb-4">Vas a reservar: <strong>{{ pistaSeleccionada?.nombre }}</strong></p>
          
          <v-text-field v-model="formReserva.fecha" label="Fecha" type="date" variant="outlined"></v-text-field>
          <v-select v-model="formReserva.horas" :items="[1, 2, 3, 4]" label="Duración (Horas)" variant="outlined"></v-select>

          <div class="text-h5 text-center mt-2 text-green font-weight-bold">
            Total: {{ precioTotal }}€
          </div>
          <p class="text-caption text-center text-grey mt-2">* El pago se realizará en el centro</p>
        </v-card-text>

        <v-card-actions class="pa-4">
          <v-btn variant="text" @click="mostrarModal = false">Cancelar</v-btn>
          <v-spacer></v-spacer>
          <v-btn color="green" variant="flat" :loading="reservaStore.isLoading" @click="confirmarReserva">
            Confirmar Reserva
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { usePistaStore } from '../stores/pistaStore';
import { useAuthStore } from '../stores/authStore';
import { useReservaStore } from '../stores/reservaStore';

const pistaStore = usePistaStore();
const authStore = useAuthStore();
const reservaStore = useReservaStore();

const mostrarModal = ref(false);
const pistaSeleccionada = ref<any>(null);
const formReserva = ref({
  fecha: new Date().toISOString().substr(0, 10),
  horas: 1
});

onMounted(() => {
  pistaStore.fetchPistas();
});

const precioTotal = computed(() => {
  return pistaSeleccionada.value ? pistaSeleccionada.value.precioHora * formReserva.value.horas : 0;
});

const prepararReserva = (pista: any) => {
  pistaSeleccionada.value = pista;
  mostrarModal.value = true;
};

const confirmarReserva = async () => {
  if (!authStore.userId) return alert("Error de sesión");

  const datos = {
    idUsuario: authStore.userId,
    idPista: pistaSeleccionada.value.idPista,
    fecha: formReserva.value.fecha,
    horas: formReserva.value.horas,
    precio: precioTotal.value
  };

  const ok = await reservaStore.crearReserva(datos);
  if (ok) {
    alert("Reserva confirmada. Te esperamos el día seleccionado.");
    mostrarModal.value = false;
  } else {
    alert("Error al guardar la reserva. Inténtalo de nuevo.");
  }
};
</script>