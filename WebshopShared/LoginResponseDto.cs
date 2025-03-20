namespace WebshopShared
{
    public class LoginResponseDto
    {
        public string TokenType { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public long ExpiresIn { get; set; }
        public UserClaimsDto Claims { get; set; } = null!;
    }
}
