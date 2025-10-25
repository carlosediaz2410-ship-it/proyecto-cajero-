using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using SimuladorCajero.Models;
using SimuladorCajero.Repositories;
using SimuladorCajero.Services;
using SimuladorCajero.Security;
using Newtonsoft.Json;

namespace SimuladorCajero
{
    class Program
    {
        static void Main(string[] args)
        {
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);

            string usuariosPath = Path.Combine(basePath, "usuarios.json");
            string cuentasPath = Path.Combine(basePath, "cuentas.json");
            string transPath = Path.Combine(basePath, "transacciones.json");
            string adminPath = Path.Combine(basePath, "admin.json");

            CrearArchivoSiNoExiste(usuariosPath);
            CrearArchivoSiNoExiste(cuentasPath);
            CrearArchivoSiNoExiste(transPath);

            var usuarioRepo = new UsuarioRepository(usuariosPath);
            var cuentaRepo = new CuentaRepository(cuentasPath);
            var transRepo = new TransaccionRepository(transPath);

            var authService = new AutenticacionService(usuarioRepo, adminPath);
            var cajeroService = new CajeroService(cuentaRepo, transRepo);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SIMULADOR DE CAJERO AUTOMÁTICO ===\n");
                Console.WriteLine("1. Iniciar sesión");
                Console.WriteLine("2. Crear usuario (nuevo cliente)");
                Console.WriteLine("3. Salir");
                Console.Write("\nSeleccione una opción: ");
                var opt = Console.ReadLine();

                if (opt == "1")
                {
                    Console.Write("\nID de usuario: ");
                    var id = Console.ReadLine()?.Trim();
                    Console.Write("Contraseña: ");
                    var pass = ReadPassword();

                    var usuario = authService.Authenticate(id, pass);
                    if (usuario == null)
                    {
                        Console.WriteLine("\n⚠️ Credenciales inválidas. Presiona Enter...");
                        Console.ReadLine();
                        continue;
                    }

                    if (usuario.Rol.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                        MenuAdmin(usuarioRepo, cuentaRepo, transRepo, usuario);
                    else
                        MenuUsuario(cajeroService, usuario, usuarioRepo, cuentaRepo);
                }
                else if (opt == "2") CrearUsuarioCliente(usuarioRepo, cuentaRepo);
                else if (opt == "3") break;
                else { Console.WriteLine("Opción inválida."); Console.ReadLine(); }
            }
        }

        static void CrearArchivoSiNoExiste(string ruta)
        {
            if (!File.Exists(ruta)) File.WriteAllText(ruta, "[]");
        }

        static string ReadPassword()
        {
            var pass = "";
            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    pass = pass[..^1]; Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar)) { pass += key.KeyChar; Console.Write("*"); }
            }
            Console.WriteLine(); return pass;
        }

        static void Pause() { Console.WriteLine("\nPresiona Enter..."); Console.ReadLine(); }

        static string GenerarNumeroCuentaUnico(CuentaRepository cuentaRepo)
        {
            var rand = new Random(); string num;
            do { num = rand.Next(10000, 99999).ToString(); } while (cuentaRepo.GetById(num) != null);
            return num;
        }
    }
}
