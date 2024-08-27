using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using XaniAPI.DatabaseContexts;

namespace XaniAPI.Entites
{
    /// <summary>
    /// 
    /// </summary>
    public class Post
    {
        [Key]
        public Int64 p_id { get; set; }
        public Int32 p_u_id { get; set; }
         [MaxLength(512, ErrorMessage= "Content must be 512 characters or less")]
        public string? p_content { get; set; }
        public DateTime? p_datetime_created { get; set; }
        public DateTime? p_datetime_edited { get; set; }
        public Int16 p_ps_id { get; set; }
        public Int64? p_id_quote_of { get; set; }
        public Int64? p_id_reply_to { get; set; }


        [NotMapped]
        public Info? p_info { get; set; }

        public class Info
        {
            public Int32 pi_likes { get; set; }
            public Int32 pi_repost { get; set; }
            public Int32 pi_quote { get; set; }
        }
    }
}
