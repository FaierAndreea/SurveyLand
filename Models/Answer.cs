using System.ComponentModel.DataAnnotations;

namespace Models;
public class Answer
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    [Range(1,2, ErrorMessage = "Must choose an option")]
    public int Option { get; set; }
}