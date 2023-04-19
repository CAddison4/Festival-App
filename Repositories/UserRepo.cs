using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.Repositories
{

    public class UserRepo
    {
        private readonly ConcertContext _db;
        public UserRepo(ConcertContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get all users in the db user table (not AspUsers). 
        /// Includes data such as first and last name. Tied to AspUsers table by uniquq email.
        /// </summary>
        /// <returns>List of User objects</returns>
        public List<User> GetUsers()
        {
            return _db.Users.ToList();

        }

        /// <summary>
        /// Get a specific user by unique email address.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User object</returns>
        public User GetUsersByEmail(string email)
        {
            User? user = _db.Users.Where(u => u.Email == email).FirstOrDefault();

            if (user == null)
            {
                throw (new Exception("Email does not exist"));
            }

            return user;
        }

        /// <summary>
        /// Edit a user's details.
        /// </summary>
        /// <param name="newUser">New user object data</param>
        /// <returns>The updated user object</returns>
        /// <throws>Exception if user does not exist or the user could not be updated</throws>
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

        /// <summary>
        /// Create a new user entry in the users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The created user object</returns>
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
