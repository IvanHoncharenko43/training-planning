using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Calendar.Model;
using Calendar.Requests;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;

namespace Calendar
{
    public partial class MainWindow : Window
    {
        private List<TrainingNote> trainingNotes;
        private readonly string NotesFile;
        private TrainingRequests _trainingRequests;

        public MainWindow()
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Calendar");
            NotesFile = Path.Combine(appDataPath, "training_notes.json");
            _trainingRequests = new();
            InitializeComponent();
            LoadNotes();
            BuildWeightPlot();
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

        private async Task BuildWeightPlot()
        {
            // Фільтруємо нотатки для поточного користувача та з наявною вагою
            // var userNotes = trainingNotes
            //     .Where(n => n.UserId.Id == LoginWindow.CurrentUser?.Id && n.Weight.HasValue)
            //     .OrderBy(n => n.Date) // Сортуємо за датою (від найдавніших до найновіших)
            //     .ToList();

            var userNotes = await _trainingRequests.GetAllNotes();
            if (userNotes.Count < 1)
            {
                return;
            }
            userNotes = userNotes.OrderBy(n => n.Date).ToList();
            // Створюємо модель графіка
            var plotModel = new PlotModel { Title = "Графік ваги" };

            // Налаштування осі X (дати)
            var dateAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Дата",
                Angle = 45 // Похилимо мітки, щоб дати не перекривалися
            };

            // Додаємо дати до осі X
            foreach (var note in userNotes)
            {
                dateAxis.Labels.Add(note.Date.ToString("dd.MM.yyyy"));
            }
            plotModel.Axes.Add(dateAxis);

            // Налаштування осі Y (вага, кг)
            var weightAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Вага (кг)",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            };
            plotModel.Axes.Add(weightAxis);

            // Додаємо лінію, що з’єднує всі точки
            var lineSeries = new LineSeries
            {
                Color = OxyColors.Gray, // Лінія сірого кольору для всіх точок
                LineStyle = LineStyle.Solid
            };

            // Додаємо точки з різними кольорами
            var trainingSeries = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerFill = OxyColors.Red // Червоний для тренувань
            };

            var noTrainingSeries = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerFill = OxyColors.Blue // Синій для відсутності тренувань
            };

            // Додаємо точки до відповідних серій
            for (int i = 0; i < userNotes.Count; i++)
            {
                var note = userNotes[i];
                double x = i; // Індекс дати
                double y = note.Weight.Value;

                lineSeries.Points.Add(new DataPoint(x, y)); // Додаємо до лінії

                if (note.WasTraining)
                {
                    trainingSeries.Points.Add(new ScatterPoint(x, y));
                }
                else
                {
                    noTrainingSeries.Points.Add(new ScatterPoint(x, y));
                }
            }

            // Додаємо серії до графіка
            plotModel.Series.Add(lineSeries);
            plotModel.Series.Add(trainingSeries);
            plotModel.Series.Add(noTrainingSeries);

            // Призначаємо модель графіку
            WeightPlot.Model = plotModel;
        }

        private async void WorkoutCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WorkoutCalendar.SelectedDate.HasValue)
            {
                DetailsPanel.Visibility = Visibility.Visible;
                DateTime selectedDate = WorkoutCalendar.SelectedDate.Value;

                SelectedDateTextBlock.Text = $"Обрана дата: {selectedDate.ToString("dd.MM.yyyy")}";

                // var note = trainingNotes.Find(n => n.Date.Date == selectedDate.Date && n.UserId == LoginWindow.CurrentUser.Id);

                var note = await _trainingRequests.GetNote(selectedDate);
                Console.WriteLine(note.Description);
                if (note != null)
                {
                    WorkoutCheckBox.IsChecked = note.WasTraining;
                    WeightTextBox.Text = note.Weight?.ToString() ?? string.Empty;
                    NotesTextBox.Text = note.Description;

                    if (note.WasTraining)
                    {
                        NotesLabel.Visibility = Visibility.Visible;
                        NotesTextBox.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        NotesLabel.Visibility = Visibility.Collapsed;
                        NotesTextBox.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    WorkoutCheckBox.IsChecked = false;
                    WeightTextBox.Text = string.Empty;
                    NotesTextBox.Text = string.Empty;
                    NotesLabel.Visibility = Visibility.Collapsed;
                    NotesTextBox.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void WorkoutCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            NotesLabel.Visibility = Visibility.Visible;
            NotesTextBox.Visibility = Visibility.Visible;
        }

        private void WorkoutCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            NotesLabel.Visibility = Visibility.Collapsed;
            NotesTextBox.Visibility = Visibility.Collapsed;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (WorkoutCalendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = WorkoutCalendar.SelectedDate.Value;
                double? weight = double.TryParse(WeightTextBox.Text, out double w) ? w : (double?)null;

                // var existingNote = trainingNotes.Find(n => n.Date.Date == selectedDate.Date && n.UserId == LoginWindow.CurrentUser.Id);
                //
                // if (existingNote != null)
                // {
                //     existingNote.WasTraining = WorkoutCheckBox.IsChecked ?? false;
                //     existingNote.Weight = weight;
                //     existingNote.Description = NotesTextBox.Text;
                // }
                // else
                // {
                    var newNote = new TrainingNote()
                    {
                        // Id = trainingNotes.Count + 1,
                        // UserId = LoginWindow.CurrentUser.Id,
                        Date = selectedDate,
                        WasTraining = WorkoutCheckBox.IsChecked ?? false,
                        Weight = weight,
                        Description = NotesTextBox.Text
                    };
                //     trainingNotes.Add(newNote);
                // }

                bool success = await _trainingRequests.SaveNote(newNote);
                // SaveNotes();
                if (success)
                {
                    MessageBox.Show("Дані збережено!");
                    await BuildWeightPlot(); // Оновлюємо графік після збереження
                }
                else
                {
                    MessageBox.Show("Помилка. Cпробуйте пізніше");
                }
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