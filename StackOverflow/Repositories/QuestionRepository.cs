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

        private List<HomePageViewModel> ExecStoredProcedureAndMap(string query, CommandType commandType = CommandType.Text, Dictionary<string, object> parameters = null)
        {
            var lstQuestions = new List<HomePageViewModel>();

            try
            {
                using (var conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (var command = new SqlCommand(query, conn))
                    {
                        command.CommandType = commandType;
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
                            da.Fill(ds, "ViewQuestions");
                            
                            var dt = ds.Tables["ViewQuestions"];

                            foreach (DataRow row in dt.Rows)
                            {
                                lstQuestions.Add(new HomePageViewModel()
                                {
                                    UserId = int.Parse(row["User_Id"].ToString()),
                                    QuestionId = int.Parse(row["Question_Id"].ToString()),
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
            return ExecStoredProcedureAndMap("sp_GetQuestions", CommandType.StoredProcedure);
        }

        public List<HomePageViewModel> GetQuestionsByTitle(string title)
        {
            var parameter = new Dictionary<string, object>();
            parameter.Add("@title", $"N'%' + {title} + N'%'");
            return ExecStoredProcedureAndMap("select * from f_GetQuestionsByTitle(@title)", CommandType.Text, parameter);
        }
        public List<HomePageViewModel> GetQuestionsByTagName(string tags)
        {
            var parameter = new Dictionary<string, object>()
            {
                { "@Tags", tags }
            };
            // parameter.Add("@Tags", tag);
            return ExecStoredProcedureAndMap("execute sp_GetQuestions null, null, null, @tags", CommandType.Text, parameter);
        }

        
        public HomePageViewModel GetQuestionsById(int questionId)
        {
            var parameter = new Dictionary<string, object>()
            {
                { "@question_id", questionId }
            };
            // parameter.Add("@Tags", tag);
            return ExecStoredProcedureAndMap("select * from f_GetQuestionsById(@question_id)", CommandType.Text, parameter).First();;
        }
        
        public void PostQuestion(int userId, string title, string body, string tags)
        {
            try
            {
                using (var conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    using (var command = new SqlCommand("sp_PostQuestion", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@user_id", SqlDbType.Int).Value = userId;
                        command.Parameters.Add("@title", SqlDbType.NVarChar, 200).Value = title;
                        command.Parameters.Add("@body", SqlDbType.NVarChar, -1).Value = body;
                        command.Parameters.Add("@tags", SqlDbType.NVarChar, 400).Value = tags;

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                // Handle exception (log it, rethrow it, etc.)
            }
        }
    }
}