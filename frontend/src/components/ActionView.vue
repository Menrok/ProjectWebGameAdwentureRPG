<script setup lang="ts">
defineProps<{
  text: string
  items?: {
    code: string
    name: string
    icon: string
  }[]
  discoveredLocations?: string[]
}>()

defineEmits<{
  (e: "close"): void
}>()
</script>

<template>
  <div class="story">
    <div class="story-card">
      <p class="story-text">
        {{ text }}
      </p>

      <div v-if="discoveredLocations?.length" class="discovered">
        <div v-for="loc in discoveredLocations" :key="loc" class="discovered-item">
          Odkryto nową lokalizację:
          <span>{{ loc }}</span>
        </div>
      </div>

      <div v-if="items?.length" class="found-items">
        <div v-for="item in items" :key="item.code" class="item">
          <img :src="item.icon" :alt="item.name"/>
        </div>
      </div>

      <div class="choices">
        <button class="choice-button" @click="$emit('close')">
          Dalej
        </button>
      </div>
    </div>
  </div>
</template>


<style scoped>
.story {
  padding-top: 60px;
}

.story-card {
  max-width: 620px;
  margin: 0 auto;
  padding: 32px 36px;

  background: rgba(15, 23, 42, 0.65);
  border: 1px solid rgba(255,255,255,0.08);
  border-radius: 10px;

  box-shadow:
    0 30px 60px rgba(0,0,0,0.6),
    inset 0 0 40px rgba(0,0,0,0.4);

  backdrop-filter: blur(6px);
}

.story-text {
  white-space: pre-line;
  font-size: 16px;
  line-height: 1.7;
  color: #e5e7eb;
  margin-bottom: 28px;
}

.choices {
  display: flex;
  justify-content: center;
}

.choice-button {
  min-width: 180px;
  padding: 12px 28px;

  background: linear-gradient(180deg, #374151, #111827);
  border: 1px solid #020617;
  border-radius: 8px;

  color: #e5e7eb;
  font-weight: 600;
  letter-spacing: 0.4px;

  cursor: pointer;
  transition: transform 0.15s ease, background 0.15s ease;
}

.choice-button:hover {
  background: linear-gradient(180deg, #4b5563, #1f2937);
  transform: translateY(-1px);
}

.found-items {
  margin: 24px 0 12px;
  display: flex;
  justify-content: center;
  gap: 24px;
}

.item {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 6px;

  font-size: 13px;
  color: #d1d5db;
}

.item img {
  width: 48px;
  height: 48px;
  object-fit: contain;

  background: rgba(255,255,255,0.06);
  border: 1px solid rgba(255,255,255,0.15);
  border-radius: 8px;
  padding: 6px;
}

.discovered {
  margin: 18px 0 10px;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.discovered-item {
  font-size: 14px;
  color: #c7d2fe;

  background: rgba(99, 102, 241, 0.08);
  border: 1px solid rgba(99, 102, 241, 0.25);
  border-radius: 6px;

  padding: 8px 12px;
}

.discovered-item span {
  font-weight: 600;
  color: #e0e7ff;
}
</style>
