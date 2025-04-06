using WpfApp1.Model;

namespace WpfApp1.Data;

public class UserRepository
{
    public void Register(string name, string email, string password)
    {
        using (var context = new AppDbContext())
        {
            var user1 = context.Users.FirstOrDefault(u => u.Email == email);
            if (user1 == null)
            {
                var user = new UserModel();
                user.Name = name;
                user.Email = email;
                user.Password = password;
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }

    public bool CheckIfExists(string email, string password)
    {
        using (var context = new AppDbContext())
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return false;
            }
            //Можливо викликати метод пов'язаний з хешуванням пароля
            return user.Password == password;
        }
    }
}