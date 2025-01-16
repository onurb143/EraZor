namespace EraZor.Model
{
    public class AuthResponse
    {
        public string Message { get; set; }
        public string? Token { get; set; } // Token can be null in case of an error
    }
}
