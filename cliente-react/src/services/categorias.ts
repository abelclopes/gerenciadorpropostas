import type { ApiEnvelope, Categoria, ListaPaginada } from '../types'
import { request } from './http'

export const categoriasApi = {
  listar(token: string, pageNumber: number, pageSize: number, buscaTermo = '') {
    return request<ListaPaginada<Categoria>>(
      `/api/categorias?PageNumber=${pageNumber}&PageSize=${pageSize}&buscaTermo=${encodeURIComponent(buscaTermo)}`,
      { token },
    )
  },

  listarTodas(token: string) {
    return request<ApiEnvelope<Categoria[]>>('/api/categorias/getall', { token })
  },

  criar(token: string, nome: string, descricao: string) {
    return request<ApiEnvelope>('/api/categorias', {
      method: 'POST',
      token,
      body: JSON.stringify({ nome, descricao }),
    })
  },

  atualizar(token: string, categoria: Categoria) {
    const query = new URLSearchParams({ nome: categoria.nome, descricao: categoria.descricao })
    return request<ApiEnvelope>(`/api/categorias/${categoria.id}?${query.toString()}`, {
      method: 'PUT',
      token,
    })
  },

  excluir(token: string, id: string) {
    return request<ApiEnvelope>(`/api/categorias/${id}`, {
      method: 'DELETE',
      token,
    })
  },
}
