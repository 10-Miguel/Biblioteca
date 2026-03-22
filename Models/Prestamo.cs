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
    