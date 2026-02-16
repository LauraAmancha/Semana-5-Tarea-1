using System.ComponentModel.DataAnnotations;

namespace GestionEscuelaTarea.Models
{
    public class Estudiante
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")] // <--- Mensaje claro
        [StringLength(50, ErrorMessage = "No puede tener más de 50 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(50)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "La cédula debe tener 10 dígitos")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido (ej: usuario@correo.com)")] // <--- Valida formato
        public string Email { get; set; }
    }
}