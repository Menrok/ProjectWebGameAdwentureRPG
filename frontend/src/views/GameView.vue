<script setup lang="ts">
import { onMounted, ref } from "vue"
import { Backend } from "@/backend"
import { useRouter } from "vue-router"
import TopBar from "@/components/TopBar.vue"
import BottomBar from "@/components/BottomBar.vue"
import InventoryModal from "@/components/InventoryModal.vue"
import WorldView from "@/components/WorldView.vue"
import ProfileModal from "@/components/ProfileModal.vue"
import JournalModal from "@/components/JournalModal.vue"
import IntroView from "@/components/IntroView.vue"

const router = useRouter()

const initialLoading = ref(true)

const mode = ref<"Story" | "World" | null>(null)
const locationName = ref("")
const showInventory = ref(false)
const showProfile = ref(false)
const showJournal = ref(false)

const location = ref<any>(null)
const actions = ref<any[]>([])
const connectedLocations = ref<any[]>([])
const flags = ref<string[]>([])

async function fetchGameState() {
  try {
    const state = await Backend.getGameState()
    mode.value = state.mode

    if (state.mode === "World") {
      location.value = state.location ?? null
      actions.value = state.actions ?? []
      connectedLocations.value = state.connectedLocations ?? []
      flags.value = state.flags ?? []
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

async function onIntroFinished() {
  await fetchGameState()
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

async function moveTo(locationId: string) {
  await Backend.moveToLocation(locationId)
  await fetchGameState()
}

onMounted(initGame)
</script>

<template>
  <div class="game-layout">
    <TopBar v-if="mode === 'World'" :location="locationName" @logout="logout"/>
    <main class="game-content">
      <div v-if="initialLoading">        
        Ładowanie gry...
      </div>

      <Transition name="fade-slide" mode="out-in">
        <IntroView v-if="mode === 'Story'" @finished="onIntroFinished"/>
        <WorldView v-else-if="mode === 'World' && location" :location="location" :actions="actions" :connectedLocations="connectedLocations" :flags="flags" @move="moveTo" @refresh="fetchGameState"/>
      </Transition>
    </main>

    <BottomBar v-if="mode === 'World'" @open-inventory="openInventory" @open-profile="openProfile" @open-journal="openJournal"/>
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