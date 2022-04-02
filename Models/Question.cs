using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Statement { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
    }
}