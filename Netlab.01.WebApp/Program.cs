using Netlab.Business.Services;
using Netlab.Domain.DTOs;
using Netlab.Domain.Interfaces;
using Netlab.Helper;
using Netlab.Infrastructure.Database;
using Netlab.Infrastructure.Repositories;
using Netlab.WebApp.Filters;
using Netlab.WebApp.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// JWT config desde appsettings
var jwtConfig = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]!);

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig["Issuer"],
        ValidAudience = jwtConfig["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            // Esto previene que se escriba la respuesta por defecto
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"message\": \"Token inválido o no proporcionado.\"}");
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"message\": \"No tienes permiso para acceder a este recurso.\"}");
        }
    };
});



// Servicios
builder.Services.AddAuthorization();
builder.Services.AddScoped<ILogAccesoRepository, LogAccesoRepository>();
builder.Services.AddSingleton<IDatabaseFactory, DatabaseFactory>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IConsultaResultadosArbovirosisRepository, ConsultaResultadosArbovirosisRepository>();
builder.Services.AddScoped<IConsultaResultadosArbovirosisService, ConsultaResultadosArbovirosisService>();
builder.Services.AddScoped<IRegistrarNotiWebRepository, RegistrarNotiWebRepository>();
builder.Services.AddScoped<IRegistrarNotiWebService, RegistrarNotiWebService>();

// Controllers
builder.Services.AddControllers(options =>
{ 
    options.Filters.Add<ApiResponseFilter>(); // Agregar filtro de respuesta
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var listaErrores = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .SelectMany(kvp => kvp.Value.Errors.Select(e => e.ErrorMessage))
            .ToList();

        var response = new ApiResponse<object>
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Success = false,
            Message = "Errores de validación",
            Errors = listaErrores,
            Data = null
        };

        return new BadRequestObjectResult(response);
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Netlab API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Ingrese su token JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>(); // Middleware de manejo de excepciones
app.UseAuthentication(); // Primero auth
app.UseAuthorization();  // Luego authorization
app.MapControllers();
//Middleware - debe ir despues de UseAuthentication y UseAuthorization


// Test básico de ruta protegida
app.MapGet("/secure", () => "Acceso autorizado")
   .RequireAuthorization();

app.Run();
