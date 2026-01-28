<script setup lang="ts">
import { onMounted, ref } from "vue"
import { Backend } from "@/backend"
import { useRouter } from "vue-router"
import TopBar from "@/components/TopBar.vue"
import BottomBar from "@/components/BottomBar.vue"
import InventoryModal from "@/components/InventoryModal.vue"
import StoryView from "@/components/StoryView.vue"
import WorldView from "@/components/WorldView.vue"
import ProfileModal from "@/components/ProfileModal.vue"
import JournalModal from "@/components/JournalModal.vue"
import ActionView from "@/components/ActionView.vue"
import type { ActionResultDto } from "@/backend/BackendClient"

const router = useRouter()

const initialLoading = ref(true)

const mode = ref<"Story" | "World">("Story")
const locationName = ref("")
const showInventory = ref(false)
const showProfile = ref(false)
const showJournal = ref(false)

const location = ref<any>(null)
const actions = ref<any[]>([])
const actionEvent = ref<ActionResultDto | null>(null)
const connectedLocations = ref<any[]>([])

async function fetchGameState() {
  try {
    const state = await Backend.getGameState()
    mode.value = state.mode

    if (state.mode === "World") {
      location.value = state.location ?? null
      actions.value = state.actions ?? []
      connectedLocations.value = state.connectedLocations ?? []
      locationName.value = state.location?.name ?? ""
    } else {
      location.value = null
      actions.value = []
      connectedLocations.value = []
      locationName.value = ""
    }
  } catch {
    localStorage.removeItem("token")
    router.push("/")
  }
}

async function initGame() {
  initialLoading.value = true
  await fetchGameState()
  initialLoading.value = false
}

function logout() {
  localStorage.removeItem("token")
  router.push("/")
}

function openInventory() {
  showInventory.value = true
}

function closeInventory() {
  showInventory.value = false
}

function openProfile() {
  showProfile.value = true
}

function closeProfile() {
  showProfile.value = false
}
function openJournal() {
  showJournal.value = true
}

function closeJournal() {
  showJournal.value = false
}

async function doAction(actionId: string) {
  actionEvent.value = await Backend.doLocationAction(actionId)
}

async function closeActionEvent() {
  actionEvent.value = null
  await fetchGameState()
}

async function moveTo(locationId: string) {
  await Backend.moveToLocation(locationId)
  await fetchGameState()
}

onMounted(initGame)
</script>


<template>
  <div class="game-layout">
    <TopBar :location="mode === 'World' ? locationName : ''" @logout="logout"/>

    <main class="game-content">
      <div v-if="initialLoading">        
        Ładowanie gry...
      </div>
      <Transition name="fade-slide" mode="out-in">
        <StoryView v-if="mode === 'Story'" @finished="fetchGameState"/>
        <ActionView v-else-if="actionEvent" :text="actionEvent.text" :items="actionEvent.items" @close="closeActionEvent"/>
        <WorldView v-else-if="location" :key="location.id" :location="location" :actions="actions" :connectedLocations="connectedLocations" @action="doAction" @move="moveTo"/>
      </Transition>
    </main>

    <BottomBar @open-inventory="openInventory" @open-profile="openProfile" @open-journal="openJournal"/>

    <InventoryModal v-if="showInventory" @close="closeInventory"/>
    <ProfileModal v-if="showProfile" @close="closeProfile"/>
    <JournalModal v-if="showJournal" @close="closeJournal"/>
  </div>
</template>

<style scoped>
.game-layout {
  display: flex;
  flex-direction: column;
  height: 100vh;
}

.game-content {
  flex: 1;
  padding: 16px;
}

.fade-slide-enter-active,
.fade-slide-leave-active {
  transition: opacity 0.3s ease, transform 0.3s ease;
}

.fade-slide-enter-from {
  opacity: 0;
  transform: translateY(10px);
}

.fade-slide-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}

</style>