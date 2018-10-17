using System.ComponentModel.DataAnnotations;

namespace Lab06.MVC.Carriage.Models
{
    public class RegisterViewModel
    {
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Incorrect email")]
        public string Email { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Length of name must be in interval 5 and 15 symbols")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string PasswordConfirm { get; set; }
    }
}