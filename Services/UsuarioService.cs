using Biblioteca.Models;
using System.Linq;

namespace Biblioteca.Services;

public class UsuarioService
{
    private List<Usuario> usuarios = new();

    public void Agregar(Usuario usuario) => usuarios.Add(usuario);
    
    public List<Usuario> ObtenerTodos() => usuarios;

    public Usuario? BuscarPorId(int id) => usuarios.FirstOrDefault(u => u.Id == id);
    
    // Ordenación por nombre
    public void OrdenarPorNombre() => usuarios = usuarios.OrderBy(u => u.Nombre).ToList();

    // KPIs
    public int TotalUsuarios() => usuarios.Count;
    public int UsuariosActivos() => usuarios.Count(u => u.Activo);
}