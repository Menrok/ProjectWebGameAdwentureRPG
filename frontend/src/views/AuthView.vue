<script setup lang="ts">
import { ref } from "vue"
import { Backend } from "@/backend"
import { useRouter } from "vue-router"

const mode = ref<"login" | "register">("login")

const username = ref("")
const password = ref("")
const confirmPassword = ref("")
const error = ref("")
const success = ref("")

const router = useRouter()

const validate = () => {
  if (!username.value.trim())
    return "Podaj nazwę użytkownika"

  if (!password.value)
    return "Podaj hasło"

  if (password.value.length < 6)
    return "Hasło musi mieć min. 6 znaków"

  if (mode.value === "register") {
    if (!confirmPassword.value)
      return "Powtórz hasło"

    if (password.value !== confirmPassword.value)
      return "Hasła nie są takie same"
  }

  return ""
}

const submit = async () => {
  error.value = ""
  success.value = ""

  const validationError = validate()
  if (validationError) {
    error.value = validationError
    return
  }

  try {
    if (mode.value === "login") {
      const res = await Backend.login(username.value, password.value)
      localStorage.setItem("token", res.token)
      await router.push("/game")
    } else {
      await Backend.register(username.value, password.value, confirmPassword.value)
      success.value = "Konto zostało utworzone. Możesz się zalogować."
      mode.value = "login"
    }
  } catch (e: any) {
    error.value = e.message || "Wystąpił błąd"
  }
}

const switchMode = () => {
  mode.value = mode.value === "login" ? "register" : "login"
  error.value = ""
  success.value = ""
  confirmPassword.value = ""
}
</script>

<template>
  <div class="login-page">
    <div class="login-card">
      <Transition name="fade-slide" mode="out-in">
        <div :key="mode">
          <h1>
            {{ mode === "login" ? "Logowanie" : "Rejestracja" }}
          </h1>

          <input v-model="username" placeholder="Nazwa użytkownika"/>

          <input v-model="password" type="password" placeholder="Hasło"/>

          <Transition name="fade-slide">
            <input v-if="mode === 'register'" v-model="confirmPassword" type="password" placeholder="Powtórz hasło"/>
          </Transition>

          <button @click="submit">
            {{ mode === "login" ? "Zaloguj" : "Zarejestruj" }}
          </button>

          <p v-if="error" class="message message--error">
            {{ error }}
          </p>

          <p v-if="success" class="message message--success">
            {{ success }}
          </p>

          <p class="register">
            <span v-if="mode === 'login'">
              Nie masz konta?
            </span>
            <span v-else>
              Masz konto?
            </span>

            <a @click="switchMode">
              {{ mode === "login" ? "Zarejestruj się" : "Zaloguj się" }}
            </a>
          </p>
        </div>
      </Transition>
    </div>
  </div>
</template>

<style scoped>
.login-page {
  height: 100dvh;
  min-height: 100svh;

  display: flex;
  justify-content: center;
  align-items: center;

  background:
    radial-gradient(circle at top, #1f2937 0%, #020617 70%);
  font-family: system-ui, sans-serif;

  overflow: hidden;

  padding:
    env(safe-area-inset-top)
    env(safe-area-inset-right)
    env(safe-area-inset-bottom)
    env(safe-area-inset-left);
}

.login-card {
  width: 100%;
  max-width: 380px;
  padding: 36px 32px;
  border-radius: 10px;

  background:
    linear-gradient(
      180deg,
      #e6dec5 0%,
      #d6ccb0 100%
    );

  color: #2b2a24;

  box-shadow:
    inset 0 0 40px rgba(0, 0, 0, 0.25),
    inset 0 0 120px rgba(0, 0, 0, 0.15),
    0 25px 50px rgba(0, 0, 0, 0.7);

  border: 1px solid #8e866d;
  position: relative;
  overflow: hidden;
}

.login-card::before {
  content: "";
  position: absolute;
  inset: 0;
  background:
    radial-gradient(circle at top left, rgba(0,0,0,0.35), transparent 45%),
    radial-gradient(circle at top right, rgba(0,0,0,0.3), transparent 45%),
    radial-gradient(circle at bottom left, rgba(0,0,0,0.35), transparent 45%),
    radial-gradient(circle at bottom right, rgba(0,0,0,0.3), transparent 45%);
  pointer-events: none;
}

.login-card::after {
  content: "";
  position: absolute;
  inset: 0;
  background:
    repeating-linear-gradient(
      0deg,
      rgba(0,0,0,0.02),
      rgba(0,0,0,0.02) 1px,
      transparent 1px,
      transparent 3px
    );
  pointer-events: none;
}

.login-card h1 {
  text-align: center;
  margin-bottom: 24px;
  font-weight: 600;
  letter-spacing: 0.5px;
}

.login-card input {
  width: 100%;
  padding: 12px 14px;
  margin-bottom: 14px;

  border-radius: 6px;
  border: 1px solid #7a745e;

  background: rgba(255, 255, 255, 0.55);
  color: #2b2a24;

  font-size: 14px;
}

.login-card input::placeholder {
  color: #5f5b4a;
}

.login-card input:focus {
  outline: none;
  border-color: #3b4c5a;
  box-shadow: inset 0 0 6px rgba(0, 0, 0, 0.3);
}

.login-card button {
  width: 100%;
  padding: 12px;
  margin-top: 6px;

  background: linear-gradient(180deg, #4b5563, #1f2933);
  border: 1px solid #111827;
  border-radius: 6px;

  color: #e5e7eb;
  font-weight: 600;
  cursor: pointer;

  transition: background 0.15s ease, transform 0.1s ease;
}

.login-card button:hover {
  background: linear-gradient(180deg, #64748b, #1f2937);
  transform: translateY(-1px);
}

.login-card button:active {
  transform: translateY(0);
}

.register {
  margin-top: 18px;
  text-align: center;
  font-size: 14px;
}

.register a {
  color: #374151;
  text-decoration: underline;
  cursor: pointer;
}

.register a:hover {
  color: #111827;
}

.message {
  margin-top: 14px;
  padding: 10px;

  border-radius: 6px;
  text-align: center;
  font-size: 14px;
}

.message--error {
  background: rgba(127, 29, 29, 0.85);
  color: #fee2e2;
}

.message--success {
  background: rgba(22, 101, 52, 0.75);
  color: #dcfce7;
}

.message {
  animation: fadeIn 0.2s ease;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(4px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.fade-slide-enter-active,
.fade-slide-leave-active {
  transition: opacity 0.25s ease, transform 0.25s ease;
}

.fade-slide-enter-from {
  opacity: 0;
  transform: translateY(6px);
}

.fade-slide-leave-to {
  opacity: 0;
  transform: translateY(-6px);
}
</style>
