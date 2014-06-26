namespace TeamCityManager.Entities
{
    using System.Collections.Generic;

    public class User
    {
        public string Email { get; set; }

        public string FullName { get; set; }

        public List<string> Groups { get; set; }

        public bool IsAdmin { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }
    }
}