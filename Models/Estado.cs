using System.ComponentModel.DataAnnotations;

namespace Presta.net_app.Models
{
    public class Estado
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo no debe ser vacío")]
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }
        public List<Prestamo>? Prestamos { get; set; }
    }
}
