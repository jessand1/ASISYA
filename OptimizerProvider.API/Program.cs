using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProviderOptimizer.Application.Interfaces;
using ProviderOptimizer.Application.Optimization.Handlers;
using ProviderOptimizer.Application.Provider.Handlers;
using ProviderOptimizer.Domain.Interfaces;
using ProviderOptimizer.Infrastructure.Persistence;
using ProviderOptimizer.Infrastructure.Persistence.Repositories;
using ProviderOptimizer.Persistence;
using System.Text;
using ProviderOptimizer.Application;

var builder = WebApplication.CreateBuilder(args);

// ================= DataBase. ==================
var connectionString = builder.Configuration["ConnectionString"];

builder.Services.AddDbContext<ProviderOptimizerDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddAutoMapper(
    typeof(ProviderOptimizer.Application.AssemblyReference).Assembly
);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(OptimizeRequestCommandHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetAvailableProvidersQueryHandler).Assembly);
});


// ================= Controllers =================
builder.Services.AddControllers();

// ================= Swagger =====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================= Jwt =========================
var jwtConfig = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
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
});

// ================= Cors ========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// ================= Pipeline ====================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReact");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
