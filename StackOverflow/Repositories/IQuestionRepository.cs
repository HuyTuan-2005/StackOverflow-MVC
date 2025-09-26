using StackOverflow.Models;
using System.Collections.Generic;
using StackOverflow.ViewModels;

namespace StackOverflow.Repositories
{
    public interface IQuestionRepository
    {
        List<HomePageViewModel> GetQuestionsByTag(string tag);
        List<HomePageViewModel> GetAllQuestions();
    }
}