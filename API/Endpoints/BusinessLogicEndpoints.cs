using Models;
using Models.Interfaces;

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
    public static async Task<List<Answer>> AddAnswersAsync(List<Models.Answer> answers, ISurveyRepository repo) {
        return await repo.AddAnswersAsync(answers);
    } 
}