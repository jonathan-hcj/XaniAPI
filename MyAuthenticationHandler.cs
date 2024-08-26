using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using XaniAPI.Entities;

namespace XaniAPI
{    
    /// <summary>
    /// Authorisation handler
    /// </summary>
    /// <param name="item"></param>
    /// <returns>creates a new post</returns>
    /// <remarks>
    /// Sample request:
    ///
    /// </remarks>
    public class MyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        /// <summary>
        /// Gets a users feed
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///        "u_id": 1,
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        private IConfiguration configuration;

        /// <summary>
        /// Constructor - additionally pulls through the configuration interface
        /// </summary>
        public MyAuthenticationHandler(IConfiguration configuration, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var errorMessage = "";
            var settings = configuration.GetSection("Settings");

            if (settings.GetValue<bool>("RequireAuthentication"))
            {
                if (Request.Headers == null)
                    errorMessage = "No authorization header";

                else if (Request.Headers.Authorization.IsNullOrEmpty())
                    errorMessage = "No authorization header";

                else
                {
                    var authHeader = AuthenticationHeaderValue.Parse(Request.Headers.Authorization);
                    if (authHeader == null || TokenRepostitory.ValidateToken(authHeader.Parameter) == null)
                    {
                        errorMessage = "No valid token supplied";
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                var claims = new[] { new Claim(ClaimTypes.Name, "") };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail(errorMessage));
            }
        }
    }
}
