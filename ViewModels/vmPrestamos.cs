using Microsoft.AspNetCore.Mvc.Rendering;
namespace Presta.net_app.Models;

public class vmPrestamos
{
    public List<Prestamo> ListaPrestamos { get; set; }
    public SelectList ListaEstados { get; set; }
    public SelectList ListaPrestatarios { get; set; }
    public int? EstadoId { get; set; }
    public int? PrestatarioId { get; set; }
}
