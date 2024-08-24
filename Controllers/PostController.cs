using Microsoft.AspNetCore.Mvc;
using XaniAPI.DatabaseContexts;
using XaniAPI.Entites;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XaniAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(IConfiguration configuration, PostDbContext postContext, LikeDbContext likeContext, RepostDbContext repostDbContext) : ControllerBase
    {
        private readonly PostDbContext postDbContext = postContext;
        private readonly LikeDbContext likeDbContext = likeContext;
        private readonly RepostDbContext repostDbContext = repostDbContext;
        private readonly IConfiguration configuration = configuration;

        // GET: api/<Post>
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
                    pi_repost = repostDbContext.Repost.Count (c=> c.r_p_id.Equals(p_id) && string.IsNullOrWhiteSpace(c.r_text)),
                    pi_quote = repostDbContext.Repost.Count(c => c.r_p_id.Equals(p_id) && !string.IsNullOrWhiteSpace(c.r_text)),
                };

                return post;
            }
        }

        // POST api/<Post>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Post>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Post>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
