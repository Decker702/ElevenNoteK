using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Data
{
   public class Note
    {//This is where our data will be stored.  Just data

        [Key]
        public int NoteID { get; set; }

        [Required]//only effects the immediate language element directly below it.
        public Guid OwnerId { get; set; }//Guid - globally unique identifier.  Computer generated id for user. Keeps user notes private to id.

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [DefaultValue(false)] //added it after database was created, not need to add it to the database.
        public bool IsStarrred { get; set; }

        [Required]
        public DateTimeOffset CreatedUtc { get; set; } //Set Time Zone - if you standardize on UTC/GMT it will make your life easier.
            //Offset will give you time zone information converted off UTC

        public DateTimeOffset? ModifiedUtc { get; set; } //ModifiedUtc is a good way to audit and compare time stamps
        //The question mark at end of DateTimeOffSet - changes it to a reference type and it can be null. (See value & reference types in notes)


    }
}
