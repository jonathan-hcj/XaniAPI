using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using XaniAPI.DatabaseContexts;
using XaniAPI.Entites;

namespace XaniAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController(IConfiguration configuration, LikeDbContext likeDbContext, PostDbContext postDbContext) : ControllerBase
    {
        private readonly PostDbContext postDbContext = postDbContext;
        private readonly LikeDbContext likeDbContext = likeDbContext;
        private readonly IConfiguration configuration = configuration;

        // GET: api/<FeedController>
        [HttpGet]
        public ActionResult<Feed> Get(Int32 u_id)
        {
            var feed = new Feed();

            feed.feed_list = [.. postDbContext.Database.SqlQuery<Feed.Item>(@$"

                SELECT      p_text AS f_p_text,
                            p_datetime AS f_p_datetime,
                            u_handle AS f_u_handle,
						    COUNT (l_id) AS f_pi_likes,
						    COUNT (r.r_id) AS f_pi_repost,
						    COUNT (q.r_id) AS f_pi_quote

                FROM        post
                JOIN        [user] ON p_u_id = u_id 
                JOIN        follow ON (f_u_id_followed = u_id AND f_u_id_audience = {u_id})
                LEFT JOIN   [like] ON (l_p_id = p_id AND l_ls_id = 0)
                LEFT JOIN   repost as r ON (r.r_p_id = p_id AND r.r_text is null)
                LEFT JOIN   repost as q ON (q.r_p_id = p_id AND q.r_text is not null)

                GROUP BY    p_text, p_datetime, u_handle
                ORDER       BY p_datetime DESC ")];


            return new ActionResult<Feed>(feed);
        }
    }
}
