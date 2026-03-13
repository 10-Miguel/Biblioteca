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

// --- SECCIÓN 1: LIBROS ---
    static void MenuLibros()
    {
        Console.Clear();
        Console.WriteLine(">> GESTIÓN DE LIBROS");
        Console.WriteLine("1.1 Registrar libro\n1.2 Listar libros\n1.3 Ver detalle (ID/ISBN)\n1.4 Actualizar libro\n1.5 Eliminar libro\n0. Volver");
        
        string op = Console.ReadLine() ?? "";
        if (op == "1.2") 
            EjecutarAccion("Submenú: Listar (Todos / Disponibles / Prestados)");
        else if (op == "1.4") 
            EjecutarAccion("Submenú: Editar (Título / Autor / Año / Categoría)");
        else if (op != "0")
            EjecutarAccion($"Ejecutando operación de libros: {op}");
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
