using System.Collections.Generic;
using System.Linq;
using SimuladorCajero.Models;
using SimuladorCajero.Utils;

namespace SimuladorCajero.Repositories
{
    public class UsuarioRepository
    {
        private readonly string _path;
        private List<Usuario> _usuarios;

        public UsuarioRepository(string path)
        {
            _path = path;
            _usuarios = JsonHelper.LoadList<Usuario>(_path);
        }

        public List<Usuario> GetAll() => _usuarios;

        public Usuario GetById(string id) =>
            _usuarios.FirstOrDefault(u => u.Id == id);

        public void Add(Usuario usuario)
        {
            _usuarios.Add(usuario);
            JsonHelper.SaveList(_path, _usuarios);
        }

        public void Update(Usuario usuario)
        {
            var idx = _usuarios.FindIndex(u => u.Id == usuario.Id);
            if (idx >= 0)
            {
                _usuarios[idx] = usuario;
                JsonHelper.SaveList(_path, _usuarios);
            }
        }
    }
}
