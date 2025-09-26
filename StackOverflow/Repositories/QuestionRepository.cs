using System.Collections.Generic;
using StackOverflow.Models;

namespace StackOverflow.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly string _connectionString;

        public QuestionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public List<Question> GetAllQuestions()
        {
            return new List<Question>();
        }
    }
}