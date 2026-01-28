<script setup lang="ts">
import type { ItemDto } from "@/types/items"

defineProps<{
  item: ItemDto
  isEquipped?: boolean
}>()

defineEmits<{
  (e: "use"): void
  (e: "equipWeapon"): void
  (e: "equipClothing"): void
  (e: "unequip"): void
}>()
</script>

<template>
  <div class="tooltip" @click.stop>
    <strong class="name">{{ item.name }}</strong>

    <p class="description">
      {{ item.description }}
    </p>

    <div class="stats">
      <div v-if="item.healAmount">❤️ Leczy: {{ item.healAmount }}</div>
      <div v-if="item.attackBonus">⚔️ Atak: +{{ item.attackBonus }}</div>
      <div v-if="item.defenseBonus">🛡️ Obrona: +{{ item.defenseBonus }}</div>
    </div>

    <button v-if="isEquipped" @click="$emit('unequip')">Zdejmij</button>
    <button v-else-if="item.itemType === 'Consumable'" @click="$emit('use')">Użyj</button>
    <button v-else-if="item.itemType === 'Weapon'" @click="$emit('equipWeapon')">Załóż broń</button>
    <button v-else-if="item.itemType === 'Clothing'" @click="$emit('equipClothing')">Załóż ubranie</button>
  </div>
</template>

<style scoped>
.tooltip {
  position: absolute;
  bottom: 110%;
  left: 50%;
  transform: translateX(-50%);
  width: 200px;

  background: linear-gradient(180deg, #e6dec5 0%, #d6ccb0 100%);
  color: #2b2a24;

  border: 1px solid #8e866d;
  border-radius: 10px;
  padding: 10px;

  font-size: 12px;
  z-index: 50;

  box-shadow:
    inset 0 0 18px rgba(0, 0, 0, 0.18),
    0 12px 30px rgba(0, 0, 0, 0.55);

    animation: tooltipIn 0.15s ease-out;
}

@keyframes tooltipIn {
  from {
    opacity: 0;
    transform: translateX(-50%) scale(0.95);
  }
  to {
    opacity: 1;
    transform: translateX(-50%) scale(1);
  }
}

.tooltip::after {
  content: "";
  position: absolute;
  top: 100%;
  left: 50%;
  transform: translateX(-50%);

  border-width: 7px;
  border-style: solid;
  border-color: #d6ccb0 transparent transparent transparent;
}

.name {
  display: block;
  font-size: 13px;
  font-weight: 600;
  margin-bottom: 4px;
}

.description {
  font-size: 12px;
  opacity: 0.85;
  margin-bottom: 6px;
}

.stats {
  font-size: 11px;
  opacity: 0.9;
  margin-bottom: 8px;
}

button {
  width: 100%;
  margin-top: 4px;
  padding: 6px;

  background: linear-gradient(180deg, #4b5563, #1f2933);
  border: 1px solid #111827;
  border-radius: 6px;

  color: #e5e7eb;
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
}
</style>
