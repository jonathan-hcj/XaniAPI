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
    /// <remarks>
    /// </remarks>
    public class MyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// Manages the authentication tokens
        /// </summary>
        /// <remarks>
        /// The tokens themselves are held in the static class TokenRepositorn.
        /// </remarks>
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
            var timestamp = DateTime.Now;

            if (settings.GetValue<bool>("RequireAuthentication"))
            {
                if (Request.Headers == null)
                    errorMessage = "No authorization header";

                else if (Request.Headers.Authorization.IsNullOrEmpty())
                    errorMessage = "No authorization header";

                else
                {
                    var authHeader = AuthenticationHeaderValue.Parse(Request.Headers.Authorization);
                    if (authHeader == null || TokenRepostitory.ValidateToken(timestamp, authHeader.Parameter) == null)
                    {
                        errorMessage = "No valid token supplied";
                    }
                }

                /* clean up the token dictionaary */
                TokenRepostitory.PurgeLapsedTokens(timestamp);
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
