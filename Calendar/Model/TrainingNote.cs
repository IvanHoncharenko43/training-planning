namespace Calendar.Model;

public class TrainingNote
{
    public int Id { get; set; }
    public int UserId { get; set; } // Посилання на UserModel.Id
    public DateTime Date { get; set; }
    public bool WasTraining { get; set; }
    public double? Weight { get; set; } // Nullable, бо вага може бути відсутньою
    public string Description { get; set; }
}