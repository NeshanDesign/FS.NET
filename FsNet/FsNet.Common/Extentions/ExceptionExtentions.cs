using System;

namespace FsNet.Common.Extentions
{
    public static class ExceptionExtentions
    {
        public static string FullTrace(this Exception e)
        {
            return e == null ? "null" : string.Format(@"[E]:{3} Exception: {0}{4} StackTrace: {1}{4} InnerException:{2}", e.Message, e.StackTrace, e.InnerException.FullTrace(), DateTime.Now, Environment.NewLine);
        }
    }
}
