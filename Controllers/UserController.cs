using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net.Http.Headers;
using XaniAPI.DatabaseContexts;
using XaniAPI.Entities;

namespace XaniAPI.Controllers
{
    /// <summary>
    /// User management controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IConfiguration configuration, UserDbContext userDbContext) : ControllerBase
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

        /// <summary>
        /// Updates or adds one or more users 
        /// </summary>
        /// <param name="userList">This is list of users to update</param>
        /// <returns>An authoristion response</returns>
        /// <remarks>
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="404">The post has not been found</response>
        [HttpPut]
        public void Put(User[] userList)
        {
            var settings = configuration.GetSection("Settings");
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers.Authorization);
            var token = TokenRepostitory.ValidateToken(authHeader.Parameter);

            if (settings.GetValue<bool>("BulkUpdateUsers"))
            {
                foreach(var user in userList)
                {
                    var userRecord = userDbContext.User.FirstOrDefault(x => x.u_id.Equals(user.u_id));
                    if (userRecord != null)
                    {
                        Helpers.DuckCopyShallow<User>(userRecord,user);
                    }
                    else
                    {
                        userDbContext.User.Add(user);
                    }
                }
                userDbContext.SaveChanges();
            }
            else
            { 


            }
        }


    }
}
