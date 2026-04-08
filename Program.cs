using System;
using Biblioteca.Models;
using System.Threading;
using System.Collections.Generic;
using Biblioteca.Services;
using System.IO;
using System.Text.Json;
using System.Linq; // Necesario para búsquedas avanzadas

namespace Biblioteca;

class Program
{
    static LibroService libroService = new();
    static UsuarioService usuarioService = new();
    static PrestamoService prestamosService = new();

    static string pathLibros = "libros.json";
    static string pathUsuarios = "usuarios.json";
    static string pathPrestamos = "prestamos.json";

    static void Main()
    {
        Console.Title = "Vivi Librería - Sistema de Gestión";
        
        // 1. Carga inicial
        if (!CargarDatosDesdeArchivos())
        {
            CargarDatosPrueba();
        }

        MenuPrincipal();
    }

    // --- PERSISTENCIA MEJORADA ---

    static void GuardarDatosEnArchivos()
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            File.WriteAllText(pathLibros, JsonSerializer.Serialize(libroService.ObtenerTodos(), options));
            File.WriteAllText(pathUsuarios, JsonSerializer.Serialize(usuarioService.ObtenerTodos(), options));
            File.WriteAllText(pathPrestamos, JsonSerializer.Serialize(prestamosService.ObtenerTodos(), options));

