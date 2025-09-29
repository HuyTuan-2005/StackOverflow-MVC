using StackOverflow.Models;
using System.Collections.Generic;
using StackOverflow.ViewModels;

namespace StackOverflow.Repositories
{
    public interface IQuestionRepository
    {
        List<HomePageViewModel> GetAllQuestions();
        List<HomePageViewModel> GetQuestionsByTagName(string tag);
        List<HomePageViewModel> GetQuestionsByTitle(string title);
    }
}