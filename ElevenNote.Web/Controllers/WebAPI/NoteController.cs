using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElevenNote.Web.Controllers.WebAPI //Differnt than MVC, got NuGet package for it.
{
    [Authorize]//allows only authorized user to login
    [RoutePrefix("api/Note")]//does not use MVC Controller, but the API Controller.  "api/.." allows it to hit API, not MVC controller
    public class NoteController : ApiController //JQuery will send the request here, not the browser;  Going to be a URL in our application
    {
        private bool SetStarState(int noteId, bool newState)
        {
            //Creates the service
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);

            //Get the note
            var detail = service.GetNoteById(noteId); 

            //Create the NoteEdit model instance with the new star
            var updatedNote =
                new NoteEdit
                {
                    NoteID = detail.NoteID,
                    Title = detail.Title,
                    Content = detail.Content,
                    IsStarred = newState
                };

            //Return a value indicating whether the update succeeded
            return service.UpdateNote(updatedNote);
        }

        [Route("{id}/Star")]
        [HttpPut]//To turn star on
        public bool ToggleStarOn(int id) => SetStarState(id, true);

    [Route("{id}/Star")]
    [HttpDelete]//To turn star off
    public bool ToggleStarOff(int id) => SetStarState(id, false);
       
    }  
}
