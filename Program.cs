using Microsoft.EntityFrameworkCore;
using StudioTattooManagement.Data;
using StudioTattooManagement.Interfaces.Irepositories;
using StudioTattooManagement.Interfaces.Iservices;
using StudioTattooManagement.Repositories;
using StudioTattooManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// Configura��es de servi�os
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inje��o de depend�ncias
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));

builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

// AutoMapper
//builder.Services.AddAutoMapper(typeof(MappingProfile));

// Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura��o do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
