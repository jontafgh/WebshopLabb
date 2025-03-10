namespace WebshopShared
{
    public class RegisterLoginResponseDto
    {
        public bool Succeeded { get; set; }
        public List<string> ErrorList { get; set; } = [];
    }
}
