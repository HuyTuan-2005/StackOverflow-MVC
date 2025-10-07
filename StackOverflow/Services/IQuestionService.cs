using System.Collections.Generic;
using StackOverflow.ViewModels;

namespace StackOverflow.Services
{
    public interface IQuestionService
    {
        IReadOnlyList<HomePageViewModel> GetAllQuestions();
        IReadOnlyList<HomePageViewModel> GetQuestionsByTagName(string tag);
        IReadOnlyList<HomePageViewModel> GetQuestionsByTitle(string title);
        HomePageViewModel GetQuestionsById(int questionId);
    }
}