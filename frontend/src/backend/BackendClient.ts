import { BackendClientBase } from "./BackendClientBase"

export class BackendClient extends BackendClientBase {

  // =========================
  // AUTH
  // =========================

  login(username: string, password: string) {
    return this.request<{ token: string }>("/api/auth/login", {
      method: "POST",
      body: JSON.stringify({ username, password })
    })
  }

  register(username: string, password: string) {
    return this.request<string>("/api/auth/register", {
      method: "POST",
      body: JSON.stringify({ username, password })
    })
  }
}