using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net.Http.Headers;
using XaniAPI.DatabaseContexts;
using XaniAPI.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XaniAPI.Controllers
{
    /// <summary>
    /// User management
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
        /// Gets an single post and its interaction stats
        /// </summary>
        /// <param name="userList">This is list of users to update</param>
        /// <returns>An authoristion response</returns>
        /// <remarks>
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="404">The post has not been found</response>
        // PUT api/<ValuesController>/5
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
                        DuckCopyShallow<User>(userRecord,user);
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

        private static void DuckCopyShallow<T>(T dst, T src)
        {
            var srcT = src.GetType();
            var dstT = dst.GetType();

            foreach (var f in srcT.GetFields())
            {
                var dstF = dstT.GetField(f.Name);
                if (dstF == null || dstF.IsLiteral)
                    continue;
                dstF.SetValue(dst, f.GetValue(src));
            }

            foreach (var f in srcT.GetProperties())
            {
                var dstF = dstT.GetProperty(f.Name);
                if (dstF == null)
                    continue;

                dstF.SetValue(dst, f.GetValue(src, null), null);
            }
        }
    }
}
