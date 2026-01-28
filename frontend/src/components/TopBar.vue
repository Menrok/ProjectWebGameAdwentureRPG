<script setup lang="ts">
defineProps<{
  location?: string
}>()

defineEmits<{
  (e: "logout"): void
}>()
</script>

<template>
  <header class="top-bar">
    <div class="left">
      Island Adventure RPG
    </div>

    <div class="center">
      {{ location || "—" }}
    </div>

    <div class="right">
      <button @click="$emit('logout')">
        Wyloguj
      </button>
    </div>
  </header>
</template>

<style scoped>
.top-bar {
  height: 56px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 16px;

  background: #1e1e1e;
  color: #fff;
}

.left {
  font-weight: bold;
}

.center {
  font-size: 1rem;
  opacity: 0.85;
}

.right button {
  background: transparent;
  border: 1px solid #555;
  color: #fff;
  padding: 6px 12px;
  cursor: pointer;
}

.right button:hover {
  background: #333;
}
</style>
