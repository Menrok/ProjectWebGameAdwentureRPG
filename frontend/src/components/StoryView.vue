<script setup lang="ts">
import { onMounted, ref } from "vue"
import { Backend } from "@/backend"

const emit = defineEmits<{
  (e: "finished"): void
}>()

const loading = ref(true)
const node = ref<any>(null)

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
  <div class="story">
    <Transition name="fade-slide" mode="out-in">
      <div v-if="loading" key="loading" class="loading">
        Ładowanie historii...
      </div>

      <div v-else-if="node" :key="node.id" class="story-card">
        <p class="story-text">
          {{ node.text }}
        </p>

        <div class="choices">
          <button v-for="choice in node.choices" :key="choice.text" class="choice-button" @click="choose(choice)">
            {{ choice.text }}
          </button>
        </div>
      </div>

      <div v-else key="empty" class="loading">
        Brak aktywnej historii
      </div>
    </Transition>
  </div>
</template>

<style scoped>
.story {
  min-height: 100%;
  padding-top: 60px;
}

.story-card {
  max-width: 620px;
  margin: 0 auto;
  padding: 32px 36px;

  background: rgba(15, 23, 42, 0.65);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 10px;

  box-shadow:
    0 30px 60px rgba(0, 0, 0, 0.6),
    inset 0 0 40px rgba(0, 0, 0, 0.4);

  backdrop-filter: blur(6px);
}

.story-text {
  white-space: pre-line;
  font-size: 16px;
  line-height: 1.7;
  color: #e5e7eb;

  margin-bottom: 28px;
}

.choices {
  display: flex;
  justify-content: center;
}

.choice-button {
  min-width: 180px;
  width: 100%;
  padding: 12px 28px;

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

.choice-button:active {
  transform: translateY(0);
}

.loading {
  text-align: center;
  color: #9ca3af;
  margin-top: 120px;
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
