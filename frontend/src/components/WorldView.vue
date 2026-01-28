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
  <div class="world">
    <h2 class="location-name">
      {{ location.name }}
    </h2>

    <p class="location-description">
      {{ location.description }}
    </p>

    <div v-if="actions.length" class="section">
      <h3>Co możesz zrobić:</h3>
      <button v-for="action in actions" :key="action.id" @click="$emit('action', action.id)">
        {{ action.text }}
      </button>
    </div>

    <div v-if="connectedLocations.length" class="section">
      <h3>Gdzie możesz pójść:</h3>
      <button v-for="loc in connectedLocations" :key="loc.id" @click="$emit('move', loc.id)">
        → {{ loc.name }}
      </button>
    </div>
  </div>
</template>

<style scoped>
.world {
  max-width: 700px;
  margin: 0 auto;
}

.location-name {
  margin-bottom: 8px;
}

.location-description {
  white-space: pre-line;
  margin-bottom: 20px;
}

.section {
  margin-top: 20px;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

button {
  text-align: left;
  background: #222;
  color: #fff;
  border: 1px solid #444;
  padding: 10px;
  cursor: pointer;
}

button:hover {
  background: #333;
}
</style>
