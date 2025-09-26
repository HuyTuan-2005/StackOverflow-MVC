using StackOverflow.ViewModels;
using System.Collections.Generic;
using StackOverflow.Repositories;

namespace StackOverflow.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public List<HomePageViewModel> GetAllQuestions()
        {
            return _questionRepository.GetAllQuestions();
        }
        
    }
}