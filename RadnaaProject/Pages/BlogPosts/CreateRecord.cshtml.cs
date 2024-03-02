using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using RadnaaProject.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RadnaaProject.DataMethods;

namespace RadnaaProject.Pages.BlogPosts
{
    public class CreateRecordModel : PageModel
    {
        
        public void OnGet()
        {
        }
        public readonly IWebHostEnvironment _env;
        private readonly UserContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public CreateRecordModel(UserContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }
        public UserRecords userRecords { get; set; }=new UserRecords();
        [BindProperty]
        [Required(ErrorMessage = "Гарчиг оруулна уу.")]
        [StringLength(100, ErrorMessage = "100 тэмдэгтээс уртгүй байна")]
        public string RecordTitle { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "Нэмэлт мэдээллээ оруулна уу.")]
        [StringLength(20000, ErrorMessage = "20000 тэмдэгтээс уртгүй байна")]
        public string RecordBody { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage ="Цагаа оруулна уу.")]
        public DateTime RDate { get; set; }

        [BindProperty]
        public List<IFormFile> zuragnuud { get; set; }

        [BindProperty]
        
        public List<IFormFile> videonuud { get; set; }

        [BindProperty]
        public List<IFormFile> audionuud { get; set; }
        [RequestSizeLimit(1024 * 1024 * 1024)]
        public async Task<IActionResult> OnPostCreateRecord()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    string userId = user.Id;
                    var wwwrootPath = Path.Combine(_env.WebRootPath, "users");
                    var wwwrootUserPath = Path.Combine(wwwrootPath, userId);
                    string recordNamePrior = Guid.NewGuid().ToString("N").Substring(0, 14);


                    userRecords.CDate = DateTime.Now;
                    userRecords.MDate = DateTime.Now;
                    userRecords.RDate = RDate;
                    userRecords.PostTitle = RecordTitle;
                    userRecords.PostBody = RecordBody;
                    userRecords.UserID = userId;
                    string DateExtn = string.Format("{0}{1}{2}", RDate.Year, RDate.Month.ToString("d2"), RDate.Day.ToString("d2"));
                    string recordName = DateExtn + "_" + recordNamePrior;
                    userRecords.Id = recordName;

                    var recordFolder=Path.Combine(wwwrootUserPath, recordName);
                    if (!Directory.Exists(recordFolder))
                    {
                        Directory.CreateDirectory(recordFolder);
                    }






                   
                    if (zuragnuud != null && zuragnuud.Count > 0)
                    {
                        int tooluur = 1;
                        foreach (var zurag in zuragnuud)
                        {

                            string filename = Guid.NewGuid().ToString("N").Substring(0, 10);
                            var extn = Path.GetExtension(zurag.FileName);
                            var fileWithExtn = tooluur.ToString()+"_img_"+filename + extn;

                            var profileImgPath = Path.Combine(recordFolder, fileWithExtn);

                            using (var FStream = new FileStream(profileImgPath, FileMode.Create))
                            {
                                await zurag.CopyToAsync(FStream);
                            }
                           
                            tooluur++;
                        }
                    }
                    if (videonuud != null && videonuud.Count > 0)
                    {
                        int tooluur = 1;
                        foreach (var video in videonuud)
                        {

                            string filename = Guid.NewGuid().ToString("N").Substring(0, 10);
                            var extn = Path.GetExtension(video.FileName);
                            var fileWithExtn = tooluur.ToString() + "_video_" + filename + extn;
                            var profileImgPath = Path.Combine(recordFolder, fileWithExtn);

                            using (var FStream = new FileStream(profileImgPath, FileMode.Create))
                            {
                                await video.CopyToAsync(FStream);
                            }

                            tooluur++;
                        }
                    }
                    if (audionuud != null && audionuud.Count > 0)
                    {
                        int tooluur = 1;
                        foreach (var audio in audionuud)
                        {

                            string filename = Guid.NewGuid().ToString("N").Substring(0, 10);
                            var extn = Path.GetExtension(audio.FileName);
                            var fileWithExtn = tooluur.ToString() + "_audio_" + filename + extn;
                            var profileImgPath = Path.Combine(recordFolder, fileWithExtn);

                            using (var FStream = new FileStream(profileImgPath, FileMode.Create))
                            {
                                await audio.CopyToAsync(FStream);
                            }

                            tooluur++;
                        }
                    }
                    _context.Records.Add(userRecords);
                    await _context.SaveChangesAsync();

                }
            }

            return Page();
        }
    }
}
