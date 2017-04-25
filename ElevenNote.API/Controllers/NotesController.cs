﻿using ElevenNote.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using ElevenNote.Models;

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
 
        public IHttpActionResult Get(int id)
        {
            var noteService = new NoteService(Guid.Parse(User.Identity.GetUserId()));
            var note = noteService.GetNoteById(id);
            if (note == null) return NotFound();
            return Ok(note);
        }


        public IHttpActionResult Post(NoteCreate model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var noteService = new NoteService(Guid.Parse(User.Identity.GetUserId()));
            return Ok(noteService.CreateNote(model));
        }


        public IHttpActionResult Put(NoteEdit model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //make sure note exists
            var noteService = new NoteService(Guid.Parse(User.Identity.GetUserId()));
            var temp = noteService.GetNoteById(model.NoteID);
            if (temp == null) return NotFound();
            //attempt to update
            return Ok(noteService.UpdateNote(model));

        }

        public IHttpActionResult Delete(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var noteService = new NoteService(Guid.Parse(User.Identity.GetUserId()));
            var temp = noteService.GetNoteById(id);
            if (temp == null) return NotFound();

            //Attempt to delete
            return Ok(noteService.DeleteNote(id));
        }
    }
}
