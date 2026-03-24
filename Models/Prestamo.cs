namespace Biblioteca.Models;

public class Prestamo
{
    public Libro LibroPrestado { get; set; } = null!;
    public Usuario Prestatario { get; set; } = null!;
    public DateTime FechaSalida { get; set; }
    public DateTime? FechaDevolucion { get; set; }
    public EstadoPrestamo Estado { get; set; }

    public Prestamo()
    {
        Estado = EstadoPrestamo.Activo;
        FechaDevolucion = null;
    }

    public Prestamo(Libro libro, Usuario usuario, DateTime fechaSalida)
    {
        LibroPrestado = libro;
        Prestatario = usuario;
        FechaSalida = fechaSalida;
        Estado = EstadoPrestamo.Activo;
        FechaDevolucion = null;
    }

    public bool EstaVencido() =>
        DiasTranscurridos() > 8 && Estado == EstadoPrestamo.Activo;

    public int DiasTranscurridos() =>
        (DateTime.Now - FechaSalida).Days;

    public string ResumenCorto() =>
        $"PRESTAMO: '{LibroPrestado.Titulo}' solicitado por {Prestatario.Nombre}";

    public string DetalleCompleto()
    {
        string fDevolucion = FechaDevolucion.HasValue
            ? FechaDevolucion.Value.ToShortDateString()
            : "PENDIENTE";

        return "---------- DETALLE DEL PRESTAMO ----------\n" +
               $"LIBRO: {LibroPrestado.Titulo} (ISBN: {LibroPrestado.Isbn})\n" +
               $"USUARIO: {Prestatario.Nombre}\n" +
               $"FECHA SALIDA: {FechaSalida.ToShortDateString()}\n" +
               $"FECHA DEVOLUCION: {fDevolucion}\n" +
               $"ESTADO: {Estado}\n" +
               $"DIAS TRANSCURRIDOS: {DiasTranscurridos()}\n" +
               $"VENCIDO: {(EstaVencido() ? "SI" : "NO")}\n" +
               "------------------------------------------";
    }

    public override string ToString() => ResumenCorto();
}