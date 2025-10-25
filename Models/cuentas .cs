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

        // Devuelve todas las cuentas
        public List<Cuenta> GetAll() => _cuentas;

        // Busca una cuenta por su Id
        public Cuenta GetById(string id) =>
            _cuentas.FirstOrDefault(c => c.Id == id);

        // Busca la cuenta según el usuario
        public Cuenta GetByUsuarioId(string usuarioId) =>
            _cuentas.FirstOrDefault(c => c.UsuarioId == usuarioId);

        // Crea una cuenta nueva con saldo 0
        public Cuenta CreateDefaultCuenta(string usuarioId)
        {
            var cuenta = new Cuenta
            {
                Id = Guid.NewGuid().ToString(),
                UsuarioId = usuarioId,
                Saldo = 0m
            };

            _cuentas.Add(cuenta);
            JsonHelper.SaveList(_path, _cuentas);
            return cuenta;
        }

        // Actualiza una cuenta existente
        public void Update(Cuenta cuenta)
        {
            var index = _cuentas.FindIndex(c => c.Id == cuenta.Id);
            if (index >= 0)
            {
                _cuentas[index] = cuenta;
                JsonHelper.SaveList(_path, _cuentas);
            }
        }
    }
}
