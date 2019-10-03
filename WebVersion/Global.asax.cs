using ConsoleVersion.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebVersion.Controllers;

namespace WebVersion
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_Error(Object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;
            var currentController = string.Empty;
            var currentAction = string.Empty;
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouteData != null)
            {
                if (!string.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (!string.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            // ��������� ����������
            var ex = Server.GetLastError();

            // �� � ������ ���������� � ������ ����������� ������ ����������� ������
            var controller = new ErrorsController();
            var routeData = new RouteData();
            // ����� �� ��������� � �����������
            var action = "Index";

            // ���� ��� ������ HTTP, � �� ����� ����, �� ��� ��� ���� �������������
            if (ex is HttpException)
            {
                switch (((HttpException)ex).GetHttpCode())
                {
                    case 403:
                        action = "AccessDenied";
                        break;
                    case 404:
                        action = "NotFound";
                        break;
                    default:
                        action = "HttpError";
                        break;
                        // ����� �������� ���� ������ ����������� ��� ����� ����� ������
                }
            }

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
            httpContext.Response.TrySkipIisCustomErrors = true;
            Exceptions.ErrorMessage = ex.Message;

            routeData.Values["controller"] = "Errors";
            routeData.Values["action"] = action;

            controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
    }
}
