using Microsoft.Extensions.DependencyInjection;
using Steady_Management.Business;
using Steady_Management.Data;

var builder = WebApplication.CreateBuilder(args);


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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
