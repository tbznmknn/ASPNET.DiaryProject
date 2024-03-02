using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RadnaaProject.Models;
using System.ComponentModel.DataAnnotations;

namespace RadnaaProject.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserContext _UContext;
        public LoginModel(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, UserContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _UContext=context;
        }




        [BindProperty]
        [Required(ErrorMessage = "Хэрэглэгчийн нэрээ оруулна уу")]
        [StringLength(30, ErrorMessage = "Хэрэглэгчийн нэр 30 тэмдэгтээс уртгүй байна")]
        public string Username { get; set; } = "";
        [BindProperty]
        [Required(ErrorMessage = "Нууц үгээ оруулна уу")]
        [StringLength(30, ErrorMessage = "Нууц үг 30 тэмдэгтээс уртгүй байна")]
        public string Password { get; set; } = "";
        [BindProperty]
        public bool RememberMe { get; set; }

        public string result = "";
        public string errmsg = "";
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {

                var urdun = await _signInManager.PasswordSignInAsync(Username, Password, RememberMe, lockoutOnFailure: true);
                if (urdun.Succeeded)
                {
                    return RedirectToPage("/Home");
                }
                else
                {
                    result = "failed";
                    var myuser = _UContext.Users.Where(u => u.UserName == Username).FirstOrDefault();
                    if (myuser != null)
                    {
                        int faildsentoo = myuser.AccessFailedCount;
                        errmsg = "Танд одоо " + (10 - faildsentoo) + " оролдлого үлдлээ.";
                    }
                }
                if (urdun.IsLockedOut)
                {
                    return RedirectToPage("/Index");
                }
            }
            return Page();
        }
    }
}
