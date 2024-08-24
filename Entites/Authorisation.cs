namespace XaniAPI.Entites
{
    public class AuthorisationRequest
    {
        public Int32 u_id { get; set; }
        public string? u_password_hash { get; set; }
    }

    public class AuthorisationResponse
    {
        public Int32 u_id { get; set; }
        public string? u_token { get; set; }
    }
}
