
 // --- SECCIÓN 4: BÚSQUEDAS Y REPORTES ---
    static void MenuReportes()
    {
        Console.Clear();
        Console.WriteLine(">> BÚSQUEDAS Y REPORTES");
        Console.WriteLine("4.1 Buscar libro (Título/Autor/ISBN/Cat)\n4.2 Buscar usuario\n4.3 Reportes generales\n0. Volver");
        
        string op = Console.ReadLine() ?? "";
        if (op != "0") EjecutarAccion("Generando reporte/búsqueda solicitada...");
    }
