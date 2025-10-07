using System.Collections.Generic;
using StackOverflow.Models;

namespace StackOverflow.ViewModels
{
    public class DetailsQuestionViewModel
    {
        public HomePageViewModel Question { get; set; }
        public IReadOnlyList<AnswerViewModel> Answers { get; set; }
    }
}