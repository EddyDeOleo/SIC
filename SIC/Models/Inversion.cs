
namespace SIC.Models
{

    // Encapsula los datos de entrada de una inversión sujeta a capitalización compuesta.
    public sealed class Inversion
    {
        // Inicializa una nueva instancia de Inversion.
        // Capital inicial (m).
        // Tasa de interés anual expresada en porcentaje (X). Ej.: 5.5 equivale a 5.5 %.
        // Tiempo de la inversión en años (n). Debe ser un entero mayor o igual a cero.
        public Inversion(decimal capital, decimal tasaPorcentual, int tiempo)
        {
            Capital = capital;
            TasaPorcentual = tasaPorcentual;
            Tiempo = tiempo;
        }

        // Obtiene el capital inicial (m).
        public decimal Capital { get; }

        // Obtiene la tasa de interés en porcentaje (X).
        public decimal TasaPorcentual { get; }

        // Obtiene el tiempo de la inversión en años (n).
        public int Tiempo { get; }
    }
}
