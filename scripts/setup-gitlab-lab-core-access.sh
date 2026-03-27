#!/usr/bin/env bash
set -euo pipefail

LAB_CORE_HOST="${1:-lab-core}"
LAB_CORE_USER="${2:-abel}"
OUTPUT_DIR="${3:-/tmp/gitlab-gerenciadorpropostas-lab-core}"
KEY_PATH="${OUTPUT_DIR}/id_ed25519_gitlab_lab_core"
DEPLOY_BASE="/home/${LAB_CORE_USER}/apps/gerenciadorpropostas"
RESOLVED_HOST="$(ssh -G "${LAB_CORE_HOST}" 2>/dev/null | awk '/^hostname / { print $2; exit }')"
RESOLVED_HOST="${RESOLVED_HOST:-${LAB_CORE_HOST}}"

mkdir -p "${OUTPUT_DIR}"
chmod 700 "${OUTPUT_DIR}"

if [[ ! -f "${KEY_PATH}" ]]; then
  ssh-keygen -t ed25519 -N "" -C "gitlab-gerenciadorpropostas-lab-core" -f "${KEY_PATH}"
fi

chmod 600 "${KEY_PATH}"
chmod 644 "${KEY_PATH}.pub"

PUB_KEY_CONTENT="$(cat "${KEY_PATH}.pub")"

ssh "${LAB_CORE_HOST}" "mkdir -p ~/.ssh && chmod 700 ~/.ssh && touch ~/.ssh/authorized_keys && chmod 600 ~/.ssh/authorized_keys"

if ssh "${LAB_CORE_HOST}" "grep -qxF '${PUB_KEY_CONTENT}' ~/.ssh/authorized_keys"; then
  echo "Chave publica ja estava autorizada em ${LAB_CORE_HOST}."
else
  ssh "${LAB_CORE_HOST}" "printf '%s\n' '${PUB_KEY_CONTENT}' >> ~/.ssh/authorized_keys"
  echo "Chave publica adicionada em ${LAB_CORE_HOST}:~/.ssh/authorized_keys"
fi

ssh "${LAB_CORE_HOST}" "mkdir -p '${DEPLOY_BASE}/develop' '${DEPLOY_BASE}/homolog'"

ssh -i "${KEY_PATH}" -o IdentitiesOnly=yes "${LAB_CORE_USER}@${LAB_CORE_HOST}" "whoami && test -d '${DEPLOY_BASE}/develop' && test -d '${DEPLOY_BASE}/homolog'"

cat <<EOF

Acesso de deploy configurado para o GitLab.

Variaveis para cadastrar no GitLab CI/CD:
LAB_CORE_HOST=${RESOLVED_HOST}
LAB_CORE_USER=${LAB_CORE_USER}
LAB_CORE_PORT=22
LAB_CORE_DEPLOY_PATH=${DEPLOY_BASE}/develop

Chave privada para LAB_CORE_SSH_PRIVATE_KEY:
Arquivo: ${KEY_PATH}

Para visualizar:
cat ${KEY_PATH}
EOF
