using Microsoft.AspNetCore.Identity;
using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.Repositories
{
    public class UserRoleRepo
    {
        IServiceProvider serviceProvider;
        private readonly ConcertContext _db;
        public UserRoleRepo(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            _db = new();
        }

        // Assign a role to a user.
        public async Task<bool> AddUserRole(string email, bool admin)
        {
            var UserManager = serviceProvider
                .GetRequiredService<UserManager<IdentityUser>>();
            var user = await UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                await UserManager.AddToRoleAsync(user, "Admin");
            }
            return true;
        }

        public bool UpdateUser(string email)
        {
            UserRepo userRepo = new UserRepo();
            User user = userRepo.GetUsersByEmail(email);
            
            try
            {
                user.Admin = true;

                _db.Update(user);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return false;
        }

        // Remove role from a user.
        public async Task<bool> RemoveUserRole(string email, string roleName)
        {
            var UserManager = serviceProvider
                .GetRequiredService<UserManager<IdentityUser>>();
            var user = await UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                await UserManager.RemoveFromRoleAsync(user, roleName);
            }
            return true;
        }



    }
}
