# Auditoria de Seguridad - VulnerableApi

Pipeline automatizado de auditoria de seguridad para una API .NET 6 con dependencias intencionalmente vulnerables. Integra **Jenkins**, **Dependency Track** y **Pandoc** para generar informes de vulnerabilidades en PDF.

## Arquitectura

```
GitHub (repo)  -->  Jenkins (CI/CD)  -->  CycloneDX (SBOM)
                                              |
                                              v
                                     Dependency Track
                                     (analisis CVE)
                                              |
                                              v
                                     Pandoc + XeLaTeX
                                     (informe PDF)
```

### Contenedores Docker

| Servicio | Puerto | Descripcion |
|----------|--------|-------------|
| `jenkins-auditoria` | `8080` | Jenkins LTS con .NET SDK 6.0, CycloneDX, Pandoc |
| `dtrack-apiserver` | `8091` | API de Dependency Track v4.14.3 |
| `dtrack-frontend` | `8082` | Panel web de Dependency Track |

Todos los contenedores comparten la red Docker `auditoria_net`.

## Dependencias Vulnerables

El proyecto `VulnerableApi` incluye intencionalmente 12 paquetes NuGet con vulnerabilidades conocidas:

| Paquete | Version | CVE(s) | Severidad | CVSS | Tipo de Vulnerabilidad |
|---------|---------|--------|-----------|------|----------------------|
| `Newtonsoft.Json` | 9.0.1 | CVE-2024-21907 | Alta | 7.5 | Stack Overflow (DoS) |
| `System.Text.RegularExpressions` | 4.3.0 | CVE-2019-0820 | Alta | 7.5 | ReDoS |
| `System.Net.Http` | 4.3.0 | CVE-2017-0248 | Alta | 8.1 | Bypass SSL (MitM) |
| `System.Diagnostics.Process` | 4.3.0 | CVE-2018-8292 | Media | 5.3 | Elevacion de privilegios |
| `System.Xml.XmlDocument` | 4.3.0 | CVE-2018-8292 | Media | 5.3 | XML External Entity (XXE) |
| `System.Data.SqlClient` | 4.3.0 | CVE-2017-8516 | Critica | 9.8 | SQL Injection / RCE |
| `System.Net.Security` | 4.3.0 | CVE-2017-0248 | Alta | 8.1 | Bypass autenticacion SSL |
| `System.DirectoryServices` | 4.3.0 | CVE-2018-8292 | Alta | 7.5 | Bypass LDAP |
| `System.Drawing.Common` | 4.3.0 | CVE-2021-24112 | Critica | 9.8 | Remote Code Execution |
| `System.Configuration.ConfigurationManager` | 4.3.0 | CVE-2024-30043 | Media | 5.5 | Elevacion de privilegios |
| `System.Security.Cryptography.Xml` | 4.3.0 | CVE-2024-30044 | Alta | 7.8 | Remote Code Execution |
| `Microsoft.AspNetCore.Mvc.NewtonsoftJson` | 3.1.0 | CVE-2024-21315 | Media | 6.5 | Denial of Service |

## Pipeline (Jenkinsfile)

El pipeline ejecuta 6 etapas:

1. **Build .NET** - Restaura y compila el proyecto
2. **Generate SBOM** - Genera el Software Bill of Materials con CycloneDX
3. **Upload SBOM** - Sube el SBOM a Dependency Track via API REST
4. **Wait for Analysis** - Espera 120s para que DT analice las vulnerabilidades
5. **Export Report** - Exporta las vulnerabilidades en formato JSON
6. **Generate PDF** - Genera el informe PDF con Pandoc y XeLaTeX

### Artefactos generados

- `reports/bom.xml` - SBOM en formato CycloneDX
- `reports/vulnerabilities.json` - Vulnerabilidades encontradas
- `reports/informe_vulnerabilidades.pdf` - Informe ejecutivo en PDF

## Prerequisitos

- Docker Desktop
- Git

## Instalacion

### 1. Clonar el repositorio

```bash
git clone https://github.com/AllanChiquinG/Pruebas-de-Seguridad.git
cd Pruebas-de-Seguridad
```

### 2. Levantar la infraestructura

```bash
docker-compose up -d --build
```

Esto inicia:
- Jenkins en `http://localhost:8080`
- Dependency Track API en `http://localhost:8091`
- Dependency Track Frontend en `http://localhost:8082`

### 3. Configurar Jenkins

