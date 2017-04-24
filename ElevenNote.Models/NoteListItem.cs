using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
   public class NoteListItem//Name is specific on the intent.  
    { //Keep independent, because using in different ways.  This is a list page, not inherit from Note
        public int NoteId { get; set; }//This will be a collection, a list of notes.  Id to tell which one to edit

        public string Title { get; set; }

        [UIHint("Star")]
        public bool IsStarred { get; set; } 

        [Display(Name = "Created")]
        public DateTimeOffset CreatedUtc { get; set; }

        public override string ToString() => $"[{NoteId}] {Title}";
        
    }
}
