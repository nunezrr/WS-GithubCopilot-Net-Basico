# Especificación Técnica - Contoso Banco

## 1. Resumen ejecutivo

Contoso Banco es un sistema bancario simple diseñado para gestionar clientes, cuentas bancarias y transacciones. El objetivo es permitir operaciones básicas de apertura y administración de cuentas, así como registrar depósitos, retiros y transferencias. El sistema estará construido con .NET 10 usando Minimal APIs para una implementación ligera y mantenible.

## 2. Entidades del dominio

### Cliente

| Campo | Tipo | Descripción |
|---|---|---|
| Id | `int` / `Guid` | Identificador único del cliente |
| Nombre | `string` | Nombre completo del cliente |
| Email | `string` | Correo electrónico |
| Telefono | `string` | Teléfono de contacto |
| Direccion | `string` | Dirección física |
| FechaCreacion | `DateTime` | Fecha de registro del cliente |
| Estado | `string` | Estado del cliente (`Activo`, `Inactivo`) |

### Cuenta bancaria

| Campo | Tipo | Descripción |
|---|---|---|
| Id | `int` / `Guid` | Identificador único de la cuenta |
| ClienteId | `int` / `Guid` | Identificador del cliente propietario |
| TipoCuenta | `string` | Tipo de cuenta (`Ahorro`, `Corriente`) |
| Saldo | `decimal` | Saldo actual de la cuenta |
| Estado | `string` | Estado de la cuenta (`Activa`, `Cerrada`, `Bloqueada`) |
| FechaApertura | `DateTime` | Fecha de apertura de la cuenta |
| Moneda | `string` | Código de moneda opcional (`CRC`, `USD`) |

### Transacción

| Campo | Tipo | Descripción |
|---|---|---|
| Id | `int` / `Guid` | Identificador único de la transacción |
| CuentaIdOrigen | `int` / `Guid` | Cuenta origen de la operación |
| CuentaIdDestino | `int` / `Guid`? | Cuenta destino para transferencias |
| Tipo | `string` | Tipo de transacción (`Deposito`, `Retiro`, `Transferencia`) |
| Monto | `decimal` | Monto de la transacción |
| Fecha | `DateTime` | Fecha y hora de la transacción |
| Descripcion | `string` | Descripción o concepto |
| Estado | `string` | Estado de la transacción (`Completada`, `Pendiente`, `Fallida`) |

## 3. Endpoints REST

### Clientes

| Método | Ruta | Descripción | Códigos de respuesta |
|---|---|---|---|
| GET | `/clientes` | Listar todos los clientes | `200 OK` |
| GET | `/clientes/{id}` | Obtener cliente por id | `200 OK`, `404 Not Found` |
| POST | `/clientes` | Crear un nuevo cliente | `201 Created`, `400 Bad Request` |
| PUT | `/clientes/{id}` | Actualizar cliente existente | `200 OK`, `400 Bad Request`, `404 Not Found` |
| DELETE | `/clientes/{id}` | Eliminar cliente | `204 No Content`, `404 Not Found` |

### Cuentas

| Método | Ruta | Descripción | Códigos de respuesta |
|---|---|---|---|
| GET | `/cuentas` | Listar todas las cuentas | `200 OK` |
| GET | `/cuentas/{id}` | Obtener cuenta por id | `200 OK`, `404 Not Found` |
| POST | `/cuentas` | Crear nueva cuenta | `201 Created`, `400 Bad Request` |
| PUT | `/cuentas/{id}` | Actualizar cuenta | `200 OK`, `400 Bad Request`, `404 Not Found` |
| DELETE | `/cuentas/{id}` | Eliminar cuenta | `204 No Content`, `404 Not Found` |
| GET | `/clientes/{clienteId}/cuentas` | Listar cuentas de un cliente | `200 OK`, `404 Not Found` |

### Transacciones

| Método | Ruta | Descripción | Códigos de respuesta |
|---|---|---|---|
| GET | `/transacciones` | Listar todas las transacciones | `200 OK` |
| GET | `/transacciones/{id}` | Obtener transacción por id | `200 OK`, `404 Not Found` |
| POST | `/transacciones` | Crear transacción general | `201 Created`, `400 Bad Request` |
| PUT | `/transacciones/{id}` | Actualizar transacción | `200 OK`, `400 Bad Request`, `404 Not Found` |
| DELETE | `/transacciones/{id}` | Eliminar transacción | `204 No Content`, `404 Not Found` |

