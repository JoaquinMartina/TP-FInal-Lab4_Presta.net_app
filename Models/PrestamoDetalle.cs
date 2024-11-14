namespace Presta.net_app.Models
{
    public class PrestamoDetalle
    {
        public int Id { get; set; }
        public int PrestatarioId { get; set; }
        public int PrestamoId { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public Decimal MontoCapital { get; set; }
        public Decimal MontoInteres { get; set; }
        public Decimal MontoGastos { get; set; }
        public Decimal MontoCuota { get; set; }
    }
}
