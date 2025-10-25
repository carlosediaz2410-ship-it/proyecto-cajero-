using SimuladorCajero.Models;
using SimuladorCajero.Repositories;
using SimuladorCajero.Security;

namespace SimuladorCajero.Services
{
    public class AutenticacionService
    {
        private readonly UsuarioRepository _usuarioRepo;

        public AutenticacionService(UsuarioRepository repo)
        {
            _usuarioRepo = repo;
        }

        // Autentica un usuario usando hash y salt
        public Usuario Authenticate(string id, string contrasena)
        {
            var usuario = _usuarioRepo.GetById(id);
            if (usuario == null) return null;

            if (PasswordHelper.VerifyPassword(contrasena, usuario.Contrasena, usuario.Salt))
                return usuario;

            return null;
        }
    }
}
