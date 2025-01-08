using System.ComponentModel.DataAnnotations;

namespace SISGESAL.web.Models
{
    public class LoginViewModel
    {
        //TODO: cambiar el usuario a tipo normal cuando se cambie el helper
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        //[EmailAddress(ErrorMessage = "El usuario es de tipo email.")]
        public string? Username { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La {0} es requerida.")]
        [StringLength(20, MinimumLength = 12, ErrorMessage = "La {0} debe tener entre {2} y {1} carácteres.")]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
