export type ItemType = "Consumable" | "Weapon" | "Clothing" | "QuestItem"

export type ItemDto = {
  id: number
  name: string
  description: string
  icon?: string
  itemType: ItemType
  healAmount?: number
  attackBonus?: number
  defenseBonus?: number
}

export type InventoryItemDto = {
  id: number
  slotIndex: number
  item: ItemDto
}

export type InventorySlot = {
  index: number
  item: InventoryItemDto | null
}
