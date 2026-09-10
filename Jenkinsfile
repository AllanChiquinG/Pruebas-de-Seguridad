pipeline {
    agent any

    environment {
        DOTNET_VERSION = '6.0'
        DTRACK_URL = 'http://dtrack-apiserver:8080'
        DTRACK_API_KEY = credentials('dtrack-api-key')
        PROJECT_NAME = 'VulnerableApi'
    }

    stages {
        stage('Clone Repository') {
            steps {
                echo 'Clonando repositorio...'
                checkout scm
            }
        }

        stage('Build .NET Project') {
            steps {
                echo 'Compilando proyecto .NET...'
                sh '''
                    docker run --rm \
                      -v "${WORKSPACE}/src:/src" \
                      -w /src \
                      mcr.microsoft.com/dotnet/sdk:6.0 \
                      dotnet publish VulnerableApi/VulnerableApi.csproj -c Release -o /app/publish
                '''
            }
        }

        stage('Generate SBOM with CycloneDX') {
            steps {
                echo 'Generando SBOM con CycloneDX...'
                sh '''
                    docker run --rm \
                      -v "${WORKSPACE}/src:/src" \
                      -w /src/VulnerableApi \
                      cyclonedx/cyclonedx-dotnet \
                      --output /src/bom.xml
                '''
            }
        }

        stage('Upload SBOM to Dependency Track') {
            steps {
                echo 'Subiendo SBOM a Dependency Track...'
                sh '''
                    curl -X POST "${DTRACK_URL}/api/v1/bom" \
                      -H "X-Api-Key: ${DTRACK_API_KEY}" \
                      -H "Content-Type: multipart/form-data" \
                      -F "autoCreate=true" \
                      -F "projectName=${PROJECT_NAME}" \
                      -F "projectVersion=1.0" \
                      -F "bom=@src/bom.xml"
                '''
            }
        }

        stage('Wait for Analysis') {
            steps {
                echo 'Esperando que Dependency Track analice las vulnerabilidades...'
                sleep(time: 60, unit: 'SECONDS')
            }
        }

        stage('Export Vulnerability Report') {
            steps {
                echo 'Exportando resultados de vulnerabilidades...'
                sh '''
                    curl -X GET "${DTRACK_URL}/api/v1/vulnerability/project" \
                      -H "X-Api-Key: ${DTRACK_API_KEY}" \
                      -H "Content-Type: application/json" \
                      -o reports/vulnerabilities.json
                '''
            }
        }

        stage('Generate Report with Pandoc') {
            steps {
                echo 'Generando informe con Pandoc...'
                sh '''
                    docker run --rm \
                      -v "${WORKSPACE}:/data" \
                      pandoc/latex \
                      templates/informe.md \
                      -o reports/informe_vulnerabilidades.pdf \
                      --pdf-engine=xelatex \
                      -V geometry:margin=1in \
                      -V title="Informe de Vulnerabilidades - ${PROJECT_NAME}" \
                      -V date="$(date +%Y-%m-%d)"
                '''
            }
        }
    }

    post {
        always {
            echo 'Pipeline completado. Archivos generados:'
            sh 'ls -la reports/'
            archiveArtifacts artifacts: 'reports/*', allowEmptyArchive: true
        }
        success {
            echo 'Pipeline ejecutado exitosamente!'
        }
        failure {
            echo 'Error en la ejecucion del pipeline.'
        }
    }
}
