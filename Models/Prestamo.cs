using System.ComponentModel.DataAnnotations;

namespace Presta.net_app.Models
{
    public class Prestamo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe asignar un Prestatario")]
        [Display(Name = "Prestatario")]
        public int PrestatarioId { get; set; }
        public Prestatario Prestatario { get; set; }

        [Required(ErrorMessage = "Debe asignar un estado")]
        [Display(Name = "Estado")]
        public int EstadoId { get; set; }
        public Estado Estado { get; set; }

        [Required(ErrorMessage = "Este campo no debe ser vacío")]
        [Display(Name = "Fecha de inicio")]
        public DateOnly FechaInicio { get; set; }

        [Required(ErrorMessage = "Este campo no debe ser vacío")]
        [Display(Name = "Monto")]
        public int MontoCapital { get; set; }

        [Required(ErrorMessage = "Este campo no debe ser vacío")]
        [Display(Name = "Cantidad de Cuotas")]
        public int CantidadCuotas { get; set; }

        [Required(ErrorMessage = "Este campo no debe ser vacío")]
        [Display(Name = "Porcentaje de Interés")]
        public decimal InteresPorcentaje { get; set; }

        public double? ValorPorCuota { get; set; }

        public double? ValorInteres { get; set; }

        public double? ValorTotal { get; set; }
        public ICollection<PrestamoDetalle> PrestamoDetalles { get; set; } = new List<PrestamoDetalle>();
    }
}
