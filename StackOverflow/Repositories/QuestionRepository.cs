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

        private List<HomePageViewModel> ExecStoredProcedureAndMap(string storedProcedureName, Dictionary<string, object> parameters = null)
        {
            var lstQuestions = new List<HomePageViewModel>();

            try
            {
                using (var conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (var command = new SqlCommand(storedProcedureName, conn))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                            }
                        }
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
                return new List<HomePageViewModel>();
            }
        }

        public List<HomePageViewModel> GetAllQuestions()
        {
            return ExecStoredProcedureAndMap("sp_GetAllQuestion");
        }

        public List<HomePageViewModel> GetQuestionsByTag(string tag)
        {
            var parameter = new Dictionary<string, object>();
            parameter.Add("@Tag", tag);
            return ExecStoredProcedureAndMap("sp_GetQuestionsByTag", parameter);
        }
    }
}