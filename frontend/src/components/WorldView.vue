<script setup lang="ts">
defineProps<{
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
}>()

defineEmits<{
  (e: "action", actionId: string): void
  (e: "move", locationId: string): void
}>()
</script>

<template>
  <div class="story">
    <div class="story-card">
      <h2 class="location-name">
        {{ location.name }}
      </h2>

      <p class="story-text">
        {{ location.description }}
      </p>

      <div v-if="actions.length" class="section">
        <h3>Co możesz zrobić:</h3>

        <button v-for="action in actions" :key="action.id" class="choice-button wide" @click="$emit('action', action.id)">
          {{ action.text }}
        </button>
      </div>

      <div v-if="connectedLocations.length" class="section">
        <h3>Gdzie możesz pójść:</h3>

        <button v-for="loc in connectedLocations" :key="loc.id" class="choice-button wide" @click="$emit('move', loc.id)">
          → {{ loc.name }}
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

.location-name {
  margin-bottom: 12px;
  color: #f9fafb;
}

.story-text {
  white-space: pre-line;
  font-size: 15px;
  line-height: 1.6;
  color: #e5e7eb;
  margin-bottom: 20px;
}

.section {
  margin-top: 24px;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.section h3 {
  font-size: 14px;
  font-weight: 600;
  color: #9ca3af;
}

.choice-button {
  padding: 12px 18px;

  background: linear-gradient(180deg, #374151, #111827);
  border: 1px solid #020617;
  border-radius: 8px;

  color: #e5e7eb;
  font-weight: 500;
  cursor: pointer;

  transition: transform 0.15s ease, background 0.15s ease;
}

.choice-button.wide {
  text-align: left;
}

.choice-button:hover {
  background: linear-gradient(180deg, #4b5563, #1f2937);
  transform: translateY(-1px);
}
</style>
