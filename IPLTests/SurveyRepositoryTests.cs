using IPL.Repos;
using IPLTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Models;

namespace IPLTests;

public class SurveyRepositoryTests
{
    [Fact]
    public async Task GetSurveyAsyncReturnsList() {
        var surveys = new Survey[] {
            new Survey {Id = 1, Title = "s1"},
            new Survey {Id = 2, Title = "s2"},
        };
        var dbSetMock = CreateDbSetMock(surveys.AsQueryable());
        var dbContextMock = new Mock<ISurveyContext>();
        dbContextMock.Setup(m => m.Surveys).Returns(dbSetMock.Object);

        var repo = new SurveyRepository(dbContextMock.Object);
        var result = await repo.GetSurveysAsync();

        Assert.NotNull(result);
        Assert.Equal(result.Count, surveys.Length);
    }

    [Fact]
    public async Task GetSurveyByIdAsyncReturnsSurvey() {
        var surveys = new Survey[] {
            new Survey {Id = 1, Title = "s1"},
            new Survey {Id = 2, Title = "s2"},
        };
        var id = 2;
        var dbSetMock = CreateDbSetMock(surveys.AsQueryable());
        var dbContextMock = new Mock<ISurveyContext>();
        dbContextMock.Setup(m => m.Surveys).Returns(dbSetMock.Object);

        var repo = new SurveyRepository(dbContextMock.Object);
        var result = await repo.GetSurveyByIdAsync(id);

        Assert.NotNull(result);
        Assert.Equal(result.Id, id);
        Assert.Equal(result.Title, surveys[1].Title);
    }
    [Fact]
    public async Task GetListOfQuestionsAsyncReturnsList() {
        var questions = new Question[] {
            new Question {Id = 1, SurveyId = 1, Statement = "q1"},
            new Question {Id = 2, SurveyId = 1, Statement = "q2"},
        };
        var dbSetMock = CreateDbSetMock(questions.AsQueryable());
        var dbContextMock = new Mock<ISurveyContext>();
        dbContextMock.Setup(m => m.Questions).Returns(dbSetMock.Object);

        var repo = new SurveyRepository(dbContextMock.Object);
        var result = await repo.GetListOfQuestionsAsync();

        Assert.NotNull(result);
        Assert.Equal(result.Count, questions.Length);
    }

    private static Mock<DbSet<T>> CreateDbSetMock<T>(IQueryable<T> items) where T : class
    {
        var dbSetMock = new Mock<DbSet<T>>();

        dbSetMock.As<IAsyncEnumerable<T>>()
            .Setup(x => x.GetAsyncEnumerator(default))
            .Returns(new TestAsyncEnumerator<T>(items.GetEnumerator()));
        dbSetMock.As<IQueryable<T>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<T>(items.Provider));
        dbSetMock.As<IQueryable<T>>()
            .Setup(m => m.Expression).Returns(items.Expression);
        dbSetMock.As<IQueryable<T>>()
            .Setup(m => m.ElementType).Returns(items.ElementType);
        dbSetMock.As<IQueryable<T>>()
            .Setup(m => m.GetEnumerator()).Returns(items.GetEnumerator());

        return dbSetMock;
    }
}