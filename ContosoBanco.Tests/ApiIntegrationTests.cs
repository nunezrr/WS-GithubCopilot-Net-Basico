using System.Net;
using System.Net.Http.Json;
using ContosoBanco.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ContosoBanco.Tests;

public class ApiIntegrationTests
{
    [Fact]
    public async Task Clientes_ListarDebeRetornarOkYContenido()
    {
        using var fabrica = new WebApplicationFactory<Program>();
        using var clienteHttp = fabrica.CreateClient();

        var respuesta = await clienteHttp.GetAsync("/api/clientes");
        var clientes = await respuesta.Content.ReadFromJsonAsync<List<Cliente>>();

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        Assert.NotNull(clientes);
        Assert.NotEmpty(clientes!);
    }

    [Fact]
    public async Task Clientes_ConsultarPorIdValidoDebeRetornarOk()
    {
        using var fabrica = new WebApplicationFactory<Program>();
        using var clienteHttp = fabrica.CreateClient();

        var respuesta = await clienteHttp.GetAsync("/api/clientes/1");
        var cliente = await respuesta.Content.ReadFromJsonAsync<Cliente>();

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        Assert.NotNull(cliente);
        Assert.Equal(1, cliente!.Id);
    }

    [Fact]
    public async Task Clientes_ConsultarPorIdInexistenteDebeRetornarNotFound()
    {
        using var fabrica = new WebApplicationFactory<Program>();
        using var clienteHttp = fabrica.CreateClient();

        var respuesta = await clienteHttp.GetAsync("/api/clientes/99999");

        Assert.Equal(HttpStatusCode.NotFound, respuesta.StatusCode);
    }

