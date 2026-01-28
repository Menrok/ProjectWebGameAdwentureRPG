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
    <button class="icon-button" @click="$emit('open-profile')" aria-label="Otwórz profil gracza">
      <img :src="profileIcon" alt="Profil gracza" class="icon-image" />
    </button>

    <button class="icon-button" @click="$emit('open-inventory')" aria-label="Otwórz plecak">
      <img :src="backpackIcon" alt="Plecak" class="icon-image" />
    </button>
    <button class="icon-button" @click="$emit('open-journal')" aria-label="Otwórz dziennik zadań">
      <img :src="bookIcon" alt="Dziennik zadań" class="icon-image" />
    </button>
  </footer>
</template>

<style scoped>
.bottom-bar {
  position: fixed;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);

  height: 64px;
  min-width: 340px;
  padding: 8px 20px 12px;

  display: flex;
  justify-content: center;
  align-items: center;
  gap: 20px;

  background:
    linear-gradient(
      180deg,
      #e6dec5 0%,
      #d6ccb0 100%
    );

  border: 1px solid #8e866d;
  border-bottom: none;

  border-radius: 60px 60px 0 0;

  box-shadow:
    inset 0 0 30px rgba(0, 0, 0, 0.25),
    0 -10px 30px rgba(0, 0, 0, 0.6);

  z-index: 100;
}

.icon-button {
  background: none;
  border: none;
  padding: 0;
  margin: 0;

  cursor: pointer;

  display: flex;
  align-items: center;
  justify-content: center;
}

.icon-image {
  width: 64px;
  height: 64px;

  display: block;

  transition: transform 0.12s ease, filter 0.12s ease;
}

.icon-button:hover .icon-image {
  transform: scale(1.06);
  filter: brightness(1.05);
}

.icon-button:active .icon-image {
  transform: scale(0.96);
}
</style>