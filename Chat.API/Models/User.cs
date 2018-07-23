namespace Chat.API.Models
{
    public class User
    {   
        public int Id {get; private set;}
        public string Username {get; private set;}
        public string PasswordHash {get; private set;}
        public string PasswordSalt {get; private set;}
        public string Email {get; private set;}

        private User() { }

        public User(string username, string passwordHash, string passwordSalt, string email)
        {
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Email = email;
        }
    }
}