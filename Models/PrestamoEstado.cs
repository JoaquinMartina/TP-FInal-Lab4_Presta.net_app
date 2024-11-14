namespace Presta.net_app.Models
{
    public class PrestamoEstado
    {
        public int PrestamoId { get; set; }
        public int EstadoId { get; set; }
        public Prestamo? Prestamo { get; set; }
        public Estado? Estado  { get; set; }
    }
}
