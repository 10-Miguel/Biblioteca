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
