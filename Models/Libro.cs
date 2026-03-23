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