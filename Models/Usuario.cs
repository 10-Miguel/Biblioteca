namespace Biblioteca.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public Usuario() { }

    public Usuario(int id, string nombre, string email)
    {
        Id = id;
        Nombre = nombre;
        Email = email;
        Activo = true;
    }

    public string ResumenCorto() => $"Usuario #{Id}: {Nombre}";

    public string DetalleCompleto() =>
        "---------- DETALLE DEL USUARIO ----------\n" +
        $"ID: {Id}\n" +
        $"NOMBRE: {Nombre}\n" +
        $"EMAIL: {Email}\n" +
        $"ACTIVO: {(Activo ? "SI" : "NO")}\n" +
        "-----------------------------------------";

    public override string ToString() => ResumenCorto();
}