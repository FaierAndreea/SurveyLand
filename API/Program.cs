using IPL;
using IPL.Identity;
using IPL.Repos;
using Microsoft.EntityFrameworkCore;
using Models.Interfaces;

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
var identityString = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<SurveyContext>(x => x.UseSqlite(connectionString),
        ServiceLifetime.Transient);
builder.Services.AddDbContext<AppIdentityContext>(x => x.UseSqlite(identityString));

builder.Services.AddScoped<ISurveyRepository, SurveyRepository>();

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

app.MapGet("/api/Surveys", async (ISurveyRepository repo) => { return await repo.GetSurveysAsync(); });
app.MapGet("/api/Surveys/{id:int}", async (int id, ISurveyRepository repo) => { return await repo.GetSurveyByIdAsync(id); });

app.MapGet("/api/Questions", async (ISurveyRepository repo) => { return await repo.GetListOfQuestionsAsync(); });

app.MapGet("/api/Answers", async (ISurveyRepository repo) => { return await repo.GetAllAnswersAsync(); });
app.MapPost("/api/Answers", async (List<Models.Answer> answers, ISurveyRepository repo) => { return await repo.AddAnswersAsync(answers); });

app.Run();
