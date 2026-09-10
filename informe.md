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

\newpage

# Resumen por Severidad

| Severidad | Cantidad |
|-----------|----------|
| Critica   | 0        |
| Alta      | 3        |
| Media     | 2        |
| Baja      | 0        |
| **Total** | **5**    |

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
