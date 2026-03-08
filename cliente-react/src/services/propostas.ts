import type {
  ApiEnvelope,
  ListaPaginada,
  Proposta,
  PropostaAnexo,
  PropostaSituacaoPayload,
} from '../types'
import { request } from './http'

export const propostasApi = {
  listar(token: string, pageNumber: number, pageSize: number, buscaTermo = '') {
    const busca = encodeURIComponent(buscaTermo)
    return request<ListaPaginada<Proposta>>(
      `/api/propostas?PageNumber=${pageNumber}&PageSize=${pageSize}&buscaTermo=${busca}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          'x-access-token': token,
        },
      },
    )
  },

  criar(token: string, data: FormData) {
    return request<ApiEnvelope>('/api/propostas', {
      method: 'POST',
      token,
      body: data,
      isFormData: true,
    })
  },

  atualizar(token: string, id: string, data: FormData) {
    return request<ApiEnvelope>(`/api/propostas/${id}`, {
      method: 'PUT',
      token,
      body: data,
      isFormData: true,
    })
  },

  excluir(token: string, id: string) {
    return request<ApiEnvelope>(`/api/propostas/${id}`, {
      method: 'DELETE',
      token,
    })
  },

  buscarAnexo(token: string, propostaId: string) {
    return request<PropostaAnexo>(`/api/propostas/anexos/${propostaId}`, { token })
  },

  atualizarAnexo(token: string, propostaId: string, file: File) {
    const data = new FormData()
    data.append('id', propostaId)
    data.append('anexo', file)
    return request<ApiEnvelope>('/api/propostas/anexos', {
      method: 'POST',
      token,
      body: data,
      isFormData: true,
    })
  },

  validarSituacao(token: string, payload: PropostaSituacaoPayload) {
    return request<ApiEnvelope>('/api/propostas/status', {
      method: 'POST',
      token,
      body: JSON.stringify(payload),
    })
  },

  aprovar(token: string, propostaId: string, payload: PropostaSituacaoPayload) {
    return request<ApiEnvelope>(`/api/propostas/status/${propostaId}`, {
      method: 'PUT',
      token,
      body: JSON.stringify(payload),
    })
  },
}
