using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace XaniAPI.Entities
{
    /// <summary>
    /// Feed injection for the user, two tables are represented here:
    /// 1. the posts themselves
    /// 2. the users who made the posts.
    /// </summary>
    [Keyless]
    public class Feed
    {
        public DateTime? f_datetime_generated { get; set; }
        public int f_u_id { get; set; }
        public IEnumerable<PostItem>? f_post_items { get; set; }
        public IEnumerable<UserItem>? f_user_items { get; set; }
         
        public class PostItem
        {
            /* post payload */
            public Int64 p_id { get; set; }
            public Int32 p_u_id { get; set; }
            public string? p_content { get; set; }
            public Int16 p_ps_id { get; set; }
            public DateTime? p_datetime_created { get; set; }
            public DateTime? p_datetime_edited { get; set; }
            public Int64? p_id_quote_of { get; set; }
            public Int64? p_id_reply_to { get; set; }

            /* these are just stats for the feed */
            public int p_total_replies { get; set; }
            public int p_total_likes { get; set; }
            public int p_total_reposts { get; set; }
            public int p_total_quotes { get; set; }
        }

        public class UserItem
        {
            public Int32 u_id { get; set; }
            public string? u_username { get; set; }
            public string? u_avitar { get; set; }
            public string? u_description { get; set; }
            public DateTime? u_joined_date { get; set; }
        }

        public class PostId
        {
            public Int64 p_id { get; set; }
        }
    }
}
