using System.ComponentModel.DataAnnotations;

namespace XaniAPI.Entites
{
    public class Repost
    {
        [Key]
        public Int64 r_id { get; set; }
        public Int32 r_u_id { get; set; }
        public DateTime r_datetime { get; set; }
        public Int64 r_p_id { get; set; }
        public string? r_text { get; set; }
    }
}
