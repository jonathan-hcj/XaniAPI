using Azure.Core;
using Azure;

namespace XaniAPI.Entites
{
    public static class TokenRepostitory
    {
        private static readonly Dictionary<Int32, Token> currentTokens = [];

        public static void RecordToken(Int32 u_id, string u_token)
        {
            if (currentTokens.TryGetValue(u_id, out Token? value))
            {
                value.t_u_token = u_token;
                value.t_u_id = u_id;
                value.t_expires = DateTime.Now.AddMinutes(10);
            }
            else
            {
                currentTokens.Add(u_id, new Token
                {
                    t_u_token = u_token,
                    t_u_id = u_id,
                    t_expires = DateTime.Now.AddMinutes(10)
                });
            }
        }

        public static bool ValidateToken(Int32 u_id, string u_token)
        {
            var result = false;

            if (currentTokens.TryGetValue(u_id, out Token? value))
            {
                if (value.t_u_token != null && value.t_u_token.Equals(u_token) && DateTime.Now < value.t_expires)
                {
                    result = true;
                }
            }

            return result;
        }

        public class Token
        {
            public Int32 t_u_id;
            public string? t_u_token;
            public DateTime t_expires;
        }
    }
}
