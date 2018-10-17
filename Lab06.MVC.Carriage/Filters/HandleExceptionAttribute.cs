using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;

namespace Lab06.MVC.Carriage.Filters
{
    public class HandleExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext exceptionContext)
        {
            //This simply surpresses MVC from raising exception
            exceptionContext.ExceptionHandled = true;

            logger.Error(exceptionContext.Exception, $"Message:{exceptionContext.Exception.Message}," +
                                                     $" ControllerMethod:{exceptionContext.Exception.TargetSite.Name}");

            exceptionContext.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                { "controller", "Home" },
                { "action", "Error" },
                { "id", HttpStatusCode.InternalServerError }
            });
        }
    }
}