namespace XaniAPI.Entities
{
    /// <summary>
    /// Authorisation API objects
    /// the reuest is the user name and the password hash (SHA-256)
    /// </summary>
    public class AuthorisationRequest
    {
        public string username { get; set; } = "";
        public string password_hash { get; set; } = "";
    }

    /// <summary>
    /// The response, if found is the user is and the current token */
    /// </summary>
    public class AuthorisationResponse
    {
        public int id { get; set; }
        public string? token { get; set; }
    }
}
