<script setup lang="ts">
import { ref } from "vue"
import { Backend } from "@/backend"
import { useRouter } from "vue-router"

const username = ref("")
const password = ref("")
const error = ref("")
const router = useRouter()

const validate = () => {
  if (!username.value.trim())
    return "Podaj nazwę użytkownika"

  if (!password.value)
    return "Podaj hasło"

  if (password.value.length < 6)
    return "Hasło musi mieć min. 6 znaków"

  return ""
}

const login = async () => {
  error.value = ""

  const validationError = validate()
  if (validationError) {
    error.value = validationError
    return
  }

  try {
    const res = await Backend.login(username.value, password.value)
    localStorage.setItem("token", res.token)
    await router.push("/game")
  } catch (e: any) {
    error.value = e.message || "Niepoprawny login lub hasło"
  }
}
</script>

<template>
  <div>
    <h1>Login</h1>

    <input v-model="username" placeholder="username" />
    <input v-model="password" type="password" placeholder="password" />

    <button @click="login">Zaloguj</button>
    <p>
    Nie masz konta?
    <router-link to="/register">Zarejestruj się</router-link>
    </p>

    <p v-if="error" class="error">{{ error }}</p>
  </div>
</template>
