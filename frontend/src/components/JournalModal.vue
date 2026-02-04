<script setup lang="ts">
import { ref, onMounted, computed } from "vue"
import { Backend } from "@/backend"
import type { QuestDto } from "@/backend/BackendClient"

const loading = ref(true)
const quests = ref<QuestDto[]>([])

const activeQuests = computed(() => quests.value.filter(q => q.status === "Active"))
const completedQuests = computed(() => quests.value.filter(q => q.status === "Completed"))

async function loadQuests() {
  loading.value = true
  try {
    quests.value = await Backend.getQuests()
  } finally {
    loading.value = false
  }
}

onMounted(loadQuests)
</script>

<template>
  <div class="backdrop" @click="$emit('close')">
    <div class="modal" @click.stop>
      <h2>Dziennik</h2>

      <div class="content">
        <div v-if="loading" class="empty"></div>

        <div v-else>
          <div v-if="activeQuests.length">
            <div v-for="quest in activeQuests" :key="quest.id" class="quest">
              <div class="quest-title">
                {{ quest.title }}
              </div>

              <div class="quest-history" v-if="quest.entries?.length">
                <div v-for="entry in quest.entries" :key="entry.stage" class="quest-entry">
                  {{ entry.text }}
                </div>
              </div>
            </div>
          </div>

          <div v-else class="empty">
            Brak aktywnych zadań
          </div>

          <div v-if="completedQuests.length" class="completed">
            <div class="section-title">
              Ukończone
            </div>

            <div v-for="quest in completedQuests" :key="quest.id" class="quest completed-quest">
              {{ quest.title }}
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.backdrop {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.25);
  backdrop-filter: blur(1px);

  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 200;
}

.modal {
  width: 94%;
  max-width: 520px;
  padding: 22px 22px 20px;

  background: rgba(14, 17, 21, 0.9);
  backdrop-filter: blur(10px);

  color: #e5e7eb;

  border: 1px solid rgba(255,255,255,0.08);
  border-radius: 14px;

  box-shadow:
    0 0 0 1px rgba(0,0,0,0.8),
    0 30px 70px rgba(0,0,0,0.9);
}

.modal h2 {
  text-align: center;
  margin-bottom: 16px;

  font-weight: 600;
  letter-spacing: 0.6px;
  color: #d1d5db;
}

.content {
  min-height: 180px;

  display: flex;
  align-items: center;
  justify-content: center;

  background: rgba(255,255,255,0.02);
  border: 1px dashed rgba(255,255,255,0.12);
  border-radius: 10px;
}

.empty {
  font-size: 13px;
  opacity: 0.6;
  letter-spacing: 0.3px;
  text-align: center;
}

button {
  margin-top: 16px;
  width: 100%;

  background: rgba(20, 24, 28, 0.9);
  border: 1px solid rgba(255,255,255,0.12);
  border-radius: 8px;

  color: #e5e7eb;
  padding: 10px 14px;

  font-weight: 600;
  letter-spacing: 0.3px;
  cursor: pointer;

  transition: background 0.15s ease;
}

button:hover {
  background: rgba(30, 36, 42, 0.9);
}

.quest {
  margin-bottom: 12px;
  padding: 10px 12px;

  background: rgba(255,255,255,0.04);
  border: 1px solid rgba(255,255,255,0.12);
  border-radius: 8px;

  font-size: 13px;
}

.quest-title {
  font-weight: 600;
  margin-bottom: 4px;
}

.quest-desc {
  font-size: 12px;
  opacity: 0.75;
}

.completed {
  margin-top: 16px;
  opacity: 0.6;
}

.section-title {
  font-size: 12px;
  margin-bottom: 6px;
  letter-spacing: 0.4px;
}

.completed-quest {
  font-size: 12px;
}

.quest-history {
  margin-top: 6px;
  padding-left: 6px;
  border-left: 2px solid rgba(255,255,255,0.08);
}

.quest-entry {
  font-size: 12px;
  opacity: 0.75;
  margin-bottom: 6px;
  line-height: 1.4;
}
</style>
