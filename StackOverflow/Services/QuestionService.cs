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

        public IReadOnlyList<HomePageViewModel> GetAllQuestions()
        {
            return _questionRepository.GetAllQuestions();
        }

        public IReadOnlyList<HomePageViewModel> GetQuestionsByTagName(string tag)
        {
            return _questionRepository.GetQuestionsByTagName(tag);
        }

        public IReadOnlyList<HomePageViewModel> GetQuestionsByTitle(string title)
        {
            return _questionRepository.GetQuestionsByTitle(title);
        }
    }
}