using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MobileWS.Helpers
{
    public class AuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase  s = filterContext.HttpContext.Session;
            bool isLoggedIn = false;

            if(s!=null)
            {
                if (s["ID"] != null)
                    isLoggedIn = true;
            }

            if (!isLoggedIn)
            {
                //is authenticated
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary{{ "controller", "Home" },
                                        { "action", "Login" }
                });
            }
            else
            {
                //check permissions aka authorization
                if (s["Role"].ToString().Trim() != "admin")
                {
                    if(s["Role"].ToString().Trim() == "worker" && filterContext.Controller.ToString()== "MobileWS.Controllers.DefaultController")
                    {
                         //do nothing, user have permission
                    }
                    else if (s["Role"].ToString().Trim() == "worker" && filterContext.Controller.ToString() != "MobileWS.Controllers.DefaultController")
                    {
                        //do nothing, user have permission
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary{{ "controller", "Default" },
                                        { "action", "Index" }
                        });
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary{{ "controller", "Home" },
                                        { "action", "Forbidden" }
                        });
                    }
                }
            }
        }
    }
}