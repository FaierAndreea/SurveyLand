using IPL.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SurveysController : ControllerBase {
        private readonly SurveyRepository _repo;

        public SurveysController(SurveyRepository repository) {
            _repo = repository;
        }
        [HttpGet]
        public async Task<ActionResult<List<Survey>>> GetSurveyListAsync() {
            return Ok(await _repo.GetSurveysAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Survey>> GetSurveyByIdAsync(int id) {
            return Ok(await _repo.GetSurveyByIdAsync(id));
        }
    }
}
