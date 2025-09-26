using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackOverflow.Models;
using Humanizer;
using System.Globalization;
using StackOverflow.Services;

namespace StackOverflow.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuestionService _questionService;

        public HomeController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        public ActionResult Index()
        {
            var lstQuestion = _questionService.GetAllQuestions();
            
            if(lstQuestion == null)
                return View("Error");
            
            ViewBag.CountQuestion = lstQuestion.Count;
            
            return View("Index", lstQuestion);
        }
    }
}