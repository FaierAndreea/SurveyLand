using IPL;
using IPL.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var apiCorsPolicy = "ApiCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: apiCorsPolicy,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:4001", "https://localhost:4001")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                      });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SurveyContext>(x => x.UseSqlite(connectionString),
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

var securityReq = new OpenApiSecurityRequirement()
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
    o.DefaultAuthenticateScheme = JwtBearerDefaults.
AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.
AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.
AuthenticationScheme;
}).AddJwtBearer(o => {
    o.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(apiCorsPolicy);

app.MapGet("/api/Surveys", async (ISurveyRepository repo) => { return await repo.GetSurveysAsync(); });
app.MapGet("/api/Surveys/{id:int}", async (int id, ISurveyRepository repo) => { return await repo.GetSurveyByIdAsync(id); });

app.MapGet("/api/Questions", async (ISurveyRepository repo) => { return await repo.GetListOfQuestionsAsync(); });

app.MapGet("/api/Answers", [Authorize] async (ISurveyRepository repo) => { return await repo.GetAllAnswersAsync(); });
app.MapPost("/api/Answers", async (List<Models.Answer> answers, ISurveyRepository repo) => { return await repo.AddAnswersAsync(answers); });

app.MapPost("/api/security/getToken",
[AllowAnonymous]
async (UserManager<IdentityUser> userMgr, User user) => {
    var identityUsr = await userMgr.FindByNameAsync(user.UserName);

    if (await userMgr.CheckPasswordAsync(identityUsr, user.Password)) {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var securityKey = new SymmetricSecurityKey
    (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey,
SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer: issuer,
            audience: audience,
            signingCredentials: credentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var stringToken = tokenHandler.WriteToken(token);
        return Results.Ok(stringToken);
    }
    else {
        return Results.Unauthorized();
    }
});
app.MapPost("/api/security/createUser",
[AllowAnonymous]
async (UserManager<IdentityUser> userMgr, User user) => {
    var identityUser = new IdentityUser() {
        UserName = user.UserName,
        Email = user.UserName + "@example.com"
    };

    var result = await userMgr.CreateAsync
(identityUser, user.Password);

    if (result.Succeeded) {
        return Results.Ok();
    }
    else {
        return Results.BadRequest();
    }
});

app.UseAuthentication();
app.UseAuthorization();

app.Run();

public class User {
    public string UserName { get; set; }
    public string Password { get; set; }
}