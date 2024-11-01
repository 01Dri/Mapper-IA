using System;
namespace MapperIA.Tests.Mapped_C_Plus_Plus
{
    public class UsuarioMapeado
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UsuarioMapeado(string user, string mail, string pass)
        {
            Username = user;
            Email = mail;
            Password = pass;
        }
        public void DisplayInfo()
        {
            Console.WriteLine($"Username: {Username}, Email: {Email}");
        }
    }
}