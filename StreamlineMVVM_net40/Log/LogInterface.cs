using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamlineMVVM
{
    public interface ILogMVVM
    {
        void LogEntry(string log);
        void Exception(string log, Exception ex);
    }

    public static class LogMVVM
    {
        private static ILogMVVM _ILogMVVM = null;
        public static void SetLogHTTP(ILogMVVM logMVVM)
        {
            lock (_ILogMVVM)
            {
                _ILogMVVM = logMVVM;
            }
        }

        private static bool _LogOutput = true;
        public static bool LogOutput
        {
            get
            {
                return _LogOutput;
            }
        }
        public static void SetOutputLogging(bool state)
        {
            _LogOutput = state;
        }

        public static void Entry(string log)
        {
            if (_ILogMVVM == null || LogOutput == false)
            {
                return;
            }

            _ILogMVVM.LogEntry(log);
        }

        public static void Exception(string log, Exception Ex)
        {
            if (_ILogMVVM == null || LogOutput == false)
            {
                return;
            }

            _ILogMVVM.Exception(log, Ex);
        }
    }
}
