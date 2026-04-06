using Biblioteca.Models;
using System.Linq;

namespace Biblioteca.Services;

public class PrestamoService
{
    private List<Prestamo> prestamos = new();

    public void RegistrarPrestamo(Prestamo p) => prestamos.Add(p);

    public List<Prestamo> ObtenerTodos() => prestamos;

    // Búsquedas
    public List<Prestamo> BuscarPorEstado(EstadoPrestamo estado) => 
        prestamos.Where(p => p.Estado == estado).ToList();

    // KPIs
    public int TotalPrestamos() => prestamos.Count;
    
    public int PrestamosActivos() => prestamos.Count(p => p.Estado == EstadoPrestamo.Activo);

    public double PromedioDiasPrestamo()
    {
        if (prestamos.Count == 0) return 0;
        // Solo contamos los que ya se devolvieron para tener una fecha final
        var devueltos = prestamos.Where(p => p.FechaDevolucion.HasValue).ToList();
        return devueltos.Count > 0 ? devueltos.Average(p => (p.FechaDevolucion!.Value - p.FechaSalida).Days) : 0;
    }
}