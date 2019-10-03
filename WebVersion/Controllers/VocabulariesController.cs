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

        public void IsUserAuth(User user)
        {
            if (user == null)
                throw new Exception("You're not authorized!");
        }

        public ActionResult Index()
        {
            User valueSession = (User)Session["User"];
            IsUserAuth(valueSession);
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
            IsUserAuth(valueSession);
            userID = valueSession.Id;
            newWord.UserID = userID;
            VocabularyController.AddRow(_vocabularyContext, newWord);
            if (Exceptions.IsError == 1)
            {
                Exceptions.IsError = 0;
                throw new Exception(Exceptions.ErrorMessage);
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
            User valueSession = (User)Session["User"];
            IsUserAuth(valueSession);

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

        [HttpPost]
        public ActionResult ChangingWord(string key, string choosedRow, Vocabulary changes)
        {
            Vocabulary changedRow = new Vocabulary();
            User valueSession = (User)Session["User"];
            IsUserAuth(valueSession);

            changedRow = VocabularyController.FindRow(choosedRow, Byte.Parse(key), _vocabularyContext);
            if (changes.ForeignWord != null && changes.ForeignWord != "")
                changedRow.ForeignWord = changes.ForeignWord;
            if (changes.Transcription != null && changes.Transcription != "")
                changedRow.Transcription = changes.Transcription;
            if (changes.LocalWord != null && changes.LocalWord != "")
                changedRow.LocalWord = changes.LocalWord;

            VocabularyController.UpdateRow(_vocabularyContext, changedRow);
            return RedirectToAction("Index", "Vocabularies");
        }
    }
}