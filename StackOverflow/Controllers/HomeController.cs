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

        private int CountQuestion()
        {
            var db = new Database();
            var conn = db.Connection();

            var command = conn.CreateCommand();
            command.CommandText = "SELECT count(*) FROM V_Question";

            var reader = command.ExecuteScalar();
            return (int)reader;
        }

        private List<Question> GetQuestions()
        {
            _questions = new List<Question>();
            var db = new Database();
            var conn = db.Connection();

            var command = conn.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_GetQuestion";

            var reader = command.ExecuteReader();

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

            reader.Close();
            return _questions;
        }
        public ActionResult TestError()
        {
            throw new Exception("Lỗi thử để kiểm tra filter");
        }

        public ActionResult Index()
        {
            // if (Session["username"] == null)
            // {
            //     return RedirectToAction("Index", "Login");
            // }
            ViewBag.CountQuestion = CountQuestion();
            
            return View(GetQuestions());
        }
    }
}