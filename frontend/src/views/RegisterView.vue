<script setup lang="ts">
import { ref } from "vue"
import { Backend } from "@/backend"
import { useRouter } from "vue-router"

const username = ref("")
const password = ref("")
const error = ref("")
const success = ref("")
const router = useRouter()

const register = async () => {
  error.value = ""
  success.value = ""

  try {
    await Backend.register(username.value, password.value)
    success.value = "Zarejestrowano! Możesz się zalogować."
    setTimeout(() => router.push("/login"), 1000)
  } catch (e: any) {
    error.value = e.message
  }
}
</script>

<template>
  <div>
    <h1>Rejestracja</h1>

    <input v-model="username" placeholder="username" />
    <input v-model="password" type="password" placeholder="password" />

    <button @click="register">Zarejestruj</button>

    <p v-if="error" style="color:red">{{ error }}</p>
    <p v-if="success" style="color:green">{{ success }}</p>

    <p>
      Masz konto?
      <router-link to="/login">Zaloguj się</router-link>
    </p>
  </div>
</template>
