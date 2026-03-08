# Cliente React (migração do Angular)

Frontend React + TypeScript para substituir gradualmente o cliente legado Angular.

## Requisitos

- Node.js 20+
- API rodando em `http://localhost:5000` (ou definir `VITE_API_BASE_URL`)

## Executar em desenvolvimento

```bash
npm install
npm run dev
```

## Build

```bash
npm run build
```

## Funcionalidades já migradas

- Login em `POST /api/auth`
- Persistência de token JWT em `localStorage`
- Listagem paginada de propostas em `GET /api/propostas`
- Busca por termo na lista de propostas

## Configuração de ambiente

Copie `.env.example` para `.env` e ajuste se necessário.
