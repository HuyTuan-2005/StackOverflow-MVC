using StackOverflow.ViewModels;
using System.Collections.Generic;
using System.Linq;
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

        public List<HomePageViewModel> GetQuestionsByTag(string tag)
        {
            return _questionRepository.GetQuestionsByTag(tag);
        }
    }
}