using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XaniAPI.DatabaseContexts;
using XaniAPI.Entities;

namespace XaniAPI.Controllers
{
    /// <summary>
    /// Authorization Controller
    /// if the user exists anf the password hash matched, caches a token and returns it to the cient
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorisationController(IConfiguration configuration, UserDbContext userDbContext ): ControllerBase
    {
        private readonly UserDbContext userDbContext = userDbContext;
        private readonly IConfiguration configuration = configuration;


        /// <summary>
        /// Gets an authorisation token
        /// </summary>
        /// <param name="request">This is the user id Int32</param>
        /// <returns>An authoristion response</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST 
        ///     {
        ///        "username": "Death",
        ///        "password_hash": "37023dfa13e7c584c259d5e383ff88c1f25e2b45403ecd5fe581132e7eb5c6ed"
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="404">The user has not been found or the password hash is invalid</response>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<AuthorisationResponse> Post(AuthorisationRequest request)
        {
            var timestamp = DateTime.Now;

            var user = userDbContext.User.FirstOrDefault(x => x.u_username != null && x.u_username.ToLower().Equals(request.username.ToLower()) && 
                    x.u_password_hash != null && x.u_password_hash.Equals(request.password_hash));
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var response = new AuthorisationResponse
                {
                    id = user.u_id,
                    token = Guid.NewGuid().ToString()
                };

                TokenRepostitory.RecordToken(timestamp, response.token, response.id);

                return response;
            }
        }
    }
}
