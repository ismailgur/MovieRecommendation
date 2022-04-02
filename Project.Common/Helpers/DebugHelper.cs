using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Helpers
{
    public static class DebugHelper
    {
        public static string GetExceptionErrorMessage(Exception ex)
        {
            StringBuilder errorLog = new StringBuilder();

            errorLog.AppendLine("\nMessage : " + StringHelper.AlwaysConvert(ex.Message));
            errorLog.AppendLine("\nStack Trace : " + StringHelper.AlwaysConvert(ex.StackTrace));
            errorLog.AppendLine("\nSource : " + StringHelper.AlwaysConvert(ex.Source));
            errorLog.AppendLine("\nTarget Site : " + StringHelper.AlwaysConvert(ex.TargetSite));

            if (ex.InnerException != null)
            {
                errorLog.AppendLine("\nInner Ex. Message : " + StringHelper.AlwaysConvert(ex.InnerException.Message));
                errorLog.AppendLine("\nInner Ex. Stack Trace : " + StringHelper.AlwaysConvert(ex.InnerException.StackTrace));
                errorLog.AppendLine("\nInner Ex. Source : " + StringHelper.AlwaysConvert(ex.InnerException.Source));
            }

            return errorLog.ToString();
        }
    }
}
