using System;
using Biblioteca.Models;
namespace Biblioteca;
class Program

{
    static void Main()
    {
        MenuPrincipal();
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
                    Console.Write("¿Guardar antes de salir? (S/N): ");
                    if (Console.ReadLine()?.ToUpper() == "S") EjecutarAccion("Guardando cambios y cerrando sesión...");
                    salir = true; 
                    break;
                default: MensajeError(); break;
            }
        }
    }

 // --- SECCIÓN 5: PERSISTENCIA ---
    static void MenuDatos()
    {
        Console.Clear();
        Console.WriteLine(">> DATOS");
        Console.WriteLine("5.1 Guardar\n5.2 Cargar\n5.3 Reiniciar sistema\n0. Volver");
        
        string op = Console.ReadLine() ?? "";
        if (op == "5.3") {
            Console.Write("¿Confirmar reinicio total? (S/N): ");
            if (Console.ReadLine()?.ToUpper() == "S") EjecutarAccion("¡Sistema reseteado!");
        } else if (op != "0") EjecutarAccion("Sincronizando con persistencia de datos...");
    }

 // --- HELPERS ---
    static void EjecutarAccion(string mensaje)
    {
        Console.WriteLine($"\n[PROCESO]: {mensaje}");
        Console.WriteLine("Presione una tecla para continuar...");
        Console.ReadKey();
    }

    static void MensajeError()
    {
        Console.WriteLine("\n[!] Opción inválida.");
        Thread.Sleep(800);
    }

}
