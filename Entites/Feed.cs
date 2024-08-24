using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace XaniAPI.Entites
{
    public class Feed 
    {
        public IEnumerable<Item>? feed_list { get; set; }

        public class Item
        {
            public string? f_p_text { get; set; }
            public DateTime f_p_datetime { get; set; }
            public string? f_u_handle { get; set; }

            public Int32 f_pi_likes { get; set; }
            public Int32 f_pi_repost { get; set; }
            public Int32 f_pi_quote { get; set; }
        }


    }
}
