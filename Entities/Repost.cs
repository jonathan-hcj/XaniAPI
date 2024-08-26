using System.ComponentModel.DataAnnotations;

namespace XaniAPI.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Repost
    {
        [Key]
        public long r_id { get; set; }
        public int r_u_id { get; set; }
        public DateTime r_datetime { get; set; }
        public long r_p_id { get; set; }
        public string? r_content { get; set; }
    }
}
