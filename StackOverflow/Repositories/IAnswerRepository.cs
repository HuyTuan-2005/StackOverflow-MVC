using System.Collections.Generic;
using StackOverflow.ViewModels;

namespace StackOverflow.Repositories
{
    public interface IAnswerRepository
    {
        List<AnswerViewModel> GetAnswerByQuestionId(int questionId);
    }
}