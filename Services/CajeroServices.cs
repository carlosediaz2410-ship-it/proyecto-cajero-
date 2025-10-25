using System;
using SimuladorCajero.Models;
using SimuladorCajero.Repositories;

namespace SimuladorCajero.Services
{
    public class CajeroService
    {
        private readonly CuentaRepository _cuentaRepo;
        private readonly TransaccionRepository _transRepo;

        public CajeroService(CuentaRepository cuentaRepo, TransaccionRepository transRepo)
        {
            _cuentaRepo = cuentaRepo;
            _transRepo = transRepo;
        }

        public void ConsultarSaldo(string usuario)
        {
            var cuenta = _cuentaRepo.GetByUsuario(usuario);
            if (cuenta == null) { Console.WriteLine("Cuenta no encontrada."); return; }
            Console.WriteLine($"Saldo actual: {cuenta.Saldo:C}");
        }

        public void Depositar(string usuario)
        {
            var cuenta = _cuentaRepo.GetByUsuario(usuario);
            if (cuenta == null) { Console.WriteLine("Cuenta no encontrada."); return; }

            Console.Write("Monto a depositar: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal monto) || monto <= 0)
            {
                Console.WriteLine("Monto inválido.");
                return;
            }

            cuenta.Saldo += monto;
            _cuentaRepo.Update(cuenta);

            _transRepo.Add(new Transaccion
            {
                CuentaOrigen = null,
                CuentaDestino = cuenta.NumeroCuenta,
                Monto = monto,
                Tipo = "Depósito",
                Fecha = DateTime.Now
            });

            Console.WriteLine("Depósito realizado con éxito.");
        }

        public void Retirar(string usuario)
        {
            var cuenta = _cuentaRepo.GetByUsuario(usuario);
            if (cuenta == null) { Console.WriteLine("Cuenta no encontrada."); return; }

            Console.Write("Monto a retirar: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal monto) || monto <= 0)
            {
                Console.WriteLine("Monto inválido.");
                return;
            }

            if (monto > cuenta.Saldo)
            {
                Console.WriteLine("Saldo insuficiente.");
                return;
            }

            cuenta.Saldo -= monto;
            _cuentaRepo.Update(cuenta);

            _transRepo.Add(new Transaccion
            {
                CuentaOrigen = cuenta.NumeroCuenta,
                CuentaDestino = null,
                Monto = monto,
                Tipo = "Retiro",
                Fecha = DateTime.Now
            });

            Console.WriteLine("Retiro realizado con éxito.");
        }

        public void Transferir(string usuario)
        {
            var origen = _cuentaRepo.GetByUsuario(usuario);
            if (origen == null) { Console.WriteLine("Cuenta origen no encontrada."); return; }

            Console.Write("Número de cuenta destino: ");
            var destinoNumero = Console.ReadLine();
            var destino = _cuentaRepo.GetById(destinoNumero);

            if (destino == null)
            {
                Console.WriteLine("Cuenta destino no encontrada.");
                return;
            }

            Console.Write("Monto a transferir: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal monto) || monto <= 0)
            {
                Console.WriteLine("Monto inválido.");
                return;
            }

            if (monto > origen.Saldo)
            {
                Console.WriteLine("Saldo insuficiente.");
                return;
            }

            origen.Saldo -= monto;
            destino.Saldo += monto;

            _cuentaRepo.Update(origen);
            _cuentaRepo.Update(destino);

            _transRepo.Add(new Transaccion
            {
                CuentaOrigen = origen.NumeroCuenta,
                CuentaDestino = destino.NumeroCuenta,
                Monto = monto,
                Tipo = "Transferencia",
                Fecha = DateTime.Now
            });

            Console.WriteLine("Transferencia realizada con éxito.");
        }

        public void MostrarHistorial(string usuario)
        {
            var cuenta = _cuentaRepo.GetByUsuario(usuario);
            if (cuenta == null) { Console.WriteLine("Cuenta no encontrada."); return; }

            var transacciones = _transRepo.GetByCuenta(cuenta.NumeroCuenta);

            if (transacciones.Count == 0)
            {
                Console.WriteLine("No hay transacciones registradas.");
                return;
            }

            Console.WriteLine($"\nHistorial de cuenta {cuenta.NumeroCuenta}:");
            foreach (var t in transacciones)
            {
                Console.WriteLine(t.ToString());
            }
        }
    }
}
