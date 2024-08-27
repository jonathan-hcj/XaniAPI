using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace XaniAPI.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [Keyless]
    public class Feed
    {

        public DateTime? f_datetime_generated { get; set; }
        public int f_u_id { get; set; }
        public IEnumerable<Item>? f_items { get; set; }

        public class Item
        {
            public Int64 f_p_id { get; set; }
            public string? f_p_content { get; set; }
            public DateTime? f_p_datetime_created { get; set; }
            public DateTime? f_p_datetime_edited { get; set; }

            public Int64? f_p_id_quote_of { get; set; }
            public Int64? f_p_id_reply_to { get; set; }




            public string? f_u_username { get; set; }

            public int f_pi_likes { get; set; }
            public int f_pi_repost { get; set; }
            public int f_pi_quote { get; set; }
        }


    }
}
