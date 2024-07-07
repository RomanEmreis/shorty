<script setup lang="ts">
import { useColorMode } from '@vueuse/core';

defineProps<{
  handleClick: () => Promise<void>
}>();
  
const mode = useColorMode();
</script>

<template>
  <button class='generate-btn' @click.prevent="handleClick">
    <img v-if="mode === 'dark'" alt="generate-img" src="./icons/generate.svg" width="38" height="38" />
    <img v-if="mode === 'light'" alt="generate-img" src="./icons/generate-dark.svg" width="38" height="38" />
  </button>
</template>

<style scoped>
.generate-btn {
  position: relative;
  height: 72%;
  width: 4rem;
  cursor: pointer;
  margin-right: 0.5rem;
  margin-left: 1rem;
  background: transparent;
  border-radius: 5px;
  border: 0px solid transparent;
  opacity: .7;
}

.generate-btn:hover {
  opacity: .4;
}

.generate-btn img {
  position: absolute;
  top: 50%;
  left: 50%;
  -webkit-transform: translate(-50%, -50%);
  -ms-transform: translate(-50%, -50%);
  transform: translate(-50%, -50%);
}
</style>