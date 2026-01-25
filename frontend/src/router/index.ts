import { createRouter, createWebHistory } from "vue-router"
import LoginView from "@/views/LoginView.vue"
import RegisterView from "@/views/RegisterView.vue"
import GameView from "@/views/GameView.vue"

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/login", component: LoginView },
    { path: "/register", component: RegisterView },
    { path: "/game", component: GameView },
    { path: "/", redirect: "/login" }
  ],
})

router.beforeEach((to) => {
  const token = localStorage.getItem("token")

  if (to.path === "/game" && !token) {
    return { path: "/login" }
  }

  if ((to.path === "/login" || to.path === "/register") && token) {
    return { path: "/game" }
  }

  return true
})

export default router
