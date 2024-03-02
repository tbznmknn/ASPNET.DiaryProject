using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RadnaaProject.Models;
using System.ComponentModel.DataAnnotations;
using RadnaaProject.DataMethods;

namespace RadnaaProject.Pages.BlogPosts
{
    public class RecordEditorModel : PageModel
    {
        public string RecordIdReceived = "test";
        public string UserId = "";

        public string urdun = "btest";
        public UserRecords oldrecord=new UserRecords();
        public async Task OnGet()
        {
            RecordIdReceived = Request.Query["dataField"];
            if (!string.IsNullOrEmpty(Request.Query["dataField"]))
            {
                
                urdun = _context.Records.Where(o => o.Id.Contains(RecordIdReceived)).Select(o=>o.Id).FirstOrDefault();
                if(urdun != null)
                {
                    oldrecord.PostTitle = _context.Records.Where(o => o.Id.Contains(RecordIdReceived)).Select(o => o.PostTitle).FirstOrDefault();
                    RecordBody = _context.Records.Where(o => o.Id.Contains(RecordIdReceived)).Select(o => o.PostBody).FirstOrDefault();
                    oldrecord.RDate= _context.Records.Where(o => o.Id.Contains(RecordIdReceived)).Select(o => o.RDate).FirstOrDefault();
                    UserId= _context.Records.Where(o => o.Id.Contains(RecordIdReceived)).Select(o => o.UserID).FirstOrDefault();
                }

            }
        }
        public readonly IWebHostEnvironment _env;
        private readonly UserContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public RecordEditorModel(UserContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }
        [BindProperty]
        [StringLength(100, ErrorMessage = "100 тэмдэгтээс уртгүй байна")]
        public string RecordTitle { get; set; } = "";

        [BindProperty]
        [StringLength(20000, ErrorMessage = "20000 тэмдэгтээс уртгүй байна")]
        public string RecordBody { get; set; } = "";

        [BindProperty]
        public DateTime RDate { get; set; }

        [BindProperty]
        public string UID { get; set; }

        [BindProperty]
        public List<IFormFile> zuragnuud { get; set; }

        [BindProperty]

        public List<IFormFile> videonuud { get; set; }

        [BindProperty]
        public List<IFormFile> audionuud { get; set; }
        public UserRecords newRecord = new UserRecords();
        public string editResult = "nofunction";
        public async Task<IActionResult> OnPostEditRecord()
        {
            string USERID = "ee";
      
            if (ModelState.IsValid)
            {
                newRecord.PostTitle = RecordTitle;
                newRecord.PostBody = RecordBody;
                newRecord.RDate = new DateTime(RDate.Year,RDate.Month,RDate.Day).Date;
                newRecord.MDate = DateTime.Now;
                newRecord.CDate = _context.Records.Where(o => o.Id.Contains(RecordIdReceived)).Select(o => o.CDate).FirstOrDefault();
                EditUser editUser = new EditUser();
                 USERID = _context.Records.Where(o => o.Id.Contains(UID)).Select(o => o.UserID).FirstOrDefault();
                editResult = await editUser.EditRecord(_context, newRecord, UID, USERID, _env, zuragnuud,videonuud,audionuud);
            }
            else
            {
                editResult = "noModelState";
            }
            return new JsonResult(new { success = true, message = "Амжилттай рекорд үүсгэгдлээ."+ RecordTitle+UID+"__"+ USERID + editResult});
        }
        public  IActionResult OnPostDeleteRecordAndRedirect()
        {
            DeleteUser deleteUser = new DeleteUser();
            var urdun = deleteUser.DeleteRow(_context, UID);
            return new JsonResult(new { success = true, message = "Амжилттай" + urdun.ToString() }) ;
        }
    }
}
