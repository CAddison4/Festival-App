using System.ComponentModel.DataAnnotations;
using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.ViewModel
{
    public class UserRoleVM
    {
        private User u;

        public UserRoleVM(User u)
        {
            this.u = u;
        }

        [Key]
        public int ID { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
