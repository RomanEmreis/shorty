<script setup lang="ts">
const props = defineProps<{
  url: string
}>();

const handleCopy = () => navigator.clipboard.writeText(props.url);
</script>

<template>
  <div class='result-field'>
    <a :href="url" target='_blank'>{{ url }}</a>
    <button class='copy-btn' @click.prevent="handleCopy">
      <img alt="copy-img" src="./icons/copy.svg" width="38" height="38" />
    </button>
  </div>
</template>

<style scoped>
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

.result-field a {
  margin-left: 1.5rem;
  font-size: 1.5rem;
  color:hsla(160, 100%, 37%, 1);
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

.copy-btn img {
  position: absolute;
  top: 50%;
  left: 50%;
  -webkit-transform: translate(-50%, -50%);
  -ms-transform: translate(-50%, -50%);
  transform: translate(-50%, -50%);
}
</style>