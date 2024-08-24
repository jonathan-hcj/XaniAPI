using Microsoft.AspNetCore.Mvc;
using System.Collections;
using XaniAPI.DatabaseContexts;
using XaniAPI.Entites;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XaniAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController (IConfiguration configuration, UserDbContext userDbContext) : ControllerBase
    {
        private readonly UserDbContext userDbContext = userDbContext;
        private readonly IConfiguration configuration = configuration;

        /* https://localhost:7049/api/user?ids=2&ids=3 */
        // GET: api/<UserController>
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get([FromQuery(Name = "ids")] int[] ids)
        {
            var users = userDbContext.User.Where(x => ids.Contains(x.u_id)).ToList();

            return users;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
