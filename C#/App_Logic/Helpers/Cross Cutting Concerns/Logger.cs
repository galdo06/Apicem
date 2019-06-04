 
using System;
using System.IO;
using System.Web;

namespace Eisk.Helpers
{

    public sealed class Logger
    {
        private Logger() { }

        public static void LogError()
        {
            System.Exception ex = System.Web.HttpContext.Current.Server.GetLastError();
            LogError(ex);

        }

        public static void LogError(Exception ex)
        {
            var currentContext = HttpContext.Current;

            if (ex.Message.Equals("File does not exist."))
            {
                currentContext.ClearError();
                return;
            }

            string logSummery, logDetails, filePath = "No file path found.", url = "No url found to be reported.";
            
            if (currentContext != null)
            {
                filePath = currentContext.Request.FilePath;
                url = currentContext.Request.Url.AbsoluteUri;
            }
            
            logSummery = ex.Message;
            logDetails = ex.ToString();

            //-----------------------------------------------------

            string path = System.Web.HttpContext.Current.Server.MapPath("~/App_Resources/system/log/log.txt");
            FileStream fStream = new FileStream(path, FileMode.Append, FileAccess.Write);
            BufferedStream bfs = new BufferedStream(fStream);
            StreamWriter sWriter = new StreamWriter(bfs);
            
            //insert a separator line
            sWriter.WriteLine("=================================================================================================");
            
            //create log for header
            sWriter.WriteLine(logSummery);
            sWriter.WriteLine("Log time:" + DateTime.Now);
            sWriter.WriteLine("URL: " + url);
            sWriter.WriteLine("File Path: " + filePath);
            
            //create log for body
            sWriter.WriteLine(logDetails);
            
            //insert a separator line
            sWriter.WriteLine("=================================================================================================");

            sWriter.Close();

        }
    }
}



