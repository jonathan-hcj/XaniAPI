using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XaniAPI.DatabaseContexts;
using XaniAPI.Entites;

namespace XaniAPI.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class AuthorisationController(IConfiguration configuration, UserDbContext userDbContext ): ControllerBase
    {
        private readonly UserDbContext userDbContext = userDbContext;
        private readonly IConfiguration configuration = configuration;

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

                TokenRepostitory.RecordToken(request.u_id, response.u_token);

                return response;
            }
        }
    }
}
