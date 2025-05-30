namespace Calendar.Model;

public class TrainingNote
{
    public int Id { get; set; }
    public int UserId { get; set; } 
    public DateTime Date { get; set; }
    public bool WasTraining { get; set; }
    public double? Weight { get; set; } 
    public string Description { get; set; }
}