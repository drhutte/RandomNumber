using Elmah;
using RandomNumber.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RandomNumber.Controllers
{
    public class ErrorController : Controller
    {
        public void Index(string message, string url, int line, int column, string stack)
        {
            ErrorSignal.FromCurrentContext().Raise(new JSException
            {
                Message = message,
                Url = url,
                LineNumber = line,
                Column = column,
                ErrorStack = stack
            });
        }
    }
}
