
using MediatR;
using Messaging.RabbitMQ.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManagement.Application.Commands;
using UserManagement.Domain.Repositories;
using UserManagement.Infrastructure.Data;
using UserManagement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Shto RabbitMQSettings në DI container
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));

// Krijo RabbitMQConnection
builder.Services.AddSingleton<RabbitMQConnection>(provider =>
{
    var settings = provider.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
    return new RabbitMQConnection(settings.HostName, "guest", "guest"); // Përdorni kredencialet e duhura
});

// Shto Swagger dhe konfigurimin e autorizimit me JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "AuthService API",
        Version = "v1"
    });

    // Shto përkufizimin për autorizimin JWT në Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Shto konfigurimin e DbContext
builder.Services.AddDbContext<UserManagmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Regjistro JWT token generator dhe shërbime të tjera 
builder.Services.AddScoped<IUserRepository, UserRepository>(); 

// Regjistro AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Regjistro MediatR 
builder.Services.AddMediatR(typeof(AddUserCommand).Assembly); 

// Konfigurimi i JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            ValidIssuer = "YourApp",
            ValidAudience = "YourApp",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService API V1");
        c.RoutePrefix = "swagger"; // e bën të hapet te /swagger/index.html
    });
}

app.UseHttpsRedirection();

// Aktivizoni Authentication dhe Authorization
app.UseAuthentication(); // Kjo është e nevojshme për të validuar tokenin JWT
app.UseAuthorization();

app.MapControllers();

app.Run();
