using ContosoBanco.Models;
using ContosoBanco.Services;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ClienteServicio>();
builder.Services.AddSingleton<CuentaServicio>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Contoso Banco",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Contoso Banco v1");
});

app.MapGet("/", () => Results.Redirect("/index.html"));

var clientes = app.MapGroup("/api/clientes").WithTags("Clientes");

clientes.MapGet("/", (ContosoBanco.Services.ClienteServicio servicio) => Results.Ok(servicio.ObtenerTodos()))
    .WithName("ObtenerClientes")
    .WithOpenApi()
    .Produces<IEnumerable<ContosoBanco.Models.Cliente>>(StatusCodes.Status200OK);

clientes.MapGet("/{id}", (int id, ContosoBanco.Services.ClienteServicio servicio) =>
{
    var cliente = servicio.ObtenerPorId(id);
    return cliente is not null ? Results.Ok(cliente) : Results.NotFound();
})
    .WithName("ObtenerClientePorId")
    .WithOpenApi()
    .Produces<ContosoBanco.Models.Cliente>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

clientes.MapPost("/", (ContosoBanco.Models.ClienteDto clienteDto, ContosoBanco.Services.ClienteServicio servicio) =>
{
    var cliente = servicio.Crear(clienteDto);
    return Results.Created($"/api/clientes/{cliente.Id}", cliente);
})
    .WithName("CrearCliente")
    .WithOpenApi()
    .Produces<ContosoBanco.Models.Cliente>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest);

clientes.MapPut("/{id}", (int id, ContosoBanco.Models.ClienteDto clienteDto, ContosoBanco.Services.ClienteServicio servicio) =>
{
    var cliente = servicio.Actualizar(id, clienteDto);
    return cliente is not null ? Results.Ok(cliente) : Results.NotFound();
})
    .WithName("ActualizarCliente")
    .WithOpenApi()
    .Produces<ContosoBanco.Models.Cliente>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status400BadRequest);

clientes.MapDelete("/{id}", (int id, ContosoBanco.Services.ClienteServicio servicio) =>
{
    return servicio.Eliminar(id) ? Results.NoContent() : Results.NotFound();
})
    .WithName("EliminarCliente")
    .WithOpenApi()
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

var cuentas = app.MapGroup("/api/cuentas").WithTags("Cuentas");

cuentas.MapGet("/", (ContosoBanco.Services.CuentaServicio servicio) => Results.Ok(servicio.ObtenerTodos()))
    .WithName("ObtenerCuentas")
    .WithOpenApi()
    .Produces<IEnumerable<ContosoBanco.Models.Cuenta>>(StatusCodes.Status200OK);

cuentas.MapGet("/{id}", (int id, ContosoBanco.Services.CuentaServicio servicio) =>
{
    var cuenta = servicio.ObtenerPorId(id);
    return cuenta is not null ? Results.Ok(cuenta) : Results.NotFound();
})
    .WithName("ObtenerCuentaPorId")
    .WithOpenApi()
    .Produces<ContosoBanco.Models.Cuenta>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

cuentas.MapPost("/", (ContosoBanco.Models.CuentaDto cuentaDto, ContosoBanco.Services.CuentaServicio servicio) =>
{
    try
    {
        var cuenta = servicio.Crear(cuentaDto);
        return Results.Created($"/api/cuentas/{cuenta.Id}", cuenta);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { Error = ex.Message });
    }
})
    .WithName("CrearCuenta")
    .WithOpenApi()
    .Produces<ContosoBanco.Models.Cuenta>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest);

cuentas.MapPut("/{id}", (int id, ContosoBanco.Models.CuentaDto cuentaDto, ContosoBanco.Services.CuentaServicio servicio) =>
{
    try
    {
        var cuenta = servicio.Actualizar(id, cuentaDto);
        return cuenta is not null ? Results.Ok(cuenta) : Results.NotFound();
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { Error = ex.Message });
    }
})
    .WithName("ActualizarCuenta")
    .WithOpenApi()
    .Produces<ContosoBanco.Models.Cuenta>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound);

cuentas.MapDelete("/{id}", (int id, ContosoBanco.Services.CuentaServicio servicio) =>
{
    return servicio.Eliminar(id) ? Results.NoContent() : Results.NotFound();
})
    .WithName("EliminarCuenta")
    .WithOpenApi()
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

app.Run();
