using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Models;

namespace IPL
{
    public class SurveyContext : DbContext 
    {
        public SurveyContext(){}
        public SurveyContext(DbContextOptions<SurveyContext> options) : base(options)
        {
        }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder opitionsBuilder) {
        }

        // protected override void OnModelCreating(ModelBuilder modelBuilder){
        // }
    }
}