using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        [Range(1,2, ErrorMessage = "Must choose an option")]
        public int Option { get; set; }
    }
}