using MediatR;
using Messaging.RabbitMQ.Service;
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
using Messaging.RabitMQ.Interfaces;
using Messaging.RabitMQ.Service; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure RabbitMQSettings in DI container
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));

// Create RabbitMQ connection
builder.Services.AddSingleton<RabbitMQConnection>(provider =>
{
    var settings = provider.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
    return new RabbitMQConnection(settings.HostName, "guest", "guest"); // Use proper credentials
});

// Configure Swagger and JWT authorization
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "UsereManagment API",
        Version = "v1"
    });

    // Add security definition for JWT Authorization
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

// Configure DbContext
builder.Services.AddDbContext<UserManagmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register JWT token generator and other services
builder.Services.AddScoped<IUserRepository, UserRepository>(); 
builder.Services.AddSingleton<IMessagePublisher, RabbitMqService>();
builder.Services.AddHostedService<UserRegisteredConsumer>();


// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register MediatR
builder.Services.AddMediatR(typeof(AddUserCommand).Assembly);

// Configure JWT Authentication
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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UsereManagment API V1");
        c.RoutePrefix = "swagger"; // Open Swagger UI at /swagger/index.html
    });
}

app.UseHttpsRedirection();

// Enable Authentication and Authorization
app.UseAuthentication(); // Necessary to validate JWT token
app.UseAuthorization();

app.MapControllers();

app.Run();
