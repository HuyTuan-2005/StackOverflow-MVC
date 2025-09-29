using System.Collections.Generic;
using System.Web.Mvc;
using StackOverflow.Services;
using StackOverflow.ViewModels;

namespace StackOverflow.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        public ActionResult Index(string search)
        {
            IReadOnlyList<HomePageViewModel> lstQuestion;
            if (string.IsNullOrEmpty(search))
            {
                lstQuestion = _questionService.GetAllQuestions();
            }
            else
            {
                lstQuestion = _questionService.GetQuestionsByTitle(search);
            }

            if (lstQuestion == null)
                return View("Error");

            ViewBag.CountQuestion = lstQuestion.Count;

            return View("Index", lstQuestion);
        }

        public ActionResult Tag(string id)
        {
            var lstQuestion = _questionService.GetQuestionsByTagName(id);

            if (lstQuestion == null)
                return View("Error");

            ViewBag.CountQuestion = lstQuestion.Count;
            return View("Index", lstQuestion);
        }
    }
}