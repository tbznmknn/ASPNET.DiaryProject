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
namespace RadnaaProject.DataMethods
{
    public class DeleteUser
    {
        public async Task<string> DeleteRow(UserContext _ucontext,string recordId)
        {
            string urdun = "pending";
            var entity = _ucontext.Records.Where(o => o.Id.Contains(recordId)).FirstOrDefault();
            _ucontext.Records.Remove(entity);
            await _ucontext.SaveChangesAsync();
            return urdun;

        }
    }

    
}
