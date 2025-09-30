using System;
using System.Collections.Generic;
using System.Data;
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
                        using (var da = new SqlDataAdapter(command))
                        {
                            var ds = new DataSet();
                            da.Fill(ds, "QuestionsView");
                            
                            var dt = ds.Tables["QuestionsView"];

                            foreach (DataRow row in dt.Rows)
                            {
                                lstQuestions.Add(new HomePageViewModel()
                                {
                                    DisplayName = row["Display_Name"].ToString(),
                                    Title = row["Title"].ToString(),
                                    Body = row["Body"].ToString(),
                                    Tags = row["Tags"].ToString().Split(',').ToList(),
                                    CreatedAt = DateTime.Parse(row["Created_At"].ToString()),
                                    AnswerCount = int.Parse(row["AnswerCount"].ToString()),
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

        public List<HomePageViewModel> GetQuestionsByTagName(string tag)
        {
            var parameter = new Dictionary<string, object>();
            parameter.Add("@tag_name", tag);
            return ExecStoredProcedureAndMap("sp_GetQuestionsByTag", parameter);
        }

        public List<HomePageViewModel> GetQuestionsByTitle(string title)
        {
            var parameter = new Dictionary<string, object>();
            parameter.Add("@title", title);
            return ExecStoredProcedureAndMap("sp_GetQuestionsByTitle", parameter);
        }
        
        
    }
}