using System.ComponentModel.DataAnnotations;

namespace Railway.Dtos
{
    public class UserDto
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        [Compare("Password",ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
    }

    public class LoginDto
    {
       [EmailAddress]      
       public string Email { get; set; }
       public string Password { get; set; }
        
    }


}
