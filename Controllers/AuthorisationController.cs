using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XaniAPI.DatabaseContexts;
using XaniAPI.Entities;

namespace XaniAPI.Controllers
{
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
        ///        "u_id": 2,
        ///        "u_password_hash": "5Gh6353=="
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="404">The user has not been found or the password hash is invalid</response>
       [AllowAnonymous]
        [HttpPost]
        public ActionResult<AuthorisationResponse> Post(AuthorisationRequest request)
        {
            var user = userDbContext.User.FirstOrDefault(x => x.u_id.Equals(request.u_id) && x.u_password_hash != null && x.u_password_hash.Equals(request.u_password_hash));
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var response = new AuthorisationResponse
                {
                    u_id = request.u_id,
                    u_token = Guid.NewGuid().ToString()
                };

                TokenRepostitory.RecordToken(response.u_token, request.u_id);

                return response;
            }
        }
    }
}
