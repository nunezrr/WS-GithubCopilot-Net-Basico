namespace ContosoBanco.Models;

/// <summary>
/// Representa un cliente de Contoso Banco.
/// </summary>
public class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
}

/// <summary>
/// DTO para crear o actualizar un cliente sin incluir el Id.
/// </summary>
public record ClienteDto(string Nombre, string Email, string Telefono, string Direccion);
