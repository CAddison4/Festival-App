using TeamRedInternalProject.Models;
using TeamRedInternalProject.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace TeamRedInternalProject.Repositories
{
    public class RoleRepo
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcertContext _db;
        public RoleRepo(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _db = new();
        }
        
        public List<RoleVM> GetAllRoles()
        {
            var roles = _db.Roles;
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

        public bool CreateRole(string roleName)
        {
            _db.Roles.Add(new IdentityRole
            { 
                Name = roleName,
                Id= roleName, 
                NormalizedName = roleName.ToUpper()
            });
            _db.SaveChanges();
            return true;
        }

        public bool CreateInitialRoles()
        {
            string roleName = "Admin";
            var created = CreateRole(roleName);

            if (!created)
            {
                return false;
            }
            return true;
        }
    }

}

