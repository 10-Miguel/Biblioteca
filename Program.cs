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



    static void Error()
    {
        Console.WriteLine("\n[!] Opción inválida.");
        Thread.Sleep(800);
    }

}
