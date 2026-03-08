import type { ApiEnvelope, Fornecedor, ListaPaginada } from '../types'
import { request } from './http'

export const fornecedoresApi = {
  listar(token: string, pageNumber: number, pageSize: number, buscaTermo = '') {
    return request<ListaPaginada<Fornecedor>>(
      `/api/fornecedores?PageNumber=${pageNumber}&PageSize=${pageSize}&buscaTermo=${encodeURIComponent(buscaTermo)}`,
      { token },
    )
  },

  buscar(token: string, busca: string) {
    return request<ApiEnvelope<Fornecedor[]>>(`/api/fornecedores/getall/${encodeURIComponent(busca)}`, { token })
  },

  criar(token: string, fornecedor: Omit<Fornecedor, 'id'>) {
    return request<ApiEnvelope>('/api/fornecedores', {
      method: 'POST',
      token,
      body: JSON.stringify(fornecedor),
    })
  },

  atualizar(token: string, fornecedor: Fornecedor) {
    return request<ApiEnvelope>(`/api/fornecedores/${fornecedor.id}`, {
      method: 'PUT',
      token,
      body: JSON.stringify(fornecedor),
    })
  },

  excluir(token: string, id: string) {
    return request<ApiEnvelope>(`/api/fornecedores/${id}`, {
      method: 'DELETE',
      token,
    })
  },
}
