import type { AuthResponse, UsuarioClans } from '../types'
import { request } from './http'

export const authApi = {
  async login(email: string, password: string): Promise<string> {
    const response = await request<AuthResponse>('/api/auth', {
      method: 'POST',
      body: JSON.stringify({ email, password }),
    })

    if (!response.token) {
      throw new Error('Token não retornado pela API')
    }

    return response.token
  },

  getClans(token: string, email: string) {
    return request<UsuarioClans>(`/api/usuarios/clans/${email}`, { token })
  },
}
