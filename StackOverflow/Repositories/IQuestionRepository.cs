using StackOverflow.Models;
using System.Collections.Generic;
using StackOverflow.ViewModels;

namespace StackOverflow.Repositories
{
    public interface IQuestionRepository
    {
        //Question GetQuestionByTitle(string title);
        List<HomePageViewModel> GetAllQuestions();
    }
}