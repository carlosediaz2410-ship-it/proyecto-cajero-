using System.Collections.Generic;
using System.Linq;
using SimuladorCajero.Models;
using SimuladorCajero.Utils;

namespace SimuladorCajero.Repositories
{
    public class TransaccionRepository
    {
        private readonly string _path;
        private List<Transaccion> _transacciones;

        public TransaccionRepository(string path)
        {
            _path = path;
            _transacciones = JsonHelper.LoadList<Transaccion>(_path);
        }

        public List<Transaccion> GetAll() => _transacciones;

        public List<Transaccion> GetByCuenta(string numeroCuenta)
        {
            return _transacciones
                .Where(t => t.CuentaOrigen == numeroCuenta || t.CuentaDestino == numeroCuenta)
                .ToList();
        }

        public void Add(Transaccion transaccion)
        {
            _transacciones.Add(transaccion);
            JsonHelper.SaveList(_path, _transacciones);
        }
    }
}
