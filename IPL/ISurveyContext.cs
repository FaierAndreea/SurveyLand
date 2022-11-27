using Microsoft.EntityFrameworkCore;
using Models;

public interface ISurveyContext {
    DbSet<Survey> Surveys { get; set; }
    DbSet<Question> Questions { get; set; }
    DbSet<Answer> Answers { get; set; }
    int SaveChanges();
    Task<int> SaveChangesAsync();
}