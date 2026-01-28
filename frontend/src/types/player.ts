export type PlayerStatusDto = {
  playerId: number
  name: string
  health: number
  maxHealth: number
  attack: number
  defense: number
  weapon?: string | null
  clothing?: string | null
}
