namespace XaniAPI.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorisationRequest
    {
        public int u_id { get; set; }
        public string? u_password_hash { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AuthorisationResponse
    {
        public int u_id { get; set; }
        public string? u_token { get; set; }
    }
}
