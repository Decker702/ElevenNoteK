using ElevenNote.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace ElevenNote.API.Controllers
{
    [Authorize]
    public class NotesController : ApiController
    {
        public IHttpActionResult GetAll()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var noteService = new NoteService(userId);
            var notes = noteService.GetNotes();
        
            return Ok(notes);
        }
    }
}
