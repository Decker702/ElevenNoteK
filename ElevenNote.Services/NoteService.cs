using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services//This is the translation level between Data, Web & Model
{
    public class NoteService
    {
        private readonly Guid _userId;//_userId is the currently logged in user.  Readonly can't be used outside of the constructor.

        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateNote(NoteCreate model)
        {
            var entity =
                new Note//puts note in the table in the database(db); value is stored in the database 
                {
                    OwnerId = _userId,
                    Title = model.Title,
                    Content = model.Content,
                    CreatedUtc = DateTimeOffset.UtcNow
                };
            using (var ctx = new ApplicationDbContext())//need to bring in another NuGet pkg to get rid of red squigglies.
            { //simplifies a try catch fewer lines of code.
                ctx.Notes.Add(entity);//Take advantage of order of operations, update db on all new or changed infomation

                return ctx.SaveChanges() == 1;//1 is the number of rows changed/added to the table in the db(database), if 0 or more than 1 you have a problem & returns false.
            }

        }

        public IEnumerable<NoteListItem> GetNotes()
        {
            using (var ctx = new ApplicationDbContext())//To get not out of the table in the db
            {
                var query =//query says how to get information, does not get it.
                    ctx
                    .Notes //looks at all the notes in the system, but only want my notes for my Id
                    .Where(e => e.OwnerId == _userId) //sorts all notes in system and selects the ones for my Id
                    .Select(//lets the information flow between layers
                       e =>
                       new NoteListItem
                       {
                           NoteId = e.NoteID,
                           Title = e.Title,
                           IsStarred = e.IsStarrred,
                           CreatedUtc = e.CreatedUtc
                       }
                    );

                return query.ToArray(); //This executes the query to get data out of the database.
            }
        }

        public NoteDetail GetNoteById(int noteId)//this is outside of MVC.  We control it, not the controller
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Notes
                    .Single(e => e.NoteID == noteId && e.OwnerId == _userId);//single forces it to execute, unlike an array

                return
                    new NoteDetail
                    {
                        NoteID = entity.NoteID,
                        Title = entity.Title,
                        Content = entity.Content,
                        IsStarred = entity.IsStarrred,//Why????
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc,
                    };

            }
        }
        public bool UpdateNote(NoteEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Notes
                    .Single(e => e.NoteID == model.NoteID && e.OwnerId == _userId);

                entity.Title = model.Title;
                entity.Content = model.Content;
                entity.IsStarrred = model.IsStarred;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteNote(int noteId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                      ctx
                      .Notes
                      .Single(e => e.NoteID == noteId && e.OwnerId == _userId);

                ctx.Notes.Remove(entity);

                return ctx.SaveChanges() == 1;

            }
        }
    }
}
