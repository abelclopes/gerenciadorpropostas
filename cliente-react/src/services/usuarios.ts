import type { ApiEnvelope, ListaPaginada, Permissao, Usuario } from '../types'
import { request } from './http'

export type UsuarioPayload = {
  nome: string
  cpf: string
  email: string
  senha?: string
  dataNacimento: string
  perfilUsuario: number
}

export const usuariosApi = {
  listar(token: string, pageNumber: number, pageSize: number, buscaTermo = '') {
    return request<ListaPaginada<Usuario>>(
      `/api/usuarios?PageNumber=${pageNumber}&PageSize=${pageSize}&buscaTermo=${encodeURIComponent(buscaTermo)}`,
      { token },
    )
  },

  listarPerfis(token: string) {
    return request<Permissao[]>('/api/usuarios/permissoes', { token })
  },

  criar(token: string, payload: UsuarioPayload) {
    return request<ApiEnvelope>('/api/usuarios', {
      method: 'POST',
      token,
      body: JSON.stringify(payload),
    })
  },

  atualizar(token: string, id: string, payload: UsuarioPayload) {
    return request<ApiEnvelope>(`/api/usuarios/${id}`, {
      method: 'PUT',
      token,
      body: JSON.stringify(payload),
    })
  },

  excluir(token: string, id: string) {
    return request<ApiEnvelope>(`/api/usuarios/${id}`, {
      method: 'DELETE',
      token,
    })
  },
}
