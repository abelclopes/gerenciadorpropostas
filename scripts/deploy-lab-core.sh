#!/usr/bin/env bash
set -euo pipefail

: "${DEPLOY_ENV:?DEPLOY_ENV nao definido}"
: "${LAB_CORE_HOST:?LAB_CORE_HOST nao definido}"
: "${LAB_CORE_USER:?LAB_CORE_USER nao definido}"

LAB_CORE_PORT="${LAB_CORE_PORT:-22}"
LAB_CORE_DEPLOY_PATH="${LAB_CORE_DEPLOY_PATH:-/home/${LAB_CORE_USER}/apps/gerenciadorpropostas/${DEPLOY_ENV}}"
ENV_FILE=".env.${DEPLOY_ENV}"
SSH_TARGET="${LAB_CORE_USER}@${LAB_CORE_HOST}"
SSH_OPTS=(-p "${LAB_CORE_PORT}" -o StrictHostKeyChecking=yes)

test -f "docker-compose.yml" || { echo "docker-compose.yml nao encontrado"; exit 1; }
test -f "${ENV_FILE}" || { echo "${ENV_FILE} nao encontrado"; exit 1; }

echo "Sincronizando arquivos para ${SSH_TARGET}:${LAB_CORE_DEPLOY_PATH}"

ssh "${SSH_OPTS[@]}" "${SSH_TARGET}" "mkdir -p '${LAB_CORE_DEPLOY_PATH}'"

rsync -az --delete \
  --exclude '.git/' \
  --exclude '.gitlab-ci.yml' \
  --exclude 'node_modules/' \
  --exclude 'cliente-react/node_modules/' \
  --exclude 'serverApi/**/bin/' \
  --exclude 'serverApi/**/obj/' \
  --exclude '.vscode/' \
  --exclude 'DATA/' \
  -e "ssh -p ${LAB_CORE_PORT}" \
  ./ "${SSH_TARGET}:${LAB_CORE_DEPLOY_PATH}/"

echo "Executando deploy remoto com docker compose (${DEPLOY_ENV})"

ssh "${SSH_OPTS[@]}" "${SSH_TARGET}" "
  set -euo pipefail
  cd '${LAB_CORE_DEPLOY_PATH}'
  test -f '${ENV_FILE}'
  docker compose --env-file '${ENV_FILE}' up -d --build
"

echo "Deploy concluido em ${SSH_TARGET}:${LAB_CORE_DEPLOY_PATH}"
