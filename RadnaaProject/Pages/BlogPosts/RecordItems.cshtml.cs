using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RadnaaProject.DataMethods;
using RadnaaProject.Models;
using System.Security.Claims;

namespace RadnaaProject.Pages.BlogPosts
{
    public class RecordItemsModel : PageModel
    {
      

        public void OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            header = _context.Records.Where(o=>o.UserID==userId).OrderByDescending(o=>o.RDate).ToList();
        }
      
        private readonly UserContext _context;
        public RecordItemsModel(UserContext context)
        {
            _context = context;
        }
        
        public IEnumerable<UserRecords> header;
        public string urdun = "";

        //public async Task<string> OnPostDeleteRecord()
        //{
        //    DeleteUser deleteUser = new DeleteUser();
        //    urdun = deleteUser.DeleteRow(_context)
        //}

    }
}
