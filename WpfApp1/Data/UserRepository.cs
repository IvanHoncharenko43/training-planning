using WpfApp1.Model;

namespace WpfApp1.Data;

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public UserRepository()
    {
    }

    public void Register(string name, string email, string password)
    {
        var user1 = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user1 == null)
        {
            var user = new UserModel();
            user.Name = name;
            user.Email = email;
            user.Password = password;
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }

    public bool CheckIfExists(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null)
        {
            return false;
        }
        return user.Password == password;
    }
}