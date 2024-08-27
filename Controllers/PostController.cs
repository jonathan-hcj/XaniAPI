using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using XaniAPI.DatabaseContexts;
using XaniAPI.Entites;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XaniAPI.Controllers
{
    /// <summary>
    /// Post controller deals with individual posts rather than a feed
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(IConfiguration configuration, PostDbContext postDbContext, LikeDbContext likeContext) : ControllerBase
    {
        private readonly PostDbContext postDbContext = postDbContext;
        private readonly LikeDbContext likeDbContext = likeContext;
        private readonly IConfiguration configuration = configuration;

        /// <summary>
        /// Gets an single post and its interaction stats
        /// </summary>
        /// <param name="p_id">This is the post id id Int64</param>
        /// <returns>An authoristion response</returns>
        /// <remarks>
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="404">The post has not been found</response>
        [HttpGet]
        public ActionResult<Post> Get(Int64 p_id)
        {
            var post = postDbContext.Post.FirstOrDefault(x => x.p_id.Equals(p_id));
            if (post == null)
            {
                return NotFound();
            }

            else
            {
                post.p_info = new Post.Info()
                {
                    pi_likes = likeDbContext.Like.Count(c => c.l_p_id.Equals(p_id)),
                    pi_repost = postDbContext.Post.Count(c => c.p_id_reply_to.Equals(p_id)),
                    pi_quote = postDbContext.Post.Count(c => c.p_id_quote_of.Equals(p_id)),
                };

                return post;
            }
        }

        // Example payload
        //{
        //  "p_u_id": 2,
        //  "p_datetime": "2024-08-24T11:16:38.100",
        //  "p_content": "I'm at this resort in Portugal, going around on my motorised scooter like her from Benidorm.  One of the things that is difficult to explain to people is that it's really annoying to have people blocking entrances, exits, bars, self service food with themselves and chairs....",
        //  "p_ps_id": 0
        // }
        [HttpPost]
        public ActionResult<Post> Post(Post post)
        {
            postDbContext.Post.Add(post);
            postDbContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = post.p_id }, null);
        }

        // Example payload
        //{
        //  "p_id": 8,
        //  "p_u_id": 2,
        //  "p_datetime": "2024-08-24T11:16:38.100",
        //  "p_content": "Ukraine has ratified amendments made in February to the UK-Ukraine trade deal, which will see Ukraine have zero tariff access to the UK market for five years (until March 2029). The UK can only do this due to Brexit.",
        //  "p_ps_id": 0
        // }
        [HttpPut]
        public ActionResult<Post> Put(Post post)
        {
            /* fail if the primary key is already in use */
            var existingPost= postDbContext.Post.FirstOrDefault(x => x.p_id.Equals(post.p_id));
            if (existingPost == null)
            {
                return NotFound();
            }

            else if (!existingPost.p_u_id.Equals(post.p_u_id))
            {
                return StatusCode(StatusCodes.Status400BadRequest, $"User ids do not match");
            }

            else
            {
                existingPost.p_content = post.p_content;
                existingPost.p_datetime_edited = existingPost.p_datetime_edited == null ? DateTime.Now.Date : existingPost.p_datetime_edited; 
                postDbContext.SaveChanges();

                return CreatedAtAction(nameof(Get), new { id = post.p_id }, null);
            }
        }
    }
}
