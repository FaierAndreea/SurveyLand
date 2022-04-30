using IPL.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase {
        private readonly SurveyRepository _repository;

        public AnswersController(SurveyRepository repository) {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Answer>>> GetAllAsync() {
            return Ok(await _repository.GetAllAnswersAsync());
        }
        [HttpPost]
        public async Task<ActionResult> AddOneAnswerAsync(List<Answer> answers) {
            var a = await _repository.AddAnswersAsync(answers);
            return Ok(a);
        }
    }
}
