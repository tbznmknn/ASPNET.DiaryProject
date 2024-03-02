using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RadnaaProject.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _sManager;
        public LogoutModel(SignInManager<IdentityUser> signInManager)
        {
            _sManager = signInManager;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(string ReturnUrl = null)
        {
            await _sManager.SignOutAsync();//log out hiine
            if (ReturnUrl != null)
            {
                return LocalRedirect(ReturnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
