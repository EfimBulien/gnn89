using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace DailyPlanner
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Note> notes;
        private string filePath = "notes.json";

        public MainWindow()
        {
            InitializeComponent();
            notes = JsonSerializer.Deserialize<ObservableCollection<Note>>(filePath) ?? new ObservableCollection<Note>();
            noteListBox.ItemsSource = notes;
            datePicker.SelectedDate = DateTime.Today;
            UpdateNotesList();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateNotesList();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Note newNote = new Note
            {
                Title = titleTextBox.Text,
                Description = descriptionTextBox.Text,
                Date = datePicker.SelectedDate ?? DateTime.Today
            };
            notes.Add(newNote);
            JsonSerializer.Serialize(notes, filePath);
            UpdateNotesList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (noteListBox.SelectedItem != null)
            {
                Note selectedNote = (Note)noteListBox.SelectedItem;
                selectedNote.Title = titleTextBox.Text;
                selectedNote.Description = descriptionTextBox.Text;
                JsonSerializer.Serialize(notes, filePath);
                UpdateNotesList();
            }
        }

        private void UpdateNotesList()
        {
            DateTime selectedDate = datePicker.SelectedDate ?? DateTime.Today;
            var notesForSelectedDate = notes.Where(note => note.Date.Date == selectedDate.Date);
            noteListBox.ItemsSource = notesForSelectedDate;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (noteListBox.SelectedItem != null)
            {
                notes.Remove((Note)noteListBox.SelectedItem);
                JsonSerializer.Serialize(notes, filePath);
                UpdateNotesList();
            }
        }

        private void ResetNotesButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите сбросить все заметки?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                notes.Clear();
                JsonSerializer.Serialize(notes, filePath);
                UpdateNotesList();
            }
        }

        private void noteListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (noteListBox.SelectedItem != null)
            {
                Note selectedNote = (Note)noteListBox.SelectedItem;
                NoteDescriptionWindow descriptionWindow = new NoteDescriptionWindow();
                descriptionWindow.descriptionTextBlock.Text = selectedNote.Description;
                descriptionWindow.ShowDialog();
                UpdateNotesList();
            }
        }
    }
}