// RoomBookingAPI/Program.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RoomBookingAPI.Model;
using RoomBookingAPI.Data;
using RoomBookingAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// =========================
// 1. Configure Services
// =========================

// 1.1 Add Controllers
builder.Services.AddControllers();

// 1.2 Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 1.3 Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("Key"));

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
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
        ValidAudience = jwtSettings.GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// 1.4 Register Services for Dependency Injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// 1.5 Add Swagger for API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1.6 Configure CORS to allow any origin, method, and header
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// =========================
// 2. Build the Application
// =========================

var app = builder.Build();

// =========================
// 3. Configure Middleware Pipeline
// =========================

// 3.1 Configure Swagger in Development Environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 3.2 Use CORS (Allowing All Origins)
app.UseCors("AllowAll");

// 3.3 Enforce HTTPS Redirection
app.UseHttpsRedirection();

// 3.4 Enable Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// 3.5 Map Controller Routes
app.MapControllers();

// =========================
// 4. Run the Application
// =========================

app.Run();
