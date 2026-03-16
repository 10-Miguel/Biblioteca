using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
namespace Biblioteca {

    // --- MODELOS PARA RECIBIR Y ENVIAR DATOS ---  
    public class Libro {
        public int Id { get; set; }
        public string Titulo { get; set; } = "";
        public string Autor { get; set; } = "";
        public string ISBN { get; set; } = "";
        public bool Prestado { get; set; } = false;
}

public class Usuario {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Correo { get; set; } = "";
        public bool Activo { get; set; } = true;
    }

    public class Prestamo {
        public int LibroId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
    }
class Program
{ 
    // Bases de datos en memoria
        static List<Libro> libros = new List<Libro>();
        static List<Usuario> usuarios = new List<Usuario>();
        static List<Prestamo> prestamos = new List<Prestamo>();
        
        const string ArchivoDatos = "biblioteca_datos.json";

        
    static void Main()
    {
        CargarDatos();
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
      switch (op)
            {
                case "1":
                    Libro nuevo = new Libro { Id = libros.Count + 1 };
                    Console.Write("Título: "); nuevo.Titulo = Console.ReadLine() ?? "";
                    Console.Write("Autor: "); nuevo.Autor = Console.ReadLine() ?? "";
                    Console.Write("ISBN: "); nuevo.ISBN = Console.ReadLine() ?? "";
                    libros.Add(nuevo);
                    EjecutarAccion("Libro registrado.");
                    break;
                case "2":
                    Console.WriteLine("\nID | Título | Autor | Estado");
                    libros.ForEach(l => Console.WriteLine($"{l.Id} | {l.Titulo} | {l.Autor} | {(l.Prestado ? "Prestado" : "Disponible")}"));
                    EjecutarAccion("Fin de lista.");
                    break;
                case "3":
                    Console.Write("ID del libro a eliminar: ");
                    if(int.TryParse(Console.ReadLine(), out int id)) {
                        libros.RemoveAll(l => l.Id == id);
                        EjecutarAccion("Libro eliminado si existía.");
                    }
                    break;
            }
        }
 
  // --- SECCIÓN 2: USUARIOS ---
        static void MenuUsuarios()
        {
            Console.Clear();
            Console.WriteLine(">> GESTIÓN DE USUARIOS");
            Console.WriteLine("1. Registrar usuario\n2. Listar usuarios\n0. Volver");
            string op = Console.ReadLine() ?? "";

            if (op == "1") {
                Usuario u = new Usuario { Id = usuarios.Count + 1 };
                Console.Write("Nombre: "); u.Nombre = Console.ReadLine() ?? "";
                Console.Write("Correo: "); u.Correo = Console.ReadLine() ?? "";
                usuarios.Add(u);
                EjecutarAccion("Usuario registrado.");
            } else if (op == "2") {
                usuarios.ForEach(u => Console.WriteLine($"{u.Id} | {u.Nombre} | {u.Correo}"));
                EjecutarAccion("Fin de lista.");
            }
        }

// --- SECCIÓN 3: PRÉSTAMOS ---
        static void MenuPrestamos()
        {
            Console.Clear();
            Console.WriteLine(">> GESTIÓN DE PRÉSTAMOS");
            Console.WriteLine("1. Crear préstamo\n2. Registrar devolución\n0. Volver");
            string op = Console.ReadLine() ?? "";

            if (op == "1") {
                Console.Write("ID Libro: "); int idL = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("ID Usuario: "); int idU = int.Parse(Console.ReadLine() ?? "0");

                var libro = libros.FirstOrDefault(l => l.Id == idL && !l.Prestado);
                var usuario = usuarios.FirstOrDefault(u => u.Id == idU);

                if (libro != null && usuario != null) {
                    libro.Prestado = true;
                    prestamos.Add(new Prestamo { LibroId = idL, UsuarioId = idU, Fecha = DateTime.Now });
                    EjecutarAccion("Préstamo exitoso.");
                } else {
                    EjecutarAccion("Error: Libro no disponible o usuario inexistente.");
                }
            } else if (op == "2") {
                Console.Write("ID Libro a devolver: "); int idL = int.Parse(Console.ReadLine() ?? "0");
                var libro = libros.FirstOrDefault(l => l.Id == idL && l.Prestado);
                if (libro != null) {
                    libro.Prestado = false;
                    prestamos.RemoveAll(p => p.LibroId == idL);
                    EjecutarAccion("Devolución procesada.");
                }
            }
        }
        
 // --- SECCIÓN 4: REPORTES ---
        static void MenuReportes()
        {
            Console.Clear();
            Console.WriteLine(">> BÚSQUEDA");
            Console.Write("Ingrese título o autor a buscar: ");
            string b = Console.ReadLine()?.ToLower() ?? "";
            var resultados = libros.Where(l => l.Titulo.ToLower().Contains(b) || l.Autor.ToLower().Contains(b)).ToList();
            
            resultados.ForEach(l => Console.WriteLine($"{l.Titulo} - {l.Autor} ({(l.Prestado ? "Ocupado":"Libre")})"));
            EjecutarAccion($"{resultados.Count} encontrados.");
        }
 // --- SECCIÓN 5: PERSISTENCIA ---
        static void MenuDatos()
        {
            Console.Clear();
            Console.WriteLine("1. Guardar\n2. Cargar\n0. Volver");
            string op = Console.ReadLine() ?? "";
            if (op == "1") GuardarDatos();
            else if (op == "2") CargarDatos();
        }

        static void GuardarDatos()
        {
            var data = new { Libros = libros, Usuarios = usuarios, Prestamos = prestamos };
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ArchivoDatos, json);
            EjecutarAccion("Datos guardados en " + ArchivoDatos);
        }

        static void CargarDatos()
        {
            if (File.Exists(ArchivoDatos)) {
                string json = File.ReadAllText(ArchivoDatos);
                var data = JsonSerializer.Deserialize<DataWrapper>(json);
                if (data != null) {
                    libros = data.Libros;
                    usuarios = data.Usuarios;
                    prestamos = data.Prestamos;
                }
                EjecutarAccion("Datos cargados correctamente.");
            }
        }
 // --- HELPERS ---
  class DataWrapper {
            public List<Libro> Libros { get; set; } = new List<Libro>();
            public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
            public List<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
        }

        static void EjecutarAccion(string mensaje)
        {
            Console.WriteLine($"\n[INFO]: {mensaje}");
            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey();
        }

        static void MensajeError()
        {
            Console.WriteLine("\n[!] Opción inválida.");
            Thread.Sleep(800);
        }
    }
}