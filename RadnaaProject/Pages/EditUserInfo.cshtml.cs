using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RadnaaProject.Models;
using System.ComponentModel.DataAnnotations;

namespace RadnaaProject.Pages
{
    public class EditUserInfoModel : PageModel
    {
        public void OnGet()
        {
        }
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserContext _UContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EditUserInfoModel(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, UserContext context, IWebHostEnvironment webHostEnvironment)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _UContext = context;
            _webHostEnvironment = webHostEnvironment;
        }
  
        [BindProperty]
        public string? mobileNumber { get; set; }

        [BindProperty]
        public string? Email { get; set; }

        [BindProperty]
        public IFormFile? zurag { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Нууц үгээ оруулна уу")]
        [StringLength(30, ErrorMessage = "Нууц үг 30 тэмдэгтээс уртгүй байна")]

        public string? Password { get; set; }
        [BindProperty]
        public string? NewPassword { get; set; }

        public string result = "";
        public async Task<IActionResult> OnPostUpdate()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var passwordCheckResult = await _userManager.CheckPasswordAsync(user, Password);
                    if (Password != null && passwordCheckResult)
                    {
                        string userId = user.Id;

                        
                        //Huuchin zurgiig ustgaad, shineer local folderd random nertei zurag upload hiigdene.
                        if (zurag != null && zurag.Length > 0)
                        {
                            
                            var wwwrootPath = Path.Combine(_webHostEnvironment.WebRootPath, "users");
                            var wwwrootUserPath = Path.Combine(wwwrootPath, userId);
                            var profilePicturePath = Path.Combine(wwwrootUserPath, "userimage");
                            if (!Directory.Exists(profilePicturePath))
                            {
                                Directory.CreateDirectory(profilePicturePath);
                            }
                            string[] fileNames = Directory.GetFiles(profilePicturePath);
                            string filename = Guid.NewGuid().ToString("N").Substring(0, 10);
                            var extn = Path.GetExtension(zurag.FileName);
                            var fileWithExtn = filename + extn;
                            var profileImgPath = Path.Combine(profilePicturePath, fileWithExtn);
                            using (var FStream = new FileStream(profileImgPath, FileMode.Create))
                            {
                                await zurag.CopyToAsync(FStream);
                            }
                            foreach (string fileName in fileNames)
                            {
                                if (System.IO.File.Exists(fileName))
                                {
                                    System.IO.File.Delete(fileName);
                                }
                            }
                        }
                        if (mobileNumber != null)
                        {
                            user.PhoneNumber = mobileNumber;
                            string phoneNumber = mobileNumber;
                        }
                        if (Email != null)
                        {
                            user.Email = Email;
                            string email = Email;
                        }
                        if (NewPassword != null)
                        {
                           
                            var passwordChangeResult = await _userManager.ChangePasswordAsync(user, Password, NewPassword);
                        }
                        var resultasync = await _userManager.UpdateAsync(user);
                        result = "Done";
                    }
                    else
                    {
                        result = "PasswordMismatch";

                    }
                    
                }

                else
                {
                    result = "NoUser";
                }
               
            }
            return Page();
        }
    }
}
