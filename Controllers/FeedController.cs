using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Numerics;
using XaniAPI.Business;
using XaniAPI.DatabaseContexts;
using XaniAPI.Entities;
using static XaniAPI.Entities.Feed;

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
        /// <returns>A structure that contains a list of users and posts to inject into the Xani timeline</returns>
        /// <remarks>
        /// Sample request:
        /// http://localhost/Xani/api/feed?u_id=2
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

            var post_id_list = new List<Int64>();

            switch (feedType)
            {
                case "FollowedOnly":

                    post_id_list = [.. postDbContext.Database.SqlQuery<Feed.PostId>(@$"
                        SELECT      p.p_id
                        FROM        post as p
                        JOIN        follow ON (f_u_id_followed = p.p_u_id AND f_u_id_audience = {u_id})").Select(x => x.p_id)];
                    break;

                case "FollowedAndReplys":

                    post_id_list = postDbContext.Database.SqlQuery<Feed.PostId>(@$"
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

                        SELECT p_id FROM @p_idList").AsEnumerable().Select(x=> x.p_id).Distinct().ToList();
                    break;

                case "FollowedReplysQuotes":
                    post_id_list = postDbContext.Database.SqlQuery<Feed.PostId>(@$"

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

                    SELECT p_id FROM @p_idList").AsEnumerable().Select(x => x.p_id).Distinct().ToList();
                    break;
            }

            /* we will have a list of posts at this pouint, recover the post and user details */
            var param = DatabaseBusiness.Int64IdParameter("@idList", post_id_list);

            feed.f_post_items = [.. postDbContext.Database.SqlQueryRaw<Feed.PostItem>(@$"

                SELECT          p.p_id AS p_id, 
	                            p.p_u_id AS p_u_id,
                                p.p_ps_id AS p_ps_id,
 			                    p.p_content AS p_content,
                                p.p_datetime_created AS p_datetime_created,
                                p.p_datetime_edited AS p_datetime_edited,
			                    p.p_id_quote_of AS p_id_quote_of,
			                    p.p_id_reply_to AS p_id_reply_to,
		                        (SELECT COUNT (l_id) FROM [like] AS l WHERE l.l_p_id = p.p_id AND l.l_ls_id = 0) AS p_total_likes,
                                (SELECT COUNT (p_id) FROM post AS po WHERE po.p_id_quote_of = p.p_id AND ISNULL(po.p_content, '') = '') AS p_total_reposts,
                                (SELECT COUNT (p_id) FROM post AS po WHERE po.p_id_quote_of = p.p_id AND ISNULL(po.p_content, '') <> '') AS p_total_quotes,
                                (SELECT COUNT (p_id) FROM post AS po WHERE po.p_id_reply_to = p.p_id) AS p_total_replies

                FROM            post as p
                WHERE           p.p_id IN (SELECT id FROM @idList) 
                ORDER BY        p.p_datetime_created DESC", param).ToList()];

            feed.f_user_items = [.. postDbContext.Database.SqlQueryRaw<Feed.UserItem>(@$"

                SELECT DISTINCT u.u_id AS u_id, 
                                u.u_username AS u_username,
                                u.u_avitar AS u_avitar,
                                u.u_description AS u_description,
                                u.u_joined_date AS u_joined_date
                FROM            [user] as u
                JOIN		    post AS p ON p.p_u_id = u.u_id
                WHERE		    p.p_id IN (SELECT id FROM @idList) ", param).ToList()];

            return new ActionResult<Feed>(feed);
        }
    }
}
