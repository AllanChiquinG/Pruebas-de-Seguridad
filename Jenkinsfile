pipeline {
    agent any

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
                sh 'dotnet CycloneDX VulnerableApi.csproj -o reports/bom.xml'
            }
        }

        stage('Upload SBOM to Dependency Track') {
            steps {
                echo '=== Subiendo SBOM a Dependency Track ==='
                withCredentials([string(credentialsId: 'dtrack-api-key', variable: 'DT_KEY')]) {
                    sh '''
                        curl -X POST "http://dtrack-apiserver:8080/api/v1/bom" \
                          -H "X-Api-Key: $DT_KEY" \
                          -F "autoCreate=true" \
                          -F "projectName=VulnerableApi" \
                          -F "projectVersion=1.0" \
                          -F "bom=@reports/bom.xml"
                    '''
                }
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
                withCredentials([string(credentialsId: 'dtrack-api-key', variable: 'DT_KEY')]) {
                    sh '''
                        curl -X GET "http://dtrack-apiserver:8080/api/v1/vulnerability/project" \
                          -H "X-Api-Key: $DT_KEY" \
                          -H "Accept: application/json" \
                          -o reports/vulnerabilities.json
                    '''
                }
                sh 'cat reports/vulnerabilities.json'
            }
        }

        stage('Generate Report with Pandoc') {
            steps {
                echo '=== Generando informe con Pandoc ==='
                sh '''
                    mkdir -p reports
                    pandoc informe.md \
                      -o reports/informe_vulnerabilidades.pdf \
                      --pdf-engine=xelatex \
                      -V geometry:margin=1in
                '''
            }
        }
    }

    post {
        always {
            echo '=== Pipeline completado ==='
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
