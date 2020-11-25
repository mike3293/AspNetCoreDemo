using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace AspNetCoreDemo.Filters
{
    public class CookiesAppendFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            context.HttpContext.Response.Cookies.Append("LastRequest", DateTime.Now.ToString("dd/MM/yyyy hh-mm-ss"));
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        { }
    }
}
