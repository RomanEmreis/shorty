<script setup lang="ts">
defineProps<{
  error: string
}>();
</script>

<template>
  <div class='error-field'>
    <span>{{ error }}</span>
  </div>
</template>

<style scoped>
.error-field {
  margin-top: 1rem;
  display: flex;
  flex-direction: row;
  background-color: transparent;
  border: 1px solid #fb5c56;
  border-radius: 5px;
  height: 4rem;
  justify-content: space-between;
  align-items: center;
  text-align: center;
}

.error-field span {
  margin-left: 1.5rem;
  font-size: 1.5rem;
  color:#fb5c56;
}
</style>