            Ok("Base de datos sincronizada (JSON).");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[!] Error de escritura: {ex.Message}");
            Ok("");
        }
    }

    static bool CargarDatosDesdeArchivos()
    {
        try
        {
            if (!File.Exists(pathLibros)) return false;

            var libros = JsonSerializer.Deserialize<List<Libro>>(File.ReadAllText(pathLibros));
            libros?.ForEach(l => libroService.Agregar(l));

            var usuarios = JsonSerializer.Deserialize<List<Usuario>>(File.ReadAllText(pathUsuarios));
            usuarios?.ForEach(u => usuarioService.Agregar(u));

            var prestamos = JsonSerializer.Deserialize<List<Prestamo>>(File.ReadAllText(pathPrestamos));
            if (prestamos != null)
            {
                foreach (var p in prestamos)
                {
                    prestamosService.RegistrarPrestamo(p);
                    // CRÍTICO: Actualizar disponibilidad del libro en el servicio
                    var libroEnSistema = libroService.BuscarPorIsbn(p.LibroPrestado.Isbn);
                    if (libroEnSistema != null) libroEnSistema.Disponible = false;
                }
            }
            return true;
        }
        catch { return false; }
    }

    static void CargarDatosPrueba()
    {
        libroService.Agregar(new Libro("978-01", "Cien Años de Soledad", "Gabo"));
        libroService.Agregar(new Libro("978-02", "El Quijote", "Cervantes"));
        usuarioService.Agregar(new Usuario(1, "Ana Torres", "ana@email.com"));
    }

    // --- INTERFAZ DE MENÚS ---

    static void MenuPrincipal()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine("  SISTEMA DE GESTIÓN BIBLIOTECARIA  ");
            Console.WriteLine("====================================");
            Console.WriteLine("1. Gestión de Libros");
            Console.WriteLine("2. Gestión de Usuarios");
            Console.WriteLine("3. Préstamos y Devoluciones");
            Console.WriteLine("4. Búsquedas y Reportes (KPIs)");
            Console.WriteLine("5. Guardar Cambios");
            Console.WriteLine("0. Salir");
            Console.Write("\nSeleccione una opción: ");

            switch (Console.ReadLine())
            {
                case "1": MenuLibros(); break;
                case "2": MenuUsuarios(); break;
                case "3": MenuPrestamos(); break;
                case "4": MenuReportes(); break;
                case "5": GuardarDatosEnArchivos(); break;
                case "0":
                    Console.Write("¿Guardar antes de salir? (S/N): ");
                    if (Console.ReadLine()?.ToUpper() == "S") GuardarDatosEnArchivos();
                    return;
                default: Error("Opción no válida."); break;
            }
        }
    }

    static void MenuLibros()
    {
        Console.Clear();
        Console.WriteLine(">> SECCIÓN LIBROS");
        Console.WriteLine("1. Nuevo Libro\n2. Inventario Total\n0. Volver");
        string op = Console.ReadLine();
        if (op == "1")
        {
            Console.Write("ISBN: "); string isbn = Console.ReadLine();
            Console.Write("Título: "); string tit = Console.ReadLine();
            Console.Write("Autor: "); string aut = Console.ReadLine();
            libroService.Agregar(new Libro(isbn, tit, aut));
            Ok("Libro indexado correctamente.");
        }
        else if (op == "2")
        {
            Console.WriteLine("\n--- INVENTARIO ---");
            libroService.ObtenerTodos().ForEach(l => Console.WriteLine(l.DetalleCompleto()));
            Ok($"Total en catálogo: {libroService.TotalLibros()}");
        }
    }

    static void MenuUsuarios()
    {
        Console.Clear();
        Console.WriteLine(">> SECCIÓN USUARIOS");
        Console.WriteLine("1. Registrar Socio\n2. Listado de Socios\n0. Volver");
        string op = Console.ReadLine();
        if (op == "1")
        {
            int id = usuarioService.TotalUsuarios() + 1;
            Console.Write("Nombre: "); string nom = Console.ReadLine();
            Console.Write("Email: "); string em = Console.ReadLine();
            usuarioService.Agregar(new Usuario(id, nom, em));
            Ok($"Socio registrado con ID: {id}");
        }
        else if (op == "2")
        {
            usuarioService.ObtenerTodos().ForEach(u => Console.WriteLine(u.DetalleCompleto()));
            Ok("");
        }
    }

    static void MenuPrestamos()
    {
        Console.Clear();
        Console.WriteLine(">> PRÉSTAMOS Y DEVOLUCIONES");
        Console.WriteLine("1. Registrar Salida (Préstamo)");
        Console.WriteLine("2. Ver Historial de Préstamos");
        Console.WriteLine("0. Volver");
        
        string op = Console.ReadLine();
        if (op == "1")
        {
            Console.Write("ISBN del Libro: "); string isbn = Console.ReadLine();
            var lib = libroService.BuscarPorIsbn(isbn);
            
            Console.Write("ID del Usuario: "); 
            int.TryParse(Console.ReadLine(), out int uid);
            var usu = usuarioService.BuscarPorId(uid);

            if (lib != null && usu != null && lib.Disponible)
            {
                lib.Disponible = false; // Marcar como no disponible
                prestamosService.RegistrarPrestamo(new Prestamo(lib, usu, DateTime.Now));
                Ok("Préstamo autorizado.");
            }
            else Error("Libro no disponible o IDs incorrectos.");
        }
        else if (op == "2")
        {
            Console.WriteLine("\n--- PRÉSTAMOS ACTIVOS ---");
            var lista = prestamosService.ObtenerTodos();
            if (lista.Count == 0) Console.WriteLine("No hay registros de préstamos.");
            else lista.ForEach(p => Console.WriteLine(p.DetalleCompleto()));
            Ok("");
        }
    }

    static void MenuReportes()
    {
        Console.Clear();
        Console.WriteLine(">> BÚSQUEDAS Y REPORTES");
        Console.WriteLine("1. Ver Libros Disponibles");
        Console.WriteLine("2. Buscar Libro por Título");
        Console.WriteLine("3. KPI: Cantidad de Préstamos Totales");
        Console.WriteLine("0. Volver");

        switch (Console.ReadLine())
        {
            case "1":
                var disp = libroService.ObtenerTodos().Where(l => l.Disponible).ToList();
                disp.ForEach(l => Console.WriteLine($"- {l.Titulo} ({l.Isbn})"));
                Ok($"Total disponibles: {disp.Count}");
                break;
            case "2":
                Console.Write("Ingrese parte del título: ");
                string busqueda = Console.ReadLine().ToLower();
                var encontrados = libroService.ObtenerTodos().Where(l => l.Titulo.ToLower().Contains(busqueda)).ToList();
                encontrados.ForEach(l => Console.WriteLine(l.DetalleCompleto()));
                Ok($"{encontrados.Count} coincidencia(s).");
                break;
            case "3":
                Ok($"Se han realizado {prestamosService.ObtenerTodos().Count} préstamos en total.");
                break;
        }
    }

    static void Ok(string msg)
    {
        if (!string.IsNullOrEmpty(msg)) Console.WriteLine($"\n[OK]: {msg}");
        Console.WriteLine("Presione una tecla para continuar...");
        Console.ReadKey();
    }

    static void Error(string msg)
    {
        Console.WriteLine($"\n[!] Error: {msg}");
        Thread.Sleep(1200);
    }
}