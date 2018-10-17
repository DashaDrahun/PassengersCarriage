using System.Web.Mvc;
using Lab06.MVC.Carriage.Filters;

namespace Lab06.MVC.Carriage
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleExceptionAttribute());
        }
    }
}