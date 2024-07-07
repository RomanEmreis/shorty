<script setup lang="ts">
import UrlShortener from '@/components/UrlShortener.vue';
import ThemeToggle from '@/components/ThemeToggle.vue';
</script>

<template>
  <header>
    <span class='logo-text not-selectable'>
      <span class="logo-text-capital">S</span>horty
    </span>
    <ThemeToggle class="topright" />
  </header>

  <main>
    <UrlShortener />
  </main>
</template>

<style scoped>
header {
  display: flex;
  place-items: center;
  justify-content: center;
  margin-bottom: 5rem;
}

main {
  display: flex;
  place-items: center;
  justify-content: center;
  align-items: start;
}

.logo-text {
  font-family: "Playwrite VN", cursive;
  font-optical-sizing: auto;
  font-size: 7rem;
  font-weight: 200;
  font-style: normal;
  color: var(--header-text);
}

.not-selectable {
  -webkit-touch-callout: none;
  -webkit-user-select: none;
  -khtml-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
}

.logo-text-capital {
  margin-left: 1rem;
  color: hsla(160, 100%, 37%, 1);
}

.topright {
  position: absolute;
  top: 1rem;
  right: 1rem;
}
</style>
