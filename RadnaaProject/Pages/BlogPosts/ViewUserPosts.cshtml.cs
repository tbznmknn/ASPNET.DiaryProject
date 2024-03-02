using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RadnaaProject.Models;

namespace RadnaaProject.Pages.BlogPosts
{
    public class ViewUserPostsModel : PageModel
    {
        public string urdun = "Hello";
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserContext _UContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ViewUserPostsModel(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, UserContext context, IWebHostEnvironment webHostEnvironment)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _UContext = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public UserRecords DataRow = new UserRecords();
        public List<string> ImageFiles= new List<string>();
        public List<string> VideoFiles=new List<string>();
        public List<string> AudioFiles= new List<string>();
        public string result = "initial";
        public string UserName = "";
        public async Task OnGet()
        {

            if (!string.IsNullOrEmpty(Request.Query["identififerParam"]))
            {
                string recordID = Request.Query["identififerParam"];
                urdun = recordID;
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    DataRow=await _UContext.Records.Where(o => o.Id == recordID).FirstOrDefaultAsync();
                    string userid = DataRow.UserID;
                    string postid = DataRow.Id;
                    UserName= await _userManager.GetUserNameAsync(user);
                   
                    var wwwrootPath = Path.Combine(_webHostEnvironment.WebRootPath, "users");
                    var wwwrootUserPath = Path.Combine(wwwrootPath, userid);
                    var postIDfolder = Path.Combine(wwwrootUserPath, postid);

                    string[] fileNames = Directory.GetFiles(postIDfolder);
                    foreach (string fileName in fileNames)
                    {
                        string extn= Path.GetExtension(fileName).ToLower();
                        string filename= Path.GetFileName(fileName);
                        if (extn == ".jpeg" || extn == ".png" || extn == ".jpg")
                        {
                            ImageFiles.Add(filename);
                        }
                        else if (extn == ".mp4" || extn == ".mkv")
                        {
                            VideoFiles.Add(filename);
                        }
                        else if(extn==".m4a" ||  extn ==".flac"||extn==".mp3" ||extn=="wav")
                        {
                            AudioFiles.Add(filename);
                        }
                       
                        
                    }
                    

                }
                else
                {
                    result = "no user";
                }
            }
            else
            {
                result = "no identifier";
            }
        }
       
    }
}
