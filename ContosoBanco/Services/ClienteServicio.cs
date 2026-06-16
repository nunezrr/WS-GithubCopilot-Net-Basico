using ContosoBanco.Models;

namespace ContosoBanco.Services;

/// <summary>
/// Servicio en memoria para gestionar clientes de Contoso Banco.
/// </summary>
public class ClienteServicio
{
    private readonly List<Cliente> clientes = new()
    {
        new Cliente { Id = 1, Nombre = "Ana Pérez", Email = "ana.perez@contoso.cr", Telefono = "+506 7000 0001", Direccion = "Av. Central 123, San José" },
        new Cliente { Id = 2, Nombre = "Juan Rodríguez", Email = "juan.rodriguez@contoso.cr", Telefono = "+506 7000 0002", Direccion = "Calle 45 #12, Alajuela" },
        new Cliente { Id = 3, Nombre = "María Gómez", Email = "maria.gomez@contoso.cr", Telefono = "+506 7000 0003", Direccion = "Barrio México 45, Heredia" }
    };

    /// <summary>
    /// Obtiene todos los clientes registrados.
    /// </summary>
    public IEnumerable<Cliente> ObtenerTodos() => clientes;

    /// <summary>
    /// Obtiene un cliente por su identificador.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    public Cliente? ObtenerPorId(int id) => clientes.FirstOrDefault(cliente => cliente.Id == id);

    /// <summary>
    /// Crea un nuevo cliente y lo agrega al almacenamiento en memoria.
    /// </summary>
    /// <param name="clienteDto">Datos del cliente a crear.</param>
    public Cliente Crear(ClienteDto clienteDto)
    {
        var nuevoId = clientes.Any() ? clientes.Max(cliente => cliente.Id) + 1 : 1;
        var cliente = new Cliente
        {
            Id = nuevoId,
            Nombre = clienteDto.Nombre,
            Email = clienteDto.Email,
            Telefono = clienteDto.Telefono,
            Direccion = clienteDto.Direccion
        };

        clientes.Add(cliente);
        return cliente;
    }

    /// <summary>
    /// Actualiza los datos de un cliente existente.
    /// </summary>
    /// <param name="id">Identificador del cliente a actualizar.</param>
    /// <param name="clienteDto">Nuevos datos del cliente.</param>
    public Cliente? Actualizar(int id, ClienteDto clienteDto)
    {
        var clienteExistente = ObtenerPorId(id);
        if (clienteExistente is null)
        {
            return null;
        }

        clienteExistente.Nombre = clienteDto.Nombre;
        clienteExistente.Email = clienteDto.Email;
        clienteExistente.Telefono = clienteDto.Telefono;
        clienteExistente.Direccion = clienteDto.Direccion;

        return clienteExistente;
    }

    /// <summary>
    /// Elimina un cliente por su identificador.
    /// </summary>
    /// <param name="id">Identificador del cliente a eliminar.</param>
    public bool Eliminar(int id)
    {
        var cliente = ObtenerPorId(id);
        if (cliente is null)
        {
            return false;
        }

        return clientes.Remove(cliente);
    }
}
