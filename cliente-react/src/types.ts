export interface AuthResponse {
  token: string
}

export interface UsuarioClans {
  id: string
  nome: string
  email: string
  cpf: string
  police: string
  dataNacimento: string
  idade: number
  excluido: boolean
}

export interface ListaPaginada<T> {
  totalItens: number
  numeroPagina: number
  tamanhoPagina: number
  resultado: T[]
  totalPaginas: number
  temPaginaAnterior: boolean
  temPaginaPosterior: boolean
}

export interface Categoria {
  id: string
  nome: string
  descricao: string
}

export interface Fornecedor {
  id: string
  nome: string
  cnpjCpf: string
  email: string
  telefone: string
}

export interface Usuario {
  id: string
  nome: string
  email: string
  cpf: string
  permissao: string
  permissaoNivel: number
  dataNacimento: string
}

export interface Permissao {
  id: string
  nome: string
  nivel: number
}

export interface Proposta {
  id: string
  nomeProposta: string
  descricao: string
  valor: string
  fornecedor: Fornecedor
  categoria: Categoria
  status: string | number
}

export interface ApiEnvelope<T = unknown> {
  ok?: boolean | string
  response?: T
  error?: string
}

export interface PropostaSituacaoPayload {
  id: string
  usuarioId: string
  status: number
}

export interface PropostaAnexo {
  id: string
  nome: string
  contentType: string
  fileContent: string
}

export interface DashboardKpi {
  totalPropostas: number
  propostasAguardando: number
  propostasAprovadas: number
  propostasOutrosStatus: number
  valorTotal: number
  updatedAtUtc: string
}
