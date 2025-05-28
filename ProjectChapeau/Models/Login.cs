namespace ProjectChapeau.Models
{
    public class Login
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public Login()
        {
        }

        public Login(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
