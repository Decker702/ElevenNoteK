using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
    public class NoteCreate
    {
        [Required]
        [MinLength(2, ErrorMessage = "Please enter at least two characters.")]
        [MaxLength(128, ErrorMessage = "You are limited to 128 characters.")]
        public string Title { get; set; }

        [Required]//you can't have just an empty string.  
        [MaxLength(8000)]
        public string Content { get; set;}

        public override string ToString() => Title;
       
    }
}
