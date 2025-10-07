﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StackOverflow.ViewModels;

namespace StackOverflow.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly string _connString;

        public AnswerRepository(string connectionString)
        {
            _connString = connectionString;
        }

        public List<AnswerViewModel> GetAnswerByQuestionId(int questionId)
        {
            var lstAnswer = new List<AnswerViewModel>();
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM f_GetAnswersByQuestionId(@Question_Id)", conn))
                {
                    cmd.Parameters.AddWithValue("@Question_Id", questionId);

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var ds = new DataSet();
                        da.Fill(ds, "Answers");
                        
                        var dt = ds.Tables["Answers"];
                        foreach (DataRow row in dt.Rows)
                        {
                            var Answer = new AnswerViewModel()
                            {
                                AnswerId = row.Field<int>("Answer_Id"),
                                QuestionId = row.Field<int>("Question_Id"),
                                Body = row.Field<string>("Body"),
                                CreatedAt = row.Field<DateTime>("Created_At"),
                                DisplayName = row.Field<string>("Display_Name"),
                                UserId = row.Field<int>("User_Id"),
                            };
                            lstAnswer.Add(Answer);
                        }
                        return lstAnswer;
                    }
                }
            }
        }
    }
}