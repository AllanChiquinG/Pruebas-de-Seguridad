pipeline {
    agent any

    environment {
        DTRACK_URL = 'http://dtrack-apiserver:8080'
        DTRACK_API_KEY = credentials('dtrack-api-key')
        PROJECT_NAME = 'VulnerableApi'
    }

    stages {
        stage('Clone Repository') {
            steps {
                echo '=== Clonando repositorio ==='
                checkout([
                    $class: 'GitSCM',
                    branches: [[name: '*/main']],
                    extensions: [],
                    userRemoteConfigs: [[url: 'https://github.com/AllanChiquinG/Pruebas-de-Seguridad.git']]
                ])
            }
        }

        stage('Build .NET Project') {
            steps {
                echo '=== Compilando proyecto .NET ==='
                sh '''
                    docker run --rm \
                      -v "${WORKSPACE}:/src" \
                      -w /src \
                      mcr.microsoft.com/dotnet/sdk:6.0 \
                      dotnet restore VulnerableApi.csproj
                '''
                sh '''
                    docker run --rm \
                      -v "${WORKSPACE}:/src" \
                      -w /src \
                      mcr.microsoft.com/dotnet/sdk:6.0 \
                      dotnet publish VulnerableApi.csproj -c Release -o /app/publish
                '''
            }
        }

        stage('Generate SBOM with CycloneDX') {
            steps {
                echo '=== Generando SBOM con CycloneDX ==='
                sh '''
                    docker run --rm \
                      -v "${WORKSPACE}:/src" \
                      -w /src \
                      cyclonedx/cyclonedx-dotnet \
                      --output /src/bom.xml
                '''
            }
        }

        stage('Upload SBOM to Dependency Track') {
            steps {
                echo '=== Subiendo SBOM a Dependency Track ==='
                sh '''
                    curl -X POST "${DTRACK_URL}/api/v1/bom" \
                      -H "X-Api-Key: ${DTRACK_API_KEY}" \
                      -H "Content-Type: multipart/form-data" \
                      -F "autoCreate=true" \
                      -F "projectName=${PROJECT_NAME}" \
                      -F "projectVersion=1.0" \
                      -F "bom=@bom.xml"
                '''
            }
        }

        stage('Wait for Analysis') {
            steps {
                echo '=== Esperando analisis de Dependency Track ==='
                sleep(time: 90, unit: 'SECONDS')
            }
        }

        stage('Export Vulnerability Report') {
            steps {
                echo '=== Exportando resultados ==='
                sh 'mkdir -p reports'
                sh '''
                    curl -X GET "${DTRACK_URL}/api/v1/vulnerability/project" \
                      -H "X-Api-Key: ${DTRACK_API_KEY}" \
                      -H "Content-Type: application/json" \
                      -o reports/vulnerabilities.json
                '''
                sh 'cat reports/vulnerabilities.json | head -50'
            }
        }

        stage('Generate Report with Pandoc') {
            steps {
                echo '=== Generando informe con Pandoc ==='
                sh '''
                    docker run --rm \
                      -v "${WORKSPACE}:/data" \
                      pandoc/latex \
                      /data/templates/informe.md \
                      -o /data/reports/informe_vulnerabilidades.pdf \
                      --pdf-engine=xelatex \
                      -V geometry:margin=1in
                '''
            }
        }
    }

    post {
        always {
            echo '=== Pipeline completado ==='
            sh 'ls -la reports/ || true'
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