    [Fact]
    public async Task Clientes_CrearDebeRetornarCreatedYEntidadCreada()
    {
        using var fabrica = new WebApplicationFactory<Program>();
        using var clienteHttp = fabrica.CreateClient();

        var nuevoCliente = new ClienteDto(
            Nombre: "Carlos Mora",
            Email: "carlos.mora@contoso.cr",
            Telefono: "+506 7000 0100",
            Direccion: "San Pedro"
        );

        var respuesta = await clienteHttp.PostAsJsonAsync("/api/clientes", nuevoCliente);
        var creado = await respuesta.Content.ReadFromJsonAsync<Cliente>();

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);
        Assert.NotNull(creado);
        Assert.True(creado!.Id > 0);
        Assert.Equal("Carlos Mora", creado.Nombre);
    }

    [Fact]
    public async Task Clientes_ActualizarDebeRetornarOkYDatosActualizados()
    {
        using var fabrica = new WebApplicationFactory<Program>();
        using var clienteHttp = fabrica.CreateClient();

        var cambios = new ClienteDto(
            Nombre: "Ana Pérez Editada",
            Email: "ana.editada@contoso.cr",
            Telefono: "+506 7000 9999",
            Direccion: "Escazu"
        );

        var respuestaActualizacion = await clienteHttp.PutAsJsonAsync("/api/clientes/1", cambios);
        var actualizado = await respuestaActualizacion.Content.ReadFromJsonAsync<Cliente>();

        Assert.Equal(HttpStatusCode.OK, respuestaActualizacion.StatusCode);
        Assert.NotNull(actualizado);
        Assert.Equal("Ana Pérez Editada", actualizado!.Nombre);

        var respuestaConsulta = await clienteHttp.GetAsync("/api/clientes/1");
        var consultado = await respuestaConsulta.Content.ReadFromJsonAsync<Cliente>();

        Assert.Equal(HttpStatusCode.OK, respuestaConsulta.StatusCode);
        Assert.NotNull(consultado);
        Assert.Equal("ana.editada@contoso.cr", consultado!.Email);
    }

    [Fact]
    public async Task Clientes_ActualizarNoExistenteDebeRetornarNotFound()
    {
        using var fabrica = new WebApplicationFactory<Program>();
        using var clienteHttp = fabrica.CreateClient();

        var cambios = new ClienteDto(
            Nombre: "Cliente Fantasma",
            Email: "fantasma@contoso.cr",
            Telefono: "+506 7000 8888",
            Direccion: "Ninguna"
        );

        var respuesta = await clienteHttp.PutAsJsonAsync("/api/clientes/99999", cambios);

        Assert.Equal(HttpStatusCode.NotFound, respuesta.StatusCode);
    }

    [Fact]
    public async Task Clientes_EliminarDebeRetornarNoContentYLuegoNotFound()
    {
        using var fabrica = new WebApplicationFactory<Program>();
        using var clienteHttp = fabrica.CreateClient();

        var respuestaEliminacion = await clienteHttp.DeleteAsync("/api/clientes/1");
        var respuestaConsulta = await clienteHttp.GetAsync("/api/clientes/1");

        Assert.Equal(HttpStatusCode.NoContent, respuestaEliminacion.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, respuestaConsulta.StatusCode);
    }

    [Fact]
    public async Task Clientes_EliminarNoExistenteDebeRetornarNotFound()
    {
        using var fabrica = new WebApplicationFactory<Program>();
        using var clienteHttp = fabrica.CreateClient();

        var respuesta = await clienteHttp.DeleteAsync("/api/clientes/99999");

        Assert.Equal(HttpStatusCode.NotFound, respuesta.StatusCode);
    }

    [Fact]
    public async Task Cuentas_ListarYConsultarDebeRetornarOk()
    {
        using var fabrica = new WebApplicationFactory<Program>();
        using var clienteHttp = fabrica.CreateClient();

        var respuestaListado = await clienteHttp.GetAsync("/api/cuentas");
        var cuentas = await respuestaListado.Content.ReadFromJsonAsync<List<Cuenta>>();

        Assert.Equal(HttpStatusCode.OK, respuestaListado.StatusCode);
        Assert.NotNull(cuentas);
        Assert.NotEmpty(cuentas!);

        var idCuenta = cuentas![0].Id;
        var respuestaConsulta = await clienteHttp.GetAsync($"/api/cuentas/{idCuenta}");
        var cuenta = await respuestaConsulta.Content.ReadFromJsonAsync<Cuenta>();

        Assert.Equal(HttpStatusCode.OK, respuestaConsulta.StatusCode);
        Assert.NotNull(cuenta);
        Assert.Equal(idCuenta, cuenta!.Id);
    }

    [Fact]
    public async Task Cuentas_CrearActualizarEliminarDebeResponderCodigosCorrectos()
    {
        using var fabrica = new WebApplicationFactory<Program>();
        using var clienteHttp = fabrica.CreateClient();

        var nuevaCuenta = new CuentaDto(
            ClienteId: 1,
            NumeroCuenta: "001-0099-000099",
            TipoCuenta: "Ahorro",
            Saldo: 1250.50m,
            Estado: "Activa"
        );

        var respuestaCreacion = await clienteHttp.PostAsJsonAsync("/api/cuentas", nuevaCuenta);
        var creada = await respuestaCreacion.Content.ReadFromJsonAsync<Cuenta>();

        Assert.Equal(HttpStatusCode.Created, respuestaCreacion.StatusCode);
        Assert.NotNull(creada);

        var cambios = new CuentaDto(
            ClienteId: creada!.ClienteId,
            NumeroCuenta: creada.NumeroCuenta,
            TipoCuenta: "Corriente",
            Saldo: 2000m,
            Estado: "Bloqueada"
        );

        var respuestaActualizacion = await clienteHttp.PutAsJsonAsync($"/api/cuentas/{creada.Id}", cambios);
        var actualizada = await respuestaActualizacion.Content.ReadFromJsonAsync<Cuenta>();

        Assert.Equal(HttpStatusCode.OK, respuestaActualizacion.StatusCode);
        Assert.NotNull(actualizada);
        Assert.Equal("Corriente", actualizada!.TipoCuenta);

        var respuestaEliminacion = await clienteHttp.DeleteAsync($"/api/cuentas/{creada.Id}");
        Assert.Equal(HttpStatusCode.NoContent, respuestaEliminacion.StatusCode);

        var respuestaConsultaEliminada = await clienteHttp.GetAsync($"/api/cuentas/{creada.Id}");
        Assert.Equal(HttpStatusCode.NotFound, respuestaConsultaEliminada.StatusCode);
    }

    [Fact]
    public async Task Cuentas_CrearConSaldoNegativoDebeRetornarBadRequest()
    {
        using var fabrica = new WebApplicationFactory<Program>();
        using var clienteHttp = fabrica.CreateClient();

        var cuentaInvalida = new CuentaDto(
            ClienteId: 1,
            NumeroCuenta: "001-0100-000100",
            TipoCuenta: "Ahorro",
            Saldo: -1m,
            Estado: "Activa"
        );

        var respuesta = await clienteHttp.PostAsJsonAsync("/api/cuentas", cuentaInvalida);

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    [Fact]
    public async Task Cuentas_ConsultarNoExistenteDebeRetornarNotFound()
    {
        using var fabrica = new WebApplicationFactory<Program>();
        using var clienteHttp = fabrica.CreateClient();

        var respuesta = await clienteHttp.GetAsync("/api/cuentas/99999");

        Assert.Equal(HttpStatusCode.NotFound, respuesta.StatusCode);
    }
}
