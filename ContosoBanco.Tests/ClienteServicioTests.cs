using ContosoBanco.Models;
using ContosoBanco.Services;

namespace ContosoBanco.Tests;

public class ClienteServicioTests
{
    [Fact]
    public void ObtenerTodos_DebeRetornarListaCompletaInicial()
    {
        var servicio = new ClienteServicio();

        var clientes = servicio.ObtenerTodos().ToList();

        Assert.Equal(3, clientes.Count);
        Assert.Contains(clientes, cliente => cliente.Id == 1 && cliente.Nombre == "Ana Pérez");
        Assert.Contains(clientes, cliente => cliente.Id == 2 && cliente.Nombre == "Juan Rodríguez");
        Assert.Contains(clientes, cliente => cliente.Id == 3 && cliente.Nombre == "María Gómez");
    }

    [Fact]
    public void ObtenerPorId_ConIdValido_DebeRetornarCliente()
    {
        var servicio = new ClienteServicio();

        var cliente = servicio.ObtenerPorId(2);

        Assert.NotNull(cliente);
        Assert.Equal(2, cliente!.Id);
        Assert.Equal("Juan Rodríguez", cliente.Nombre);
    }

    [Fact]
    public void ObtenerPorId_ConIdInvalido_DebeRetornarNull()
    {
        var servicio = new ClienteServicio();

        var cliente = servicio.ObtenerPorId(999);

        Assert.Null(cliente);
    }

    [Fact]
    public void Crear_ConDatosValidos_DebeCrearClienteConNuevoId()
    {
        var servicio = new ClienteServicio();
        var dto = new ClienteDto(
            Nombre: "Laura Campos",
            Email: "laura.campos@contoso.cr",
            Telefono: "+506 7000 0004",
            Direccion: "Cartago Centro"
        );

        var creado = servicio.Crear(dto);
        var clientes = servicio.ObtenerTodos().ToList();

        Assert.Equal(4, creado.Id);
        Assert.Equal("Laura Campos", creado.Nombre);
        Assert.Equal(4, clientes.Count);
        Assert.Contains(clientes, cliente => cliente.Id == 4 && cliente.Email == "laura.campos@contoso.cr");
    }

    [Fact]
    public void Crear_ConEmailVacio_DebeCrearClienteConEmailVacio()
    {
        var servicio = new ClienteServicio();
        var dto = new ClienteDto(
            Nombre: "Cliente Sin Email",
            Email: string.Empty,
            Telefono: "+506 7000 0005",
            Direccion: "Puntarenas"
        );

        var creado = servicio.Crear(dto);

        Assert.Equal(4, creado.Id);
        Assert.Equal(string.Empty, creado.Email);
    }

    [Fact]
    public void Crear_ConNombreVacio_DebeCrearClienteConNombreVacio()
    {
        var servicio = new ClienteServicio();
        var dto = new ClienteDto(
            Nombre: string.Empty,
            Email: "sin.nombre@contoso.cr",
            Telefono: "+506 7000 0006",
            Direccion: "Limón"
        );

        var creado = servicio.Crear(dto);

        Assert.Equal(4, creado.Id);
        Assert.Equal(string.Empty, creado.Nombre);
    }

    [Fact]
    public void Actualizar_ConClienteExistente_DebeActualizarDatos()
    {
        var servicio = new ClienteServicio();
        var dto = new ClienteDto(
            Nombre: "Juan Rodríguez Actualizado",
            Email: "juan.actualizado@contoso.cr",
            Telefono: "+506 7111 1111",
            Direccion: "Alajuela Centro"
        );

        var actualizado = servicio.Actualizar(2, dto);
        var consultado = servicio.ObtenerPorId(2);

        Assert.NotNull(actualizado);
        Assert.NotNull(consultado);
        Assert.Equal("Juan Rodríguez Actualizado", consultado!.Nombre);
        Assert.Equal("juan.actualizado@contoso.cr", consultado.Email);
        Assert.Equal("+506 7111 1111", consultado.Telefono);
        Assert.Equal("Alajuela Centro", consultado.Direccion);
    }

    [Fact]
    public void Actualizar_ConClienteNoExistente_DebeRetornarNull()
    {
        var servicio = new ClienteServicio();
        var dto = new ClienteDto(
            Nombre: "No Existe",
            Email: "no.existe@contoso.cr",
            Telefono: "+506 7222 2222",
            Direccion: "Desconocida"
        );

        var actualizado = servicio.Actualizar(999, dto);

        Assert.Null(actualizado);
    }

    [Fact]
    public void Eliminar_ConClienteExistente_DebeRetornarTrueYEliminarCliente()
    {
        var servicio = new ClienteServicio();

        var eliminado = servicio.Eliminar(1);
        var cliente = servicio.ObtenerPorId(1);
        var clientes = servicio.ObtenerTodos().ToList();

        Assert.True(eliminado);
        Assert.Null(cliente);
        Assert.Equal(2, clientes.Count);
    }

    [Fact]
    public void Eliminar_ConClienteNoExistente_DebeRetornarFalse()
    {
        var servicio = new ClienteServicio();

        var eliminado = servicio.Eliminar(999);
        var clientes = servicio.ObtenerTodos().ToList();

        Assert.False(eliminado);
        Assert.Equal(3, clientes.Count);
    }
}
