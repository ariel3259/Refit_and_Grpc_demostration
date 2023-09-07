using ApiRefitConsumer.Utils;
using ApiRefitConsumer.Utils.Interfaces;
using Microsoft.OpenApi.Models;
using SdkClient;
using Refit;
using Microsoft.AspNetCore.DataProtection;
using ApiRefitConsumer.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IAuthTokenStorage, AuthTokenStorage>();
builder.Services.AddTransient<AuthDelegate>();

builder.Services.AddRefitClient<IApiRefit>()
    .ConfigureHttpClient(options =>
    {
        options.BaseAddress = new Uri("http://localhost:5093");
    })
    .AddHttpMessageHandler<AuthDelegate>();

builder.Services.AddSwaggerGen((option) =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<TokenMiddleware>();
app.MapControllers();

app.Run();
