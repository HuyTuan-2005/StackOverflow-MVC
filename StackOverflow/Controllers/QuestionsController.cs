using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using StackOverflow.Services;
using StackOverflow.ViewModels;

namespace StackOverflow.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;

        public QuestionsController(IQuestionService questionService,  IAnswerService answerService)
        {
            _questionService = questionService;
            _answerService = answerService;
        }

        [Route("questions/{id:int?}")]
        public ActionResult Index(int? id, HomePageViewModel model)
        {
            if (id.HasValue)
            {
                ViewBag.Id = id;
                
                // Tra ve chi tiet cau hoi va danh sach cac cau tra loi cua cau hoi đó
                return View("Details", new DetailsQuestionViewModel()
                {
                    Question = _questionService.GetQuestionsById(id.Value),
                    Answers = _answerService.GetAnswerByQuestionId(id.Value)
                });
            }
            IReadOnlyList<HomePageViewModel> lstQuestion;
            if (string.IsNullOrEmpty(model.Title))
            {
                lstQuestion = _questionService.GetAllQuestions();
            }
            else
            {
                lstQuestion = _questionService.GetQuestionsByTitle(model.Title);
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

        [HttpGet]
        public ActionResult Ask()
        {
            if (Session["UserName"] != null)
                return View();    
            
            string returnUrl = Request.Url.PathAndQuery;
            return RedirectToAction("Login", "Users", new { returnUrl = returnUrl });
        }

        [HttpPost]
        public ActionResult Ask(int userId, string title, string body, string tags)
        {
            if (Session["UserName"] == null)
            {
                string returnUrl = Request.Url.PathAndQuery;
                return RedirectToAction("Login", "Users", new { returnUrl = returnUrl });
            }
            _questionService.PostQuestion(userId, title, body, tags);
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult PostAnswer(AnswerViewModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Index");
            }
        
            if (Session["UserName"] == null)
            {
                string returnUrl = "/questions/" + model.QuestionId;
                return RedirectToAction("Login", "Users", new { returnUrl = returnUrl });
            }
            
            if(model.QuestionId <= 0 || string.IsNullOrEmpty(model.Body) || model.UserId <= 0)
            {
                return RedirectToAction("Index");
            }
            
            _answerService.PostAnswer(model.UserId, model.QuestionId, model.Body);
            return RedirectToAction("Index", new { id = model.QuestionId });
        }
    }
}