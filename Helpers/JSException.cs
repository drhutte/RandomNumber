using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RandomNumber.Helpers
{
    public class JSException : Exception
    {
        public string Message { get; set; }
        public string Url { get; set; }
        public int LineNumber { get; set; }
        public string ErrorStack { get; set; }
        public int Column { get; set; }
        public override string ToString()
        {
            return Message+ Environment.NewLine +
                "Url: " + Url + Environment.NewLine +
                "Line: " + LineNumber + Environment.NewLine +
                "Column: " + Column + Environment.NewLine +
                (!string.IsNullOrEmpty(ErrorStack) ? ("Stack Trace: " + ErrorStack + Environment.NewLine) : string.Empty) + base.ToString();
        }
    }
}