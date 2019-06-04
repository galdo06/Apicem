 
using System;

namespace Eisk.Helpers
{
    public sealed class ExceptionManager
    {
        public static string DoLogAndGetFriendlyMessageForException(Exception ex)
        {
            try
            {
                Logger.LogError(ex);
            }
            catch (Exception e)
            {
            }
            
            return GetFriendlyMessageForException(ex);
        }

        public static string GetFriendlyMessageForException(Exception ex)
        {
            string message = "Error: There was a problem while processing your request: " + ex.Message;

            if (ex.InnerException != null)
            {
                Exception inner = ex.InnerException;
                if (inner is System.Data.Common.DbException)
                    message = "Our database is currently experiencing problems. " + inner.Message;
                else if (inner is NullReferenceException)
                    message = "There are one or more required fields that are missing.";
                else if (inner is ArgumentException)
                {
                    string paramName = ((ArgumentException)inner).ParamName;
                    message = string.Concat("The ", paramName, " value is illegal.");
                }
                else if (inner is ApplicationException)
                    message = "Exception in application" + inner.Message;
                else
                    message = inner.Message;

            }

            return MessageFormatter.GetFormattedErrorMessage(message);
        }
    }
}