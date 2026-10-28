
namespace SIC.Algorithms
{
    // Motor matemático recursivo puro para el cálculo de capitalización compuesta.
    // No contiene estructuras iterativas
    public static class CalculadoraRecursiva
    {
        
        public static decimal CalcularCapital(decimal m, decimal i, int n)
        {
            // Guarda de seguridad: evita recursión infinita ante n negativo.
            if (n < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), n, "El tiempo debe ser mayor o igual a cero.");
            }

            // Caso base: sin años transcurridos no se aplican intereses.
            if (n == 0)
            {
                return m;
            }

            // Caso recursivo: capital del año anterior multiplicado por el factor de crecimiento.
            return CalcularCapital(m, i, n - 1) * (1 + i);
        }
    }
}
