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
    <div v-if="loading">
      Ładowanie historii...
    </div>

    <div v-else-if="node">
      <p class="story-text">
        {{ node.text }}
      </p>

      <div class="choices">
        <button v-for="choice in node.choices" :key="choice.text" @click="choose(choice)">
          {{ choice.text }}
        </button>
      </div>
    </div>

    <div v-else>
      <p>Brak aktywnej historii</p>
    </div>
  </div>
</template>

<style scoped>
.story {
  max-width: 700px;
  margin: 0 auto;
}

.story-text {
  white-space: pre-line;
  margin-bottom: 20px;
}

.choices {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

button {
  text-align: left;
  background: #222;
  color: #fff;
  border: 1px solid #444;
  padding: 10px;
  cursor: pointer;
}

button:hover {
  background: #333;
}
</style>
