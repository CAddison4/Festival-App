using TeamRedInternalProject.Models;
using TeamRedInternalProject.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace TeamRedInternalProject.Repositories
{
    public class RoleRepo
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcertContext _db;
        private RoleManager<IdentityRole> _roleManager;
        public RoleRepo(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _db = new();
            _roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        }

        /// <summary>
        /// Get all AspRoles in the db
        /// </summary>
        /// <returns>list of RoleVM objects</returns>
        public List<RoleVM> GetAllRoles()
        {

            var roles = _roleManager.Roles;
            List<RoleVM> roleList = new List<RoleVM>();

            foreach (var role in roles)
            {
                roleList.Add(new RoleVM()
                {
                    RoleName = role.Name,
                    Id = role.Id
                });
            }
            return roleList;
        }

        /// <summary>
        /// Create a new AspRole
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>True when created</returns>
        public async Task<bool> CreateRole(string roleName)
        {
            await _roleManager.CreateAsync(new IdentityRole
            {
                Name = roleName,
                Id = roleName,
                NormalizedName = roleName.ToUpper()
            });
            _db.SaveChanges();
            return true;
        }

        // Seed AspNet Roles with option to 
        public async Task<bool> CreateInitialRoles()
        {
            string roleName = "Admin";
            var created = await this.CreateRole(roleName);

            if (!created)
            {
                return false;
            }
            return true;
        }
    }

}

