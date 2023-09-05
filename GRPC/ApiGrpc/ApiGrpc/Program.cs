using ApiGrpc.GrpcServices;
using ApiGrpc.Services;
using GrpcProduct;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProductsService>();

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGrpcService<ProductsServiceGrpc>();

app.MapControllers();

app.Run();
