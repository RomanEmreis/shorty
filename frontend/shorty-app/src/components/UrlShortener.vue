<script setup lang="ts">
import UrlInput  from '@/components/UrlInput.vue';
import UrlOutput from '@/components/UrlOutput.vue'
</script>

<template>
  <div class='center-screen'>
    <UrlInput />
    <UrlOutput />
    <div class='copyright'>Created by Roman Emreis</div>
  </div>
</template>

<style scoped>
.center-screen {
  width: 65%;
  display: block;
  text-align: center;
}

.copyright {
  margin-top: 0.7rem;
  font-size: small;
}
</style>