namespace Models;
public class Question
{
    public int Id { get; set; }
    public int SurveyId { get; set; }
    public string Statement { get; set; }
    public string Option1 { get; set; }
    public string Option2 { get; set; }
    public List<Answer> Answers { get; set; }
}