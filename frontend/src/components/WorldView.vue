<script setup lang="ts">
import { Backend } from "@/backend"
import type { ActionResultDto } from "@/backend/BackendClient"
import { ref, onMounted, computed, watch } from "vue"
import LocationInteractionModal from "@/components/LocationInteractionModal.vue"

const props = defineProps<{
  location: any
  actions: any[]
  connectedLocations: any[]
  flags: string[]
}>()

const emit = defineEmits<{
  (e: "action", actionId: string): void
  (e: "move", locationId: string): void
}>()

const discoveredWorldLocations = computed(() =>
  WORLD_LOCATIONS.filter(loc =>
    localFlags.value.includes(loc.discoveredFlag)
  )
)

watch(
  () => props.flags,
  (newFlags) => {
    localFlags.value = [...newFlags]
  }
)

const WORLD_LOCATIONS = [
  {
    id: "beach",
    name: "Plaża",
    icon: "/map/locations/beach.png",
    x: 0.12,
    y: 0.40,
    discoveredFlag: "beach_discovered",
    connections: ["forest"]
  },

  {
    id: "forest",
    name: "Las",
    icon: "/map/locations/forest.png",
    x: 0.20,
    y: 0.38,
    discoveredFlag: "forest_discovered",
    connections: ["beach", "cave", "clearing_house"]
  },

  {
    id: "cave",
    name: "Jaskinia",
    icon: "/map/locations/cave.png",
    x: 0.28,
    y: 0.30,
    discoveredFlag: "cave_discovered",
    connections: ["forest"]
  },

  {
    id: "clearing_house",
    name: "Dom na polanie",
    icon: "/map/locations/clearinghouse.png",
    x: 0.27,
    y: 0.40,
    discoveredFlag: "clearing_house_discovered",
    connections: ["forest", "village"]
  },

  {
    id: "village",
    name: "Wioska",
    icon: "/map/locations/village.png",
    x: 0.32,
    y: 0.54,
    discoveredFlag: "village_discovered",
    connections: ["clearing_house"]
  }
]

const openedLocationId = ref<string | null>(null)
const actionResult = ref<ActionResultDto | null>(null)
const localFlags = ref<string[]>([...props.flags])

const MAP_WIDTH = 2985
const MAP_HEIGHT = 2000

const offset = ref({ x: 0, y: 0 })
const dragging = ref(false)
const lastPos = ref({ x: 0, y: 0 })

const viewportRef = ref<HTMLElement | null>(null)
const viewportSize = ref({ width: 0, height: 0 })

onMounted(() => {
  if (!viewportRef.value) return

  viewportSize.value = {
    width: viewportRef.value.clientWidth,
    height: viewportRef.value.clientHeight
  }

  offset.value = { x: 0, y: 0 }
})

function clamp(value: number, min: number, max: number) {
  return Math.min(Math.max(value, min), max)
}

function getPoint(e: MouseEvent | TouchEvent) {
  if ("touches" in e) {
    return e.touches[0] ?? e.changedTouches[0] ?? null
  }
  return e
}

function enterLocation(locationId: string) {
  emit("move", locationId)
  openedLocationId.value = locationId
}


function startDrag(e: MouseEvent | TouchEvent) {
  e.preventDefault()

  const point = getPoint(e)
  if (!point) return

  dragging.value = true
  lastPos.value = { x: point.clientX, y: point.clientY }
}

function onDrag(e: MouseEvent | TouchEvent) {
  if (!dragging.value) return
  e.preventDefault()

  const point = getPoint(e)
  if (!point) return

  const dx = point.clientX - lastPos.value.x
  const dy = point.clientY - lastPos.value.y

  const minX = viewportSize.value.width - MAP_WIDTH
  const minY = viewportSize.value.height - MAP_HEIGHT

  offset.value.x = clamp(offset.value.x + dx, minX, 0)
  offset.value.y = clamp(offset.value.y + dy, minY, 0)

  lastPos.value = { x: point.clientX, y: point.clientY }
}

async function handleAction(actionId: string) {
  const result = await Backend.doLocationAction(actionId)
  actionResult.value = result

  if (result.discoveredLocations?.length) {
    for (const loc of result.discoveredLocations) {
      const flag = `${loc}_discovered`

      if (!localFlags.value.includes(flag)) {
        localFlags.value.push(flag)
      }
    }
  }
}

const visibleConnections = computed(() => {
  const seen = new Set<string>()

  return WORLD_LOCATIONS.flatMap(loc => {
    if (!localFlags.value.includes(loc.discoveredFlag)) return []

    return loc.connections
      .map(targetId => {
        const target = WORLD_LOCATIONS.find(l => l.id === targetId)
        if (!target) return null
        if (!localFlags.value.includes(target.discoveredFlag)) return null

        const key = [loc.id, target.id].sort().join("-")
        if (seen.has(key)) return null

        seen.add(key)
        return { from: loc, to: target }
      })
      .filter(
        (c): c is { from: any; to: any } => c !== null
      )
  })
})

