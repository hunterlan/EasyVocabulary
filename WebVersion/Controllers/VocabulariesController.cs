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
            foreach(var row in _vocabularyContext.Vocabularies.ToList())
            {
                if (row.UserID == userID)
                    rows.Add(row);
            }
            return View(rows);
        }
    }
}