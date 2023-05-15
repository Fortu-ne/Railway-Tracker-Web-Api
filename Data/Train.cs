using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Railway.Data
{
    public class Train
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TrainNumber { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Destination { get ; set; }  = string.Empty;
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<Station> Stations { get; set; }
    }

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
       
    }
    public class Admin : User
    {
        public Admin()
        {
            Roles = "Admin";
        }
      
    }

    public class Supervisior : User
    {
        public Supervisior()
        {
            Roles = "Supervisor";
        }
    }



    
}
