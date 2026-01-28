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
  background: rgba(0, 0, 0, 0.65);
  backdrop-filter: blur(2px);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 200;
}

.modal {
  width: 94%;
  max-width: 520px;
  padding: 20px 18px 18px;
  background: linear-gradient(180deg, #e6dec5 0%, #d6ccb0 100%);
  color: #2b2a24;
  border: 1px solid #8e866d;
  border-radius: 12px;
  box-shadow:
    inset 0 0 30px rgba(0, 0, 0, 0.25),
    0 25px 60px rgba(0, 0, 0, 0.8);
}

.modal h2 {
  text-align: center;
  margin-bottom: 14px;
  font-weight: 600;
}

.content {
  min-height: 180px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.empty {
  font-size: 13px;
  opacity: 0.7;
  font-style: italic;
}

button {
  margin-top: 14px;
  width: 100%;
  background: linear-gradient(180deg, #4b5563, #1f2933);
  border: 1px solid #111827;
  border-radius: 6px;
  color: #e5e7eb;
  padding: 8px 12px;
  font-weight: 600;
  cursor: pointer;
}

button:hover {
  background: linear-gradient(180deg, #64748b, #1f2937);
}
</style>
