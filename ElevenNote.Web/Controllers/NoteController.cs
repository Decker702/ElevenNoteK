using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.Web.Controllers
{
    [Authorize]//When you get a request, check to see the user is authenicated or send them to login page.
    public class NoteController : Controller//Can't use a construtor at this point because way MVC works.  Not initialized that piece of the controller until later.

    {
        public ActionResult Index()
        {
            var service = CreateNoteService();//Lets you reuse same code and if needed only have to correct it once.
            var model = service.GetNotes();

            return View(model);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateNoteService();

            if (service.CreateNote(model))
            {
                TempData ["SaveResult"] = "Your note was created";//TempData last across the redirect (across HTTP request)
                 return RedirectToAction("Index");//redirect the route result - go get this page
            }
            ModelState.AddModelError("", "Your note could not be created.");//tells user note was not created
            return View(model);//If doesn't suceed, then go back to the form
        }

        public ActionResult Details(int id)// use small case --id-- because that is how identified in the RouteConfig
        {
            var svc = CreateNoteService();
            var detail = svc.GetNoteById(id);
            var model =
                new NoteDetail
                {
                    NoteID = detail.NoteID,
                    Title = detail.Title,
                    Content = detail.Content
                };

            return View(model);
        }


        public ActionResult Edit(int id)
        {
            var service = CreateNoteService();
            var detail = service.GetNoteById(id);
            var model =
                new NoteEdit
                {
                    NoteID = detail.NoteID,
                    Title = detail.Title,
                    Content = detail.Content
                };
                
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit (int id, NoteEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.NoteID != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateNoteService();

            if (service.UpdateNote(model))
            {
                TempData["SaveResult"] = "Your note was updated";//TempData last across the redirect (across HTTP request)
                return RedirectToAction("Index");//redirect the route result - go get this page
            }
            ModelState.AddModelError("", "Your note could not be updated.");//tells user note was not created
            return View(model);//If doesn't suceed, then go back to the form
        }
        public ActionResult Delete(int id)//TODO need to create delete view
        {
            var service = CreateNoteService();
            var detail = service.GetNoteById(id);

            return View();
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
         public ActionResult DeletePost(int id)
        {
            var service = CreateNoteService();

            //TODO: Handle failure, need if statement
            service.DeleteNote(id);

            TempData["SaveResult"] = "Your note was deleted!";

            return RedirectToAction("Index");
        }

        private NoteService CreateNoteService()//This was creates with hightlight, edit, refactor and get new method, renamed it.
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            return service;
        }
    }
}