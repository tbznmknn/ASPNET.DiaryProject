using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadnaaProject.Models
{
    [Table("UserRecords")]
    public class UserRecords
    {
        
        [Key]
        [StringLength(450)]
        public string Id { get; set; }

        [StringLength(450)]
        public string UserID { get; set; } = "";

        [Required(ErrorMessage = "гарчиг оруулна уу")]
        [StringLength(100, ErrorMessage = "100 Тэмдэгтээс уртгүй байн")]
        public string PostTitle { get; set; } = "";

        [Required(ErrorMessage = "тэмдэглэл оруулна уу")]
        [StringLength(20000, ErrorMessage = "20000 тэмдэгтээс уртгүй байна")]
        public string PostBody { get; set; } = "";

        public DateTime CDate { get; set; }
        public DateTime MDate { get; set; }
        public DateTime RDate { get; set; } = DateTime.MinValue;
    }
}
