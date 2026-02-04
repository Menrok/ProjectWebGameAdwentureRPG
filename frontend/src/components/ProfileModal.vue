<script setup lang="ts">
import { ref, computed, onMounted } from "vue"
import { Backend } from "@/backend"
import type { PlayerStatusDto } from "@/backend/BackendClient"
import TooltipItem from "@/components/TooltipItem.vue"

defineEmits<{ (e: "close"): void }>()

const player = ref<PlayerStatusDto | null>(null)
const activeSlot = ref<"Weapon" | "Boots" | "Pants" | "Jacket" | null>(null)

const healthPercent = computed(() => player.value ? Math.min(100, (player.value.health / player.value.maxHealth) * 100) : 0)

const equippedMap = computed(() => {
  const map = new Map<string, any>()
  player.value?.equippedItems.forEach(item => {
    map.set(item.slot, item)
  })
  return map
})

function toggleSlot(slot: any) {
  activeSlot.value = activeSlot.value === slot ? null : slot
}

async function unequip(slot: any) {
  const item = equippedMap.value.get(slot)
  if (!item) return

  await Backend.unequipItem(item.inventoryItemId)
  await loadPlayer()
}

async function loadPlayer() {
  player.value = await Backend.getPlayerStatus()
}

onMounted(loadPlayer)
</script>

<template>
  <div class="backdrop" @click="$emit('close')">
    <div class="modal" @click.stop>
      <h2>Profil</h2>

      <div v-if="!player"></div>

      <div v-else class="content">
        <div class="left">
          <div class="name">
            {{ player.name }}
          </div>

          <div class="hp">
            <div class="hp-label">
              HP: {{ player.health }} / {{ player.maxHealth }}
            </div>
            <div class="hp-bar">
              <div class="hp-fill" :style="{ width: healthPercent + '%' }" />
            </div>
          </div>

          <div class="stats">
            <div class="stat">
              <span>Atak</span>
              <strong>{{ player.minAttack }} – {{ player.maxAttack }}</strong>
            </div>
            <div class="stat">
              <span>Obrona</span>
              <strong>{{ player.defense }}</strong>
            </div>
            <div class="stat">
              <span>Kryształy</span>
              <strong>{{ player.crystals }}</strong>
            </div>
          </div>
        </div>

        <div class="right">
          <div class="equipment-layout">
            <div class="slot weapon" :class="{ empty: !equippedMap.get('Weapon') }" @click="equippedMap.get('Weapon') && toggleSlot('Weapon')">
              <img v-if="equippedMap.get('Weapon')" :src="equippedMap.get('Weapon').icon" class="icon"/>
              <span v-else class="slot-label">Broń</span>
              <TooltipItem v-if="activeSlot === 'Weapon' && equippedMap.get('Weapon')" :item="equippedMap.get('Weapon')" :isEquipped="true" @unequip="unequip('Weapon')"/>
            </div>

            <div class="slot pants" :class="{ empty: !equippedMap.get('Pants') }" @click="equippedMap.get('Pants') && toggleSlot('Pants')" >
              <img v-if="equippedMap.get('Pants')" :src="equippedMap.get('Pants').icon" class="icon"/>
              <span v-else class="slot-label">Spodnie</span>
              <TooltipItem v-if="activeSlot === 'Pants' && equippedMap.get('Pants')" :item="equippedMap.get('Pants')" :isEquipped="true" @unequip="unequip('Pants')"/>
            </div>

            <div class="slot jacket" :class="{ empty: !equippedMap.get('Jacket') }" @click="equippedMap.get('Jacket') && toggleSlot('Jacket')">
              <img v-if="equippedMap.get('Jacket')" :src="equippedMap.get('Jacket').icon" class="icon"/>
              <span v-else class="slot-label">Bluza</span>
              <TooltipItem v-if="activeSlot === 'Jacket' && equippedMap.get('Jacket')" :item="equippedMap.get('Jacket')" :isEquipped="true" @unequip="unequip('Jacket')"/>
            </div>

            <div class="slot boots" :class="{ empty: !equippedMap.get('Boots') }" @click="equippedMap.get('Boots') && toggleSlot('Boots')">
              <img v-if="equippedMap.get('Boots')" :src="equippedMap.get('Boots').icon" class="icon"/>
              <span v-else class="slot-label">Buty</span>
              <TooltipItem v-if="activeSlot === 'Boots' && equippedMap.get('Boots')" :item="equippedMap.get('Boots')" :isEquipped="true" @unequip="unequip('Boots')"/>
            </div>
          </div>
        </div>
      </div>
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
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 18px;
}

.left {
  display: flex;
  flex-direction: column;
  gap: 14px;
}

.name {
  font-size: 20px;
  font-weight: 700;
  text-align: center;
  letter-spacing: 0.4px;
}

.hp-label {
  font-size: 12px;
  opacity: 0.75;
  margin-bottom: 6px;
}

.hp-bar {
  height: 10px;
  background: rgba(255,255,255,0.08);
  border-radius: 6px;
  overflow: hidden;
}

.hp-fill {
  height: 100%;
  background: linear-gradient(180deg, #b91c1c, #7f1d1d);
  transition: width 0.2s ease;
}

.stats {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.stat {
  display: flex;
  justify-content: space-between;
  align-items: center;

  padding: 8px 10px;

  background: rgba(255,255,255,0.04);
  border: 1px solid rgba(255,255,255,0.12);
  border-radius: 8px;

  font-size: 13px;
  letter-spacing: 0.3px;
}

.right {
  display: flex;
  justify-content: center;
  align-items: flex-start;
}

.equipment-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 10px;
}

.slot {
  aspect-ratio: 1 / 1;
  width: 72px;

  background: rgba(255,255,255,0.04);
  border: 1px solid rgba(255,255,255,0.12);
  border-radius: 8px;

  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;

  cursor: pointer;
  transition: background 0.15s ease, border-color 0.15s ease;
}

.slot:hover {
  background: rgba(255,255,255,0.07);
}

.slot.empty {
  background: rgba(255,255,255,0.02);
  border-style: dashed;
  cursor: default;
}

.slot-label {
  position: absolute;
  inset: 0;

  display: flex;
  align-items: center;
  justify-content: center;

  font-size: 12px;
  opacity: 0.45;
  text-align: center;
  pointer-events: none;
}

.icon {
  max-width: 72%;
  max-height: 72%;
  filter:
    drop-shadow(0 2px 6px rgba(0,0,0,0.9));
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

.equipment-layout {
  display: grid;
  grid-template-columns: repeat(3, 72px);
  grid-template-rows: repeat(2, 72px);
  gap: 12px;
  justify-content: center;
}

.slot.weapon {
  grid-column: 1;
  grid-row: 2;
}

.slot.pants {
  grid-column: 2;
  grid-row: 2;
}

.slot.jacket {
  grid-column: 2;
  grid-row: 1;
}

.slot.boots {
  grid-column: 2;
  grid-row: 3;
}
</style>
