using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace Kaizos
{
    public enum ErrorSource
    {
        Client,
        Service,
    }

    public class ErrorLog
    {

        public static string ExtractError(Exception error)
        {
            string TotalError = null;

            while (error != null)
            {
                TotalError = TotalError + error.Message;
                error = error.InnerException;
            }
            return TotalError;
        }

        public static string GetErrorLogMessage(string UserName, ErrorSource errorSource, string MethodName, string ErrorDetails)
        {
            string errorMessage = DateTime.Now.ToString() + "|" + UserName.Trim() + "|" + errorSource.ToString() + "|" + MethodName.Trim() + "|" + ErrorDetails.Trim();
            return errorMessage;
        }
    }
}