1. Abrir `http://localhost:8080`
2. Obtener la password inicial:
   ```bash
   docker exec jenkins-auditoria cat /var/jenkins_home/secrets/initialAdminPassword
   ```
3. Seguir el wizard de instalacion (instalar plugins sugeridos)
4. Crear usuario admin o saltar con `Skip and continue as admin`

### 4. Configurar credenciales en Jenkins

1. Ir a **Manage Jenkins** > **Credentials** > **System** > **Global credentials**
2. Editar la credencial existente `dtrack-api-key` o crear una nueva:
   - **Kind**: Secret text
   - **Secret**: `<API key de Dependency Track>`
   - **ID**: `dtrack-api-key`
   - **Description**: `Dependency Track API Key`

### 5. Configurar Dependency Track

1. Abrir `http://localhost:8082`
2. Login con `admin` / `admin` (cambiar password en el primer ingreso)
3. Ir a **Administration** > **Vulnerability Sources**:
   - Habilitar **Google OSV** con ecosistema: `NuGet`
   - Habilitar **National Vulnerability Database** con API mirroring
4. Ir a **Administration** > **Access Management** > **Teams**:
   - Crear equipo `Jenkins` con permisos:
     - `BOM_UPLOAD`
     - `PORTFOLIO_MANAGEMENT`
     - `PROJECT_CREATION_UPLOAD`
     - `SYSTEM_CONFIGURATION`
     - `VIEW_PORTFOLIO`
     - `VIEW_VULNERABILITY`
     - `VULNERABILITY_MANAGEMENT`
   - Generar API Key y copiarla

### 6. Ejecutar el pipeline

1. Ir a Jenkins > **VulnerableApi-Audit**
2. Hacer clic en **Build Now**
3. Esperar a que termine (~3 minutos)
4. Los artefactos estaran en **Build** > **Console Output** y en **Build Artifacts**

## Estructura del Proyecto

```
.
├── docker-compose.yml          # Infraestructura Docker
├── Dockerfile.jenkins          # Imagen custom de Jenkins
├── entrypoint.sh               # Fix permisos Docker socket
├── credentials.xml             # Credenciales Jenkins (auto)
└── repo/                       # Repositorio del proyecto .NET
    ├── Jenkinsfile             # Pipeline de CI/CD
    ├── VulnerableApi.csproj    # Proyecto .NET 6
    ├── Program.cs              # Entry point
    ├── informe.md              # Plantilla del informe
    ├── Controllers/
    │   └── UsuariosController.cs
    ├── Models/
    │   └── Usuario.cs
    └── Services/
        └── JsonService.cs      # Servicio con dependencias vulnerables
```

## Herramientas Utilizadas

| Herramienta | Version | Funcion |
|-------------|---------|---------|
| Jenkins | LTS | Orquestacion de pipeline CI/CD |
| Dependency Track | 4.14.3 | Gestion y analisis de vulnerabilidades |
| CycloneDX | 4.x | Generacion de SBOM |
| .NET SDK | 6.0 | Compilacion del proyecto |
| Pandoc | 3.1.11.1 | Conversion Markdown a PDF |
| XeLaTeX | TeX Live | Motor de renderizado PDF |
| Google OSV | - | Base de datos de vulnerabilidades NuGet |
| NVD | API v2.0 | National Vulnerability Database |

## Comandos Utiles

```bash
# Ver logs de Dependency Track
docker logs dtrack-apiserver -f

# Ver logs de Jenkins
docker logs jenkins-auditoria -f

# Verificar vulnerabilidades en DT via API
curl -s -H "X-Api-Key: <API_KEY>" \
  "http://localhost:8091/api/v1/vulnerability/project/<UUID>" | python -m json.tool

# Subir BOM manualmente
curl -X POST "http://localhost:8091/api/v1/bom" \
  -H "X-Api-Key: <API_KEY>" \
  -F "autoCreate=true" \
  -F "projectName=VulnerableApi" \
  -F "projectVersion=1.0" \
  -F "bom=@reports/bom.xml"

# Reiniciar todo
docker-compose down -v && docker-compose up -d --build
```

## Notas

- El mirror de NVD puede tardar varias horas en completarse (descarga ~390,000 CVEs)
- El mirror de OSV para NuGet es rapido (~2 segundos, 1890 advisories)
- El `docker-compose down -v` borra todos los datos incluyendo volumes
- El socket Docker se comparte con Jenkins para permitir Docker-in-Docker
