using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hr_developing.Models;

public partial class AuthClientModel
{

    public string Id { get; set; } = null!;
    [Required(ErrorMessage = "Заполните поле Name")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Заполните поле Surname")]
    public string Surname { get; set; } = null!;
    [Required(ErrorMessage = "Заполните поле Age")]
    public short Age { get; set; }
    [Required(ErrorMessage = "Заполните поле Email")]
    public string Password { get; set; } = null!;
    [Required(ErrorMessage = "Заполните поле Email")]
    public string Email { get; set; } = null!;

    public string? Phone { get; set; }
    [Required(ErrorMessage = "выберите одно из двух")]
    public bool Status { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();

    public virtual ICollection<WorkExperience> WorkExperiences { get; set; } = new List<WorkExperience>();
}
