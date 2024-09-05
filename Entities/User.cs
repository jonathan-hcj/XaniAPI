using System.ComponentModel.DataAnnotations;

namespace XaniAPI.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class User
    {
        [Key]
        public int u_id { get; set; }
        [MaxLength(50, ErrorMessage = "Handle must be 50 characters or less"), MinLength(0)]
        public string? u_username { get; set; }
        [MaxLength(100, ErrorMessage = "Email address must be 100 characters or less"), MinLength(0)]
        public string? u_email_address { get; set; }
        [MaxLength(100, ErrorMessage = "The password hash must be 100 characters or less"), MinLength(0)]
        public string? u_password_hash { get; set; }
        [MaxLength(200, ErrorMessage = "The description hash must be 200 characters or less"), MinLength(0)]
        public string? u_description { get; set; }
        public DateTime? u_joined_date { get; set; }
    }
}
