using IPL;
using IPL.Identity;
using IPL.Repos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var apiCorsPolicy = "ApiCorsPolicy";

// Add services to the container.

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

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var identityString = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<SurveyContext>(x => x.UseSqlite(connectionString),
        ServiceLifetime.Transient);
builder.Services.AddDbContext<AppIdentityContext>(x => x.UseSqlite(identityString));

builder.Services.AddScoped<SurveyRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(apiCorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
