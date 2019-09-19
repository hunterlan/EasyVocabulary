using ConsoleVersion;
using ConsoleVersion.Helper;
using ConsoleVersion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebVersion.Controllers
{
    public class VocabulariesController : Controller
    {
        // GET: Vocabularies
        int userID;
        VocabularyContext _vocabularyContext;
        public VocabulariesController()
        {
            _vocabularyContext = new VocabularyContext();
        }

        public ActionResult Index()
        {
            User valueSession = (User)Session["User"];
            userID = valueSession.Id;
            List<Vocabulary> rows = new List<Vocabulary>();
            foreach (var row in _vocabularyContext.Vocabularies.ToList())
            {
                if (row.UserID == userID)
                    rows.Add(row);
            }
            return View(rows);
        }

        public ActionResult AddWord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddWord(Vocabulary newWord)
        {
            User valueSession = (User)Session["User"];
            userID = valueSession.Id;
            newWord.UserID = userID;
            VocabularyController.AddRow(_vocabularyContext, newWord);
            if (Exceptions.IsError == 1)
            {
                return View();
            }
            else
                return RedirectToAction("Index");
        }

        public ActionResult ExitFromAccount()
        {
            Session.Abandon();
            HttpContext.Response.Cookies["UserID"].Value = null;
            return RedirectToAction("Index", "Start");
        }

        public ActionResult DeleteWord()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeleteWord(string key, string deletingData)
        {
            var row = VocabularyController.FindRow(deletingData, Byte.Parse(key), _vocabularyContext);

            if (row != null)
            {
                VocabularyController.RemoveRow(_vocabularyContext, row);
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult ChangingWord()
        {
            return View();
        }
    }
}