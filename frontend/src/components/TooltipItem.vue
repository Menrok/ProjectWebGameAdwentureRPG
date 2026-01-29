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

  width: 220px;
  padding: 12px 12px 10px;

  background: rgba(14, 17, 21, 0.95);
  backdrop-filter: blur(10px);

  color: #e5e7eb;
  font-size: 12px;

  border: 1px solid rgba(255,255,255,0.12);
  border-radius: 10px;

  box-shadow:
    0 0 0 1px rgba(0,0,0,0.8),
    0 16px 40px rgba(0,0,0,0.85);

  z-index: 50;
  animation: tooltipIn 0.15s ease-out;
}

@keyframes tooltipIn {
  from {
    opacity: 0;
    transform: translateX(-50%) translateY(4px) scale(0.96);
  }
  to {
    opacity: 1;
    transform: translateX(-50%) translateY(0) scale(1);
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
  border-color:
    rgba(14, 17, 21, 0.95)
    transparent
    transparent
    transparent;
}

.name {
  display: block;
  font-size: 13px;
  font-weight: 600;
  letter-spacing: 0.3px;
  margin-bottom: 4px;
  color: #f3f4f6;
}

.description {
  font-size: 12px;
  opacity: 0.8;
  margin-bottom: 6px;
  line-height: 1.4;
}

.stats {
  font-size: 11px;
  opacity: 0.85;
  margin-bottom: 8px;

  display: flex;
  flex-direction: column;
  gap: 2px;
}

button {
  width: 100%;
  margin-top: 6px;
  padding: 6px 8px;

  background: rgba(20, 24, 28, 0.95);
  border: 1px solid rgba(255,255,255,0.14);
  border-radius: 6px;

  color: #e5e7eb;
  font-size: 12px;
  font-weight: 600;
  letter-spacing: 0.3px;

  cursor: pointer;
  transition: background 0.15s ease;
}

button:hover {
  background: rgba(30, 36, 42, 0.95);
}
</style>
