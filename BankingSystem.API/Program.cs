using BankingSystem.API.Data;
using BankingSystem.API.Models;
using BankingSystem.API.Services;
using BankingSystem.API.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Email Settings
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<EmailService>();

// 2. DB Connection
builder.Services.AddDbContext<BankingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<BankingDbContext>()
    .AddDefaultTokenProviders();

// 4. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
      policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// 5. JWT Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
}).AddJwtBearer("JwtBearer", opts =>
{
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 6. Swagger + JWT
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Banking API", Version = "v1" });
    opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT auth (Bearer {token})",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    opts.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Use middleware in correct order:
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();            // ← Must come before CORS & Auth
app.UseCors("AllowAll");     // ← Handle CORS preflight

app.UseHttpsRedirection();
app.UseAuthentication();     // ← Validate JWT
app.UseAuthorization();      // ← Authorize roles

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Seed roles & admin
using (var scope = app.Services.CreateScope())
{
    var roles = new[] { "Admin", "Customer" };
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    foreach (var r in roles)
        if (!await roleMgr.RoleExistsAsync(r))
            await roleMgr.CreateAsync(new IdentityRole(r));

    var adminEmail = "admin@bank.com";
    var adminPwd = "Admin@123";
    if (await userMgr.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new ApplicationUser
        {
            Email = adminEmail,
            UserName = adminEmail,
            FullName = "System Admin"
        };
        if ((await userMgr.CreateAsync(admin, adminPwd)).Succeeded)
            await userMgr.AddToRoleAsync(admin, "Admin");
    }
}

app.Run();





