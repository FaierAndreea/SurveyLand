using Microsoft.EntityFrameworkCore;
using Models;

namespace IPL;

public class SurveyContext : DbContext, ISurveyContext
{
    public SurveyContext() { }
    public SurveyContext(DbContextOptions<SurveyContext> options) : base(options)
    {
    }

    public DbSet<Survey> Surveys { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        SeedData(builder);
    }
    private static void SeedData(ModelBuilder builder)
    {
        var surveys = new Survey[] {
            new Survey { Id = 1, Title = "Brands", Description = "What do you think about these brands that compete? Tell us your preferences"},
            new Survey { Id = 2, Title = "Relationship", Description = "Try answering these questions regarding relationships"}
        };

        var questions = new Question[] {
            new Question { Id = 1, SurveyId = 1, Statement = "Which one is better?", Option1 = "Coca-Cola", Option2 = "	Pepsi"},
            new Question { Id = 2, SurveyId = 1, Statement = "Which one would you get?", Option1 = "Nike", Option2 = "Reebok"},
            new Question { Id = 3, SurveyId = 1, Statement = "Which do you think is best?", Option1 = "Apple", Option2 = "Samsung"},
            new Question { Id = 4, SurveyId = 2, Statement = "Which do you prefer?", Option1 = "Small and intimate wedding", Option2 = "Large wedding"}
        };

        builder.Entity<Survey>().HasData(surveys);
        builder.Entity<Question>().HasData(questions);
    }
    public Task<int> SaveChangesAsync() => base.SaveChangesAsync();
}