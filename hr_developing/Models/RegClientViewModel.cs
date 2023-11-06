using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hr_developing.Models;

public partial class RegClientViewModel
{

    public string Id { get; set; } = null!;

    [Required(ErrorMessage = "Заполните поле Name")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Заполните поле Surname")]
    public string Surname { get; set; } = null!;

    [Required(ErrorMessage = "Заполните поле Age")]
    public short Age { get; set; }

    [Required(ErrorMessage = "Заполните поле Password")]
    [MinLength(8, ErrorMessage = "Пароль должен быть длиннее 8 символов")]
    //[Remote(controller: "Registraion", action: "checkpassword", ErrorMessage = "Введённый пароль слишком лёгкий")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Заполните поле Email")]
    [Remote(controller: "Registration", action: "CheckEmail", ErrorMessage = "Пользователь с данныи Email уже зарегестрирован")]
    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    [Required(ErrorMessage = "выберите одно из двух")]
    public bool Status { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();

    public virtual ICollection<WorkExperience> WorkExperiences { get; set; } = new List<WorkExperience>();
}
