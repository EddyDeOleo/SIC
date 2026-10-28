using SIC.Algorithms;
using SIC.Models;


namespace SIC.Services
{
    /*
     Servicio que orquesta el cálculo de una inversión: adapta los datos del modelo
     y delega el cómputo matemático al algoritmo recursivo.
    */
    public sealed class InversionService
    {
        // Divisor utilizado para convertir una tasa porcentual a su forma decimal.
        private const decimal DivisorPorcentaje = 100m;

        // Calcula el capital final de una inversión.
        public decimal CalcularCapitalFinal(Inversion inversion)
        {
            ArgumentNullException.ThrowIfNull(inversion);

            // i = X / 100
            decimal tasaDecimal = ConvertirTasaADecimal(inversion.TasaPorcentual);

            return CalculadoraRecursiva.CalcularCapital(inversion.Capital, tasaDecimal, inversion.Tiempo);
        }

        // Convierte una tasa porcentual (X) a decimal (i = X / 100).
        private static decimal ConvertirTasaADecimal(decimal tasaPorcentual)
            => tasaPorcentual / DivisorPorcentaje;
    }
}
