using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using StackOverflow.Models;
using StackOverflow.ViewModels;

namespace StackOverflow.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly string _connString;

        public QuestionRepository(string connString)
        {
            _connString = connString;
        }

        public List<HomePageViewModel> GetAllQuestions()
        {
            var lstQuestions = new List<HomePageViewModel>();

            try
            {
                using (var conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (var command = new SqlCommand("sp_GetAllQuestion", conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstQuestions.Add(new HomePageViewModel()
                                {
                                    DisplayName = reader["Display_Name"].ToString(),
                                    Title = reader["Title"].ToString(),
                                    Body = reader["Body"].ToString(),
                                    Tags = reader["Tags"].ToString().Split(',').ToList(),
                                    CreatedAt = DateTime.Parse(reader["Created_At"].ToString()),
                                    AnswerCount = int.Parse(reader["AnswerCount"].ToString()),
                                });
                            }
                            return lstQuestions;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<HomePageViewModel>();
            }
        }
    }
}