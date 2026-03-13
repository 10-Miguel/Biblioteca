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

  // --- SECCIÓN 2: USUARIOS ---
    static void MenuUsuarios()
    {
        Console.Clear();
        Console.WriteLine(">> GESTIÓN DE USUARIOS");
        Console.WriteLine("2.1 Registrar usuario\n2.2 Listar usuarios\n2.3 Ver detalle\n2.4 Actualizar\n2.5 Eliminar\n0. Volver");
        
        string op = Console.ReadLine() ?? "";
        if (op == "2.4")
            EjecutarAccion("Submenú: Editar (Nombre / Contacto / Estado Activo)");
        else if (op != "0")
            EjecutarAccion($"Ejecutando operación de usuarios: {op}");
    }

        // --- SECCIÓN 3: PRÉSTAMOS ---
    static void MenuPrestamos()
    {
        Console.Clear();
        Console.WriteLine(">> GESTIÓN DE PRÉSTAMOS");
        Console.WriteLine("3.1 Crear préstamo\n3.2 Listar préstamos\n3.3 Ver detalle\n3.4 Registrar devolución\n3.5 Eliminar préstamo\n0. Volver");
        
        string op = Console.ReadLine() ?? "";
        if (op == "3.1")
            EjecutarAccion("Validando disponibilidad de libro y estado de usuario...");
        else if (op == "3.4")
            EjecutarAccion("Procesando devolución y actualizando stock...");
        else if (op != "0")
            EjecutarAccion($"Operación de préstamo: {op}");
    }
    
 // --- SECCIÓN 4: BÚSQUEDAS Y REPORTES ---
    static void MenuReportes()
    {
        Console.Clear();
        Console.WriteLine(">> BÚSQUEDAS Y REPORTES");
        Console.WriteLine("4.1 Buscar libro (Título/Autor/ISBN/Cat)\n4.2 Buscar usuario\n4.3 Reportes generales\n0. Volver");
        
        string op = Console.ReadLine() ?? "";
        if (op != "0") EjecutarAccion("Generando reporte/búsqueda solicitada...");
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
