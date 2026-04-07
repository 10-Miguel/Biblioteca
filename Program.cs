using System;
using Biblioteca.Models;
using System.Threading;
using System.Collections.Generic;
using Biblioteca.Services;
using System.IO;         // Requerido para manejar archivos
using System.Text.Json; // Requerido para JSON

namespace Biblioteca;

class Program
{
    static LibroService libroService = new();
    static UsuarioService usuarioService = new();
    static PrestamoService prestamosService = new();

    // Rutas de archivos en la carpeta del proyecto
    static string pathLibros = "libros.json";
    static string pathUsuarios = "usuarios.json";
    static string pathPrestamos = "prestamos.json";

    static void Main()
    {
        // 1. Intentamos cargar datos guardados
        bool existenDatos = CargarDatosDesdeArchivos();

        // 2. SOLO si no hay archivos guardados, cargamos los de prueba
        if (!existenDatos)
        {
            CargarDatosPrueba();
        }

        MenuPrincipal();
    }

    // LÓGICA DE GUARDADO REAL
    static void GuardarDatosEnArchivos()
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            File.WriteAllText(pathLibros, JsonSerializer.Serialize(libroService.ObtenerTodos(), options));
            File.WriteAllText(pathUsuarios, JsonSerializer.Serialize(usuarioService.ObtenerTodos(), options));
            File.WriteAllText(pathPrestamos, JsonSerializer.Serialize(prestamosService.ObtenerTodos(), options));

            Ok("Datos guardados exitosamente en archivos JSON.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[!] Error crítico al guardar: {ex.Message}");
            Ok("");
        }
    }

    // LÓGICA DE CARGA REAL
    static bool CargarDatosDesdeArchivos()
    {
        try
        {
            if (!File.Exists(pathLibros)) return false;

            // Cargar Libros
            var jsonLibros = File.ReadAllText(pathLibros);
            var libros = JsonSerializer.Deserialize<List<Libro>>(jsonLibros);
            libros?.ForEach(l => libroService.Agregar(l));

            // Cargar Usuarios
            var jsonUsuarios = File.ReadAllText(pathUsuarios);
            var usuarios = JsonSerializer.Deserialize<List<Usuario>>(jsonUsuarios);
            usuarios?.ForEach(u => usuarioService.Agregar(u));

            // Cargar Préstamos
            var jsonPrestamos = File.ReadAllText(pathPrestamos);
            var prestamos = JsonSerializer.Deserialize<List<Prestamo>>(jsonPrestamos);
            prestamos?.ForEach(p => prestamosService.RegistrarPrestamo(p));

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    static void CargarDatosPrueba()
    {
        var libro1 = new Libro("978-0-06-112008-4", "Cien Anos de Soledad", "Gabriel Garcia Marquez");
        var libro2 = new Libro("978-84-376-0494-7", "El Quijote", "Miguel de Cervantes");
        var usuario1 = new Usuario(1, "Ana Torres", "ana@email.com");
        var usuario2 = new Usuario(2, "Luis Perez", "luis@email.com");

        libroService.Agregar(libro1);
        libroService.Agregar(libro2);
        usuarioService.Agregar(usuario1);
        usuarioService.Agregar(usuario2);
    }

    static void MenuPrincipal()
    {
        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("      SISTEMA DE GESTIÓN BIBLIOTECARIA   ");
            Console.WriteLine("1. Libros\n2. Usuarios\n3. Préstamos\n4. Búsquedas y Reportes\n5. Guardar Datos\n0. Salir");
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
                    salir = true;
                    break;
                default: Error(); break;
            }
        }
    }

    // --- SUBMENÚS (Lógica de tus servicios) ---

    static void MenuLibros()
    {
        Console.Clear();
        Console.WriteLine(">> LIBROS\n1. Agregar\n2. Listar\n0. Volver");
        string op = Console.ReadLine();
        if (op == "1")
        {
            Console.Write("ISBN: "); string isbn = Console.ReadLine();
            Console.Write("Titulo: "); string tit = Console.ReadLine();
            Console.Write("Autor: "); string aut = Console.ReadLine();
            libroService.Agregar(new Libro(isbn, tit, aut));
            Ok("Libro agregado.");
        }
        else if (op == "2")
        {
            libroService.ObtenerTodos().ForEach(l => Console.WriteLine(l.DetalleCompleto()));
            Ok($"Total: {libroService.TotalLibros()}");
        }
    }

    static void MenuUsuarios()
    {
        Console.Clear();
        Console.WriteLine(">> USUARIOS\n1. Agregar\n2. Listar\n0. Volver");
        string op = Console.ReadLine();
        if (op == "1")
        {
            int id = usuarioService.TotalUsuarios() + 1;
            Console.Write("Nombre: "); string nom = Console.ReadLine();
            Console.Write("Email: "); string em = Console.ReadLine();
            usuarioService.Agregar(new Usuario(id, nom, em));
            Ok("Usuario creado.");
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
        Console.WriteLine(">> PRESTAMOS\n1. Crear\n2. Listar\n0. Volver");
        if (Console.ReadLine() == "1")
        {
            Console.Write("ISBN Libro: "); string isbn = Console.ReadLine();
            var lib = libroService.BuscarPorIsbn(isbn);
            Console.Write("ID Usuario: "); 
            int.TryParse(Console.ReadLine(), out int uid);
            var usu = usuarioService.BuscarPorId(uid);

            if (lib != null && usu != null && lib.Disponible)
            {
                lib.Disponible = false;
                prestamosService.RegistrarPrestamo(new Prestamo(lib, usu, DateTime.Now));
                Ok("Préstamo registrado.");
            }
            else Error();
        }
    }

    static void MenuReportes() { Ok("Módulo de reportes activo."); }

    static void Ok(string msg)
    {
        if (!string.IsNullOrEmpty(msg)) Console.WriteLine($"\n[OK]: {msg}");
        Console.WriteLine("Presione una tecla...");
        Console.ReadKey();
    }

    static void Error()
    {
        Console.WriteLine("\n[!] Error en la operación.");
        Thread.Sleep(800);
    }
}