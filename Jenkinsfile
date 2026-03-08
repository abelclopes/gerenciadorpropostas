pipeline {
  agent any

  parameters {
    choice(name: 'DEPLOY_ENV', choices: ['develop', 'homolog'], description: 'Ambiente para deploy')
  }

  stages {
    stage('Checkout') {
      steps {
        checkout scm
      }
    }

    stage('Deploy') {
      steps {
        sh '''
          set -euo pipefail
          test -f "docker-compose.yml" || { echo "docker-compose.yml nao encontrado"; exit 1; }
          test -f ".env.${DEPLOY_ENV}" || { echo ".env.${DEPLOY_ENV} nao encontrado"; exit 1; }
          docker compose --env-file ".env.${DEPLOY_ENV}" up -d --build
        '''
      }
    }
  }
}
