using System;
using log4net;

namespace DasApp.Log4Net
{
    public class LogHelper
    {
        public static readonly ILog loginfo = LogManager.GetLogger("loginfo");

        public static readonly ILog logerror = LogManager.GetLogger("logerror");

        public static void WriteLog(string info)
        {
            if (loginfo.IsInfoEnabled)
                loginfo.Info(info);
        }

        public static void WriteLog(string info, Exception se)
        {
            if (logerror.IsErrorEnabled)
                logerror.Error(info, se);
        }
    }
}