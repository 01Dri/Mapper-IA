using System;
namespace MapperIA.Tests
{
    public class Usuario
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Usuario(string user, string mail, string pass)
        {
            Username = user;
            Email = mail;
            Password = pass;
        }
        public void DisplayInfo()
        {
            Console.WriteLine("Username: " + Username + ", Email: " + Email);
        }
    }
}