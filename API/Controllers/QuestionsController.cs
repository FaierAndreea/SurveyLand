using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IPL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : Controller
    {
        private readonly SurveyContext _context;
        public QuestionsController(SurveyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Question>> GetQurstion() {
            var question = _context.Questions.ToList();
            return Ok(question);
        } 
    }
}