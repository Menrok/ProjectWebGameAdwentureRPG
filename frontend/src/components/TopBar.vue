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
    <div class="panel left">
      Island Adventure RPG
    </div>

    <div class="panel center">
      {{ location || "—" }}
    </div>

    <div class="panel right">
      <button @click="$emit('logout')">
        Wyloguj
      </button>
    </div>
  </header>
</template>

<style scoped>
.top-bar {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;

  height: 56px;
  padding: 0 18px;

  display: grid;
  grid-template-columns: auto 1fr auto;
  align-items: center;

  background: rgba(14, 17, 21, 0.9);
  backdrop-filter: blur(8px);

  color: #e5e7eb;

  border-bottom: 1px solid rgba(255,255,255,0.08);
  box-shadow:
    0 4px 20px rgba(0,0,0,0.6);

  z-index: 150;
}

.left {
  font-weight: 600;
  letter-spacing: 0.4px;
  white-space: nowrap;
}

.center {
  text-align: center;
  font-size: 14px;
  opacity: 0.85;
  letter-spacing: 0.3px;
}

.right {
  display: flex;
  justify-content: flex-end;
}

.right button {
  background: rgba(20, 24, 28, 0.9);
  border: 1px solid rgba(255,255,255,0.14);
  border-radius: 8px;

  color: #e5e7eb;
  padding: 6px 14px;

  font-size: 13px;
  font-weight: 600;
  letter-spacing: 0.3px;

  cursor: pointer;
  transition: background 0.15s ease;
}

.right button:hover {
  background: rgba(30, 36, 42, 0.9);
}
</style>
