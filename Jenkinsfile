pipeline {
  agent any

  options {
    buildDiscarder(logRotator(numToKeepStr: '10'))
    disableConcurrentBuilds()
    skipDefaultCheckout(true)
  }

  triggers {
    githubPush()
  }

  parameters {
    string(name: "REPO_URL", defaultValue: "git@github.com:abelclopes/gerenciadorpropostas.git", description: "URL Git/SSH do repositorio no GitHub")
    string(name: "GIT_CREDENTIALS_ID", defaultValue: "github-ssh", description: "Credential ID para checkout no GitHub")
    string(name: "BRANCH", defaultValue: "develop", description: "Branch para checkout")
    choice(name: "ENVIRONMENT", choices: ["develop", "homolog"], description: "Ambiente de deploy")
    choice(name: "DOCKER_CONTEXT", choices: ["kvmmint"], description: "Contexto Docker para build/deploy")
    string(name: "WORKDIR", defaultValue: ".", description: "Diretorio do projeto dentro do repositorio")
    string(name: "IMAGE_TAG", defaultValue: "", description: "Tag opcional da imagem. Vazio usa BUILD_NUMBER")
    booleanParam(name: "RUN_SONAR", defaultValue: true, description: "Executar analise SonarQube")
    string(name: "SONARQUBE_ENV", defaultValue: "sonarqube-local", description: "Nome do servidor SonarQube no Jenkins")
    booleanParam(name: "DO_DEPLOY", defaultValue: true, description: "Executar deploy apos build")
  }

  environment {
    PROJECT_SLUG = "gerenciadorpropostas"
    ENV_FILE = ".env.${params.ENVIRONMENT}"
    ENV_LOCAL_FILE = ".env.${params.ENVIRONMENT}.local"
    COMPOSE_PROJECT_NAME = "${PROJECT_SLUG}-${params.ENVIRONMENT}"
    EFFECTIVE_IMAGE_TAG = "${params.IMAGE_TAG ?: env.BUILD_NUMBER}"
  }

  stages {
    stage("Prepare Workspace") {
      steps {
        deleteDir()
      }
    }

    stage("Checkout GitHub") {
      steps {
        script {
          def remoteCfg = [url: params.REPO_URL]
          if (params.GIT_CREDENTIALS_ID?.trim()) {
            remoteCfg.credentialsId = params.GIT_CREDENTIALS_ID.trim()
          }
          checkout([
            $class: "GitSCM",
            branches: [[name: "*/${params.BRANCH}"]],
            userRemoteConfigs: [remoteCfg]
          ])
          sh 'git log -1 --oneline'
        }
      }
    }

    stage("Preflight") {
      steps {
        sh '''
          set -euo pipefail
          cd "${WORKDIR}"
          test -f "${ENV_FILE}" || { echo "Arquivo ${ENV_FILE} nao encontrado"; exit 1; }
          test -f "docker-compose.yml" || { echo "docker-compose.yml nao encontrado"; exit 1; }
          test -f "sonar-project.properties" || { echo "sonar-project.properties nao encontrado"; exit 1; }
          if [ -f "${ENV_LOCAL_FILE}" ]; then
            echo "Usando overrides locais de ${ENV_LOCAL_FILE}"
          fi
        '''
      }
    }

    stage("SonarQube Analysis") {
      when {
        expression { return params.RUN_SONAR }
      }
      steps {
        script {
          def scannerHome = tool 'SonarScanner for .NET'
          withSonarQubeEnv(params.SONARQUBE_ENV) {
            sh """
              set -euo pipefail
              cd "${WORKDIR}"
              dotnet "${scannerHome}/SonarScanner.MSBuild.dll" begin \
                /k:"gerenciadorpropostas" \
                /n:"GerenciadorPropostas" \
                /v:"${BUILD_NUMBER}" \
                /d:sonar.host.url="${SONAR_HOST_URL}" \
                /d:sonar.token="${SONAR_AUTH_TOKEN}"
              dotnet build serverApi/serverApi.sln --no-incremental
              dotnet "${scannerHome}/SonarScanner.MSBuild.dll" end \
                /d:sonar.token="${SONAR_AUTH_TOKEN}"
            """
          }
        }
      }
    }

    stage("Quality Gate") {
      when {
        expression { return params.RUN_SONAR }
      }
      steps {
        timeout(time: 5, unit: 'MINUTES') {
          waitForQualityGate abortPipeline: true
        }
      }
    }

    stage("Build Docker Images") {
      steps {
        sh '''
          set -euo pipefail
          if ! docker context use "${DOCKER_CONTEXT}" >/dev/null 2>&1; then
            echo "Contexto ${DOCKER_CONTEXT} nao encontrado. Usando contexto atual."
            unset DOCKER_CONTEXT
          fi
          cd "${WORKDIR}"
          set -a
          . "${ENV_FILE}"
          if [ -f "${ENV_LOCAL_FILE}" ]; then
            . "${ENV_LOCAL_FILE}"
          fi
          set +a
          export APP_IMAGE="register.devos.abellinux.com/${PROJECT_SLUG}/app:${EFFECTIVE_IMAGE_TAG}"
          export FRONTEND_IMAGE="register.devos.abellinux.com/${PROJECT_SLUG}/frontend:${EFFECTIVE_IMAGE_TAG}"
          export APP_NAME="${PROJECT_SLUG}"
          export ENVIRONMENT="${ENVIRONMENT}"
          export COMPOSE_PROJECT_NAME="${COMPOSE_PROJECT_NAME}"
          docker-compose --env-file "${ENV_FILE}" build
        '''
      }
    }

    stage("Deploy") {
      when {
        expression { return params.DO_DEPLOY }
      }
      steps {
        sh '''
          set -euo pipefail
          if ! docker context use "${DOCKER_CONTEXT}" >/dev/null 2>&1; then
            echo "Contexto ${DOCKER_CONTEXT} nao encontrado. Usando contexto atual."
            unset DOCKER_CONTEXT
          fi
          cd "${WORKDIR}"
          set -a
          . "${ENV_FILE}"
          if [ -f "${ENV_LOCAL_FILE}" ]; then
            . "${ENV_LOCAL_FILE}"
          fi
          set +a
          export APP_IMAGE="register.devos.abellinux.com/${PROJECT_SLUG}/app:${EFFECTIVE_IMAGE_TAG}"
          export FRONTEND_IMAGE="register.devos.abellinux.com/${PROJECT_SLUG}/frontend:${EFFECTIVE_IMAGE_TAG}"
          export APP_NAME="${PROJECT_SLUG}"
          export ENVIRONMENT="${ENVIRONMENT}"
          export COMPOSE_PROJECT_NAME="${COMPOSE_PROJECT_NAME}"
          docker-compose --env-file "${ENV_FILE}" up -d --remove-orphans
          docker-compose --env-file "${ENV_FILE}" ps
        '''
      }
    }
  }

  post {
    success {
      echo "Pipeline GitHub + Sonar + Docker finalizada com sucesso."
    }
    failure {
      echo "Pipeline falhou. Verifique checkout GitHub, SonarQube e docker-compose."
    }
    always {
      deleteDir()
    }
  }
}
