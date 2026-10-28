using SIC.Services;
using SIC.UI;

namespace SIC
{
    class Program
    {
        static void Main(string[] args)
        {
            new ConsoleUI(new InversionService()).Iniciar();
        }
    }
}