using System;
using Biblioteca.Models;
using System.Threading;
using System.Collections.Generic;
using Biblioteca.Services;

namespace Biblioteca;
class Program
{
    static LibroService libroService = new();
    static UsuarioService usuarioService = new();
    static PrestamoService prestamoService = new();

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

    // Ahora usamos el servicio para agregar
    libroService.Agregar(libro1);
    libroService.Agregar(libro2);
    usuarioService.Agregar(usuario1);
    usuarioService.Agregar(usuario2);
    prestamoService.RegistrarPrestamo(prestamo1);
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
        // usa el servicio
        libroService.Agregar(new Libro(isbn, titulo, autor));
        Ok("Libro agregado.");
        break;

    case "2":
        Console.Clear();
        var todos = libroService.ObtenerTodos(); // Pides la lista al servicio
        if (todos.Count == 0) { Ok("No hay libros."); break; }
        foreach (var l in todos)
        {
            Console.WriteLine(l.DetalleCompleto());
        }
        // KPIs
        Console.WriteLine($"\nTOTAL LIBROS: {libroService.TotalLibros()}");
        Console.WriteLine($"DISPONIBLES: {libroService.LibrosDisponibles()} | PRESTADOS: {libroService.LibrosPrestados()}");
        Ok("");
        break;

    case "3":
        Console.Write("ISBN: "); string buscar = Console.ReadLine() ?? "";
        // Llamas al método del servicio
        var lib = libroService.BuscarPorIsbn(buscar); 
        Ok(lib != null ? lib.DetalleCompleto() : "No encontrado.");
        break;

    case "4":
        // Punto 5 obligatorio 
        libroService.OrdenarPorTitulo();
        Ok("Libros ordenados por título satisfactoriamente.");
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
        Console.WriteLine("1. Agregar\n2. Listar\n3. Ordenar por Nombre\n0. Volver");
        Console.Write("\nOpcion: ");
        switch (Console.ReadLine())
        {
            case "1":
                // Usamos el total del servicio para generar el ID
                int id = usuarioService.TotalUsuarios() + 1;
                Console.Write("Nombre: "); string nombre = Console.ReadLine() ?? "";
                Console.Write("Email: "); string email = Console.ReadLine() ?? "";
                
                usuarioService.Agregar(new Usuario(id, nombre, email));
                Ok($"Usuario creado con ID {id}.");
                break;
            case "2":
                Console.Clear();
                var lista = usuarioService.ObtenerTodos();
                if (lista.Count == 0) { Ok("No hay usuarios."); break; }
                foreach (var u in lista)
                {
                    Console.WriteLine(u.DetalleCompleto());
                }
                // KPI Obligatorio
                Console.WriteLine($"\nTotal Usuarios: {usuarioService.TotalUsuarios()}");
                Console.WriteLine($"Usuarios Activos: {usuarioService.UsuariosActivos()}");
                Ok("");
                break;
            case "3":
                usuarioService.OrdenarPorNombre();
                Ok("Usuarios ordenados alfabéticamente.");
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


 static void MenuReportes()
    {
        bool volver = false;
        while (!volver)
        {
            Console.Clear();
            Console.WriteLine(">> REPORTES");
            Console.WriteLine("1. Prestamos activos\n2. Prestamos vencidos\n3. Libros disponibles\n4. Objetos de prueba\n0. Volver");
            Console.Write("\nOpcion: ");
            switch (Console.ReadLine())
            {
                case "1":
                    var activos = prestamos.FindAll(x => x.Estado == EstadoPrestamo.Activo);
                    Console.Clear();
                    if (activos.Count == 0) { Ok("No hay prestamos activos."); break; }
                    activos.ForEach(x => Console.WriteLine(x.ResumenCorto()));
                    Ok("");
                    break;
                case "2":
                    var vencidos = prestamos.FindAll(x => x.EstaVencido());
                    Console.Clear();
                    if (vencidos.Count == 0) { Ok("No hay prestamos vencidos."); break; }
                    vencidos.ForEach(x => Console.WriteLine(x.DetalleCompleto()));
                    Ok("");
                    break;
                case "3":
                    var disp = libros.FindAll(l => l.Disponible);
                    Console.Clear();
                    if (disp.Count == 0) { Ok("No hay libros disponibles."); break; }
                    disp.ForEach(l => Console.WriteLine(l.ResumenCorto()));
                    Ok("");
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine("=== OBJETOS DE PRUEBA ===\n");
                    Console.WriteLine("-- LIBROS --");
                    foreach (var l in libros)
                    {
                        Console.WriteLine(l.DetalleCompleto());
                        Console.WriteLine($"  Disponible: {(l.Disponible ? "SI" : "NO")}\n");
                    }
                    Console.WriteLine("-- USUARIOS --");
                    foreach (var u in usuarios)
                    {
                        Console.WriteLine(u.DetalleCompleto());
                        Console.WriteLine($"  Activo: {(u.Activo ? "SI" : "NO")}\n");
                    }
                    Console.WriteLine("-- PRESTAMOS --");
                    foreach (var pr in prestamos)
                    {
                        Console.WriteLine(pr.DetalleCompleto());
                        Console.WriteLine($"  Estado: {pr.Estado} | Vencido: {(pr.EstaVencido() ? "SI" : "NO")} | Dias: {pr.DiasTranscurridos()}\n");
                    }
                    Ok("");
                    break;
                case "0": volver = true; break;
                default: Error(); break;
            }
        }
    }

    static void MenuDatos()
    {
        Console.Clear();
        Console.WriteLine(">> DATOS");
        Console.WriteLine("1. Guardar\n2. Cargar\n3. Reiniciar\n0. Volver");
        Console.Write("\nOpcion: ");
        string op = Console.ReadLine() ?? "";
        if (op == "3")
        {
            Console.Write("Confirmar reinicio? (S/N): ");
            if (Console.ReadLine()?.ToUpper() == "S")
            {
                libros.Clear(); usuarios.Clear(); prestamos.Clear();
                Ok("Sistema reseteado.");
            }
        }
        else if (op != "0") Ok("Sincronizando datos...");
    }

    static void Ok(string msg)
    {
        if (!string.IsNullOrEmpty(msg)) Console.WriteLine($"\n[OK]: {msg}");
        Console.WriteLine("Presione una tecla...");
        Console.ReadKey();
    }    
    static void Error()
    {
        Console.WriteLine("\n[!] Opción inválida.");
        Thread.Sleep(800);
    }

}
