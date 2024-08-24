using System.ComponentModel.DataAnnotations;

namespace XaniAPI.Entites
{
    public class User
    {
        [Key]
        public Int32 u_id { get; set; }

        [MaxLength(50, ErrorMessage = "Handle must be 50 characters or less")]
        public string? u_handle { get; set; }

        [MaxLength(100, ErrorMessage = "Email address must be 100 characters or less")]
        public string? u_email { get; set; }
    }
}
