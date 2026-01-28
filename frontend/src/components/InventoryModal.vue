<script setup lang="ts">
import { onMounted, ref, computed } from "vue"
import { Backend } from "@/backend"

defineEmits<{
  (e: "close"): void
}>()

type InventoryItem = {
  id: number
  item: {
    id: number
    name: string
    description: string
    icon: string
    itemType: "Consumable" | "Equipment"
    healAmount?: number
    attackBonus?: number
    defenseBonus?: number
  }
}

type InventorySlot =
  | { empty: true }
  | { empty: false; item: InventoryItem }

const MAX_SLOTS = 10

const loading = ref(true)
const items = ref<InventoryItem[]>([])
const selectedItem = ref<InventoryItem | null>(null)

function mapItemType(value: number | string): "Consumable" | "Equipment" {
  if (value === 0 || value === "Consumable") return "Consumable"
  return "Equipment"
}

async function loadInventory() {
  loading.value = true
  try {
    const raw = await Backend.getInventory()

    items.value = raw.map((i: any): InventoryItem => ({
      id: i.id,
      item: {
        id: i.item.id,
        name: i.item.name,
        description: i.item.description,
        icon: i.item.icon,
        itemType: mapItemType(i.item.itemType),
        healAmount: i.item.healAmount,
        attackBonus: i.item.attackBonus,
        defenseBonus: i.item.defenseBonus
      }
    }))
  } catch (e) {
    console.error("Inventory error", e)
    items.value = []
  } finally {
    loading.value = false
  }

  if (
    selectedItem.value &&
    !items.value.find(i => i.id === selectedItem.value!.id)
  ) {
    selectedItem.value = null
  }
}

const slots = computed<InventorySlot[]>(() => {
  const filled = items.value.slice(0, MAX_SLOTS)

  return [
    ...filled.map(item => ({
      empty: false as const,
      item
    })),
    ...Array.from(
      { length: MAX_SLOTS - filled.length },
      () => ({ empty: true as const })
    )
  ]
})

function selectSlot(slot: InventorySlot) {
  if (slot.empty) {
    selectedItem.value = null
    return
  }

  selectedItem.value = slot.item
}

async function useItem(inventoryItemId: number) {
  items.value = items.value.filter(i => i.id !== inventoryItemId)
  selectedItem.value = null

  await Backend.useItem(inventoryItemId)
}

onMounted(loadInventory)
</script>

<template>
  <div class="backdrop" @click="$emit('close')">
    <div class="modal" @click.stop>
      <h2>Plecak</h2>

      <div v-if="loading">
        Ładowanie...
      </div>

      <div v-else class="grid">
        <div v-for="(slot, index) in slots" :key="index" class="slot" :class="{
            empty: slot.empty, selected: !slot.empty && selectedItem?.id === slot.item.id
          }"
          @click="selectSlot(slot)">
          <template v-if="!slot.empty">
            <img v-if="slot.item.item.icon" :src="slot.item.item.icon" alt="" class="icon"/>
          </template>

          <template v-else>
            <span class="empty-text">Pusto</span>
          </template>
        </div>
      </div>

      <div v-if="selectedItem" class="details">
        <h3>{{ selectedItem.item.name }}</h3>

        <p class="description">
          {{ selectedItem.item.description }}
        </p>

        <div class="stats">
          <div v-if="selectedItem.item.healAmount">
            ❤️ Leczy: {{ selectedItem.item.healAmount }} HP
          </div>

          <div v-if="selectedItem.item.attackBonus">
            ⚔️ Atak: +{{ selectedItem.item.attackBonus }}
          </div>

          <div v-if="selectedItem.item.defenseBonus">
            🛡️ Obrona: +{{ selectedItem.item.defenseBonus }}
          </div>
        </div>

        <button v-if="selectedItem.item.itemType === 'Consumable'" @click="useItem(selectedItem.id)">
          Użyj
        </button>
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
  width: 92%;
  max-width: 420px;
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

.grid {
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 8px;
  margin: 14px 0 10px;
}

.slot {
  aspect-ratio: 1 / 1;
  background: rgba(255, 255, 255, 0.35);
  border: 1px solid #7a745e;
  border-radius: 6px;
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
}

.slot.empty {
  background: rgba(255, 255, 255, 0.15);
  border-style: dashed;
}

.slot.selected {
  border-color: #3b4c5a;
  box-shadow: inset 0 0 6px rgba(0, 0, 0, 0.25);
}

.icon {
  max-width: 70%;
  max-height: 70%;
}

.details {
  margin-top: 12px;
  padding-top: 10px;
  border-top: 1px solid #8e866d;
}

.description {
  font-size: 13px;
  margin-bottom: 8px;
  opacity: 0.85;
}

.stats {
  font-size: 12px;
  margin-bottom: 10px;
  opacity: 0.85;
}

button {
  background: linear-gradient(180deg, #4b5563, #1f2933);
  border: 1px solid #111827;
  border-radius: 6px;
  color: #e5e7eb;
  padding: 8px 12px;
  font-weight: 600;
  cursor: pointer;
}

.close {
  width: 100%;
  margin-top: 12px;
}
</style>
