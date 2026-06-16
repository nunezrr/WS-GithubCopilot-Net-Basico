using ContosoBanco.Models;

namespace ContosoBanco.Services;

/// <summary>
/// Servicio en memoria para gestionar cuentas bancarias de Contoso Banco.
/// </summary>
public class CuentaServicio
{
    private readonly List<Cuenta> cuentas = new()
    {
        new Cuenta { Id = 1, ClienteId = 1, NumeroCuenta = "001-0001-000001", TipoCuenta = "Ahorro", Saldo = 1500m, Estado = "Activa", FechaApertura = new DateTime(2026, 6, 1) },
        new Cuenta { Id = 2, ClienteId = 2, NumeroCuenta = "001-0002-000002", TipoCuenta = "Corriente", Saldo = 2500m, Estado = "Activa", FechaApertura = new DateTime(2026, 6, 5) },
        new Cuenta { Id = 3, ClienteId = 3, NumeroCuenta = "001-0003-000003", TipoCuenta = "Ahorro", Saldo = 500m, Estado = "Activa", FechaApertura = new DateTime(2026, 6, 10) }
    };

    /// <summary>
    /// Obtiene todas las cuentas registradas.
    /// </summary>
    public IEnumerable<Cuenta> ObtenerTodos() => cuentas;

    /// <summary>
    /// Obtiene una cuenta por su identificador.
    /// </summary>
    /// <param name="id">Identificador de la cuenta.</param>
    public Cuenta? ObtenerPorId(int id) => cuentas.FirstOrDefault(cuenta => cuenta.Id == id);

    /// <summary>
    /// Crea una nueva cuenta y la agrega al almacenamiento en memoria.
    /// </summary>
    /// <param name="cuentaDto">Datos de la cuenta a crear.</param>
    public Cuenta Crear(CuentaDto cuentaDto)
    {
        if (cuentaDto.Saldo < 0)
        {
            throw new ArgumentException("El saldo no puede ser negativo.", nameof(cuentaDto.Saldo));
        }

        var nuevoId = cuentas.Any() ? cuentas.Max(cuenta => cuenta.Id) + 1 : 1;
        var cuenta = new Cuenta
        {
            Id = nuevoId,
            ClienteId = cuentaDto.ClienteId,
            NumeroCuenta = cuentaDto.NumeroCuenta,
            TipoCuenta = cuentaDto.TipoCuenta,
            Saldo = cuentaDto.Saldo,
            Estado = cuentaDto.Estado,
            FechaApertura = DateTime.UtcNow
        };

        cuentas.Add(cuenta);
        return cuenta;
    }

    /// <summary>
    /// Actualiza los datos de una cuenta existente.
    /// </summary>
    /// <param name="id">Identificador de la cuenta a actualizar.</param>
    /// <param name="cuentaDto">Nuevos datos de la cuenta.</param>
    public Cuenta? Actualizar(int id, CuentaDto cuentaDto)
    {
        var cuentaExistente = ObtenerPorId(id);
        if (cuentaExistente is null)
        {
            return null;
        }

        if (cuentaDto.Saldo < 0)
        {
            throw new ArgumentException("El saldo no puede ser negativo.", nameof(cuentaDto.Saldo));
        }

        cuentaExistente.ClienteId = cuentaDto.ClienteId;
        cuentaExistente.NumeroCuenta = cuentaDto.NumeroCuenta;
        cuentaExistente.TipoCuenta = cuentaDto.TipoCuenta;
        cuentaExistente.Saldo = cuentaDto.Saldo;
        cuentaExistente.Estado = cuentaDto.Estado;

        return cuentaExistente;
    }

    /// <summary>
    /// Elimina una cuenta por su identificador.
    /// </summary>
    /// <param name="id">Identificador de la cuenta a eliminar.</param>
    public bool Eliminar(int id)
    {
        var cuenta = ObtenerPorId(id);
        if (cuenta is null)
        {
            return false;
        }

        return cuentas.Remove(cuenta);
    }
}
