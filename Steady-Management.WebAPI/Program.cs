using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Steady_Management.Business;
using Steady_Management.Data;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣  JWT configuration
var jwtCfg = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtCfg["Key"]!);

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Steady-Management.WebAPI", Version = "v1" });

    // 🔐 Configuración de seguridad JWT para Swagger
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Ingresa el token JWT con 'Bearer ' al inicio.",

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

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtCfg["Issuer"],
            ValidAudience = jwtCfg["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddScoped<SteadyManagement.Business.DepartmentBusiness>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connStr = config.GetConnectionString("DefaultConnection");
    return new SteadyManagement.Business.DepartmentBusiness(connStr);
});

builder.Services.AddScoped<Steady_Management.DataAccess.EmployeeData>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new Steady_Management.DataAccess.EmployeeData(config);
});


builder.Services.AddScoped<Steady_Management.Business.EmployeeBusiness>(sp =>
{
    var employeeData = sp.GetRequiredService<Steady_Management.DataAccess.EmployeeData>();
    return new Steady_Management.Business.EmployeeBusiness(employeeData);
});

builder.Services.AddScoped<Steady_Management.Business.RoleBusiness>();
builder.Services.AddScoped<Steady_Management.DataAccess.RoleData>();

builder.Services.AddScoped<ProductData>(sp =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var connStr = cfg.GetConnectionString("DefaultConnection");
    return new ProductData(connStr!);
});

builder.Services.AddScoped<ProductData>(sp =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var connStr = cfg.GetConnectionString("DefaultConnection");
    return new ProductData(connStr!);
});

builder.Services.AddScoped<ProductBusiness>();

builder.Services.AddScoped<CategoryData>(sp =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var connStr = cfg.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connStr))
        throw new InvalidOperationException(
            "Falta la connection string de appsettings.json");
    return new CategoryData(connStr);
});

builder.Services.AddScoped<CategoryBusiness>();

// Add services to the container.

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();
*/

app.MapControllers();

app.Run();
