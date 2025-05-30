using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Windows;
using Calendar.Model;

namespace Calendar.Requests;

public class TrainingRequests
{
    private HttpClient _httpClient;

    public TrainingRequests()
    {
        _httpClient = new HttpClient();
        string token = LoginWindow.UserToken;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    }

    public async Task<bool> SaveNote(TrainingNote trainingNote)
    {
        trainingNote.Date = trainingNote.Date.Date;
        string json = JsonSerializer.Serialize(trainingNote);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            HttpResponseMessage response = await _httpClient.PutAsync("http://localhost:8080/workout/save", content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
        }
        catch (HttpRequestException){}
        return false;
    }
    
    public async Task<TrainingNote> GetNote(DateTime date)
    {
        var note = new TrainingNote()
        {
            Date = date,
            // UserId = LoginWindow.CurrentUser
        };
        var note1 = new NoteModel(date.ToString("yyyy-MM-dd")); 
        string json = JsonSerializer.Serialize(note1);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        
        TrainingNote currentNote = new();
        try
        {
            string currentDate = date.ToString("yyyy-MM-dd");
            Console.WriteLine(note1.Date);
            HttpResponseMessage response = await _httpClient.PutAsync("http://localhost:8080/workout/get", content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                currentNote = JsonSerializer.Deserialize<TrainingNote>(responseString);
            }
        }
        catch (HttpRequestException) {}
        return currentNote;
    }
}