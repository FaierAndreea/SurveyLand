using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IPL;
using IPL.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : Controller
    {
        private readonly SurveyRepository _repo;
        public QuestionsController(SurveyRepository repository)
        {
            _repo = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Question>>> GetQurstions() {
            return Ok(await _repo.GetListOfQuestionsAsync());
        } 
    }
}