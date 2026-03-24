namespace Biblioteca.Models;

public class Libro
{
    public string Isbn { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public bool Disponible { get; set; } = true;

    public Libro() { }

    public Libro(string isbn, string titulo, string autor)
    {
        Isbn = isbn;
        Titulo = titulo;
        Autor = autor;
        Disponible = true;
    }

    public string ResumenCorto() =>
        $"[{(Disponible ? "DISPONIBLE" : "PRESTADO")}] {Titulo} - {Autor}";

    public string DetalleCompleto() =>
        "---------- DETALLE DEL LIBRO ----------\n" +
        $"ISBN: {Isbn}\n" +
        $"TITULO: {Titulo}\n" +
        $"AUTOR: {Autor}\n" +
        $"DISPONIBLE: {(Disponible ? "SI" : "NO")}\n" +
        "---------------------------------------";

    public override string ToString() => ResumenCorto();
}