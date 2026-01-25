export class BackendClientBase {
  protected baseUrl: string

  constructor(baseUrl: string) {
    this.baseUrl = baseUrl
  }

  protected async request<T>(
    url: string,
    options: RequestInit = {}
  ): Promise<T> {
    const token = localStorage.getItem("token")

    const response = await fetch(this.baseUrl + url, {
      ...options,
      headers: {
        "Content-Type": "application/json",
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
        ...options.headers
      }
    })

    if (!response.ok) {
      let message = "Błąd serwera"

      try {
        const data = await response.json()
        message = data.message ?? message
      } catch {}

      throw new Error(message)
    }

    return response.json()
  }
}