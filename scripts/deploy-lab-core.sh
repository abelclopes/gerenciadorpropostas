#!/usr/bin/env bash
set -euo pipefail

: "${DEPLOY_ENV:?DEPLOY_ENV nao definido}"
: "${LAB_CORE_HOST:?LAB_CORE_HOST nao definido}"
: "${LAB_CORE_USER:?LAB_CORE_USER nao definido}"
: "${DOCKER_REGISTRY:?DOCKER_REGISTRY nao definido}"
: "${DOCKER_REGISTRY_USERNAME:?DOCKER_REGISTRY_USERNAME nao definido}"
: "${DOCKER_REGISTRY_PASSWORD:?DOCKER_REGISTRY_PASSWORD nao definido}"
: "${APP_IMAGE:?APP_IMAGE nao definido}"
: "${FRONTEND_IMAGE:?FRONTEND_IMAGE nao definido}"
: "${IMAGE_TAG:?IMAGE_TAG nao definido}"

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

ssh "${SSH_OPTS[@]}" "${SSH_TARGET}" bash -s -- \
  "${LAB_CORE_DEPLOY_PATH}" \
  "${ENV_FILE}" \
  "${DOCKER_REGISTRY}" \
  "${DOCKER_REGISTRY_USERNAME}" \
  "${DOCKER_REGISTRY_PASSWORD}" \
  "${APP_IMAGE}:${IMAGE_TAG}" \
  "${FRONTEND_IMAGE}:${IMAGE_TAG}" <<'EOF'
set -euo pipefail

deploy_path="$1"
env_file="$2"
docker_registry="$3"
docker_registry_username="$4"
docker_registry_password="$5"
app_image="$6"
frontend_image="$7"

cd "${deploy_path}"
test -f "${env_file}"
printf '%s' "${docker_registry_password}" | docker login "${docker_registry}" -u "${docker_registry_username}" --password-stdin
export APP_IMAGE="${app_image}"
export FRONTEND_IMAGE="${frontend_image}"
docker compose --env-file "${env_file}" pull app frontend
docker compose --env-file "${env_file}" up -d
EOF

echo "Deploy concluido em ${SSH_TARGET}:${LAB_CORE_DEPLOY_PATH}"
