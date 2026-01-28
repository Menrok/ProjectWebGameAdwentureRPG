<script setup lang="ts">
defineProps<{
  text: string
}>()

const emit = defineEmits<{
  (e: "close"): void
}>()
</script>

<template>
  <div class="action-view">
    <p class="action-text">
      {{ text }}
    </p>

    <button class="continue" @click="$emit('close')">
      Dalej
    </button>
  </div>
</template>

<style scoped>
.action-view {
  max-width: 700px;
  margin: 0 auto;
}

.action-text {
  white-space: pre-line;
  margin-bottom: 20px;
}

.continue {
  text-align: left;
  background: #222;
  color: #fff;
  border: 1px solid #444;
  padding: 10px;
  cursor: pointer;
}

.continue:hover {
  background: #333;
}
</style>