### Operaciones bancarias

| Método | Ruta | Descripción | Códigos de respuesta |
|---|---|---|---|
| POST | `/cuentas/{id}/deposito` | Realizar depósito en una cuenta | `200 OK`, `400 Bad Request`, `404 Not Found` |
| POST | `/cuentas/{id}/retiro` | Realizar retiro desde una cuenta | `200 OK`, `400 Bad Request`, `404 Not Found` |
| POST | `/cuentas/{id}/transferencia` | Transferir entre cuentas | `200 OK`, `400 Bad Request`, `404 Not Found` |

## 4. Reglas de negocio

- `TipoCuenta` solo puede ser `Ahorro` o `Corriente`.
- `Estado` de cuenta solo puede ser `Activa`, `Cerrada` o `Bloqueada`.
- Para depósitos, el monto debe ser mayor que cero.
- Para retiros, el monto debe ser mayor que cero y no puede exceder el saldo disponible.
- Para transferencias, la cuenta origen debe tener saldo suficiente y ambas cuentas deben estar activas.
- No se debe permitir operar sobre cuentas en estado `Cerrada` o `Bloqueada`.
- Las transacciones deben registrar un `Estado` válido: `Completada`, `Pendiente`, `Fallida`.
- Un cliente puede tener múltiples cuentas; cada cuenta pertenece a un único cliente.
- Al crear una cuenta nueva, el saldo inicial puede ser cero o el valor inicial definido en la solicitud.

## 5. Stack tecnológico

- `.NET 10`
- `Minimal APIs`
- `ASP.NET Core`
- `Entity Framework Core` (opcional para persistencia)
- `Swashbuckle.AspNetCore` para Swagger/OpenAPI
- `xUnit` para pruebas unitarias
- `C# 12` (según .NET 10)

## 6. Estructura de carpetas esperada

```text
src/
  ContosoBanco.Api/
    Program.cs
    appsettings.json
    Models/
      Cliente.cs
      Cuenta.cs
      Transaccion.cs
    DTOs/
      ClienteDto.cs
      CuentaDto.cs
      TransaccionDto.cs
    Services/
      IBancoRepository.cs
      BancoRepository.cs
    Data/
      BankContext.cs
    Extensions/
      ServiceCollectionExtensions.cs
  ContosoBanco.Tests/
    ClienteTests.cs
    CuentaTests.cs
    TransaccionTests.cs

docs/
  spec.md

README.md
```

## 7. Datos de ejemplo

### Clientes

1. `Id`: 1
   - `Nombre`: Ana Pérez
   - `Email`: ana.perez@contoso.cr
   - `Telefono`: +506 7000 0001
   - `Direccion`: Av. Central 123, San José
   - `FechaCreacion`: `2026-06-16`
   - `Estado`: `Activo`

2. `Id`: 2
   - `Nombre`: Juan Rodríguez
   - `Email`: juan.rodriguez@contoso.cr
   - `Telefono`: +506 7000 0002
   - `Direccion`: Calle 45 #12, Alajuela
   - `FechaCreacion`: `2026-06-16`
   - `Estado`: `Activo`

3. `Id`: 3
   - `Nombre`: María Gómez
   - `Email`: maria.gomez@contoso.cr
   - `Telefono`: +506 7000 0003
   - `Direccion`: Barrio México 45, Heredia
   - `FechaCreacion`: `2026-06-16`
   - `Estado`: `Activo`

### Cuentas

1. `Id`: 1
   - `ClienteId`: 1
   - `TipoCuenta`: `Ahorro`
   - `Saldo`: `1500.00`
   - `Estado`: `Activa`
   - `FechaApertura`: `2026-06-01`
   - `Moneda`: `CRC`

2. `Id`: 2
   - `ClienteId`: 2
   - `TipoCuenta`: `Corriente`
   - `Saldo`: `2500.00`
   - `Estado`: `Activa`
   - `FechaApertura`: `2026-06-05`
   - `Moneda`: `CRC`

3. `Id`: 3
   - `ClienteId`: 3
   - `TipoCuenta`: `Ahorro`
   - `Saldo`: `500.00`
   - `Estado`: `Activa`
   - `FechaApertura`: `2026-06-10`
   - `Moneda`: `CRC`
