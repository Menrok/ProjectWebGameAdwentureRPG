export enum ItemType {
  Consumable = "Consumable",
  QuestItem = "QuestItem",
  Equipment = "Equipment"
}

export enum EquipmentSlot {
  Weapon = "Weapon",
  Boots = "Boots",
  Pants = "Pants",
  Jacket = "Jacket"
}

export enum QuestStatus {
  Active = "Active",
  Completed = "Completed"
}

export interface ItemDto {
  id: number
  name: string
  description: string
  icon: string

  itemType: ItemType
  slot?: EquipmentSlot

  minDamage?: number
  maxDamage?: number
  defenseBonus: number
  healAmount?: number
}

export interface InventoryItemDto {
  id: number
  slotIndex: number | null
  isEquipped: boolean
  item: ItemDto
}

export interface ItemRewardDto {
  code: string
  name: string
  icon: string
}

export interface ActionResultDto {
  text: string
  hpChange?: number
  crystalChange?: number
  items: ItemRewardDto[]
  discoveredLocations: string[]
  openTradeId?: string
}

export interface QuestDto {
  id: string
  title: string
  description: string
  stage: number
  status: QuestStatus
}

export interface TradeItemDto {
  code: string
  name: string
  description: string
  icon: string
  price: number
  quantity: number
}

export interface EquippedItemDto {
  slot: EquipmentSlot
  name: string
  icon: string
}

export interface PlayerStatusDto {
  playerId: number
  name: string

  health: number
  maxHealth: number

  minAttack: number
  maxAttack: number
  defense: number

  crystals: number

  equippedItems: EquippedItemDto[]
  inventoryCount: number
}

export interface GameStateDto {
  mode: "Story" | "World"

  storyNode?: string

  player?: {
    id: number
    name: string
    health: number
    maxHealth: number
  }

  location?: {
    id: string
    name: string
    description: string
  }

  connectedLocations?: {
    id: string
    name: string
  }[]

  actions?: {
    id: string
    text: string
  }[]

  flags?: {
    flag: string
  }[]
}

import { BackendClientBase } from "./BackendClientBase"

export class BackendClient extends BackendClientBase {
  login(username: string, password: string) {
    return this.request<{ token: string }>("/api/auth/login", {
      method: "POST",
      body: JSON.stringify({ username, password })
    })
  }

  register(username: string, password: string, confirmPassword: string) {
    return this.request<{ message: string }>("/api/auth/register", {
      method: "POST",
      body: JSON.stringify({ username, password, confirmPassword })
    })
  }

  getGameState() {
    return this.request<GameStateDto>("/api/game/state")
  }

  moveToLocation(locationId: string) {
    return this.request<void>(
      `/api/game/location/move/${locationId}`,
      { method: "POST" }
    )
  }

  doLocationAction(actionId: string) {
    return this.request<ActionResultDto>(
      `/api/game/location/action/${actionId}`,
      { method: "POST" }
    )
  }

  getInventory() {
    return this.request<InventoryItemDto[]>(
      "/api/game/inventory"
    )
  }

  addItem(itemId: number) {
    return this.request<void>(
      `/api/game/inventory/add/${itemId}`,
      { method: "POST" }
    )
  }

  useItem(inventoryItemId: number) {
    return this.request<void>(
      `/api/game/inventory/use/${inventoryItemId}`,
      { method: "POST" }
    )
  }

  equipItem(inventoryItemId: number) {
    return this.request<void>(
      `/api/game/equipment/equip/${inventoryItemId}`,
      { method: "POST" }
    )
  }

  unequipItem(inventoryItemId: number) {
    return this.request<void>(
      `/api/game/equipment/unequip/${inventoryItemId}`,
      { method: "POST" }
    )
  }

  getQuests() {
    return this.request<QuestDto[]>(
      "/api/game/quests"
    )
  }

  getCurrentStoryNode() {
    return this.request<any>(
      "/api/game/story/current"
    )
  }

  chooseStory(nextNodeId: string) {
    return this.request<any>(
      "/api/game/story/choose",
      {
        method: "POST",
        body: JSON.stringify({ nextNodeId })
      }
    )
  }

  getTrade(tradeId: string) {
    return this.request<TradeItemDto[]>(
      `/api/game/trade/${tradeId}`
    )
  }

  buyTradeItem(tradeId: string, itemCode: string) {
    return this.request<{
      success: boolean
      crystalsLeft: number
    }>(
      `/api/game/trade/buy`,
      {
        method: "POST",
        body: JSON.stringify({ tradeId, itemCode })
      }
    )
  }
  
  getPlayerStatus() {
    return this.request<PlayerStatusDto>(
      "/api/game/player/status"
    )
  }
}
