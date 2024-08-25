using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using XaniAPI.DatabaseContexts;
using XaniAPI.Entites;

namespace XaniAPI.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Basic")]
    public class FeedController(IConfiguration configuration, LikeDbContext likeDbContext, PostDbContext postDbContext) : ControllerBase
    {
        private readonly PostDbContext postDbContext = postDbContext;
        private readonly LikeDbContext likeDbContext = likeDbContext;
        private readonly IConfiguration configuration = configuration;

        /// <summary>
        /// Gets a users feed
        /// </summary>
        /// <param name="u_id">This is the user id Int32</param>
        /// <returns>A newly created TodoItem</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///        "u_id": 1,
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        // GET: api/<FeedController>
        [HttpGet]
        public ActionResult<Feed> Get(Int32 u_id)
        {
            var feed = new Feed
            {
                f_datetime_generated = DateTime.Now,
                f_u_id = u_id,
                f_items = [.. postDbContext.Database.SqlQuery<Feed.Item>(@$"

                SELECT      p_content AS f_p_content,
                            p_datetime_created AS f_p_datetime_created,
                            p_datetime_edited AS f_p_datetime_edited,
                            u_username AS f_u_username,
						    COUNT (l_id) AS f_pi_likes,
						    COUNT (r.r_id) AS f_pi_repost,
						    COUNT (q.r_id) AS f_pi_quote

                FROM        post
                JOIN        [user] ON p_u_id = u_id 
                JOIN        follow ON (f_u_id_followed = u_id AND f_u_id_audience = {u_id})
                LEFT JOIN   [like] ON (l_p_id = p_id AND l_ls_id = 0)
                LEFT JOIN   repost as r ON (r.r_p_id = p_id AND r.r_content is null)
                LEFT JOIN   repost as q ON (q.r_p_id = p_id AND q.r_content is not null)

                GROUP BY    p_content, p_datetime_created, p_datetime_edited, u_id, u_username
                ORDER       BY p_datetime_created DESC ")]
            };


            return new ActionResult<Feed>(feed);
        }
    }
}
