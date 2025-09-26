using StackOverflow.Models;
using System.Collections.Generic;


namespace StackOverflow.Repositories
{
    public interface IQuestionRepository
    {
        //Question GetQuestionByTitle(string title);
        List<Question> GetAllQuestions();
    }
}