using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace hr_developing.Models
{
    public class AuthClientViewModel
    {
        [Required(ErrorMessage = "Заполните поле Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Заполните поле Password")]
        public string Password { get; set; }
    }
}
