using Microsoft.EntityFrameworkCore;
using QuickKartDB.DataAccessLayer.Models;
using QuickKartDB.DataAccessLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using QuickKartServices;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json and environment-specific file
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsetting.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add Azure Key Vault secrets provider
var keyVaultName = "quickkartsecrets";
var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200", "https://antojude-quickkartdb.azurewebsites.net")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Configure DbContext with connection string from config (including Key Vault)
builder.Services.AddDbContext<QuickKartDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QuickKartDBConnectionString")));

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register your application services and repositories
builder.Services.AddScoped<TokenService>();
builder.Services.AddTransient<IQuickKartRepository, QuickKartRepository>();

var app = builder.Build();

// Use the CORS policy
app.UseCors("AllowSpecificOrigin");

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
