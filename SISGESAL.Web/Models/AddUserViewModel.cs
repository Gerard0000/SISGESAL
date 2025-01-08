using System.ComponentModel.DataAnnotations;

namespace SISGESAL.web.Models
{
    public class AddUserViewModel : EditUserViewModel
    {
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La {0} es requerida.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La {0} debe tener entre {2} y {1} carácteres.")]
        public string? Password { get; set; }

        [Display(Name = "Confirmación de Contraseña")]
        [Required(ErrorMessage = "La {0} es requerida.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La {0} debe tener entre {2} y {1} carácteres.")]
        [Compare("Password")]
        public string? PasswordConfirm { get; set; }
    }
}
