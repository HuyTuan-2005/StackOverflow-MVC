using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace StackOverflow.Models
{
    public class BCP
    {
        private readonly string ServerName;
        private readonly string DatabaseName;
        //private readonly string UserName;
        //private readonly string Password;

        public BCP(string serverName, string databaseName)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
        }
        public string ExportTable(string tableName, string filePath)
        {
            string fullTableName = $"{DatabaseName}.dbo.{tableName}";
            string bcpCommand = $"bcp {fullTableName} out \"{filePath}\" -c -t; -S {ServerName} -T -w";
            return RunBCP(bcpCommand);
        }
        public string ImportTable(string tableName, string filePath)
        {
            string fullTableName = $"{DatabaseName}.dbo.{tableName}";
            string bcpCommand = $"bcp {fullTableName} in \"{filePath}\" -c -t; -S {ServerName} -T -w";
            return RunBCP(bcpCommand);
        }

        private string RunBCP(string command)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/C " + command,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(error))
                        throw new Exception(error);

                    return output;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi chạy BCP: " + ex.Message);
            }
        }
    }
}
