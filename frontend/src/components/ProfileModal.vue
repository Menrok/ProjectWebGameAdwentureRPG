<script setup lang="ts">
import { ref, computed, onMounted } from "vue"
import { Backend } from "@/backend"
import TooltipItem from "@/components/TooltipItem.vue"
import type { PlayerStatusDto } from "@/types/player"

defineEmits<{ (e: "close"): void }>()

const loading = ref(true)
const player = ref<PlayerStatusDto | null>(null)
const activeSlot = ref<"weapon" | "clothing" | null>(null)

const healthPercent = computed(() =>
  player.value ? Math.min(100, (player.value.health / player.value.maxHealth) * 100) : 0
)

async function loadPlayer() {
  loading.value = true
  try {
    player.value = await Backend.getPlayerStatus()
  } finally {
    loading.value = false
  }
}

function toggleSlot(slot: "weapon" | "clothing") {
  activeSlot.value = activeSlot.value === slot ? null : slot
}

async function unequip(slot: "weapon" | "clothing") {
  if (!player.value) return

  if (slot === "weapon") {
    await Backend.unequipWeapon()
    player.value.weapon = null
  } else {
    await Backend.unequipClothing()
    player.value.clothing = null
  }

  activeSlot.value = null

  Backend.getPlayerStatus().then(p => {
    player.value = p
  })
}

const weaponItem = computed(() =>
  player.value?.weapon ? {
        id: -1,
        name: "Broń",
        description: "Aktualnie założona broń",
        itemType: "Weapon" as const
      } : null
)

const clothingItem = computed(() =>
  player.value?.clothing ? {
        id: -2,
        name: "Ubranie",
        description: "Aktualnie założone ubranie",
        itemType: "Clothing" as const
      } : null
)

onMounted(loadPlayer)
</script>

<template>
  <div class="backdrop" @click="$emit('close')">
    <div class="modal" @click.stop>
      <h2>Profil</h2>

      <div v-if="loading">
        Ładowanie profilu...
      </div>

      <div v-else-if="player" class="content">
        <div class="left">
          <div class="name">
            {{ player.name }}
          </div>

          <div class="hp">
            <div class="hp-label">
              HP: {{ player.health }} / {{ player.maxHealth }}
            </div>
            <div class="hp-bar">
              <div class="hp-fill" :style="{ width: healthPercent + '%' }"/>
            </div>
          </div>

          <div class="stats">
            <div class="stat">
              <span>Atak</span>
              <strong>{{ player.attack }}</strong>
            </div>
            <div class="stat">
              <span>Obrona</span>
              <strong>{{ player.defense }}</strong>
            </div>
          </div>
        </div>

        <div class="right">
          <div class="equipment-grid">
            <div class="slot" :class="{ empty: !player.weapon }" @click="player.weapon && toggleSlot('weapon')">
              <img v-if="player.weapon" :src="player.weapon" class="icon"/>
              <span v-else class="slot-label">Broń</span>

              <TooltipItem v-if="activeSlot === 'weapon' && weaponItem" :item="weaponItem" :isEquipped="true" @unequip="unequip('weapon')"/>
            </div>

            <div class="slot" :class="{ empty: !player.clothing }" @click="player.clothing && toggleSlot('clothing')">
              <img v-if="player.clothing" :src="player.clothing" class="icon"/>
              <span v-else class="slot-label">Ubranie</span>

              <TooltipItem v-if="activeSlot === 'clothing' && clothingItem" :item="clothingItem" :isEquipped="true" @unequip="unequip('clothing')"/>
            </div>

            <div class="slot empty">
              <span class="slot-label">???</span>
            </div>
          </div>
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
</style>
