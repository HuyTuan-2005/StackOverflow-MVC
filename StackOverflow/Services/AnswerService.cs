using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using StackOverflow.Repositories;
using StackOverflow.ViewModels;

namespace StackOverflow.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;

        public AnswerService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public IReadOnlyList<AnswerViewModel> GetAnswerByQuestionId(int questionId)
        {
            return _answerRepository.GetAnswerByQuestionId(questionId);
        }
        
        public void PostAnswer(int modelUserId, int modelQuestionId, string modelBody)
        {
            var model = new AnswerViewModel
            {
                UserId = modelUserId,
                QuestionId = modelQuestionId,
                Body = modelBody,
                CreatedAt = DateTime.Now,
            };
            _answerRepository.PostAnswer(model);
        }
    }
}