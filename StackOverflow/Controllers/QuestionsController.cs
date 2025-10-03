using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
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

        [Route("questions/{id:int?}")]
        public ActionResult Index(int? id, HomePageViewModel model)
        {
            IReadOnlyList<HomePageViewModel> lstQuestion;
            if (string.IsNullOrEmpty(model.Title))
            {
                lstQuestion = _questionService.GetAllQuestions();
            }
            else
            {
                lstQuestion = _questionService.GetQuestionsByTitle(model.Title);
            }

            if (id.HasValue)
            {
                ViewBag.Id = id;
                return View("Details", lstQuestion.Where(t => t.QuestionId == id.Value).FirstOrDefault());
            }

            if (lstQuestion == null)
                return View("Error");

            ViewBag.CountQuestion = lstQuestion.Count;

            return View(lstQuestion);
        }

        public ActionResult Tag(string id)
        {
            var lstQuestion = _questionService.GetQuestionsByTagName(id.ToLower());

            if (lstQuestion == null)
                return View("Error");

            ViewBag.CountQuestion = lstQuestion.Count;
            return View("Index", lstQuestion);
        }

        public ActionResult Ask()
        {
            if (Session["UserName"] != null)
                return View();    
            return RedirectToAction("Index", "Login");
        }
    }
}