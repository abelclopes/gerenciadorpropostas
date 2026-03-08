import { useEffect, useMemo, useState } from 'react'
import type { FormEvent } from 'react'
import { authApi } from './services/auth'
import { categoriasApi } from './services/categorias'
import { fornecedoresApi } from './services/fornecedores'
import { propostasApi } from './services/propostas'
import { usuariosApi } from './services/usuarios'
import type { UsuarioPayload } from './services/usuarios'
import type {
  Categoria,
  Fornecedor,
  ListaPaginada,
  Permissao,
  Proposta,
  Usuario,
  UsuarioClans,
} from './types'

const PAGE_SIZE = 10
type TabKey = 'propostas' | 'categorias' | 'fornecedores' | 'usuarios'

function App() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [tab, setTab] = useState<TabKey>('propostas')
  const [token, setToken] = useState<string | null>(null)
  const [clans, setClans] = useState<UsuarioClans | null>(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const [categorias, setCategorias] = useState<ListaPaginada<Categoria> | null>(null)
  const [fornecedores, setFornecedores] = useState<ListaPaginada<Fornecedor> | null>(null)
  const [usuarios, setUsuarios] = useState<ListaPaginada<Usuario> | null>(null)
  const [propostas, setPropostas] = useState<ListaPaginada<Proposta> | null>(null)
  const [perfis, setPerfis] = useState<Permissao[]>([])

  const [buscaCategorias, setBuscaCategorias] = useState('')
  const [buscaFornecedores, setBuscaFornecedores] = useState('')
  const [buscaUsuarios, setBuscaUsuarios] = useState('')
  const [buscaPropostas, setBuscaPropostas] = useState('')

  const [categoriaForm, setCategoriaForm] = useState({ id: '', nome: '', descricao: '' })
  const [fornecedorForm, setFornecedorForm] = useState({ id: '', nome: '', cnpjCpf: '', email: '', telefone: '' })
  const [usuarioForm, setUsuarioForm] = useState<UsuarioPayload>({
    nome: '',
    cpf: '',
    email: '',
    senha: '',
    dataNacimento: '',
    perfilUsuario: 1,
  })
  const [usuarioEditId, setUsuarioEditId] = useState('')

  const [propostaForm, setPropostaForm] = useState({
    id: '',
    nomeProposta: '',
    descricao: '',
    valor: '',
    fornecedorID: '',
    categoriaID: '',
  })
  const [propostaAnexoFile, setPropostaAnexoFile] = useState<File | null>(null)
  const [novoAnexoPropostaId, setNovoAnexoPropostaId] = useState('')
  const [novoAnexoFile, setNovoAnexoFile] = useState<File | null>(null)

  const autenticado = useMemo(() => Boolean(token), [token])

  useEffect(() => {
    const userRaw = localStorage.getItem('usuarioCorrente')
    if (!userRaw) return
    try {
      const user = JSON.parse(userRaw) as { token?: string; email?: string }
      if (user.token) {
        setToken(user.token)
      }
      if (user.email) {
        setEmail(user.email)
      }
    } catch {
      localStorage.removeItem('usuarioCorrente')
    }

    const clansRaw = localStorage.getItem('usuarioClans')
    if (clansRaw) {
      try {
        setClans(JSON.parse(clansRaw) as UsuarioClans)
      } catch {
        localStorage.removeItem('usuarioClans')
      }
    }
  }, [])

  useEffect(() => {
    if (!token) return

    void carregarDadosIniciais(token)
  }, [token])

  async function carregarDadosIniciais(jwt: string) {
    setLoading(true)
    setError(null)
    try {
      const [categoriasResp, fornecedoresResp, usuariosResp, propostasResp, perfisResp] = await Promise.all([
        categoriasApi.listar(jwt, 1, PAGE_SIZE, buscaCategorias),
        fornecedoresApi.listar(jwt, 1, PAGE_SIZE, buscaFornecedores),
        usuariosApi.listar(jwt, 1, PAGE_SIZE, buscaUsuarios),
        propostasApi.listar(jwt, 1, PAGE_SIZE, buscaPropostas),
        usuariosApi.listarPerfis(jwt),
      ])

      setCategorias(categoriasResp)
      setFornecedores(fornecedoresResp)
      setUsuarios(usuariosResp)
      setPropostas(propostasResp)
      setPerfis(perfisResp)
    } catch (erro) {
      setError(erro instanceof Error ? erro.message : 'Erro ao carregar dados')
    } finally {
      setLoading(false)
    }
  }

  async function carregarCategorias() {
    if (!token) return
    setCategorias(await categoriasApi.listar(token, 1, PAGE_SIZE, buscaCategorias))
  }

  async function carregarFornecedores() {
    if (!token) return
    setFornecedores(await fornecedoresApi.listar(token, 1, PAGE_SIZE, buscaFornecedores))
  }

  async function carregarUsuarios() {
    if (!token) return
    setUsuarios(await usuariosApi.listar(token, 1, PAGE_SIZE, buscaUsuarios))
  }

  async function carregarPropostas() {
    if (!token) return
    setPropostas(await propostasApi.listar(token, 1, PAGE_SIZE, buscaPropostas))
  }

  async function handleLogin(event: FormEvent) {
    event.preventDefault()
    setLoading(true)
    setError(null)
    try {
      const jwt = await authApi.login(email, password)
      const clansResp = await authApi.getClans(jwt, email)
      setToken(jwt)
      setClans(clansResp)
      localStorage.setItem('usuarioCorrente', JSON.stringify({ email, token: jwt }))
      localStorage.setItem('usuarioClans', JSON.stringify(clansResp))
      await carregarDadosIniciais(jwt)
    } catch (erro) {
      setError(erro instanceof Error ? erro.message : 'Falha no login')
    } finally {
      setLoading(false)
    }
  }

  function handleLogout() {
    localStorage.removeItem('usuarioCorrente')
    localStorage.removeItem('usuarioClans')
    setToken(null)
    setClans(null)
    setPropostas(null)
    setCategorias(null)
    setFornecedores(null)
    setUsuarios(null)
    setError(null)
  }

  async function salvarCategoria(event: FormEvent) {
    event.preventDefault()
    if (!token) return
    setLoading(true)
    setError(null)
    try {
      if (categoriaForm.id) {
        await categoriasApi.atualizar(token, categoriaForm)
      } else {
        await categoriasApi.criar(token, categoriaForm.nome, categoriaForm.descricao)
      }
      setCategoriaForm({ id: '', nome: '', descricao: '' })
      await carregarCategorias()
    } catch (erro) {
      setError(erro instanceof Error ? erro.message : 'Falha ao salvar categoria')
    } finally {
      setLoading(false)
    }
  }

  async function removerCategoria(id: string) {
    if (!token) return
    await categoriasApi.excluir(token, id)
    await carregarCategorias()
  }

  async function salvarFornecedor(event: FormEvent) {
    event.preventDefault()
    if (!token) return
    setLoading(true)
    setError(null)
    try {
      if (fornecedorForm.id) {
        await fornecedoresApi.atualizar(token, fornecedorForm)
      } else {
        const { id, ...novo } = fornecedorForm
        await fornecedoresApi.criar(token, novo)
      }
      setFornecedorForm({ id: '', nome: '', cnpjCpf: '', email: '', telefone: '' })
      await carregarFornecedores()
    } catch (erro) {
      setError(erro instanceof Error ? erro.message : 'Falha ao salvar fornecedor')
    } finally {
      setLoading(false)
    }
  }

  async function removerFornecedor(id: string) {
    if (!token) return
    await fornecedoresApi.excluir(token, id)
    await carregarFornecedores()
  }

  async function salvarUsuario(event: FormEvent) {
    event.preventDefault()
    if (!token) return
    setLoading(true)
    setError(null)
    try {
      if (usuarioEditId) {
        await usuariosApi.atualizar(token, usuarioEditId, usuarioForm)
      } else {
        await usuariosApi.criar(token, usuarioForm)
      }
      setUsuarioEditId('')
      setUsuarioForm({ nome: '', cpf: '', email: '', senha: '', dataNacimento: '', perfilUsuario: 1 })
      await carregarUsuarios()
    } catch (erro) {
      setError(erro instanceof Error ? erro.message : 'Falha ao salvar usuário')
    } finally {
      setLoading(false)
    }
  }

  async function removerUsuario(id: string) {
    if (!token) return
    await usuariosApi.excluir(token, id)
    await carregarUsuarios()
  }

  function buildPropostaFormData() {
    const formData = new FormData()
    formData.append('nomeProposta', propostaForm.nomeProposta)
    formData.append('descricao', propostaForm.descricao)
    formData.append('valor', propostaForm.valor)
    formData.append('fornecedorID', propostaForm.fornecedorID)
    formData.append('categoriaID', propostaForm.categoriaID)
    formData.append('status', '1')
    formData.append('usuario', clans?.id ?? '')
    if (propostaAnexoFile) {
      formData.append('anexo', propostaAnexoFile)
    }
    return formData
  }

  async function salvarProposta(event: FormEvent) {
    event.preventDefault()
    if (!token || !clans) return
    setLoading(true)
    setError(null)
    try {
      const formData = buildPropostaFormData()
      if (propostaForm.id) {
        await propostasApi.atualizar(token, propostaForm.id, formData)
      } else {
        await propostasApi.criar(token, formData)
      }
      setPropostaForm({ id: '', nomeProposta: '', descricao: '', valor: '', fornecedorID: '', categoriaID: '' })
      setPropostaAnexoFile(null)
      await carregarPropostas()
    } catch (erro) {
      setError(erro instanceof Error ? erro.message : 'Falha ao salvar proposta')
    } finally {
      setLoading(false)
    }
  }

  async function removerProposta(id: string) {
    if (!token) return
    await propostasApi.excluir(token, id)
    await carregarPropostas()
  }

  async function aprovarProposta(proposta: Proposta) {
    if (!token || !clans) return
    const status = Number(proposta.status)
    await propostasApi.aprovar(token, proposta.id, {
      id: proposta.id,
      usuarioId: clans.id,
      status,
    })
    await carregarPropostas()
  }

  async function validarProposta(proposta: Proposta) {
    if (!token || !clans) return
    const status = Number(proposta.status)
    await propostasApi.validarSituacao(token, {
      id: proposta.id,
      usuarioId: clans.id,
      status,
    })
  }

  async function baixarAnexo(propostaId: string) {
    if (!token) return
    const anexo = await propostasApi.buscarAnexo(token, propostaId)
    if (!anexo?.fileContent) return

    const bytes = Uint8Array.from(atob(anexo.fileContent), (char) => char.charCodeAt(0))
    const blob = new Blob([bytes], { type: anexo.contentType || 'application/octet-stream' })
    const url = URL.createObjectURL(blob)
    const anchor = document.createElement('a')
    anchor.href = url
    anchor.download = anexo.nome || 'anexo'
    anchor.click()
    URL.revokeObjectURL(url)
  }

  async function salvarNovoAnexo(event: FormEvent) {
    event.preventDefault()
    if (!token || !novoAnexoPropostaId || !novoAnexoFile) return
    await propostasApi.atualizarAnexo(token, novoAnexoPropostaId, novoAnexoFile)
    setNovoAnexoPropostaId('')
    setNovoAnexoFile(null)
  }

  return (
    <main className="container">
      <header className="header">
        <h1>Gerenciador de Propostas - React</h1>
        {autenticado && (
          <button onClick={handleLogout} className="secondary">
            Sair
          </button>
        )}
      </header>

      {!autenticado ? (
        <form className="card" onSubmit={handleLogin}>
          <h2>Entrar</h2>
          <label>
            E-mail
            <input
              type="email"
              value={email}
              onChange={(event) => setEmail(event.target.value)}
              required
            />
          </label>
          <label>
            Senha
            <input
              type="password"
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              required
            />
          </label>
          <button type="submit" disabled={loading}>
            {loading ? 'Entrando...' : 'Entrar'}
          </button>
        </form>
      ) : (
        <section className="card">
          <div className="tabs">
            <button className={tab === 'propostas' ? 'active' : ''} onClick={() => setTab('propostas')}>Propostas</button>
            <button className={tab === 'categorias' ? 'active' : ''} onClick={() => setTab('categorias')}>Categorias</button>
            <button className={tab === 'fornecedores' ? 'active' : ''} onClick={() => setTab('fornecedores')}>Fornecedores</button>
            <button className={tab === 'usuarios' ? 'active' : ''} onClick={() => setTab('usuarios')}>Usuários</button>
          </div>

          {loading && <p>Carregando...</p>}

          {tab === 'categorias' && (
            <>
              <form className="inline-form" onSubmit={async (event) => { event.preventDefault(); await carregarCategorias() }}>
                <input placeholder="Buscar categorias" value={buscaCategorias} onChange={(event) => setBuscaCategorias(event.target.value)} />
                <button type="submit">Buscar</button>
              </form>
              <form className="grid-form" onSubmit={salvarCategoria}>
                <input placeholder="Nome" value={categoriaForm.nome} onChange={(event) => setCategoriaForm((old) => ({ ...old, nome: event.target.value }))} required />
                <input placeholder="Descrição" value={categoriaForm.descricao} onChange={(event) => setCategoriaForm((old) => ({ ...old, descricao: event.target.value }))} required />
                <button type="submit">{categoriaForm.id ? 'Atualizar' : 'Criar'}</button>
              </form>
              <table>
                <thead><tr><th>Nome</th><th>Descrição</th><th>Ações</th></tr></thead>
                <tbody>
                  {categorias?.resultado.map((item) => (
                    <tr key={item.id}>
                      <td>{item.nome}</td>
                      <td>{item.descricao}</td>
                      <td>
                        <button onClick={() => setCategoriaForm(item)}>Editar</button>
                        <button className="danger" onClick={() => removerCategoria(item.id)}>Excluir</button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </>
          )}

          {tab === 'fornecedores' && (
            <>
              <form className="inline-form" onSubmit={async (event) => { event.preventDefault(); await carregarFornecedores() }}>
                <input placeholder="Buscar fornecedores" value={buscaFornecedores} onChange={(event) => setBuscaFornecedores(event.target.value)} />
                <button type="submit">Buscar</button>
              </form>
              <form className="grid-form" onSubmit={salvarFornecedor}>
                <input placeholder="Nome" value={fornecedorForm.nome} onChange={(event) => setFornecedorForm((old) => ({ ...old, nome: event.target.value }))} required />
                <input placeholder="CNPJ/CPF" value={fornecedorForm.cnpjCpf} onChange={(event) => setFornecedorForm((old) => ({ ...old, cnpjCpf: event.target.value }))} required />
                <input placeholder="Email" value={fornecedorForm.email} onChange={(event) => setFornecedorForm((old) => ({ ...old, email: event.target.value }))} required />
                <input placeholder="Telefone" value={fornecedorForm.telefone} onChange={(event) => setFornecedorForm((old) => ({ ...old, telefone: event.target.value }))} required />
                <button type="submit">{fornecedorForm.id ? 'Atualizar' : 'Criar'}</button>
              </form>
              <table>
                <thead><tr><th>Nome</th><th>CNPJ/CPF</th><th>Email</th><th>Telefone</th><th>Ações</th></tr></thead>
                <tbody>
                  {fornecedores?.resultado.map((item) => (
                    <tr key={item.id}>
                      <td>{item.nome}</td><td>{item.cnpjCpf}</td><td>{item.email}</td><td>{item.telefone}</td>
                      <td>
                        <button onClick={() => setFornecedorForm(item)}>Editar</button>
                        <button className="danger" onClick={() => removerFornecedor(item.id)}>Excluir</button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </>
          )}

          {tab === 'usuarios' && (
            <>
              <form className="inline-form" onSubmit={async (event) => { event.preventDefault(); await carregarUsuarios() }}>
                <input placeholder="Buscar usuários" value={buscaUsuarios} onChange={(event) => setBuscaUsuarios(event.target.value)} />
                <button type="submit">Buscar</button>
              </form>
              <form className="grid-form" onSubmit={salvarUsuario}>
                <input placeholder="Nome" value={usuarioForm.nome} onChange={(event) => setUsuarioForm((old) => ({ ...old, nome: event.target.value }))} required />
                <input placeholder="CPF" value={usuarioForm.cpf} onChange={(event) => setUsuarioForm((old) => ({ ...old, cpf: event.target.value }))} required />
                <input placeholder="Email" value={usuarioForm.email} onChange={(event) => setUsuarioForm((old) => ({ ...old, email: event.target.value }))} required />
                <input type="date" value={usuarioForm.dataNacimento} onChange={(event) => setUsuarioForm((old) => ({ ...old, dataNacimento: event.target.value }))} required />
                <input placeholder="Senha" value={usuarioForm.senha ?? ''} onChange={(event) => setUsuarioForm((old) => ({ ...old, senha: event.target.value }))} />
                <select value={usuarioForm.perfilUsuario} onChange={(event) => setUsuarioForm((old) => ({ ...old, perfilUsuario: Number(event.target.value) }))}>
                  {perfis.map((perfil) => (<option key={perfil.id} value={perfil.nivel}>{perfil.nome} ({perfil.nivel})</option>))}
                </select>
                <button type="submit">{usuarioEditId ? 'Atualizar' : 'Criar'}</button>
              </form>
              <table>
                <thead><tr><th>Nome</th><th>Email</th><th>CPF</th><th>Perfil</th><th>Ações</th></tr></thead>
                <tbody>
                  {usuarios?.resultado.map((item) => (
                    <tr key={item.id}>
                      <td>{item.nome}</td><td>{item.email}</td><td>{item.cpf}</td><td>{item.permissao}</td>
                      <td>
                        <button onClick={() => {
                          setUsuarioEditId(item.id)
                          setUsuarioForm({
                            nome: item.nome,
                            cpf: item.cpf,
                            email: item.email,
                            senha: '',
                            dataNacimento: String(item.dataNacimento).slice(0, 10),
                            perfilUsuario: item.permissaoNivel,
                          })
                        }}>Editar</button>
                        <button className="danger" onClick={() => removerUsuario(item.id)}>Excluir</button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </>
          )}

          {tab === 'propostas' && (
            <>
              <form className="inline-form" onSubmit={async (event) => { event.preventDefault(); await carregarPropostas() }}>
                <input placeholder="Buscar propostas" value={buscaPropostas} onChange={(event) => setBuscaPropostas(event.target.value)} />
                <button type="submit">Buscar</button>
              </form>

              <form className="grid-form" onSubmit={salvarProposta}>
                <input placeholder="Nome da proposta" value={propostaForm.nomeProposta} onChange={(event) => setPropostaForm((old) => ({ ...old, nomeProposta: event.target.value }))} required />
                <input placeholder="Descrição" value={propostaForm.descricao} onChange={(event) => setPropostaForm((old) => ({ ...old, descricao: event.target.value }))} required />
                <input placeholder="Valor" value={propostaForm.valor} onChange={(event) => setPropostaForm((old) => ({ ...old, valor: event.target.value }))} required />
                <select value={propostaForm.fornecedorID} onChange={(event) => setPropostaForm((old) => ({ ...old, fornecedorID: event.target.value }))} required>
                  <option value="">Fornecedor</option>
                  {fornecedores?.resultado.map((item) => (<option key={item.id} value={item.id}>{item.nome}</option>))}
                </select>
                <select value={propostaForm.categoriaID} onChange={(event) => setPropostaForm((old) => ({ ...old, categoriaID: event.target.value }))} required>
                  <option value="">Categoria</option>
                  {categorias?.resultado.map((item) => (<option key={item.id} value={item.id}>{item.nome}</option>))}
                </select>
                <input type="file" onChange={(event) => setPropostaAnexoFile(event.target.files?.[0] ?? null)} />
                <button type="submit">{propostaForm.id ? 'Atualizar' : 'Criar'}</button>
              </form>

              <form className="inline-form" onSubmit={salvarNovoAnexo}>
                <input placeholder="ID da proposta" value={novoAnexoPropostaId} onChange={(event) => setNovoAnexoPropostaId(event.target.value)} />
                <input type="file" onChange={(event) => setNovoAnexoFile(event.target.files?.[0] ?? null)} />
                <button type="submit">Atualizar anexo</button>
              </form>

              <table>
                <thead><tr><th>Nome</th><th>Fornecedor</th><th>Categoria</th><th>Valor</th><th>Status</th><th>Ações</th></tr></thead>
                <tbody>
                  {propostas?.resultado.map((item) => (
                    <tr key={item.id}>
                      <td>{item.nomeProposta}</td>
                      <td>{item.fornecedor?.nome}</td>
                      <td>{item.categoria?.nome}</td>
                      <td>{item.valor}</td>
                      <td>{item.status}</td>
                      <td>
                        <button onClick={() => setPropostaForm({
                          id: item.id,
                          nomeProposta: item.nomeProposta,
                          descricao: item.descricao,
                          valor: item.valor,
                          fornecedorID: item.fornecedor?.id ?? '',
                          categoriaID: item.categoria?.id ?? '',
                        })}>Editar</button>
                        <button onClick={() => validarProposta(item)}>Validar</button>
                        <button onClick={() => aprovarProposta(item)}>Aprovar</button>
                        <button onClick={() => baixarAnexo(item.id)}>Anexo</button>
                        <button className="danger" onClick={() => removerProposta(item.id)}>Excluir</button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </>
          )}
        </section>
      )}

      {error && <p className="error">{error}</p>}
      {!error && autenticado && clans && <p className="muted">Usuário: {clans.nome} ({clans.email})</p>}
    </main>
  )
}

export default App
