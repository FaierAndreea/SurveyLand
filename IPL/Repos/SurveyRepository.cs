using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPL.Repos
{
    public class SurveyRepository
    {
        private readonly SurveyContext _context;

        public SurveyRepository(SurveyContext context)
        {
            _context = context;
        }
        public async Task<List<Question>> GetListOfQuestionsAsync() { 
            return await _context.Questions.ToListAsync();
        }
        public async Task<List<Survey>> GetSurveysAsync() {
            return await _context.Surveys.Include(x => x.Questions).ToListAsync();
        }
    }
}
