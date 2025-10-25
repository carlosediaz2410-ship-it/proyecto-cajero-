using System;
using System.Collections.Generic;
using System.Linq;
using SimuladorCajero.Models;
using SimuladorCajero.Utils;

namespace SimuladorCajero.Repositories
{
    public class CuentaRepository
    {
        private readonly string _path;
        private List<Cuenta> _cuentas;

        public CuentaRepository(string path)
        {
            _path = path;
            _cuentas = JsonHelper.LoadList<Cuenta>(_path);
        }

        public List<Cuenta> GetAll() => _cuentas;

        public Cuenta GetById(string id) =>
            _cuentas.FirstOrDefault(c => c.NumeroCuenta == id);

        public Cuenta GetByUsuario(string usuario)
        {
            return _cuentas.FirstOrDefault(c => c.Propietario == usuario);
        }

        public void Add(Cuenta cuenta)
        {
            _cuentas.Add(cuenta);
            JsonHelper.SaveList(_path, _cuentas);
        }

        public void Update(Cuenta cuenta)
        {
            var idx = _cuentas.FindIndex(c => c.NumeroCuenta == cuenta.NumeroCuenta);
            if (idx >= 0)
            {
                _cuentas[idx] = cuenta;
                JsonHelper.SaveList(_path, _cuentas);
            }
        }
    }
}
