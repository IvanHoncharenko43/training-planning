using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Model;
using WpfApp1.Services;

namespace WpfApp1.View;

public partial class HomeView : UserControl
{
    private readonly AppDbContext _context;
    private readonly INavigationService _navigationService;

    public HomeView(AppDbContext context, INavigationService navigationService)
    {
        InitializeComponent();
        _context = context;
        _navigationService = navigationService;
    }

    private void TrainingCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TrainingCalendar.SelectedDate.HasValue)
        {
            NotePanel.Visibility = Visibility.Visible;
            LoadNote(TrainingCalendar.SelectedDate.Value);
        }
    }

    private void LoadNote(DateTime date)
    {
        int userId = _navigationService.CurrentUserId;
        if (userId == 0)
        {
            MessageBox.Show("Будь ласка, увійдіть в акаунт.");
            return;
        }

        var note = _context.TrainingNotes.FirstOrDefault(n => n.UserId == userId && n.Date.Date == date.Date);
        if (note != null)
        {
            WasTrainingCheckBox.IsChecked = note.WasTraining;
            WeightTextBox.Text = note.Weight?.ToString() ?? "";
            DescriptionTextBox.Text = note.Description ?? "";
        }
        else
        {
            WasTrainingCheckBox.IsChecked = false;
            WeightTextBox.Text = "";
            DescriptionTextBox.Text = "";
        }
    }

    private void SaveNote_Click(object sender, RoutedEventArgs e)
    {
        int userId = _navigationService.CurrentUserId;
        if (userId == 0)
        {
            MessageBox.Show("Будь ласка, увійдіть в акаунт.");
            return;
        }

        var date = TrainingCalendar.SelectedDate.Value;
        var note = _context.TrainingNotes.FirstOrDefault(n => n.UserId == userId && n.Date.Date == date.Date);

        if (note == null)
        {
            note = new TrainingNote
            {
                UserId = userId,
                Date = date
            };
            _context.TrainingNotes.Add(note);
        }

        note.WasTraining = WasTrainingCheckBox.IsChecked ?? false;
        if (double.TryParse(WeightTextBox.Text, out double weight))
            note.Weight = weight;
        else
            note.Weight = null;
        note.Description = DescriptionTextBox.Text;

        _context.SaveChanges();
        NotePanel.Visibility = Visibility.Collapsed;
    }

    private void CloseNotePanel_Click(object sender, RoutedEventArgs e)
    {
        NotePanel.Visibility = Visibility.Collapsed;
    }
}