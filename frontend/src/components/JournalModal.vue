<script setup lang="ts">
defineEmits<{
  (e: "close"): void
}>()
</script>

<template>
  <div class="backdrop" @click="$emit('close')">
    <div class="modal" @click.stop>
      <h2>Dziennik</h2>

      <div class="content">
        <div class="empty">
          Brak aktywnych zadań
        </div>
      </div>

      <button class="close" @click="$emit('close')">
        Zamknij
      </button>
    </div>
  </div>
</template>

<style scoped>
.backdrop {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.25);
  backdrop-filter: blur(1px);

  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 200;
}

.modal {
  width: 94%;
  max-width: 520px;
  padding: 22px 22px 20px;

  background: rgba(14, 17, 21, 0.9);
  backdrop-filter: blur(10px);

  color: #e5e7eb;

  border: 1px solid rgba(255,255,255,0.08);
  border-radius: 14px;

  box-shadow:
    0 0 0 1px rgba(0,0,0,0.8),
    0 30px 70px rgba(0,0,0,0.9);
}

.modal h2 {
  text-align: center;
  margin-bottom: 16px;

  font-weight: 600;
  letter-spacing: 0.6px;
  color: #d1d5db;
}

.content {
  min-height: 180px;

  display: flex;
  align-items: center;
  justify-content: center;

  background: rgba(255,255,255,0.02);
  border: 1px dashed rgba(255,255,255,0.12);
  border-radius: 10px;
}

.empty {
  font-size: 13px;
  opacity: 0.6;
  letter-spacing: 0.3px;
  text-align: center;
}

button {
  margin-top: 16px;
  width: 100%;

  background: rgba(20, 24, 28, 0.9);
  border: 1px solid rgba(255,255,255,0.12);
  border-radius: 8px;

  color: #e5e7eb;
  padding: 10px 14px;

  font-weight: 600;
  letter-spacing: 0.3px;
  cursor: pointer;

  transition: background 0.15s ease;
}

button:hover {
  background: rgba(30, 36, 42, 0.9);
}
</style>
