namespace Models.Interfaces;
public interface ISurveyRepository
{
    Task<List<Question>> GetListOfQuestionsAsync();
    Task<List<Survey>> GetSurveysAsync();
    Task<Survey> GetSurveyByIdAsync(int surveyId);
    Task<List<Answer>> GetAllAnswersAsync();
    Task<Answer> GetAnswerByIdAsync(int answerId);
    Task<List<Answer>> AddAnswersAsync(List<Answer> answers);
}
