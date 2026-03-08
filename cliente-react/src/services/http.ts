const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5000'

type RequestConfig = RequestInit & {
  token?: string
  isFormData?: boolean
}

export async function request<T>(path: string, init?: RequestConfig): Promise<T> {
  const headers = new Headers(init?.headers ?? {})

  if (init?.token) {
    headers.set('Authorization', `Bearer ${init.token}`)
    headers.set('x-access-token', init.token)
  }

  if (!init?.isFormData && !headers.has('Content-Type')) {
    headers.set('Content-Type', 'application/json')
  }

  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...init,
    headers,
  })

  if (!response.ok) {
    const message = await response.text()
    throw new Error(message || `Erro HTTP ${response.status}`)
  }

  if (response.status === 204) {
    return {} as T
  }

  return (await response.json()) as T
}
