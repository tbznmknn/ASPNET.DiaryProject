using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RadnaaProject.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RadnaaProject.Pages
{
    public class UserRegistrationModel : PageModel
    {
        public void OnGet()
        {
        }
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserContext _UContext;
        public UserRegistrationModel(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, UserContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _UContext = context;
        }
        [BindProperty]
        [Required(ErrorMessage = "Хэрэглэгчийн нэрээ оруулна уу")]
        [StringLength(30, ErrorMessage = "Хэрэглэгчийн нэр 30 тэмдэгтээс уртгүй байна")]
        public string Username { get; set; } = "";
        [BindProperty]
       
        public string mobileNumber { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "Цахим шуудангаа оруулна уу.")]
        public string In_Mail { get; set; } = "";
        [BindProperty]
        [Required(ErrorMessage = "Нууц үгээ оруулна уу")]
        [StringLength(30, ErrorMessage = "Нууц үг 30 тэмдэгтээс уртгүй байна")]
        public string Password { get; set; } = "";
        [BindProperty]
        [Required(ErrorMessage = "Нууц үгийн давталтыг оруулна уу")]
        [Compare("Password", ErrorMessage = "нууц үгийн давталт буруу байна")]
        public string PasswordConfirmation { get; set; } = "";
        public string result = "";
        public async Task<IActionResult> OnPostUserRegister()
        {
            if (ModelState.IsValid)
            {
                IdentityUser PendingUser = new IdentityUser();
                PendingUser.UserName = Username;
                PendingUser.Email = In_Mail;
                PendingUser.PhoneNumber = mobileNumber;
                var urdun=await _userManager.CreateAsync(PendingUser, Password);
                if (urdun.Succeeded)
                {
                    result = "success";
                }
                else
                {
                    result = "error";
                }
                
            }
            return Page();
        }
    }
}
