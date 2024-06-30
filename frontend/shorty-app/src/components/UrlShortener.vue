<script setup lang="ts">
import { useUrlStore } from '@/stores/url';
import ShortyApi from '@/api/ShortyApi';

const url          = useUrlStore();
const api          = new ShortyApi();

const handleFocus  = (e: any) => e.target?.select?.();
const handleChange = (e: any) => url.set(e.target?.value ?? '');
const handleCopy   = () => navigator.clipboard.writeText(url.new);
const handleClick  = () => {
  if (url.validate()) {
    api.create(
      url.original,
      (newUrl) => url.shorten(newUrl),
      (error) => url.setError(error)
    );
  }
};

</script>

<template>
  <div class='center-screen'>
    <div class='create-field'>
      <input class='input-field' type='text' :value="url.original" placeholder="https://www.very-long-url.com" @change.prevent="handleChange" @focus.prevent="handleFocus" />
      <button class='create-btn' @click.prevent="handleClick" >
        <img alt="create-img" src="./icons/generate.svg" width="38" height="38" />
      </button>
    </div>
    <div class='result-field' v-if="url.new">
      <a class='result-text' :href="url.new" target='_blank'>{{ url.new }}</a>
      <button class='copy-btn' @click.prevent="handleCopy">
        <img alt="copy-img" src="./icons/copy.svg" width="38" height="38" />
      </button>
    </div>
    <div class='error-field' v-if="url.error">
      <span class='error-text'>{{ url.error }}</span>
    </div>
    <div class='copyright'>Created by Roman Emreis & AI</div>
  </div>
</template>

<style scoped>
.center-screen {
  width: 65%;
  display: block;
  text-align: center;
}

.create-field {
  display: flex;
  flex-direction: row;
  border: 1px solid var(--color-border);
  border-radius: 15px;
  height: 4.3rem;
  justify-content: center;
  align-items: center;
  text-align: center;
  background-color: #232325;
}

.input-field {
  width: 90%;
  margin-left: 1.5rem;
  margin-right: 1rem;
  font-size: 1.7rem;
  color: var(--vt-c-white-mute);
  border: none;
  border-color: transparent;
  background-color: transparent;
}

.input-field:focus {
  outline-width: 0;
}

.result-field {
  margin-top: 1rem;
  display: flex;
  flex-direction: row;
  background-color: transparent;
  border: 1px solid hsla(160, 100%, 37%, 1);
  border-radius: 5px;
  height: 4rem;
  justify-content: space-between;
  align-items: center;
  text-align: center;
}

.result-text {
  margin-left: 1.5rem;
  font-size: 1.5rem;
  color:hsla(160, 100%, 37%, 1);
}

.create-btn {
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

.create-btn:hover {
  opacity: .4;
}

.create-btn img {
  position: absolute;
  top: 50%;
  left: 50%;
  -webkit-transform: translate(-50%, -50%);
  -ms-transform: translate(-50%, -50%);
  transform: translate(-50%, -50%);
}

.copy-btn {
  position: relative;
  margin-right: 0.5rem;
  height: 72%;
  width: 4rem;
  cursor: pointer;
  background: transparent;
  border-radius: 5px;
  border: 0px solid transparent;
  opacity: .8;
}

.copy-btn:hover {
  opacity: .4;
}

.create-btn img {
  position: absolute;
  top: 50%;
  left: 50%;
  -webkit-transform: translate(-50%, -50%);
  -ms-transform: translate(-50%, -50%);
  transform: translate(-50%, -50%);
}

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

.error-text {
  margin-left: 1.5rem;
  font-size: 1.5rem;
  color:#fb5c56;
}

.copyright {
  margin-top: 0.7rem;
  font-size: small;
}
</style>