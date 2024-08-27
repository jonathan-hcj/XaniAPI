using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.DataAnnotations;
using XaniAPI.DatabaseContexts;
using XaniAPI.Entities;

namespace XaniAPI.Controllers

{
    /// <summary>
    /// Feed management, this is likley to be the most complicated part of the software at 
    /// it curates the users experience.
    /// 
    /// Version 1: returns the posts belonging to those users you follow.
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="likeDbContext"></param>
    /// <param name="postDbContext"></param>
    /// <returns>A newly created TodoItem</returns>
    /// <remarks>
    /// </remarks>
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
        /// <response code="201">Returns the json encoded list of posts to display</response>
        /// <response code="400">If the item is null</response>
        // GET: api/<FeedController>
        [HttpGet()]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<Feed> Get(Int32 u_id)
        {
            var settings = configuration.GetSection("Settings");
            var feedType = settings.GetValue<string>("FeedType");
            var feed = new Feed
            {
                f_datetime_generated = DateTime.Now,
                f_u_id = u_id
            };

            switch (feedType)
            {
                case "FollowedOnly":
                    feed.f_items = [.. postDbContext.Database.SqlQuery<Feed.Item>(@$"

                    DECLARE     @p_idList TABLE ( p_id BIGINT )

                    INSERT INTO @p_idList
                    SELECT      p.p_id 
                    FROM        post as p
                    JOIN        follow ON (f_u_id_followed = p.p_u_id AND f_u_id_audience = {u_id})

                    SELECT      p.p_id AS f_p_id, 
			                    p.p_content AS f_p_content,
                                p.p_datetime_created AS f_p_datetime_created,
                                p.p_datetime_edited AS f_p_datetime_edited,
			                    p.p_id_quote_of AS f_p_id_quote_of,
			                    p.p_id_reply_to AS f_p_id_reply_to,
                                u_username AS f_u_username,
			                    COUNT (l_id) AS f_pi_likes,
			                    COUNT (rp.p_id) AS f_pi_repost,
			                    COUNT (qp.p_id) AS f_pi_quote

                    FROM		@p_idList AS pl
                    JOIN        post as p ON (pl.p_id = p.p_id)
                    JOIN        [user] ON p.p_u_id = u_id 
                    LEFT JOIN   [like] ON (l_p_id = p.p_id AND l_ls_id = 0)
                    LEFT JOIN   post as qp ON (qp.p_id_quote_of = p.p_id)
                    LEFT JOIN   post as rp ON (rp.p_id_quote_of = p.p_id)

                    GROUP BY    p.p_id, p.p_content, p.p_datetime_created, p.p_datetime_edited, p.p_id_quote_of, p.p_id_reply_to, u_id, u_username
                    ORDER       BY p.p_datetime_created DESC ")];
                    break;

                case "FollowedAndReplys":

                    feed.f_items = [.. postDbContext.Database.SqlQuery<Feed.Item>(@$"

                    DECLARE     @p_idList TABLE ( p_id BIGINT )

                    INSERT INTO @p_idList
                    SELECT      p.p_id 
                    FROM        post as p
                    JOIN        follow ON (f_u_id_followed = p.p_u_id AND f_u_id_audience = {u_id})

                    INSERT INTO @p_idList
                    SELECT		p.p_id_reply_to 
                    FROM		@p_idList AS pl
                    JOIN		post AS p ON p.p_id = pl.p_id
                    WHERE		p.p_id_reply_to is not null

                    SELECT      p.p_id AS f_p_id, 
			                    p.p_content AS f_p_content,
                                p.p_datetime_created AS f_p_datetime_created,
                                p.p_datetime_edited AS f_p_datetime_edited,
			                    p.p_id_quote_of AS f_p_id_quote_of,
			                    p.p_id_reply_to AS f_p_id_reply_to,
                                u_username AS f_u_username,
			                    COUNT (l_id) AS f_pi_likes,
			                    COUNT (rp.p_id) AS f_pi_repost,
			                    COUNT (qp.p_id) AS f_pi_quote

                    FROM		@p_idList AS pl
                    JOIN        post as p ON (pl.p_id = p.p_id)
                    JOIN        [user] ON p.p_u_id = u_id 
                    LEFT JOIN   [like] ON (l_p_id = p.p_id AND l_ls_id = 0)
                    LEFT JOIN   post as qp ON (qp.p_id_quote_of = p.p_id)
                    LEFT JOIN   post as rp ON (rp.p_id_quote_of = p.p_id)

                    GROUP BY    p.p_id, p.p_content, p.p_datetime_created, p.p_datetime_edited, p.p_id_quote_of, p.p_id_reply_to, u_id, u_username
                    ORDER       BY p.p_datetime_created DESC")];

                    break;

                case "FollowedReplysQuotes":
                    feed.f_items = [.. postDbContext.Database.SqlQuery<Feed.Item>(@$"

                    DECLARE     @p_idList TABLE ( p_id BIGINT )

                    INSERT INTO @p_idList
                    SELECT      p.p_id 
                    FROM        post as p
                    JOIN        follow ON (f_u_id_followed = p.p_u_id AND f_u_id_audience = {u_id})

                    INSERT INTO @p_idList
                    SELECT		p.p_id_reply_to 
                    FROM		@p_idList AS pl
                    JOIN		post AS p ON p.p_id = pl.p_id
                    WHERE		p.p_id_reply_to is not null

                    INSERT INTO @p_idList
                    SELECT		p.p_id_quote_of 
                    FROM		@p_idList AS pl
                    JOIN		post AS p ON p.p_id = pl.p_id
                    WHERE		p.p_id_quote_of is not null

                    SELECT      p.p_id AS f_p_id, 
			                    p.p_content AS f_p_content,
                                p.p_datetime_created AS f_p_datetime_created,
                                p.p_datetime_edited AS f_p_datetime_edited,
			                    p.p_id_quote_of AS f_p_id_quote_of,
			                    p.p_id_reply_to AS f_p_id_reply_to,
                                u_username AS f_u_username,
			                    COUNT (l_id) AS f_pi_likes,
			                    COUNT (rp.p_id) AS f_pi_repost,
			                    COUNT (qp.p_id) AS f_pi_quote

                    FROM		@p_idList AS pl
                    JOIN        post as p ON (pl.p_id = p.p_id)
                    JOIN        [user] ON p.p_u_id = u_id 
                    LEFT JOIN   [like] ON (l_p_id = p.p_id AND l_ls_id = 0)
                    LEFT JOIN   post as qp ON (qp.p_id_quote_of = p.p_id)
                    LEFT JOIN   post as rp ON (rp.p_id_quote_of = p.p_id)

                    GROUP BY    p.p_id, p.p_content, p.p_datetime_created, p.p_datetime_edited, p.p_id_quote_of, p.p_id_reply_to, u_id, u_username
                    ORDER       BY p.p_datetime_created DESC")];




                    break;
            }

            return new ActionResult<Feed>(feed);
        }

        public class IdRow
        {
            public Int64 p_id { get; set; }
        }

    }
}
