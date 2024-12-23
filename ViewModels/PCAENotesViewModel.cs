using CommunityToolkit.Mvvm.Input;
using Notes.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class PCAENotesViewModel : IQueryAttributable
{
    public ObservableCollection<ViewModels.PCAENoteViewModel> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    public PCAENotesViewModel()
    {
        AllNotes = new ObservableCollection<ViewModels.PCAENoteViewModel>(Models.Note.LoadAll().Select(n => new PCAENoteViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<ViewModels.PCAENoteViewModel>(SelectNoteAsync);
    }

    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.NotePage));
    }

    private async Task SelectNoteAsync(ViewModels.PCAENoteViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.Identifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            PCAENoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note exists, delete it
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            PCAENoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note is found, update it
            if (matchedNote != null)
                matchedNote.Reload();

            // If note isn't found, it's new; add it.
            else
                AllNotes.Add(new PCAENoteViewModel(Note.Load(noteId)));
        }
    }
}