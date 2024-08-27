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
        public static Token? ValidateToken(string? u_token)
        {
            Token? token = null;

            if (u_token != null && currentTokens.TryGetValue(u_token, out Token? value))
            {
                if (value.t_u_token != null && value.t_u_token.Equals(u_token) && DateTime.Now < value.t_expires)
                {
                    token = value;
                }
            }

            return token;
        }

        /// <summary>
        /// Token 
        /// These volotile objects are held in a dictionary
        /// </summary>
        public class Token
        {
            public int t_u_id;
            public string? t_u_token;
            public DateTime t_expires;
        }
    }
}
