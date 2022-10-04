namespace APITests;
public class BusinessLogicEndpointsTests {
    [Fact]
    public async Task GetSurveysAsyncReturnsAllFromDatabase() {
        // Arrange
        var mock = new Mock<ISurveyRepository>();
        var surveys = new List<Survey> { 
            new Survey {
                Id = 1,
                Title = "Title",
                Description = "Description",
                Picture = "Picture"
            },
            new Survey {
                Id = 2,
                Title = "Title",
                Description = "Description",
                Picture = "Picture"
            }
        };

        mock.Setup(m => m.GetSurveysAsync()).ReturnsAsync(surveys);

        // Act
        var result = await BusinessLogicEndpoints.GetSurveysAsync(mock.Object);

        //Assert
        Assert.NotEmpty(result);
        Assert.Collection(result, survey1 => {
            Assert.Equal(1, survey1.Id);
            Assert.Equal("Title", survey1.Title);
            Assert.Equal("Description", survey1.Description);
            Assert.Equal("Picture", survey1.Picture);
        }, survey2 => {
            Assert.Equal(2, survey2.Id);
            Assert.Equal("Title", survey2.Title);
            Assert.Equal("Description", survey2.Description);
            Assert.Equal("Picture", survey2.Picture);
        });
    }

    [Fact]
    public async Task GetSurveyByIdAsyncReturnsFromDatabase() {
        // Arrange
        var mock = new Mock<ISurveyRepository>();
        var survey = new Survey {
            Id = 1,
            Title = "Title",
            Description = "Description",
            Picture = "Picture"
        };

        mock.Setup(m => m.GetSurveyByIdAsync(1)).ReturnsAsync(survey);

        // Act
        var result = await BusinessLogicEndpoints.GetSurveyByIdAsync(1, mock.Object);

        //Assert
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task AddAnswersAsyncAddsListOfAnswersInDatabase() {
        // Arrange
        var mock = new Mock<ISurveyRepository>();

        var answers = new List<Answer>();
        var answersToAdd = new List<Answer> {
            new Answer {
                Id = 1,
                QuestionId = 1,
                Option = 1
            }
        };
        mock.Setup(m => m.AddAnswersAsync(answersToAdd))
            .Callback<List<Answer>>(a => answers.AddRange(a))
            .ReturnsAsync(answers);

        // Act
        var result = await BusinessLogicEndpoints.AddAnswersAsync(answersToAdd, mock.Object);

        // Assert
        Assert.NotEmpty(result);
        Assert.Collection(result, a => {
            Assert.Equal(1, a.Id);
            Assert.Equal(1, a.QuestionId);
            Assert.Equal(1, a.Option);
        });
    }
}
