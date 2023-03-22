using Microsoft.AspNetCore.Identity;
using TeamRedInternalProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TeamRedInternalProject.Data;

namespace TeamRedInternalProject.Repositories
{
    public class UserRoleRepo
    {
        IServiceProvider serviceProvider;
        private readonly ConcertContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public UserRoleRepo(IServiceProvider serviceProvider, ConcertContext db)
        {
            this.serviceProvider = serviceProvider;
            _db = db;
            _userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        }

        public async Task<List<string>> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.ToList();
            }
            return new List<string>();
        }



        // Assign a role to a user.
        public async Task<bool> AddUserRole(string email, string userRole)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, userRole);
                //this.UpdateUser(email);
                return true;
            }

            return false;
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
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
