namespace LoginandRegistration.API_Model
{
    public class SignUp
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Role { get; set; }
        public string? Password { get; set; }

        public long MobileNumber { get; set; }
    }
}
