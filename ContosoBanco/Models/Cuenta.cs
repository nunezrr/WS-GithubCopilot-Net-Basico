namespace ContosoBanco.Models;

/// <summary>
/// Representa una cuenta bancaria de Contoso Banco.
/// </summary>
public class Cuenta
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string NumeroCuenta { get; set; } = string.Empty;
    public string TipoCuenta { get; set; } = string.Empty;
    public decimal Saldo { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaApertura { get; set; }
}

/// <summary>
/// DTO para crear o actualizar una cuenta bancaria sin incluir Id ni FechaApertura.
/// </summary>
public record CuentaDto(int ClienteId, string NumeroCuenta, string TipoCuenta, decimal Saldo, string Estado);
