using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Calendar.Model;

public class UserModel
{
    [Key]
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
}