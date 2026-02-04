<script setup lang="ts">
const backpackIcon = "/icons/ui/backpack.png"
const profileIcon = "/icons/ui/profile.png"
const bookIcon = "/icons/ui/book.png"

defineEmits<{
  (e: "open-inventory"): void
  (e: "open-profile"): void
  (e: "open-journal"): void
}>()
</script>

<template>
  <footer class="bottom-bar">
    <button class="icon-button" @click="$emit('open-profile')">
      <img :src="profileIcon" alt="Profil" />
    </button>

    <button class="icon-button main" @click="$emit('open-inventory')">
      <img :src="backpackIcon" alt="Plecak" />
    </button>

    <button class="icon-button" @click="$emit('open-journal')">
      <img :src="bookIcon" alt="Dziennik" />
    </button>
  </footer>
</template>

<style scoped>
.bottom-bar {
  position: fixed;
  bottom: 12px;
  left: 50%;
  transform: translateX(-50%);

  height: 64px;
  padding: 0 42px;

  display: flex;
  align-items: center;
  gap: 44px;

  background: rgba(12, 15, 18, 0.75);
  backdrop-filter: blur(12px);

  border: 1px solid rgba(255,255,255,0.08);
  border-radius: 22px;

  box-shadow:
    0 0 0 1px rgba(0,0,0,0.8),
    0 20px 40px rgba(0,0,0,0.9);

  z-index: 100;
}

.icon-button {
  background: none;
  border: none;
  padding: 0;
  cursor: pointer;

  opacity: 0.75;
  transition: opacity 0.15s ease, transform 0.15s ease;
}

.icon-button img {
  width: 45px;
  height: 45px;

  filter:
    drop-shadow(0 0 8px rgba(0,0,0,0.9));
}

.icon-button:hover {
  opacity: 1;
  transform: translateY(-1px);
}

.icon-button:active {
  transform: translateY(0) scale(0.95);
}

.icon-button.main {
  opacity: 1;
}

.icon-button.main img {
  filter:
    drop-shadow(0 0 10px rgba(255,255,255,0.15));
}
</style>