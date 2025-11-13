using System.Collections.Generic;
using StackOverflow.ViewModels;

namespace StackOverflow.Services
{
    public interface IAnswerService
    {
        IReadOnlyList<AnswerViewModel> GetAnswerByQuestionId(int questionId);
        void PostAnswer(int modelUserId, int modelQuestionId, string modelBody);
    }
}