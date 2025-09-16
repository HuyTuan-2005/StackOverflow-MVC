using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

            string bcpCommand = $"bcp {fullTableName} out \"{filePath}\" -c -t, -S {ServerName} -T -w";

            string output = RunBCP(bcpCommand);

            string content = System.IO.File.ReadAllText(filePath, System.Text.Encoding.Unicode);
            System.IO.File.WriteAllText(filePath, content, new System.Text.UTF8Encoding(true));

            return output;
        }
        public string ImportTable(string tableName, string filePath)
        {

            string fullTableName = $"{DatabaseName}.dbo.{tableName}";

            string tempFile = Path.GetTempFileName();
            string content = System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            System.IO.File.WriteAllText(tempFile, content, System.Text.Encoding.Unicode);

            string bcpCommand = $"bcp {fullTableName} in \"{tempFile}\" -c -t, -S {ServerName} -T -w";

            string output = RunBCP(bcpCommand);

            if (System.IO.File.Exists(tempFile))
                System.IO.File.Delete(tempFile);
          
            return output;
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
