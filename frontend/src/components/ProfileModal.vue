<script setup lang="ts">
import { ref, computed, onMounted } from "vue"
import { Backend } from "@/backend"

defineEmits<{
  (e: "close"): void
}>()

type PlayerStatusDto = {
  playerId: number
  name: string

  health: number
  maxHealth: number

  attack: number
  defense: number
  weapon?: string | null
  clothing?: string | null

  inventoryCount: number
}

const loading = ref(true)
const player = ref<PlayerStatusDto | null>(null)

const healthPercent = computed(() => {
  if (!player.value) return 0
  return Math.max(
    0,
    Math.min(
      100,
      (player.value.health / player.value.maxHealth) * 100
    )
  )
})

async function loadPlayer() {
  loading.value = true
  try {
    player.value = await Backend.getPlayerStatus()
  } catch (e) {
    console.error("Błąd pobierania profilu gracza", e)
    player.value = null
  } finally {
    loading.value = false
  }
}

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
            <div class="slot" :class="{ empty: !player.weapon }">
              <img v-if="player.weapon" :src="player.weapon" class="icon" alt="Broń"/>
              <span v-else class="slot-label">Broń</span>
            </div>

            <div class="slot" :class="{ empty: !player.clothing }">
              <img v-if="player.clothing" :src="player.clothing" class="icon" alt="Ubranie"/>
              <span v-else class="slot-label">Ubranie</span>
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
  background: rgba(0, 0, 0, 0.65);
  backdrop-filter: blur(2px);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 200;
}

.modal {
  width: 94%;
  max-width: 480px;
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
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
}

.left {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.right {
  display: flex;
  justify-content: center;
  align-items: flex-start;
}

.name {
  font-size: 20px;
  font-weight: 700;
  text-align: center;
}

.hp-label {
  font-size: 12px;
  margin-bottom: 4px;
}

.hp-bar {
  height: 10px;
  background: rgba(0, 0, 0, 0.2);
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
  gap: 6px;
}

.stat {
  display: flex;
  justify-content: space-between;
  padding: 6px 8px;
  background: rgba(255, 255, 255, 0.35);
  border: 1px solid #8e866d;
  border-radius: 6px;
  font-size: 13px;
}

.equipment-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 8px;
  justify-items: center;
}

.slot {
  aspect-ratio: 1 / 1;
  width: 64px;

  background: rgba(255, 255, 255, 0.35);
  border: 1px solid #7a745e;
  border-radius: 6px;
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
}

.slot-label {
  position: absolute;
  inset: 0;

  display: flex;
  align-items: center;
  justify-content: center;

  font-size: 12px;
  opacity: 0.55;
  text-align: center;
  pointer-events: none;
}

.slot.empty {
  background: rgba(255, 255, 255, 0.15);
  border-style: dashed;
}

.icon {
  max-width: 70%;
  max-height: 70%;
}

.empty-text {
  font-size: 14px;
  opacity: 0.4;
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
