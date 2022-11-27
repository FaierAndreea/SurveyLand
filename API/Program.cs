using API.Endpoints;
using API.Extras;
using IPL;
using IPL.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var apiCorsPolicy = "ApiCorsPolicy";

builder.Services.AddCors(options => {
    options.AddPolicy(name: apiCorsPolicy,
                      builder => {
                          builder.WithOrigins("http://localhost:4001", "https://localhost:4001")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                      });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ISurveyContext,SurveyContext>(x => x.UseSqlite(connectionString),
        ServiceLifetime.Transient);
builder.Services.AddDbContext<AppIdentityDbContext>(x => x.UseSqlite(connectionString));

builder.Services.AddScoped<ISurveyRepository, SurveyRepository>();

var securityScheme = new OpenApiSecurityScheme() {
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JSON Web Token based security",
};

var securityReq = new OpenApiSecurityRequirement() {
    {
        new OpenApiSecurityScheme {
        Reference = new OpenApiReference {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
        },
        new string[] {}
    }
};

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => {
    o.AddSecurityDefinition("Bearer", securityScheme);
    o.AddSecurityRequirement(securityReq);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
builder.Services.AddAuthentication(o => {
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(o => {
        o.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.Position));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(apiCorsPolicy);

app.MapBusinessLogicEndpoints();
app.MapSecurityEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
