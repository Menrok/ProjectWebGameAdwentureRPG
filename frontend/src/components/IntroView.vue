<script setup lang="ts">
import { onMounted, ref, computed } from "vue"
import { Backend } from "@/backend"

const emit = defineEmits<{
  (e: "finished"): void
}>()

const loading = ref(true)
const node = ref<any>(null)

const isStorm = computed(() => {
  if (!node.value) return true
  return node.value.id === "prologue_intro"
})

async function loadStory() {
  loading.value = true
  const result = await Backend.getCurrentStoryNode()
  node.value = result
  loading.value = false
}

async function choose(choice: any) {
  if (!choice.nextNodeId) {
    node.value.text = choice.responseText
    return
  }

  const nextNode = await Backend.chooseStory(choice.nextNodeId)

  if (nextNode?.isFreeMode) {
    emit("finished")
    return
  }

  node.value = nextNode
}

onMounted(loadStory)
</script>

<template>
  <div class="intro">
    <div class="background-layer storm" :class="{ visible: isStorm }" />
    <div class="background-layer beach" :class="{ visible: !isStorm }" />

    <div class="story-card">
      <Transition name="fade-slide" mode="out-in">
        <div v-if="loading" key="loading" class="loading">
          Ładowanie...
        </div>

        <div v-else-if="node" :key="node.id">
          <p class="story-text">
            {{ node.text }}
          </p>

          <div class="choices">
            <button v-for="choice in node.choices" :key="choice.text" class="choice-button" @click="choose(choice)">
              {{ choice.text }}
            </button>
          </div>
        </div>
      </Transition>
    </div>
  </div>
</template>

<style scoped>
.intro {
  position: fixed;
  inset: 0;
  overflow: hidden;
  z-index: 2000;
}

.background-layer {
  position: absolute;
  inset: 0;

  background-size: cover;
  background-position: center;

  filter: brightness(0.5) contrast(1.1);

  opacity: 0;
  transition: opacity 1.2s ease-in-out;
  will-change: opacity;
}

.background-layer.visible {
  opacity: 1;
}

.background-layer.storm {
  background-image: url("/intro.png");
}

.background-layer.beach {
  background-image: url("/introbeach.png");
}

.story-card {
  position: relative;
  z-index: 1;

  max-width: 720px;
  margin: 0 auto;
  padding: 40px 42px;

  top: 20%;

  background: rgba(15, 23, 42, 0.7);
  border: 1px solid rgba(255,255,255,0.08);
  border-radius: 12px;

  box-shadow:
    0 40px 80px rgba(0,0,0,0.8),
    inset 0 0 60px rgba(0,0,0,0.4);

  backdrop-filter: blur(8px);
}

.story-text {
  white-space: pre-line;
  font-size: 17px;
  line-height: 1.8;
  color: #e5e7eb;
  margin-bottom: 32px;
}

.choices {
  display: flex;
  justify-content: center;
}

.choice-button {
  min-width: 200px;
  padding: 14px 32px;

  background: linear-gradient(180deg, #374151, #111827);
  border: 1px solid #020617;
  border-radius: 8px;

  color: #e5e7eb;
  font-weight: 600;
  letter-spacing: 0.4px;

  cursor: pointer;
  transition: transform 0.15s ease, background 0.15s ease;
}

.choice-button:hover {
  background: linear-gradient(180deg, #4b5563, #1f2937);
  transform: translateY(-1px);
}

.loading {
  text-align: center;
  color: #9ca3af;
}

.fade-slide-enter-active,
.fade-slide-leave-active {
  transition: opacity 0.4s ease, transform 0.4s ease;
}

.fade-slide-enter-from {
  opacity: 0;
  transform: translateY(12px);
}

.fade-slide-leave-to {
  opacity: 0;
  transform: translateY(-12px);
}
</style>
