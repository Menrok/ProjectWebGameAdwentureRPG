<script setup lang="ts">
import { onMounted, ref, computed } from "vue"
import { Backend } from "@/backend"
import TooltipItem from "@/components/TooltipItem.vue"
import type { InventoryItemDto } from "@/backend/BackendClient"

const MAX_SLOTS = 10

const loading = ref(true)
const items = ref<InventoryItemDto[]>([])
const selectedItemId = ref<number | null>(null)

async function loadInventory(showLoader = false) {
  if (showLoader) loading.value = true

  try {
    const raw = await Backend.getInventory()

    items.value = raw.filter(i => !i.isEquipped)
  } finally {
    loading.value = false
  }
}

const slots = computed(() =>
  Array.from({ length: MAX_SLOTS }, (_, index) => ({
    index,
    item: items.value.find(i => i.slotIndex === index) ?? null
  }))
)

function selectSlot(slot: { item: InventoryItemDto | null }) {
  if (!slot.item) return

  selectedItemId.value =
    selectedItemId.value === slot.item.id ? null : slot.item.id
}

async function useItem(id: number) {
  await Backend.useItem(id)
  await loadInventory()
  selectedItemId.value = null
}

async function equipItem(id: number) {
  await Backend.equipItem(id)
  await loadInventory()
  selectedItemId.value = null
}

onMounted(() => loadInventory(true))
</script>

<template>
  <div class="backdrop" @click="$emit('close')">
    <div class="modal" @click.stop>
      <h2>Plecak</h2>

      <div v-if="loading"></div>

      <div v-else class="grid">
        <div v-for="slot in slots" :key="slot.index" class="slot" :class="{
            empty: !slot.item,
            selected: slot.item && selectedItemId === slot.item.id
          }"
          @click="selectSlot(slot)">

          <Transition name="slot">
            <img v-if="slot.item" :key="slot.item.id" :src="slot.item.item.icon" class="icon"/>
          </Transition>

          <TooltipItem v-if="slot.item && selectedItemId === slot.item.id" :item="slot.item.item" @use="useItem(slot.item.id)" @equip="equipItem(slot.item.id)"/>
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
  width: 92%;
  max-width: 440px;
  padding: 22px 20px 20px;

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

.grid {
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 8px;
  margin: 14px 0 10px;
}

.slot {
  aspect-ratio: 1 / 1;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid rgba(255,255,255,0.12);
  border-radius: 8px;

  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
  position: relative;

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

.slot.selected {
  border-color: rgba(255,255,255,0.35);
  box-shadow:
    inset 0 0 10px rgba(0,0,0,0.6),
    0 0 12px rgba(255,255,255,0.15);
}

.slot-enter-from {
  opacity: 0;
  transform: scale(0.8);
}

.slot-enter-active {
  transition: all 0.2s ease;
}

.icon {
  max-width: 72%;
  max-height: 72%;
  filter:
    drop-shadow(0 2px 6px rgba(0,0,0,0.9));
}

button {
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

.close {
  width: 100%;
  margin-top: 14px;
}
</style>
