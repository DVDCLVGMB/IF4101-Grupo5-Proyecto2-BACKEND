using Microsoft.Extensions.DependencyInjection;

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
