using Microsoft.AspNetCore.Mvc;
using XaniAPI.DatabaseContexts;
using XaniAPI.Entites;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XaniAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController(IConfiguration configuration, LikeDbContext context) : ControllerBase
    {
        private readonly LikeDbContext likeDbContext = context;
        private readonly IConfiguration configuration = configuration;

        // GET: api/<LikeController>
        [HttpGet]
        public ActionResult<IEnumerable<Like>> Get(Int64 p_id)
        {
            var likeList = likeDbContext.Like.Where(x => x.l_p_id.Equals(p_id));

            return new ActionResult<IEnumerable<Like>> (likeList); 
        }

        // POST api/<LikeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LikeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
