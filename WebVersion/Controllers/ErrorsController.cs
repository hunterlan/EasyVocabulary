using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConsoleVersion.Helper;

namespace WebVersion.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: Errors
        public ActionResult Index()
        {
            ViewBag.textError = Exceptions.ErrorMessage;
            return View("Index");
        }

        public ViewResult AccessDenied()
        {
            // представление для кода 403
            return View("AccessDenied");
        }

        public ViewResult NotFound()
        {
            // представление для кода 404
            return View("NotFound");
        }

        public ViewResult HttpError()
        {
            // представление всех остальных кодов HTTP
            return View("HttpError");
        }
    }
}