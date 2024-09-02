using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StundetInformation.Filters
{
    public class _SecurityFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName != "Login")
            {
                // login controller değilse 

                string isAuthorized = "None";


                if (HttpContext.Current.Session["Admin"] != null)
                {
                    isAuthorized = "Admin";
                }
                else if (HttpContext.Current.Session["Instructor"] != null)
                {
                    isAuthorized = "Instructor";

                    if (controllerName == "Department" || controllerName == "Semester" || controllerName == "User")
                    {
                        filterContext.Result = new RedirectResult("/Login/NoneAuthorized");
                        return;
                    }
                }
                else if (HttpContext.Current.Session["Student"] != null)
                {
                    isAuthorized = "Student";

                    if (controllerName == "Department" || controllerName == "Semester" || controllerName == "User")
                    {
                        filterContext.Result = new RedirectResult("/Login/NoneAuthorized");
                        return;
                    }
                }



                if (isAuthorized == "None")
                {
                    filterContext.Result = new RedirectResult("/Login/NoneAuthorized");
                    return;
                }
            }
        }
    }
}
