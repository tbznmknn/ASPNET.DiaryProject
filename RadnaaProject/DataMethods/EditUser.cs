using Microsoft.EntityFrameworkCore;
using RadnaaProject.Models;

namespace RadnaaProject.DataMethods
{
    public class EditUser
    {
        public async Task<string> EditRecord(UserContext _ucontext, UserRecords userRecord, string recordId, string userId, IWebHostEnvironment _env,
            List<IFormFile> zuragnuud, List<IFormFile> videonuud, List<IFormFile> audionuud)
        {
            //Database aa shinechleh
            var entity= _ucontext.Records.Where(o => o.Id.Contains(recordId)).FirstOrDefault();
            entity.PostBody = userRecord.PostBody;
            entity.PostTitle= userRecord.PostTitle;
            await _ucontext.SaveChangesAsync();

            //Upload hiih (zurag bichleg audio)
            var wwwrootPath = Path.Combine(_env.WebRootPath, "users");
            var wwwrootUserPath = Path.Combine(wwwrootPath, userId);
            var recordFolder = Path.Combine(wwwrootUserPath, recordId);


            string[] fileNames = Directory.GetFiles(recordFolder);
            int tooluurZurag = 0;
            int tooluurBichleg = 0;
            int tooluurAudio = 0;
            foreach (string fileName in fileNames)
            {
                string extn = Path.GetExtension(fileName);
                string filename = Path.GetFileName(fileName);
                if (extn == ".jpeg" || extn == ".png" || extn == ".jpg")
                {
                    tooluurZurag++;
                }
                else if (extn == ".mp4" || extn == ".mkv")
                {
                    tooluurBichleg++;
                }
                else
                {
                    tooluurAudio++;
                }


            }
            if (zuragnuud != null && zuragnuud.Count > 0)
            {
            
                foreach (var zurag in zuragnuud)
                {

                    string filename = Guid.NewGuid().ToString("N").Substring(0, 10);
                    var extn = Path.GetExtension(zurag.FileName);
                    var fileWithExtn = tooluurZurag.ToString() + "_img_" + filename + extn;

                    var profileImgPath = Path.Combine(recordFolder, fileWithExtn);

                    using (var FStream = new FileStream(profileImgPath, FileMode.Create))
                    {
                        await zurag.CopyToAsync(FStream);
                    }
                    tooluurZurag++;
                  
                }
            }
            if (videonuud != null && videonuud.Count > 0)
            {
               
                foreach (var video in videonuud)
                {

                    string filename = Guid.NewGuid().ToString("N").Substring(0, 10);
                    var extn = Path.GetExtension(video.FileName);
                    var fileWithExtn = tooluurBichleg.ToString() + "_video_" + filename + extn;
                    var profileImgPath = Path.Combine(recordFolder, fileWithExtn);

                    using (var FStream = new FileStream(profileImgPath, FileMode.Create))
                    {
                        await video.CopyToAsync(FStream);
                    }
                    tooluurBichleg++;
                  
                }
            }
            if (audionuud != null && audionuud.Count > 0)
            {
           
                foreach (var audio in audionuud)
                {

                    string filename = Guid.NewGuid().ToString("N").Substring(0, 10);
                    var extn = Path.GetExtension(audio.FileName);
                    var fileWithExtn = tooluurAudio.ToString() + "_audio_" + filename + extn;
                    var profileImgPath = Path.Combine(recordFolder, fileWithExtn);

                    using (var FStream = new FileStream(profileImgPath, FileMode.Create))
                    {
                        await audio.CopyToAsync(FStream);
                    }
                    tooluurAudio++;
                   
                }
            }
            string urdun = recordFolder;
            return urdun;

        }
    }
}
