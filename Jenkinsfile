pipeline {
    agent any

    environment {
        DTRACK_URL = 'http://dtrack-apiserver:8080'
        DTRACK_API_KEY = credentials('dtrack-api-key')
        PROJECT_NAME = 'VulnerableApi'
        PATH = "/usr/share/dotnet:/root/.dotnet/tools:${env:PATH}"
    }

    stages {
        stage('Build .NET Project') {
            steps {
                echo '=== Compilando proyecto .NET ==='
                sh 'dotnet restore VulnerableApi.csproj'
                sh 'dotnet publish VulnerableApi.csproj -c Release -o ./publish'
            }
        }

        stage('Generate SBOM with CycloneDX') {
            steps {
                echo '=== Generando SBOM con CycloneDX ==='
                sh 'mkdir -p reports'
                sh 'dotnet CycloneDX VulnerableApi.csproj --output reports/bom.xml'
            }
        }

        stage('Upload SBOM to Dependency Track') {
            steps {
                echo '=== Subiendo SBOM a Dependency Track ==='
                sh '''
                    curl -X POST "${DTRACK_URL}/api/v1/bom" \
                      -H "X-Api-Key: ${DTRACK_API_KEY}" \
                      -F "autoCreate=true" \
                      -F "projectName=${PROJECT_NAME}" \
                      -F "projectVersion=1.0" \
                      -F "bom=@reports/bom.xml"
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
                sh '''
                    curl -X GET "${DTRACK_URL}/api/v1/vulnerability/project" \
                      -H "X-Api-Key: ${DTRACK_API_KEY}" \
                      -H "Accept: application/json" \
                      -o reports/vulnerabilities.json
                '''
                sh 'cat reports/vulnerabilities.json'
            }
        }

        stage('Generate Report with Pandoc') {
            steps {
                echo '=== Generando informe con Pandoc ==='
                sh '''
                    docker run --rm \
                      -v "${WORKSPACE}:/data" \
                      pandoc/latex \
                      /data/informe.md \
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