function snakePath(a: any, b: any) {
  const x1 = a.x * MAP_WIDTH
  const y1 = a.y * MAP_HEIGHT
  const x2 = b.x * MAP_WIDTH
  const y2 = b.y * MAP_HEIGHT

  const dx = x2 - x1
  const dy = y2 - y1
  const length = Math.hypot(dx, dy)

  const nx = -dy / length
  const ny = dx / length

  const waves = 2
  const amplitude = 10
  const segments = 50

  let d = `M ${x1} ${y1}`

  for (let i = 1; i <= segments; i++) {
    const t = i / segments

    const baseX = x1 + dx * t
    const baseY = y1 + dy * t

    const wave = Math.sin(t * Math.PI * 2 * waves) * amplitude

    const x = baseX + nx * wave
    const y = baseY + ny * wave

    d += ` L ${x} ${y}`
  }

  return d
}

function endDrag() {
  dragging.value = false
}
</script>

<template>
<div class="map-viewport" ref="viewportRef">
    <div class="map-canvas" :style="{ transform: `translate(${offset.x}px, ${offset.y}px)` }" @mousedown="startDrag" @mousemove="onDrag" @mouseup="endDrag" @mouseleave="endDrag" @touchstart="startDrag" @touchmove="onDrag" @touchend="endDrag">
        <img src="/map/map.png" class="world-map"/>
        <svg class="map-routes" :width="MAP_WIDTH" :height="MAP_HEIGHT">
            <path v-for="(conn, i) in visibleConnections" :key="`${conn.from.id}-${conn.to.id}`" :d="snakePath(conn.from, conn.to)" class="route-path"/>
        </svg>

        <TransitionGroup name="map-icon" tag="div">
            <button v-for="loc in discoveredWorldLocations" :key="loc.id" class="map-icon" :style="{
                left: `${loc.x * 100}%`,
                top: `${loc.y * 100}%`
            }" @click.stop="enterLocation(loc.id)">
            <img :src="loc.icon"/>
            </button>
        </TransitionGroup>
    </div>
    
    <Teleport to="body">
      <LocationInteractionModal v-if="openedLocationId" :location="location" :actions="actions" :connectedLocations="connectedLocations" :actionResult="actionResult" @action="handleAction" @move="emit('move', $event)" @close="openedLocationId = null" @back="actionResult = null"/>
    </Teleport>
</div>
</template>

<style scoped>
.map-viewport {
  position: relative;
  width: 100%;
  height: 100%;
  overflow: hidden;
  cursor: grab;
  touch-action: none;
}

.map-viewport:active {
  cursor: grabbing;
}

.map-canvas {
  position: absolute;
  top: 0;
  left: 0;
}

.world-map {
  width: 3000px;
  height: auto;
  display: block;
  user-select: none;
  -webkit-user-drag: none;
  pointer-events: none;
}

.map-icon {
  position: absolute;
  transform: translate(-50%, -50%);
  background: none;
  border: none;
}

.map-icon img {
  width: 120%;

  filter:
    drop-shadow(0 0 6px rgba(255, 230, 150, 0.8))
    drop-shadow(0 4px 8px rgba(0,0,0,0.6));

  transition: transform 0.15s ease, filter 0.15s ease;
}

.map-icon:hover img {
  transform: scale(1.1);
  filter:
    drop-shadow(0 0 10px rgba(255, 240, 180, 1))
    drop-shadow(0 6px 12px rgba(0,0,0,0.8));
}

.map-icon-enter-from {
  opacity: 0;
  transform: translate(-50%, -50%) scale(0.4);
}

.map-icon-enter-active {
  transition:
    opacity 0.4s ease,
    transform 0.4s cubic-bezier(0.22, 1, 0.36, 1);
}

.map-icon-enter-to {
  opacity: 1;
  transform: translate(-50%, -50%) scale(1);
}

.map-routes {
  position: absolute;
  top: 0;
  left: 0;
  pointer-events: auto;
}

.route-path {
  fill: none;
  stroke: rgba(255, 230, 150, 0.85);
  stroke-width: 4;
  stroke-dasharray: 10 8;
  stroke-linecap: round;

  filter:
    drop-shadow(0 0 6px rgba(255, 220, 150, 0.6));

  animation: dash 20s linear infinite;
  cursor: pointer;
}

@keyframes dash {
  to {
    stroke-dashoffset: -100;
  }
}
</style>
