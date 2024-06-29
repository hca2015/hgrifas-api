namespace api_lrpd.Models.DTO
{
    public class AccessTokenResponse
    {
        public string accessToken { get; set; }
        public DateTime expiration { get; set; }
        public UsuarioResponse usuario { get; set; }
    }
}

