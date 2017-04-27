using ElevenNote.MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ElevenNote.MobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotesPage : ContentPage
    {
        #region Class Vars and Constructor

        private List<NoteListItemViewModel> Notes { get; set; }

        public NotesPage()
        {
            InitializeComponent();
            SetupUi();
        }
        #endregion

        #region Utility
        //Sets up the user interface without confusing the designer

        private void SetupUi()
        {
            //Wire up refreshing.
            lvwNotes.IsPullToRefreshEnabled = true;
            lvwNotes.Refreshing += async (o, args) =>
            {
                await PopulateNotesList();
                lvwNotes.IsRefreshing = false;
                lvwNotes.IsVisible = !Notes.Any();
            };
        }
        #endregion

        /// <summary>
        /// Updates the notes list view.
        /// </summary>
        /// <returns></returns>
        private async Task PopulateNotesList()
        {
            await App
                .NoteService
                .GetAll()
                .ContinueWith(task =>
                {
                    var notes = task.Result;

                    Notes = notes
                        .OrderByDescending(note => note.IsStarred) // descending because 1 is greater than 0, and true == 1
                        .ThenByDescending(note => note.CreatedUtc) // show newest notes first
                        .Select(s => new NoteListItemViewModel
                        {
                            NoteId = s.NoteId,
                            Title = s.Title,
                            StarImage = s.IsStarred ? "starred.png" : "notstarred.png"
                        })
                        .ToList();

                    lvwNotes.ItemsSource = Notes;

                    // Clear any item selection.
                    lvwNotes.SelectedItem = null;

                }, TaskScheduler.FromCurrentSynchronizationContext());

        }




        /*  public NotesPage ()
          {
              InitializeComponent ();
          }
          */
    }
}
