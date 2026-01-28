<script setup lang="ts">
import { onMounted, ref, computed } from "vue"
import { Backend } from "@/backend"

defineEmits<{
  (e: "close"): void
}>()

type InventoryItem = {
  id: number
  slotIndex: number
  item: {
    id: number
    name: string
    description: string
    icon: string
    itemType: "Consumable" | "Weapon" | "Clothing" | "QuestItem"
    healAmount?: number
    attackBonus?: number
    defenseBonus?: number
  }
}

type InventorySlot = {
  index: number
  item: InventoryItem | null
}

const MAX_SLOTS = 10

const loading = ref(true)
const items = ref<InventoryItem[]>([])
const selectedItem = ref<InventoryItem | null>(null)

async function loadInventory() {
  loading.value = true
  try {
    const raw = await Backend.getInventory()

    items.value = raw
      .filter((i: any) => !i.isEquipped)
      .map((i: any): InventoryItem => ({
        id: i.id,
        slotIndex: i.slotIndex,
        item: {
          id: i.item.id,
          name: i.item.name,
          description: i.item.description,
          icon: i.item.icon,
          itemType: i.item.itemType,
          healAmount: i.item.healAmount,
          attackBonus: i.item.attackBonus,
          defenseBonus: i.item.defenseBonus
        }
      }))

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
  const result: InventorySlot[] = []

  for (let i = 0; i < MAX_SLOTS; i++) {
    result.push({
      index: i,
      item: items.value.find(it => it.slotIndex === i) ?? null
    })
  }

  return result
})

function selectSlot(slot: InventorySlot) {
  selectedItem.value = slot.item
}

async function useItem(inventoryItemId: number) {
  await Backend.useItem(inventoryItemId)
  await loadInventory()
}

async function equipWeaponItem(inventoryItemId: number) {
  await Backend.equipWeapon(inventoryItemId)
  await loadInventory()
}

async function equipClothingItem(inventoryItemId: number) {
  await Backend.equipClothing(inventoryItemId)
  await loadInventory()
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
        <div v-for="slot in slots" :key="slot.index" class="slot" :class="{
            empty: !slot.item,
            selected: slot.item && selectedItem?.id === slot.item.id
          }" @click="selectSlot(slot)">
          <Transition name="slot">
            <img v-if="slot.item" :key="slot.item.id" :src="slot.item.item.icon" class="icon"/>
          </Transition>

          <span v-if="!slot.item" class="empty-text">Pusto</span>  </div>
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

        <div class="actions">
          <button v-if="selectedItem.item.itemType === 'Consumable'" @click="useItem(selectedItem.id)">
            Użyj
          </button>

          <button v-else-if="selectedItem.item.itemType === 'Weapon'" @click="equipWeaponItem(selectedItem.id)">
            Załóż broń
          </button>

          <button v-else-if="selectedItem.item.itemType === 'Clothing'" @click="equipClothingItem(selectedItem.id)">
            Załóż ubranie
          </button>
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

.slot-enter-active,
.slot-leave-active {
  transition: all 0.25s ease;
}

.slot-enter-from {
  opacity: 0;
  transform: scale(0.8);
}

.slot-leave-to {
  opacity: 0;
  transform: scale(0.6);
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

.actions {
  display: flex;
  gap: 8px;
}

.actions button {
  flex: 1;
}
</style>
