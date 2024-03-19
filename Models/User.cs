using System.ComponentModel.DataAnnotations;
namespace Scheduling_Simulator.Models
{
    public class User
    {
            [Key]
            public int UserId { get; set; }

            /*
            [Required(ErrorMessage = "Username is required")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
            public string Username { get; set; }

            public string Password { get; set; }*/

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid Email Address")]
            public string Email { get; set; }
        }
    }

