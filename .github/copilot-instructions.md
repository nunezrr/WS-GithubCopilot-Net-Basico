# Instrucciones para GitHub Copilot - Proyecto Contoso Banco

## Idioma
- Todo el código, comentarios y documentación debe estar en **español**.
- Mensajes de error en español.
- Nombres de variables, propiedades y métodos en español (excepto palabras técnicas estándar como Get, Post, Api, Id).

## Estándares de Código
| Aspecto | Estándar |
|---------|----------|
| Tecnología | .NET 10 (LTS) |
| Estilo de API | Minimal APIs |
| Swagger | Incluir configuración para generar OpenAPI automáticamente |
| Estilo | Convenciones de C# de Microsoft |
| Documentación | Comentarios XML (`///`) en español |

## Nomenclatura
- Propiedades y métodos públicos: PascalCase en español (`ObtenerClientes`, `CrearCuenta`).
- Variables locales y parámetros: camelCase en español (`clienteId`, `saldoActual`).
- Clases y records: PascalCase (`Cliente`, `CuentaBancaria`, `ClienteServicio`).
- Constantes: PascalCase (`SaldoMinimo`, `MaximoTransferencia`).
- Interfaces: prefijo `I` + PascalCase (`IClienteServicio`).

## Contexto del Proyecto
Este es un sistema bancario para Contoso Banco que gestiona clientes, cuentas y transacciones. Usa datos en memoria (colecciones C#) sin base de datos externa. Usa Minimal APIs (no Controllers). Usa records para DTOs cuando sea apropiado. La especificación completa está en `docs/spec.md`.
