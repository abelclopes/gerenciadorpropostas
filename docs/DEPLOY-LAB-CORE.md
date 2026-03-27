# Deploy No Lab Core Com GitLab CI

Este documento descreve como o projeto `gerenciadorpropostas` faz deploy para o host `lab-core` usando GitLab CI.

## Visao geral

O pipeline definido em `.gitlab-ci.yml` possui dois grupos de jobs:

- `api_test`: restore, build e testes do backend `.NET 10`
- `frontend_build`: instala dependencias e executa o build do frontend `React`
- `deploy_lab_core_develop`: publica automaticamente a branch `develop`
- `deploy_lab_core_homolog`: publica manualmente `main` ou `master`

O deploy remoto usa o script `scripts/deploy-lab-core.sh`.

## Como o deploy funciona

O job de deploy executa estes passos:

1. inicia um agente SSH no runner do GitLab
2. carrega a chave privada vinda da variavel `LAB_CORE_SSH_PRIVATE_KEY`
3. registra o host remoto no `known_hosts`
4. sincroniza o repositorio para o `lab-core` com `rsync`
5. acessa o host remoto por SSH
6. executa `docker compose --env-file .env.<ambiente> up -d --build`

## Ambientes

### develop

- branch: `develop`
- execucao: automatica
- arquivo de ambiente: `.env.develop`
- URL esperada: `https://gerenciadorpropostas.develop.devops.abellinux.com`
- caminho remoto padrao: `/home/abel/apps/gerenciadorpropostas/develop`

### homolog

- branch: `main` ou `master`
- execucao: manual
- arquivo de ambiente: `.env.homolog`
- URL esperada: `https://gerenciadorpropostas.homolog.devops.abellinux.com`
- caminho remoto padrao: `/home/abel/apps/gerenciadorpropostas/homolog`

## Variaveis de CI obrigatorias

Cadastre no GitLab em `Settings > CI/CD > Variables`:

- `LAB_CORE_HOST`: IP ou hostname do `lab-core`
- `LAB_CORE_USER`: usuario SSH do host remoto
- `LAB_CORE_SSH_PRIVATE_KEY`: chave privada que o runner usara para conectar no host

## Variaveis de CI opcionais

- `LAB_CORE_PORT`: porta SSH; padrao `22`
- `LAB_CORE_DEPLOY_PATH`: caminho remoto customizado para o deploy

## Requisitos no host lab-core

O host remoto precisa ter:

- Docker Engine instalado
- plugin `docker compose`
- usuario com permissao para executar `docker compose`
- acesso de escrita ao caminho remoto configurado

No estado atual validado neste ambiente:

- host: `lab-core` (`10.0.0.246`)
- usuario viavel para deploy: `abel`
- `docker` e `docker compose` estao instalados no host
- nao foi possivel criar um usuario dedicado via automacao porque o host exige senha para `sudo`

Por isso, o caminho recomendado aqui e usar uma chave SSH exclusiva do GitLab para o usuario `abel`, em vez de reutilizar a chave pessoal.

## Estrutura usada no deploy

Arquivos principais:

- `.gitlab-ci.yml`
- `scripts/deploy-lab-core.sh`
- `scripts/setup-gitlab-lab-core-access.sh`
- `.env.develop`
- `.env.homolog`
- `docker-compose.yml`

## Bootstrap do acesso SSH do GitLab

Para preparar o `lab-core` para o runner do GitLab, execute localmente:

```bash
./scripts/setup-gitlab-lab-core-access.sh
```

O script:

1. gera uma chave `ed25519` exclusiva para o deploy do GitLab em `/tmp/gitlab-gerenciadorpropostas-lab-core`
2. adiciona a chave publica em `~/.ssh/authorized_keys` do usuario remoto
3. cria os diretorios remotos `develop` e `homolog`
4. valida o login SSH usando a chave gerada

Se precisar sobrescrever host, usuario ou pasta de saida:

```bash
./scripts/setup-gitlab-lab-core-access.sh lab-core abel /tmp/gitlab-gerenciadorpropostas-lab-core
```

Depois do bootstrap, cadastre no GitLab:

- `LAB_CORE_HOST=10.0.0.246`
- `LAB_CORE_USER=abel`
- `LAB_CORE_PORT=22`
- `LAB_CORE_SSH_PRIVATE_KEY`: conteudo do arquivo privado gerado pelo script

## Comando remoto executado

O deploy roda este comando no host remoto:

```bash
docker compose --env-file ".env.${DEPLOY_ENV}" up -d --build
```

## Exemplo de configuracao das variaveis

Exemplo de valores:

- `LAB_CORE_HOST=10.0.0.246`
- `LAB_CORE_USER=abel`
- `LAB_CORE_PORT=22`

## Fluxo operacional recomendado

### Deploy de develop

1. fazer push para a branch `develop`
2. aguardar `api_test` e `frontend_build`
3. o job `deploy_lab_core_develop` executa automaticamente

### Deploy de homolog

1. fazer merge para `main` ou `master`
2. abrir o pipeline no GitLab
3. executar manualmente `deploy_lab_core_homolog`

## Troubleshooting

### Erro de SSH

Verifique:

- se `LAB_CORE_SSH_PRIVATE_KEY` foi cadastrada corretamente
- se a chave publica correspondente existe em `~/.ssh/authorized_keys` no `lab-core`
- se `LAB_CORE_HOST` e `LAB_CORE_USER` estao corretos

### Erro de docker compose no host remoto

Verifique:

- se o usuario remoto tem permissao para Docker
- se o `docker compose` existe no host
- se o arquivo `.env.<ambiente>` foi sincronizado com sucesso

### Erro de build do frontend

Verifique:

- se `npm ci` instala sem conflito
- se `VITE_API_BASE_URL` no `.env` do ambiente esta correto

### Erro de build do backend

Verifique:

- se a solucao `serverApi/serverApi.sln` continua compilando em `Release`
- se os testes passam localmente com `dotnet test`
