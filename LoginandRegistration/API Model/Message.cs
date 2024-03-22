namespace LoginandRegistration.API_Model
{
    public class SignUpMessage
    {
        public bool EmailExists { get; set; }
        public bool MobileNumberExists { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }

    }

    public class AuthMessageModel
    {
        public bool AccountExists { get; set; }
        public bool PasswordStatus { get; set; }
        public string Token { get; set; }

    }
}

