<script setup lang="ts">
import { useColorMode, useCycleList, usePreferredColorScheme } from '@vueuse/core';
import { watchEffect } from 'vue-demi';
import { computed } from 'vue'

const mode = useColorMode({ emitAuto: true });
const { state, next } = useCycleList(['dark', 'light'] as const, { initialValue: mode })
const prefferedMode = usePreferredColorScheme();
const currentMode = computed(() => mode.value === 'auto' ? prefferedMode.value : mode.value);

watchEffect(() => mode.value = state.value);
</script>

<template>
  <button class="theme-toggle" @click="next()">
    <img v-if="currentMode === 'dark'" alt="dark-mode-img" src="./icons/dark-mode.svg" width="22" height="22" />
    <img v-if="currentMode === 'light'" alt="light-mode-img" src="./icons/light-mode.svg" width="26" height="26" />
  </button>
</template>

<style scoped>
.theme-toggle {
  cursor: pointer;
  background-color: transparent;
  border: 0px solid transparent;
  opacity: .8;
}

.theme-toggle:hover {
  opacity: .5;
}
</style>