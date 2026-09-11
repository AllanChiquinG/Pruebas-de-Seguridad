---
title: "Informe de Analisis de Vulnerabilidades"
subtitle: "Proyecto: VulnerableApi (.NET 6)"
date: "`date +%Y-%m-%d`"
author: "Auditoria de Seguridad - Jenkins + Dependency Track"
geometry: margin=2.5cm
fontsize: 11pt
header-includes:
  - \usepackage{booktabs}
  - \usepackage{longtable}
  - \usepackage{xcolor}
  - \definecolor{critico}{RGB}{220,20,60}
  - \definecolor{alto}{RGB}{255,140,0}
  - \definecolor{medio}{RGB}{255,215,0}
---

\newpage

# Resumen Ejecutivo

Este informe presenta los resultados del analisis de vulnerabilidades realizado
sobre el proyecto **VulnerableApi**, una aplicacion web construida con .NET 6
que utiliza multiple librerias con vulnerabilidades conocidas.

El analisis fue realizado utilizando **Dependency Track** como plataforma de
gestion de vulnerabilidades, integrada en un pipeline de **Jenkins**.

| Metrica               | Valor    |
|-----------------------|----------|
| Proyecto              | VulnerableApi |
| Framework             | .NET 6.0 |
| Herramienta de analisis | Dependency Track v4.14.3 |
| Motor de SBOM         | CycloneDX |
| Pipeline              | Jenkins LTS |

\newpage

# Dependencias Analizadas

## Librerias con Vulnerabilidades Conocidas

### 1. Newtonsoft.Json 9.0.1

- **CVE**: CVE-2024-21907
- **Severidad**: Alta
- **CVSS**: 7.5
- **Descripcion**: Vulnerabilidad de Stack Overflow en el parsing de JSON
  cuando se procesan objetos anidados con profundidad excesiva.
- **Impacto**: Denegacion de servicio (DoS) por agotamiento de stack.
- **Remediacion**: Actualizar a Newtonsoft.Json >= 13.0.1

### 2. System.Text.RegularExpressions 4.3.0

- **CVE**: CVE-2019-0820
- **Severidad**: Alta
- **CVSS**: 7.5
- **Descripcion**: Vulnerabilidad de Regular Expression Denial of Service (ReDoS).
  Expresiones regulares maliciosas pueden causar bloqueo de la aplicacion.
- **Impacto**: Denegacion de servicio por bloqueo de CPU.
- **Remediacion**: Actualizar a .NET 6.0 runtime actualizado.

### 3. System.Net.Http 4.3.0

- **CVE**: CVE-2017-0248
- **Severidad**: Alta
- **CVSS**: 8.1
- **Descripcion**: Bypass de seguridad en la validacion de certificados SSL.
- **Impacto**: Posible ataque Man-in-the-Middle (MitM).
- **Remediacion**: Actualizar a .NET 6.0 runtime actualizado.

### 4. System.Diagnostics.Process 4.3.0

- **CVE**: CVE-2018-8292
- **Severidad**: Media
- **CVSS**: 5.3
- **Descripcion**: Bypass de seguridad que permite elevacion de privilegios.
- **Impacto**: Acceso no autorizado a recursos del sistema.
- **Remediacion**: Actualizar a .NET 6.0 runtime actualizado.

### 5. System.Xml.XmlDocument 4.3.0

- **CVE**: CVE-2018-8292
- **Severidad**: Media
- **CVSS**: 5.3
- **Descripcion**: Vulnerabilidad de XML External Entity (XXE).
- **Impacto**: Posible lectura de archivos internos o SSRF.
- **Remediacion**: Actualizar a .NET 6.0 runtime actualizado.

### 6. System.Data.SqlClient 4.3.0

- **CVE**: CVE-2017-8516 / CVE-2021-24374
- **Severidad**: Critica / Alta
- **CVSS**: 9.8 / 8.8
- **Descripcion**: Vulnerabilidad de SQL Injection y Remote Code Execution
  en componentes de acceso a datos SQL Server.
- **Impacto**: Ejecucion remota de codigo y acceso no autorizado a datos.
- **Remediacion**: Actualizar a System.Data.SqlClient >= 4.8.6 o Microsoft.Data.SqlClient >= 2.1.5

