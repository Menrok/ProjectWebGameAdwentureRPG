<script setup lang="ts">
import type { ActionResultDto } from "@/backend/BackendClient"

const props = defineProps<{
  location: {
    id: string
    name: string
    description: string
  }
  actions: {
    id: string
    text: string
  }[]
  connectedLocations: {
    id: string
    name: string
  }[]
  actionResult?: ActionResultDto | null
}>()

const emit = defineEmits<{
  (e: "action", actionId: string): void
  (e: "move", locationId: string): void
  (e: "close"): void
  (e: "back"): void
}>()
</script>

<template>
<div class="modal-overlay">
  <div class="story-card">
      <template v-if="!actionResult">
        <h2 class="location-name">{{ location.name }}</h2>

        <p class="story-text">
          {{ location.description }}
        </p>

        <div v-if="actions.length" class="section">
          <h3>Co możesz zrobić:</h3>

          <button v-for="action in actions" :key="action.id" type="button" class="choice-button wide" @click="$emit('action', action.id)">
            {{ action.text }}
          </button>
        </div>

        <button class="close" @click="$emit('close')">
          Zamknij
        </button>
      </template>

      <template v-else>
        <p class="story-text">
          {{ actionResult.text }}
        </p>

        <div v-if="actionResult.discoveredLocations?.length" class="discovered">
          <div v-for="loc in actionResult.discoveredLocations" :key="loc" class="discovered-item">
            Odkryto nową lokalizację:
            <span>{{ loc }}</span>
          </div>
        </div>

        <div v-if="actionResult.items?.length" class="found-items">
          <div v-for="item in actionResult.items" :key="item.code" class="item">
            <img :src="item.icon" :alt="item.name" />
          </div>
        </div>

        <div class="choices">
          <button class="choice-button" @click="$emit('back')">
            Dalej
          </button>
        </div>
      </template>
    </div>
  </div>
</template>

<style scoped>
.modal-overlay {
  position: fixed;
  inset: 0;
  z-index: 3000;

  display: flex;
  align-items: center;
  justify-content: center;

  background: rgba(0, 0, 0, 0.25);
  backdrop-filter: blur(4px);

  pointer-events: auto;
}

.story-card {
  width: 90vw;
  max-width: 720px;

  padding: 32px 34px 30px;

  background: rgba(14, 17, 21, 0.92);
  backdrop-filter: blur(10px);

  color: #e5e7eb;

  border: 1px solid rgba(255,255,255,0.08);
  border-radius: 16px;

  box-shadow:
    0 0 0 1px rgba(0,0,0,0.8),
    0 40px 90px rgba(0,0,0,0.9);
}

.location-name {
  text-align: center;
  font-size: 20px;
  font-weight: 600;
  letter-spacing: 0.6px;

  margin-bottom: 18px;
  color: #f3f4f6;
}

.story-text {
  white-space: pre-line;
  font-size: 16px;
  line-height: 1.8;

  color: #e5e7eb;
  opacity: 0.95;

  margin-bottom: 28px;
}

.section {
  margin-top: 24px;
}

.section h3 {
  font-size: 13px;
  font-weight: 600;
  letter-spacing: 0.4px;
  opacity: 0.7;
  margin-bottom: 12px;
}

.choice-button {
  width: 100%;
  margin-bottom: 10px;

  padding: 14px 16px;

  background: rgba(20, 24, 28, 0.95);
  border: 1px solid rgba(255,255,255,0.14);
  border-radius: 10px;

  color: #e5e7eb;
  font-size: 14px;
  font-weight: 600;
  letter-spacing: 0.4px;

  cursor: pointer;
  transition: background 0.15s ease, transform 0.15s ease;
}

.choice-button:hover {
  background: rgba(30, 36, 42, 0.95);
  transform: translateY(-1px);
}

.discovered {
  margin-top: 18px;
  font-size: 13px;
  opacity: 0.85;
}

.discovered-item {
  margin-bottom: 6px;
}

.discovered-item span {
  font-weight: 600;
  margin-left: 6px;
}

.found-items {
  display: flex;
  gap: 10px;
  margin-top: 16px;
}

.found-items .item {
  width: 48px;
  height: 48px;

  background: rgba(255,255,255,0.04);
  border: 1px solid rgba(255,255,255,0.12);
  border-radius: 8px;

  display: flex;
  align-items: center;
  justify-content: center;
}

.found-items img {
  max-width: 70%;
  max-height: 70%;
  filter: drop-shadow(0 2px 6px rgba(0,0,0,0.9));
}

.close,
.choices button {
  margin-top: 18px;
  width: 100%;

  background: rgba(20, 24, 28, 0.95);
  border: 1px solid rgba(255,255,255,0.14);
  border-radius: 10px;

  color: #e5e7eb;
  padding: 12px 14px;

  font-size: 14px;
  font-weight: 600;
  letter-spacing: 0.4px;

  cursor: pointer;
  transition: background 0.15s ease;
}

.close:hover,
.choices button:hover {
  background: rgba(30, 36, 42, 0.95);
}
</style>
