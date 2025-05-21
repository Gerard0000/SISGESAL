using System.ComponentModel.DataAnnotations;

namespace SISGESAL.web.Models
{
    public class ResetPasswordViewModel : AddUserViewModel
    {
        [Display(Name = "Nueva Contraseña")]
        [Required(ErrorMessage = "La {0} es requerida.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 12, ErrorMessage = "La {0} debe tener entre {2} y {1} carácteres.")]
        public string? ResetPassword { get; set; }

        [Display(Name = "Confirmación de Contraseña")]
        [Required(ErrorMessage = "La {0} es requerida.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La {0} debe tener entre {2} y {1} carácteres.")]
        [Compare("ResetPassword", ErrorMessage = "La Nueva Contraseña y la {0} no coinciden.")]
        public string? ResetPasswordConfirm { get; set; }
    }
}