using System.Collections.Generic;
using StackOverflow.Models;

namespace StackOverflow.ViewModels
{
    public class HomePageViewModel
    {
        List<Question> LstQuestions {get;set;}
        List<Answer> LstAnswers {get;set;}
    }
}