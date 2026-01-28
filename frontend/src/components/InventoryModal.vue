<script setup lang="ts">
import { onMounted, ref, computed } from "vue"
import { Backend } from "@/backend"
import TooltipItem from "@/components/TooltipItem.vue"
import type { InventoryItemDto, InventorySlot } from "@/types/items"

defineEmits<{
  (e: "close"): void
}>()

const MAX_SLOTS = 10

const loading = ref(true)
const items = ref<InventoryItemDto[]>([])
const selectedItemId = ref<number | null>(null)
const initialLoading = ref(true)

async function loadInventory(showLoader = false) {
  if (showLoader) loading.value = true

  try {
    const raw = await Backend.getInventory()
      items.value = raw
        .filter((i: any) => !i.isEquipped)
        .map((i: any): InventoryItemDto => ({
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
    initialLoading.value = false
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
  if (!slot.item) return

  if (selectedItemId.value === slot.item.id) {
    selectedItemId.value = null
  } else {
    selectedItemId.value = slot.item.id
  }
}

async function useItem(id: number) {
  await Backend.useItem(id)
  items.value = items.value.filter(i => i.id !== id)
  selectedItemId.value = null
}

async function equipWeaponItem(id: number) {
  await Backend.equipWeapon(id)
  items.value = items.value.filter(i => i.id !== id)
  selectedItemId.value = null
}

async function equipClothingItem(id: number) {
  await Backend.equipClothing(id)
  items.value = items.value.filter(i => i.id !== id)
  selectedItemId.value = null
}

onMounted(() => loadInventory(true))
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
            selected: slot.item && selectedItemId === slot.item.id
          }" @click="selectSlot(slot)">
          <Transition name="slot">
            <img v-if="slot.item" :key="slot.item.id" :src="slot.item.item.icon" class="icon"/>
          </Transition>

          <TooltipItem v-if="slot.item && selectedItemId === slot.item.id" :item="slot.item.item"
            @use="useItem(slot.item.id)" @equipWeapon="equipWeaponItem(slot.item.id)" @equipClothing="equipClothingItem(slot.item.id)"/>
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
  position: relative;
}

.slot.empty {
  background: rgba(255, 255, 255, 0.15);
  border-style: dashed;
  cursor: default;
}

.slot.selected {
  border-color: #3b4c5a;
  box-shadow: inset 0 0 6px rgba(0, 0, 0, 0.25);
}

.slot-enter-from {
  opacity: 0;
  transform: scale(0.8);
}

.slot-enter-active {
  transition: all 0.2s ease;
}

.icon {
  max-width: 70%;
  max-height: 70%;
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
