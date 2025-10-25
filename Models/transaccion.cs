using System;

namespace SimuladorCajero.Models
{
    public class Transaccion
    {
        public string Id { get; set; }               // ID único de la transacción
        public string CuentaOrigen { get; set; }     // Número de cuenta de origen (null si es depósito)
        public string CuentaDestino { get; set; }    // Número de cuenta destino (null si es retiro)
        public decimal Monto { get; set; }           // Cantidad de dinero
        public string Tipo { get; set; }             // "Depósito", "Retiro", "Transferencia"
        public DateTime Fecha { get; set; }          // Fecha y hora de la transacción

        public override string ToString()
        {
            return $"[{Fecha}] Tipo: {Tipo} | Monto: {Monto:C} | De: {CuentaOrigen ?? "-"} | A: {CuentaDestino ?? "-"}";
        }
    }
}
