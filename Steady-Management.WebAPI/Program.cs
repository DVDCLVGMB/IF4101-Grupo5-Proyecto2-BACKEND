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
using Steady_Management.DataAccess;
using SteadyManagement.Business;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣  JWT configuration (COMENTADA)
var jwtCfg = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtCfg["Key"]!);

// 2️⃣  Filtro global de autorización (COMENTADO)
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// 3️⃣  Swagger + JWT (COMENTADO)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Steady-Management.WebAPI", Version = "v1" });
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

// 4️⃣  JWT Bearer setup (COMENTADO)
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

// 5️⃣  Registro de servicios / dependencias
builder.Services.AddScoped<DepartmentBusiness>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connStr = config.GetConnectionString("DefaultConnection");
    return new DepartmentBusiness(connStr!);
});

builder.Services.AddScoped<Steady_Management.DataAccess.EmployeeData>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new Steady_Management.DataAccess.EmployeeData(config);
});
builder.Services.AddScoped<Steady_Management.Business.EmployeeBusiness>();

builder.Services.AddScoped<Steady_Management.DataAccess.RoleData>();
builder.Services.AddScoped<Steady_Management.Business.RoleBusiness>();

// ProductData (versión 1)
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

builder.Services.AddScoped<InventoryData>(sp =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var connStr = cfg.GetConnectionString("DefaultConnection")
                  ?? throw new InvalidOperationException("Falta DefaultConnection");
    return new InventoryData(connStr);
});
builder.Services.AddScoped<InventoryBusiness>();

// 6️⃣  MVC / OpenAPI básico
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var app = builder.Build();

// 7️⃣  Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();    // COMENTADO si no usas
    app.UseSwagger();    // COMENTADO si no usas
    app.UseSwaggerUI();  // COMENTADO si no usas
}

app.UseHttpsRedirection();   // COMENTADO: HTTP puro
app.UseAuthentication();     // COMENTADO
app.UseAuthorization();      // COMENTADO

app.MapControllers();
app.Run();
