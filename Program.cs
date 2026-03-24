using System;
using Biblioteca.Models;
using System.Threading;
using System.Collections.Generic;


namespace Biblioteca;
class Program
{
    static List<Libro> libros = new();
    static List<Usuario> usuarios = new();
    static List<Prestamo> prestamos = new();

    static void Main()
    {
        CargarDatosPrueba();
        MenuPrincipal();
    }
    static void CargarDatosPrueba()
    {
        var libro1 = new Libro("978-0-06-112008-4", "Cien Anos de Soledad", "Gabriel Garcia Marquez");
        var libro2 = new Libro("978-84-376-0494-7", "El Quijote", "Miguel de Cervantes");
        var usuario1 = new Usuario(1, "Ana Torres", "ana@email.com");
        var usuario2 = new Usuario(2, "Luis Perez", "luis@email.com");
        libro1.Disponible = false;
        var prestamo1 = new Prestamo(libro1, usuario1, DateTime.Now.AddDays(-10));
        libros.AddRange(new[] { libro1, libro2 });
        usuarios.AddRange(new[] { usuario1, usuario2 });
        prestamos.Add(prestamo1);
    }

    static void MenuPrincipal()
        {
        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("     SISTEMA DE GESTIÓN BIBLIOTECARIA   ");
            Console.WriteLine("1. Libros");
            Console.WriteLine("2. Usuarios");
            Console.WriteLine("3. Préstamos");
            Console.WriteLine("4. Búsquedas y Reportes");
            Console.WriteLine("5. Guardar / Cargar Datos");
            Console.WriteLine("0. Salir");
            Console.Write("\nSeleccione una opción: ");

            switch (Console.ReadLine())
            {
                case "1": MenuLibros(); break;
                case "2": MenuUsuarios(); break;
                case "3": MenuPrestamos(); break;
                case "4": MenuReportes(); break;
                case "5": MenuDatos(); break;
                case "0":
                    Console.Write("Guardar antes de salir? (S/N): ");
                    if (Console.ReadLine()?.ToUpper() == "S") Ok("Guardando...");
                    salir = true;
                    break;
                default: Error(); break;
            }
        }
    }

static void MenuLibros()
    {
        bool volver = false;
        while (!volver)
        {
            Console.Clear();
            Console.WriteLine(">> LIBROS");
            Console.WriteLine("1. Agregar\n2. Listar\n3. Buscar ISBN\n0. Volver");
            Console.Write("\nOpcion: ");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("ISBN: "); string isbn = Console.ReadLine() ?? "";
                    Console.Write("Titulo: "); string titulo = Console.ReadLine() ?? "";
                    Console.Write("Autor: "); string autor = Console.ReadLine() ?? "";
                    libros.Add(new Libro(isbn, titulo, autor));
                    Ok("Libro agregado.");
                    break;
                case "2":
                    Console.Clear();
                    if (libros.Count == 0) { Ok("No hay libros."); break; }
                    foreach (var l in libros)
                    {
                        Console.WriteLine(l.DetalleCompleto());
                        Console.WriteLine($"  Disponible: {(l.Disponible ? "SI" : "NO")}\n");
                    }
                    Ok("");
                    break;
                case "3":
                    Console.Write("ISBN: "); string buscar = Console.ReadLine() ?? "";
                    var lib = libros.Find(l => l.Isbn == buscar);
                    Ok(lib != null ? lib.DetalleCompleto() : "No encontrado.");
                    break;
                case "0": volver = true; break;
                default: Error(); break;
            }
        }
    }

static void MenuUsuarios()
    {
        bool volver = false;
        while (!volver)
        {
            Console.Clear();
            Console.WriteLine(">> USUARIOS");
            Console.WriteLine("1. Agregar\n2. Listar\n0. Volver");
            Console.Write("\nOpcion: ");
            switch (Console.ReadLine())
            {
                case "1":
                    int id = usuarios.Count + 1;
                    Console.Write("Nombre: "); string nombre = Console.ReadLine() ?? "";
                    Console.Write("Email: "); string email = Console.ReadLine() ?? "";
                    usuarios.Add(new Usuario(id, nombre, email));
                    Ok($"Usuario creado con ID {id}.");
                    break;
                case "2":
                    Console.Clear();
                    if (usuarios.Count == 0) { Ok("No hay usuarios."); break; }
                    foreach (var u in usuarios)
                    {
                        Console.WriteLine(u.DetalleCompleto());
                        Console.WriteLine($"  Activo: {(u.Activo ? "SI" : "NO")}\n");
                    }
                    Ok("");
                    break;
                case "0": volver = true; break;
                default: Error(); break;
            }
        }
    }

static void MenuPrestamos()
    {
        bool volver = false;
        while (!volver)
        {
            Console.Clear();
            Console.WriteLine(">> PRESTAMOS");
            Console.WriteLine("1. Crear\n2. Devolver\n3. Listar\n0. Volver");
            Console.Write("\nOpcion: ");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("ISBN del libro: "); string isbn = Console.ReadLine() ?? "";
                    var libro = libros.Find(l => l.Isbn == isbn && l.Disponible);
                    if (libro == null) { Ok("Libro no disponible."); break; }
                    Console.Write("ID usuario: ");
                    if (!int.TryParse(Console.ReadLine(), out int uid)) { Ok("ID invalido."); break; }
                    var usuario = usuarios.Find(u => u.Id == uid && u.Activo);
                    if (usuario == null) { Ok("Usuario no encontrado."); break; }
                    libro.Disponible = false;
                    prestamos.Add(new Prestamo(libro, usuario, DateTime.Now));
                    Ok($"Prestamo creado.");
                    break;
                case "2":
                    Console.Write("ISBN a devolver: "); string isbnDev = Console.ReadLine() ?? "";
                    var p = prestamos.Find(x => x.LibroPrestado.Isbn == isbnDev && x.Estado == EstadoPrestamo.Activo);
                    if (p == null) { Ok("Prestamo activo no encontrado."); break; }
                    p.FechaDevolucion = DateTime.Now;
                    p.Estado = p.EstaVencido() ? EstadoPrestamo.Vencido : EstadoPrestamo.Devuelto;
                    p.LibroPrestado.Disponible = true;
                    Ok($"Devolucion registrada. Estado: {p.Estado}");
                    break;
                case "3":
                    Console.Clear();
                    if (prestamos.Count == 0) { Ok("No hay prestamos."); break; }
                    foreach (var pr in prestamos)
                    {
                        Console.WriteLine(pr.DetalleCompleto());
                        Console.WriteLine($"  Vencido: {(pr.EstaVencido() ? "SI" : "NO")} | Dias: {pr.DiasTranscurridos()}\n");
                    }
                    Ok("");
                    break;
                case "0": volver = true; break;
                default: Error(); break;
            }
        }
    }
    
    static void Error()
    {
        Console.WriteLine("\n[!] Opción inválida.");
        Thread.Sleep(800);
    }

}