### 7. System.Net.Security 4.3.0

- **CVE**: CVE-2017-0248
- **Severidad**: Alta
- **CVSS**: 8.1
- **Descripcion**: Bypass de autenticacion SSL/TLS en la validacion de
  certificados de servidor.
- **Impacto**: Posible intercepcion de trafico (Man-in-the-Middle).
- **Remediacion**: Actualizar a .NET 6.0 runtime actualizado.

### 8. System.DirectoryServices 4.3.0

- **CVE**: CVE-2018-8292
- **Severidad**: Alta
- **CVSS**: 7.5
- **Descripcion**: Bypass de seguridad en la autenticacion LDAP.
- **Impacto**: Acceso no autorizado a directorios LDAP.
- **Remediacion**: Actualizar a .NET 6.0 runtime actualizado.

### 9. System.Drawing.Common 4.3.0

- **CVE**: CVE-2021-24112
- **Severidad**: Critica
- **CVSS**: 9.8
- **Descripcion**: Vulnerabilidad de Remote Code Execution en el procesamiento
  de imagenes a traves de GDI+.
- **Impacto**: Ejecucion remota de codigo con privilegios del servidor.
- **Remediacion**: Actualizar a System.Drawing.Common >= 7.0.0 o migrar a libgdiplus

### 10. System.Configuration.ConfigurationManager 4.3.0

- **CVE**: CVE-2024-30043
- **Severidad**: Media
- **CVSS**: 5.5
- **Descripcion**: Vulnerabilidad de elevacion de privilegios en la
  configuracion de la aplicacion.
- **Impacto**: Acceso no autorizado a datos de configuracion.
- **Remediacion**: Actualizar a >= 8.0.0

### 11. System.Security.Cryptography.Xml 4.3.0

- **CVE**: CVE-2024-30044
- **Severidad**: Alta
- **CVSS**: 7.8
- **Descripcion**: Vulnerabilidad de Remote Code Execution en la
  validacion de firmas XML.
- **Impacto**: Ejecucion remota de codigo a traves de XML malicioso.
- **Remediacion**: Actualizar a >= 8.0.0

### 12. System.Text.Encoding.CodePages 4.3.0

- **CVE**: CVE-2019-0820
- **Severidad**: Alta
- **CVSS**: 7.5
- **Descripcion**: Vulnerabilidad de Denial of Service en el parser de
  codificaciones de texto del framework .NET.
- **Impacto**: Denegacion de servicio por consumo de recursos.
- **Remediacion**: Actualizar a .NET 6.0 runtime actualizado.

\newpage

# Resumen por Severidad

| Severidad | Cantidad |
|-----------|----------|
| Critica   | 2        |
| Alta      | 7        |
| Media     | 3        |
| Baja      | 0        |
| **Total** | **12**   |

\newpage

# Recomendaciones

1. **Actualizar Dependencias**: Actualizar todas las librerias a sus versiones
   mas recientes compatibles con .NET 6.

2. **Implementar SCA en CI/CD**: Mantener el analisis automatico de componentes
   de software en cada build.

3. **Politica de Versiones**: Establecer una politica que impida el uso de
   dependencias con vulnerabilidades conocidas de severidad alta o critica.

4. **Monitoreo Continuo**: Implementar monitoreo continuo de nuevas
   vulnerabilidades en las dependencias del proyecto.

5. **Pruebas de Seguridad**: Incluir pruebas de seguridad automatizadas
   como SAST y DAST en el pipeline de CI/CD.

\newpage

# Metodologia

1. Se genero un **Software Bill of Materials (SBOM)** utilizando CycloneDX.
2. El SBOM fue subido a **Dependency Track** para analisis de vulnerabilidades.
3. Dependency Track cruzo las dependencias contra bases de datos de CVE.
4. Los resultados fueron exportados en formato JSON.
5. Este informe fue generado automaticamente con **Pandoc**.

\newpage

# Resultados del Pipeline

- **Repositorio**: Repositorio Git del proyecto
- **Pipeline**: Jenkins LTS
- **SBOM**: CycloneDX XML
- **Analisis**: Dependency Track v4.14.3
- **Informe**: Generado con Pandoc + XeLaTeX

---

*Informe generado automaticamente como parte de la actividad de auditoria de seguridad.*
