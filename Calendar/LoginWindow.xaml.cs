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
using System.Text.Json;

namespace Calendar
{
    using System.Text.Json;
    public partial class LoginWindow : Window
    {
        private List<UserModel> users;
        private readonly string UsersFile;
        
        private HttpClient _httpClient;
        public LoginWindow()
        {
            // Визначаємо шлях до файлу в директорії %appdata%/Calendar
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Calendar");
            UsersFile = Path.Combine(appDataPath, "users.json");

            InitializeComponent();
            LoadUsers();
            _httpClient = new HttpClient();
            CurrentUser = new();
        }

        public static UserModel CurrentUser { get; set; }
        public static string UserToken { get; set; }

        private void LoadUsers()
        {
            try
            {
                users = new List<UserModel>();
                if (File.Exists(UsersFile))
                {
                    string json = File.ReadAllText(UsersFile);
                    users = JsonConvert.DeserializeObject<List<UserModel>>(json) ?? new List<UserModel>();
                }
                else
                {
                    // Створюємо директорію, якщо її немає
                    Directory.CreateDirectory(Path.GetDirectoryName(UsersFile));
                }
            }
            catch (Exception ex)
            {
                // Замість викидання діалогу просто логуємо помилку (можна додати логування в файл)
                Console.WriteLine($"Помилка при завантаженні користувачів: {ex.Message}");
                users = new List<UserModel>(); // Ініціалізуємо порожній список
            }
        }

        private void SaveUsers()
        {
            try
            {
                // Переконаємося, що директорія існує
                Directory.CreateDirectory(Path.GetDirectoryName(UsersFile));

                string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText(UsersFile, json);
            }
            catch (Exception ex)
            {
                // Замість викидання діалогу логуємо помилку
                Console.WriteLine($"Помилка при збереженні користувачів: {ex.Message}");
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = LoginEmailTextBox.Text;
            string password = LoginPasswordBox.Password;
            await Login(email, password);
            /*var user = users.Find(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                CurrentUser = user;
                MenuWindow menuWindow = new MenuWindow();
                menuWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Невірний email або пароль!", "Помилка входу", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/
        }

        private async Task Login(string username, string password)
        {
            var newUser = new UserModel
            {
                Email = username,
                Password = password
            };
            string json = JsonSerializer.Serialize(newUser);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:8080/auth/login", content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    CurrentUser.Email = username;
                    // CurrentUser.Username = username;
                    UserToken = await response.Content.ReadAsStringAsync();
                    MenuWindow menuWindow = new MenuWindow();
                    menuWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Невірний email або пароль!", "Помилка входу", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Сервер недоступний. Спробуйте пізніше");
            }
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string name = RegisterNameTextBox.Text;
            string email = RegisterEmailTextBox.Text;
            string password = RegisterPasswordBox.Password;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заповніть усі поля!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            /*if (users.Exists(u => u.Email == email))
            {
                MessageBox.Show("Користувач із таким email уже існує!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }*/
            await Register(name, email, password);
        }

        private async Task Register(string name, string email, string password)
        {
            var newUser = new UserModel
            {
                Name = name,
                Email = email,
                Password = password
            };
            string json = JsonSerializer.Serialize(newUser);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:8080/auth/register", content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    MessageBox.Show("Реєстрація успішна! Тепер увійдіть.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    CurrentUser.Name = name;
                    // CurrentUser.Username = username;
                }
                else
                {
                    string problemMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(problemMessage);
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Сервер недоступний. Спробуйте пізніше");
            }
        }
    }
}