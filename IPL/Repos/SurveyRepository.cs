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
            return await _context.Surveys.ToListAsync();
        }
        public async Task<Survey> GetSurveyByIdAsync(int surveyId) {
            return await _context.Surveys.Where(x => x.Id == surveyId).Include(x => x.Questions).FirstOrDefaultAsync();
        }
        public async Task<List<Answer>> GetAllAnswersAsync() {
            return await _context.Answers.ToListAsync();
        }
        public async Task<Answer> GetAnswerByIdAsync(int answerId) {
            return await _context.Answers.Where(x => x.Id == answerId).FirstOrDefaultAsync();
        }

        public async Task<List<Answer>> AddAnswersAsync(List<Answer> answers) {
            foreach(Answer answer in answers) {
                await _context.Answers.AddAsync(answer);
            }
            await _context.SaveChangesAsync();
            return answers;
        }

        //public async Task<Answer> AddOneAnswer(Answer a) {
        //    await _context.Answers.AddAsync(a);
        //    await _context.SaveChangesAsync();
        //    return a;
        //}
    }
}
