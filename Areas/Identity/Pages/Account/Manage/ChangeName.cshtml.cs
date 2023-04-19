// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeamRedInternalProject.Models;
using TeamRedInternalProject.Repositories;

namespace TeamRedInternalProject.Areas.Identity.Pages.Account.Manage
{
    public class ChangeNameModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ConcertContext _concertContext;
        private readonly UserRepo _userRepo;

        public ChangeNameModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ConcertContext concertContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _concertContext = concertContext;
            _userRepo = new UserRepo(concertContext);
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {

            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }
        }
        private async Task LoadAsync(IdentityUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            User gettingUser = _userRepo.GetUsersByEmail(email);
            var userEdit = _userRepo.EditUser(gettingUser);

            Email = email;

            Input = new InputModel
            {
                FirstName = userEdit.FirstName, 
                LastName = userEdit.LastName
            };

    }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);

            //var hasPassword = await _userManager.HasPasswordAsync(user);

            //if (hasPassword)
            //{
            //    return RedirectToPage("./ChangePassword");
            //}

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            User userInfo = _userRepo.GetUsersByEmail(user.UserName);

            if (userInfo == null)
            {
                return Page();
            }

            userInfo.FirstName = Input.FirstName;
            userInfo.LastName = Input.LastName;

            var userEdit = _userRepo.EditUser(userInfo);



            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your name has been Changed.";

            return RedirectToPage();
        }
    }
}
