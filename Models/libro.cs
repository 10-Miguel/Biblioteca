// --- SECCIÓN 1: LIBROS ---
namespace system_books.Models
{
    public class Libro
    {
        public string Isbn { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public bool Disponible { get; set; } = true;

        public Libro() { }

        public Libro(string isbn, string titulo, string autor)
        {
            Isbn = isbn;
            Titulo = titulo;
            Autor = autor;
        }

        public string ResumenCorto() => $"{Titulo} - {Autor}";

        public string DetalleCompleto() => $"ISBN: {Isbn} | Título: {Titulo} | Autor: {Autor} | Estado: {(Disponible ? "Disponible" : "Prestado")}";

        public override string ToString() => DetalleCompleto();
    }
}