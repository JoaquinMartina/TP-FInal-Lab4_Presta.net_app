namespace Presta.net_app.Models
{
    public class PrestamoDetalle
    {
        public int Id { get; set; }
        public int PrestamoId { get; set; }
        public Prestamo Prestamo { get; set; }
        public int NroCuota { get; set; }
        public DateOnly FechaPago { get; set; }
        public decimal MontoCuota { get; set; }
    }
}
