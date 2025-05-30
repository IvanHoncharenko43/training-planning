using System.Text.Json.Serialization;

namespace Calendar.Model;

public class NoteModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("userId")]
    public UserModel UserId { get; set; } 
    [JsonPropertyName("date")]
    public string Date { get; set; }
    
    [JsonPropertyName("hasTrained")]
    public bool WasTraining { get; set; }
    
    [JsonPropertyName("weight")]
    public double? Weight { get; set; } 
    
    [JsonPropertyName("notes")]
    public string Description { get; set; }

    public NoteModel(string date)
    {
        Date = date;
    }
}