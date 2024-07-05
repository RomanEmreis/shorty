<script setup lang="ts">
import { useUrlStore } from '@/stores/url';
import UrlOutputError from '@/components/UrlOutputError.vue';
import UrlOutputResult from '@/components/UrlOutputResult.vue';
const url        = useUrlStore();
const handleCopy = () => navigator.clipboard.writeText(url.new);
</script>

<template>
  <UrlOutputResult v-if="url.new" :url="url.new" />
  <UrlOutputError v-if="url.error" :error="url.error" />
</template>

<style scoped>
</style>