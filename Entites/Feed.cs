using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace XaniAPI.Entites
{
    public class Feed 
    {
        public DateTime? f_datetime_generated { get; set; }
        public Int32 f_u_id { get; set; }
        public IEnumerable<Item>? f_items { get; set; }

        public class Item
        {
            public string? f_p_content { get; set; }
            public DateTime? f_p_datetime_created { get; set; }
            public DateTime? f_p_datetime_edited { get; set; }
            public string? f_u_username { get; set; }

            public Int32 f_pi_likes { get; set; }
            public Int32 f_pi_repost { get; set; }
            public Int32 f_pi_quote { get; set; }
        }


    }
}
