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

