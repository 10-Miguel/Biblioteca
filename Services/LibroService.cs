using Biblioteca.Models;
using System.Linq;

namespace Biblioteca.Services;

public class LibroService
{
    private List<Libro> libros = new();

    public void Agregar(Libro libro) => libros.Add(libro);
    
    public List<Libro> ObtenerTodos() => libros;

    // Búsquedas
    public Libro? BuscarPorIsbn(string isbn) => libros.FirstOrDefault(l => l.Isbn == isbn);
    public List<Libro> BuscarPorAutor(string autor) => 
        libros.Where(l => l.Autor.Contains(autor, StringComparison.OrdinalIgnoreCase)).ToList();

    // Ordenación
    public void OrdenarPorTitulo() => libros = libros.OrderBy(l => l.Titulo).ToList();

    // KPIs
    public int TotalLibros() => libros.Count;
    public int LibrosPrestados() => libros.Count(l => !l.Disponible);
    public int LibrosDisponibles() => libros.Count(l => l.Disponible);
}