using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ServiCuentas.Application.DTOs;
using ServiCuentas.Application.Services;
using ServiCuentas.Application.Services.FuncionServices;
using ServiCuentas.Application.Validators;
using ServiCuentas.Data;
using ServiCuentas.Infraestructure.Repository;
using ServiCuentas.Model;
using ServiCuentas.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: TimeSpan.FromSeconds(30),
        errorNumbersToAdd: null
        );
    }
    ));

// ------------

// Cargar configuración desde appsettings.json
builder.Services.Configure<PaginationSettings>(builder.Configuration.GetSection("Pagination"));

// Cargar configuracion de automapper
builder.Services.AddAutoMapper(typeof(Program));

// Cargar Validadores
builder.Services.AddScoped<FechaProcesoRepository>();
builder.Services.AddScoped<CuentaValidator>();
builder.Services.AddScoped<FuncionRequestValidator>();
builder.Services.AddScoped<FuncionRequestAsientoValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<CuentaValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<FuncionRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<FuncionRequestAsientoValidator>();

builder.Services.AddScoped<IValidator<CuentaDTO>, CuentaValidator>();
builder.Services.AddScoped<IValidator<FuncionRequestDTO>, FuncionRequestValidator>();
builder.Services.AddScoped<IValidator<FuncionRequestAsientoDTO>, FuncionRequestAsientoValidator>();

// Cargar configuracion de dependencias
// Repository

builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();
builder.Services.AddScoped<IFechaProcesoRepository, FechaProcesoRepository>();
builder.Services.AddScoped<IMovimientoRepository, MovimientoRepository>();
builder.Services.AddScoped<INumeracionRepository, NumeracionRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services

builder.Services.AddScoped<ICuentaService, CuentaService>();
builder.Services.AddScoped<IFechaProcesoService, FechaProcesoService>();
builder.Services.AddScoped<IMovimientoService, MovimientoService>();


builder.Services.AddScoped<IFuncionCredito, FuncionCreditoService>();
builder.Services.AddScoped<IFuncionDebito, FuncionDebitoService>();

// ------------

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "ServiCuenta - Modulo de cuentas al saldo",
        Description = "APIs para implementar modulo de cuentas al saldo: ABM de cuentas, funciones de debitos y creditos, movimientos y consultas varias",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Cesar Romano (tchami007)",
            Email = "cesarromano2007@gmail.com",
            Url = new Uri("https://github.com/tchami007")
        }
    });

    // Opcional: Agregar comentarios de código
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        // Hace que solo sea documentacion
        //options.SupportedSubmitMethods(Array.Empty<Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod>());
        // Hace que Swagger esté disponible en la raíz.
        //options.RoutePrefix = ""; 
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
