using System.ComponentModel.DataAnnotations;

namespace Presta.net_app.Models
{
    public class Prestatario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo no debe ser vacío")]
        [Display(Name = "Nombre")]
        [StringLength(60)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Este campo no debe ser vacío")]
        [Display(Name = "Apellido")]
        [StringLength(60)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Este campo no debe ser vacío")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo no debe ser vacío")]
        [Display(Name = "Teléfono")]
        public long Telefono { get; set; }

        [Required(ErrorMessage = "Este campo no debe ser vacío")]
        [Display(Name = "Fecha de nacimiento")]
        public DateOnly? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Este campo no debe ser vacío")]
        [Display(Name = "DNI")]
        public int DNI { get; set; }

        [Display(Name = "Foto Frente DNI")]
        public string? FotoFrenteDni { get; set; }

        [Display(Name = "Foto Dorso DNI")]
        public string? FotoDorsoDni { get; set; }

        [Required(ErrorMessage = "Este campo no debe ser vacío")]
        [Display(Name = "Provincia")]
        public string Provincia { get; set; }

        [Required(ErrorMessage = "Este campo no debe ser vacío")]
        [Display(Name = "Localidad")]
        public string Localidad { get; set; }

        [Required(ErrorMessage = "Este campo no debe ser vacío")]
        [Display(Name = "Direccion")]
        public string Direccion { get; set; }

        public virtual List<Prestamo>? Prestamos { get; set; }
    }
}
