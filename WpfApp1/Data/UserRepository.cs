using WpfApp1.Model;

namespace WpfApp1.Data;

public class UserRepository
{
    public void Register(string name, string email, string password)
    {
        if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(password))
        {
            return;
        }
        using (var context = new AppDbContext())
        {
            var user = new UserModel();
            user.Name = name;
            user.Email = email;
            user.Password = password;
            context.Users.Add(user);
        }
    }

    public bool CheckIfExists(string email, string password)
    {
        if (String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        using (var context = new AppDbContext())
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            if (user.Email is null)
            {
                return false;
            }
            //Можливо викликати метод пов'язаний з хешуванням пароля
            return user.Password == password;
        }
    }
}