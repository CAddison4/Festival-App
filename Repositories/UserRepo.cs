using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.Repositories
{

    public class UserRepo
    {
        private readonly ConcertContext _db;
        public UserRepo()
        {
            _db = new();
        }
        public List<User> GetUsers()
        {
            return _db.Users.ToList();

        }

        public User GetUsersByEmail(string email)
        {
            User? user = _db.Users.Where(u => u.Email == email).FirstOrDefault();

            if (user == null)
            {
                throw (new Exception("Email does not exist"));
            }

            return user;
        }

        public User EditUser(User newUser)
        {
            User? user = _db.Users.Where(u => u.Email == newUser.Email).FirstOrDefault();

            if (user == null)
            {
                throw (new Exception("User does not exist"));
            }
            try
            {
                user.LastName = newUser.LastName;
                user.FirstName = newUser.FirstName;
                user.Admin = newUser.Admin;

                _db.SaveChanges();
            }
            catch
            {
                throw (new Exception("Could not update user"));
            }

            return user;
        }

        public User CreateUser(User user)
        {
            try
            {
                _db.Users.Add(user);
                _db.SaveChanges();
            }
            catch
            {
                throw (new Exception("Could not add user"));
            }

            return user;
        }
    }
}
