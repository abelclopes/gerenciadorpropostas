# Gerenciador de Propostas

Sistema de gestão de propostas comerciais modernizado para stack atual:

- **Backend:** .NET 10 (Web API)
- **Frontend:** React + Vite + TypeScript

---

## 1) Visão geral

Este repositório concentra:

- API de autenticação e regras de negócio de propostas
- Módulos de cadastro (categorias, fornecedores e usuários)
- Fluxo completo de propostas (criação, edição, upload de anexo, validação/aprovação)

### Estado atual

- Projeto legado Angular removido da branch de migração
- Frontend React implementado com paridade funcional dos módulos principais
- API migrada para .NET 10 e validada com build/test

---

## 2) Estrutura do repositório

```text
gerenciadorpropostas/
├─ serverApi/        # Backend .NET 10
├─ cliente-react/    # Frontend React + Vite + TS
├─ DATA/             # Dados/artefatos auxiliares do projeto
└─ README.md
```

---

## 3) Requisitos

### Backend

- .NET SDK 10 (`dotnet --version`)

### Frontend

- Node.js 20+
- npm 10+

### Banco e configuração

- A API usa a string de conexão definida em `serverApi/SRC/API/appsettings*.json`
- Ajuste a conexão local antes de rodar em ambiente novo

---

## 4) Setup rápido (primeira execução)

### 4.1 Backend (API)

```bash
cd serverApi
dotnet restore serverApi.sln
dotnet build serverApi.sln
dotnet run --project SRC/API/API.csproj
```

API padrão: `http://localhost:5000`

### Segurança de senha (Argon2id + pepper)

Defina a variável de ambiente abaixo antes de subir a API:

```bash
export PAPER_SECRETY="troque-por-uma-chave-forte-e-unica"
```

> `PAPER_SECRETY` é usada como *pepper key* no hash de senha.

Swagger (quando habilitado):

- `http://localhost:5000/swagger`

### 4.2 Frontend (React)

```bash
cd cliente-react
cp .env.example .env
npm install
npm run dev
```

Frontend padrão: `http://localhost:5173`

Variável principal de ambiente:

```env
VITE_API_BASE_URL=http://localhost:5000
```

---

## 5) Comandos úteis

### Backend

```bash
cd serverApi
dotnet restore serverApi.sln
dotnet build serverApi.sln -c Debug
dotnet test serverApi.sln -c Debug
```

### Frontend

```bash
cd cliente-react
npm run dev
npm run build
npm run preview
```

---

## 6) Funcionalidades implementadas no React

### Autenticação

- Login via `POST /api/auth`
- Persistência de token JWT no `localStorage`
- Recuperação de contexto do usuário (`/api/usuarios/clans/{email}`)

### Cadastros

- **Categorias:** listar, criar, editar e excluir
- **Fornecedores:** listar, criar, editar, excluir e busca
- **Usuários:** listar, criar, editar, excluir e listar perfis/permissões

### Propostas

- Listagem e busca
- Criação/edição com `FormData`
- Upload de anexo no cadastro
- Atualização de anexo existente
- Download de anexo
- Validação e aprovação de proposta
- Exclusão de proposta

---

## 7) Troubleshooting

### Erro de CORS no frontend

- Verifique se a API está de pé em `VITE_API_BASE_URL`
- Confira política CORS no `Startup` da API

### Erro de autenticação (401)

- Faça login novamente para renovar token
- Confirme se cabeçalhos `Authorization` e `x-access-token` estão sendo enviados

### Falha ao executar `dotnet`

- Confira versão com `dotnet --info`
- Se necessário, instale SDK 10 e execute novamente

### Build do frontend falhou

- Execute `npm install`
- Refaça com `npm run build`

---

## 8) Histórico de migração (resumo)

- API atualizada de `netcoreapp2.0` para `net10.0`
- Dependências legadas substituídas por versões suportadas
- Compatibilidade Swagger ajustada para manter atributos antigos
- Front Angular removido da branch de migração
- Novo frontend React criado com serviços compatíveis com os endpoints existentes

---

## 9) Próximos passos recomendados

- Componentizar o frontend por domínio (`categorias`, `fornecedores`, `usuarios`, `propostas`)
- Adicionar paginação real por módulo no React
- Melhorar feedback de erro/sucesso por operação
- Incluir pipeline CI para build/test automatizado
- Revisar warnings restantes da API (obsoletos e analyzers)

## 10) Pipeline GitLab para deploy no lab-core

O repositório agora inclui um pipeline GitLab em `.gitlab-ci.yml` com:

- `api_test`: restore, build e testes do backend .NET
- `frontend_build`: build do frontend React
- `deploy_lab_core_develop`: deploy automatico da branch `develop` no `lab-core`
- `deploy_lab_core_homolog`: deploy manual de `main` ou `master` no `lab-core`

O deploy usa o script `scripts/deploy-lab-core.sh`, que:

- sincroniza o repositorio por `rsync` para o host remoto
- entra no host `lab-core` por SSH
- executa `docker compose --env-file .env.<ambiente> up -d --build`

### Variaveis de CI necessarias

Cadastre no GitLab:

- `LAB_CORE_HOST`: host ou IP do `lab-core` (`10.0.0.246` ou alias SSH)
- `LAB_CORE_USER`: usuario SSH do host remoto, por exemplo `abel`
- `LAB_CORE_SSH_PRIVATE_KEY`: chave privada usada pelo runner para acessar o `lab-core`

O pipeline tambem aceita overrides opcionais:

- `LAB_CORE_PORT`: porta SSH, padrao `22`
- `LAB_CORE_DEPLOY_PATH`: caminho remoto do deploy

Bootstrap do acesso SSH do GitLab no `lab-core`:

- executar `./scripts/setup-gitlab-lab-core-access.sh`
- cadastrar a chave privada gerada em `LAB_CORE_SSH_PRIVATE_KEY`

### Caminhos remotos padrao

- `develop`: `/home/abel/apps/gerenciadorpropostas/develop`
- `homolog`: `/home/abel/apps/gerenciadorpropostas/homolog`

Documentacao detalhada:

- `docs/DEPLOY-LAB-CORE.md`
