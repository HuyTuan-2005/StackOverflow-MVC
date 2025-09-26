using System.Collections.Generic;
using StackOverflow.ViewModels;

namespace StackOverflow.Services
{
    public interface IQuestionService
    {
        List<HomePageViewModel> GetAllQuestions();
    }
}