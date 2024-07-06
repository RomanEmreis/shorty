<script setup lang="ts">
import { useUrlStore } from '@/stores/url';
import ShortyApi       from '@/api/ShortyApi';

const url          = useUrlStore();
const api          = new ShortyApi();

const handleFocus  = (e: FocusEvent)    => (e.target as HTMLInputElement)?.select?.();
const handleChange = (e: Event)         => url.set((e.target as HTMLInputElement)?.value ?? '');
const handleKeyUp  = (e: KeyboardEvent) => e.code === 'Enter'&& handleClick();
const handleClick  = async () => {
  if (url.validate()) {
    const newUrl = await api.create(url.original);
    if (newUrl) {
      url.shorten(newUrl);
    } else {
      url.setError('An error occurred while creating a short URL');
    }
  }
};

</script>

<template>
  <div class='url-input'>
    <input 
      type='text'  
      placeholder="https://www.very-long-url.com"
      :value="url.original"
      @keyup.prevent="handleKeyUp"
      @change.prevent="handleChange" 
      @focus.prevent="handleFocus" />

    <button class='generate-btn' @click.prevent="handleClick">
      <img alt="generate-img" src="./icons/generate.svg" width="38" height="38" />
    </button>
  </div>
</template>

<style scoped>
.url-input {
  display: flex;
  flex-direction: row;
  border: 1px solid var(--color-border);
  border-radius: 15px;
  height: 4.3rem;
  justify-content: center;
  align-items: center;
  text-align: center;
  background-color: var(--vt-c-divider-light-1);
}

.url-input input {
  width: 90%;
  margin-left: 1.5rem;
  margin-right: 1rem;
  font-size: 1.7rem;
  color: var(--vt-c-white-mute);
  border: none;
  border-color: transparent;
  background-color: transparent;
}

.url-input input:focus {
  outline-width: 0;
}

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