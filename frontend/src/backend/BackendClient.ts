import { BackendClientBase } from "./BackendClientBase"

export type ActionResultDto = {
  text: string
  items: {
    code: string
    name: string
    icon: string
  }[]
  discoveredLocations: string[]
}

export class BackendClient extends BackendClientBase {

  // =========================
  // AUTH
  // =========================

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

  // =========================
  // GAME (CORE)
  // =========================

  getGameState() {
    return this.request<{
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
    }>("/api/game/state")
  }

  // =========================
  // LOCATIONS
  // =========================

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
  // =========================
  // INVENTORY
  // =========================

  getInventory() {
    return this.request<{
      id: number
      item: {
        id: number
        name: string
        itemType: string
      }
    }[]>("/api/game/inventory")
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

  equipWeapon(inventoryItemId: number) {
    return this.request<void>(
      `/api/game/inventory/equip/weapon/${inventoryItemId}`,
      { method: "POST" }
    )
  }

  equipClothing(inventoryItemId: number) {
    return this.request<void>(
      `/api/game/inventory/equip/clothing/${inventoryItemId}`,
      { method: "POST" }
    )
  }

  unequipWeapon() {
    return this.request<void>(
      `/api/game/inventory/unequip/weapon`,
      { method: "POST" }
    )
  }

  unequipClothing() {
    return this.request<void>(
      `/api/game/inventory/unequip/clothing`,
      { method: "POST" }
    )
  }

  getPlayerStatus() {
    return this.request<{
      playerId: number
      name: string
      health: number
      maxHealth: number
      attack: number
      defense: number
      weapon?: string
      clothing?: string
      inventoryCount: number
    }>("/api/game/inventory/status")
  }

  // =========================
  // STORY
  // =========================

  getCurrentStoryNode() {
    return this.request<any>("/api/game/story/current")
  }

  chooseStory(nextNodeId: string) {
    return this.request<any>(
      `/api/game/story/choose`,
      {
        method: "POST",
        body: JSON.stringify({ nextNodeId })
      }
    )
  }
}
