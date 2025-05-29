using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Calendar.Model;

namespace Calendar
{
    public partial class LoginWindow : Window
    {
        private List<UserModel> users;
        private readonly string UsersFile;

        public LoginWindow()
        {
            // Визначаємо шлях до файлу в директорії %appdata%/Calendar
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Calendar");
            UsersFile = Path.Combine(appDataPath, "users.json");

            InitializeComponent();
            LoadUsers();
        }

        public static UserModel CurrentUser { get; set; }

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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = LoginEmailTextBox.Text;
            string password = LoginPasswordBox.Password;

            var user = users.Find(u => u.Email == email && u.Password == password);
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
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string name = RegisterNameTextBox.Text;
            string email = RegisterEmailTextBox.Text;
            string password = RegisterPasswordBox.Password;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заповніть усі поля!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (users.Exists(u => u.Email == email))
            {
                MessageBox.Show("Користувач із таким email уже існує!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newUser = new UserModel
            {
                Id = users.Count + 1,
                Name = name,
                Email = email,
                Password = password
            };

            users.Add(newUser);
            SaveUsers();

            MessageBox.Show("Реєстрація успішна! Тепер увійдіть.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}