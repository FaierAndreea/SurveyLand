using Models;
using Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Endpoints;

public static class BusinessLogicEndpoints {
    public static void MapBusinessLogicEndpoints(this WebApplication app) {
        app.MapGet("/api/Surveys", GetSurveysAsync);
        app.MapGet("/api/Surveys/{id:int}", GetSurveyByIdAsync);

        app.MapGet("/api/Questions", GetQuestionsAsync);

        app.MapGet("/api/Answers", GetAnswersAsync).RequireAuthorization();
        app.MapPost("/api/Answers", AddAnswersAsync);
    }
    public static async Task<List<Survey>> GetSurveysAsync(ISurveyRepository repo) {
        return await repo.GetSurveysAsync();
    }
    public static async Task<Survey> GetSurveyByIdAsync(int id, ISurveyRepository repo) {
        return await repo.GetSurveyByIdAsync(id);
    }
    public static async Task<List<Question>> GetQuestionsAsync(ISurveyRepository repo) {
        return await repo.GetListOfQuestionsAsync();
    }
    public static async Task<List<Answer>> GetAnswersAsync(ISurveyRepository repo) {
        return await repo.GetAllAnswersAsync();
    }
    public static async Task<List<Answer>> AddAnswersAsync(List<Models.Answer> answers, ISurveyRepository repo, HttpContext context) {
        // add userEmail here
        var email = context.User.FindFirstValue(ClaimTypes.Email);
        foreach(var answer in answers) {
            answer.UserEmail = email;
        }
        return await repo.AddAnswersAsync(answers);
    } 
}