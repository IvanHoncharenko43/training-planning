using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Calendar.Model;

namespace Calendar
{
    public partial class MainWindow : Window
    {
        private List<TrainingNote> trainingNotes;
        private readonly string NotesFile;

        public MainWindow()
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Calendar");
            NotesFile = Path.Combine(appDataPath, "training_notes.json");

            InitializeComponent();
            LoadNotes();
        }

        private void LoadNotes()
        {
            try
            {
                trainingNotes = new List<TrainingNote>();
                if (File.Exists(NotesFile))
                {
                    string json = File.ReadAllText(NotesFile);
                    trainingNotes = JsonConvert.DeserializeObject<List<TrainingNote>>(json) ?? new List<TrainingNote>();
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(NotesFile));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при завантаженні нотаток: {ex.Message}");
                trainingNotes = new List<TrainingNote>();
            }
        }

        private void SaveNotes()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(NotesFile));
                string json = JsonConvert.SerializeObject(trainingNotes, Formatting.Indented);
                File.WriteAllText(NotesFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні нотаток: {ex.Message}");
            }
        }

        private void WorkoutCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WorkoutCalendar.SelectedDate.HasValue)
            {
                DetailsPanel.Visibility = Visibility.Visible;
                DateTime selectedDate = WorkoutCalendar.SelectedDate.Value;

                var note = trainingNotes.Find(n => n.Date.Date == selectedDate.Date && n.UserId == LoginWindow.CurrentUser.Id);
                if (note != null)
                {
                    WorkoutCheckBox.IsChecked = note.WasTraining;
                    WeightTextBox.Text = note.Weight?.ToString() ?? string.Empty;
                    NotesTextBox.Text = note.Description;
                }
                else
                {
                    WorkoutCheckBox.IsChecked = false;
                    WeightTextBox.Text = string.Empty;
                    NotesTextBox.Text = string.Empty;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (WorkoutCalendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = WorkoutCalendar.SelectedDate.Value;
                double? weight = double.TryParse(WeightTextBox.Text, out double w) ? w : (double?)null;

                var existingNote = trainingNotes.Find(n => n.Date.Date == selectedDate.Date && n.UserId == LoginWindow.CurrentUser.Id);
                if (existingNote != null)
                {
                    existingNote.WasTraining = WorkoutCheckBox.IsChecked ?? false;
                    existingNote.Weight = weight;
                    existingNote.Description = NotesTextBox.Text;
                }
                else
                {
                    var newNote = new TrainingNote
                    {
                        Id = trainingNotes.Count + 1,
                        UserId = LoginWindow.CurrentUser.Id,
                        Date = selectedDate,
                        WasTraining = WorkoutCheckBox.IsChecked ?? false,
                        Weight = weight,
                        Description = NotesTextBox.Text
                    };
                    trainingNotes.Add(newNote);
                }

                SaveNotes();
                MessageBox.Show("Дані збережено!");
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            Close();
        }
    }
}