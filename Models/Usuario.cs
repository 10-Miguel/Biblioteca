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
