using Azure.Core;
using Azure;

namespace XaniAPI.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public static class TokenRepostitory
    {
        private static readonly Dictionary<string, Token> currentTokens = [];

        /// <summary>
        /// 
        /// </summary>
        public static void RecordToken(DateTime timestamp, string u_token, int u_id)
        {
            if (currentTokens.TryGetValue(u_token, out Token? value))
            {
                value.t_u_token = u_token;
                value.t_u_id = u_id;
                value.t_expires = timestamp.AddMinutes(10);
            }
            else
            {
                currentTokens.Add(u_token, new Token
                {
                    t_u_token = u_token,
                    t_u_id = u_id,
                    t_expires = timestamp.AddMinutes(10)
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Token? ValidateToken(DateTime timestamp, string? u_token)
        {
            Token? token = null;

            if (u_token != null && currentTokens.TryGetValue(u_token, out Token? value))
            {
                if (value.t_u_token != null && value.t_u_token.Equals(u_token) && timestamp < value.t_expires)
                {
                    token = value;
                }
            }

            return token;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void PurgeLapsedTokens(DateTime timestamp)
        {
            var lapsedTokenList = currentTokens.Where(x => x.Value.t_expires < timestamp).Select(y=> y.Value.t_u_token);

            foreach (var lapsedToken in lapsedTokenList)
            {
                if (!string.IsNullOrWhiteSpace(lapsedToken))
                {
                    currentTokens.Remove(lapsedToken);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Token? GetToken(string? u_token)
        {
            Token? token = null;

            if (u_token != null && currentTokens.TryGetValue(u_token, out Token? value))
            {
                if (value.t_u_token != null && value.t_u_token.Equals(u_token))
                {
                    token = value;
                }
            }

            return token;
        }


        /// <summary>
        /// Token 
        /// These volatile objects are held in a dictionary so they'll clear whenever the API restarts
        /// should be secure as they aren't stored on media anywhere.
        /// </summary>
        public class Token
        {
            public int t_u_id;
            public string? t_u_token;
            public DateTime t_expires;
        }
    }
}
