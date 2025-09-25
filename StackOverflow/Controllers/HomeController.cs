using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackOverflow.Models;
using Humanizer;
using System.Globalization;

namespace StackOverflow.Controllers
{
    public class HomeController : Controller
    {
        private List<Question> _questions { get; set; }

        private int CountQuestion(List<Question> lstQuestion)
        {
            return lstQuestion.Count;
        }

        private List<Question> GetAllQuestions()
        {
            _questions = new List<Question>();
            var db = new Database();
            var conn = db.Connection();

            var command = conn.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_GetAllQuestion";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var question = new Question()
                    {
                        DisplayName = reader["display_name"].ToString(),
                        title = reader["title"].ToString(),
                        body = reader["body"].ToString(),
                        gioDang = DateTime.Parse(reader["GioDang"].ToString())
                    };
                    question.AddTags(reader["tags"].ToString());
                    _questions.Add(question);
                }
            }
            return _questions;
        }
        
        private List<Question> GetQuestions(string search)
        {
            if (string.IsNullOrEmpty(search))
                return GetAllQuestions();
            
            List<Question> lstQuestion = new List<Question>();
            
            var db = new Database();
            var conn = db.Connection();

            var command = conn.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_GetQuestion";
            command.Parameters.AddWithValue("@keyword", search);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var question = new Question()
                    {
                        DisplayName = reader["display_name"].ToString(),
                        title = reader["title"].ToString(),
                        body = reader["body"].ToString(),
                        gioDang = DateTime.Parse(reader["GioDang"].ToString())
                    };
                    question.AddTags(reader["tags"].ToString());
                    lstQuestion.Add(question);
                }
            }

            return lstQuestion;
        }

        public ActionResult Index()
        {
            List<Question> lstQuestion = GetAllQuestions();
            ViewBag.CountQuestion = CountQuestion(lstQuestion);
            
            return View("Index", lstQuestion);
        }
        
        [HttpGet]
        public ActionResult Search(string search)
        {
            List<Question> lstQuestion = GetQuestions(search);
            ViewBag.CountQuestion = CountQuestion(lstQuestion);
            return View("Index", lstQuestion);
        }
    }
}