import { createRouter, createWebHistory } from "vue-router"
import AuthView from "@/views/AuthView.vue"
import GameView from "@/views/GameView.vue"

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/", name: "auth", component: AuthView },
    { path: "/game", name: "game", component: GameView },
  ]
})

router.beforeEach((to) => {
  const token = localStorage.getItem("token")

  if (to.name === "game" && !token) {
    return { name: "auth" }
  }

  return true
})

export default router
