using SIC.Models;
using SIC.Services;
using System.Globalization;


namespace SIC.UI
{
    // Capa de presentación: interactúa con el usuario, valida entradas y formatea la salida.
    public sealed class ConsoleUI
    {
        // Límite superior de años aceptado, para proteger la pila de llamadas de la recursión.
        private const int TiempoMaximo = 1000;

        private readonly InversionService _servicio;

        // Inicializa una nueva instancia de ConsoleUI.
        public ConsoleUI(InversionService servicio)
        {
            _servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));
        }

        // Inicia la interacción con el usuario. Repite el flujo hasta que el usuario decida salir.
        public void Iniciar()
        {
            MostrarEncabezado();
            ProcesarCalculo();

            if (PreguntarSiContinuar())
            {
                Iniciar();
            }
            else
            {
                Console.WriteLine("\nGracias por usar SIC. ¡Hasta pronto!");
            }
        }

        // Captura los datos, ejecuta el cálculo y muestra el resultado sin propagar excepciones.
        private void ProcesarCalculo()
        {
            decimal capital = LeerDecimal("Capital inicial (m): ", valor => valor >= 0m, "El capital no puede ser negativo.");
            decimal tasa = LeerDecimal("Tasa de interés anual en % (X): ", valor => valor > -100m, "La tasa debe ser mayor que -100 %.");
            int tiempo = LeerEntero($"Tiempo en años (n, 0 a {TiempoMaximo}): ", valor => valor is >= 0 and <= TiempoMaximo,
                $"El tiempo debe ser un entero entre 0 y {TiempoMaximo}.");

            try
            {
                var inversion = new Inversion(capital, tasa, tiempo);
                decimal resultado = _servicio.CalcularCapitalFinal(inversion);
                MostrarResultado(inversion, resultado);
            }
            catch (OverflowException)
            {
                MostrarError("El resultado excede el valor máximo representable. Reduzca el capital, la tasa o el tiempo.");
            }
            catch (ArgumentException ex)
            {
                MostrarError(ex.Message);
            }
        }

        // Lee un valor decimal desde consola, reintentando recursivamente hasta obtener una entrada válida.
        private static decimal LeerDecimal(string mensaje, Func<decimal, bool> esValido, string mensajeError)
        {
            Console.Write(mensaje);
            string? entrada = Console.ReadLine();

            if (!TryParseDecimal(entrada, out decimal valor))
            {
                MostrarError("Entrada no válida. Ingrese un número decimal (ej. 1500.50).");
                return LeerDecimal(mensaje, esValido, mensajeError);
            }

            if (!esValido(valor))
            {
                MostrarError(mensajeError);
                return LeerDecimal(mensaje, esValido, mensajeError);
            }

            return valor;
        }

        // Lee un valor entero desde consola, reintentando recursivamente hasta obtener una entrada válida.
        private static int LeerEntero(string mensaje, Func<int, bool> esValido, string mensajeError)
        {
            Console.Write(mensaje);
            string? entrada = Console.ReadLine();

            if (!int.TryParse(entrada?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int valor))
            {
                MostrarError("Entrada no válida. Ingrese un número entero (ej. 0, 1, 10).");
                return LeerEntero(mensaje, esValido, mensajeError);
            }

            if (!esValido(valor))
            {
                MostrarError(mensajeError);
                return LeerEntero(mensaje, esValido, mensajeError);
            }

            return valor;
        }

        // Intenta convertir texto a decimal aceptando tanto punto como coma como separador decimal.
        private static bool TryParseDecimal(string? entrada, out decimal valor)
        {
            valor = 0m;

            if (string.IsNullOrWhiteSpace(entrada))
            {
                return false;
            }

            string normalizada = entrada.Trim().Replace(',', '.');
            return decimal.TryParse(normalizada, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out valor);
        }

        // Pregunta al usuario si desea realizar otro cálculo.
        private static bool PreguntarSiContinuar()
        {
            Console.Write("\n¿Desea realizar otro cálculo? (S/N): ");
            string? respuesta = Console.ReadLine()?.Trim().ToUpperInvariant();

            return respuesta switch
            {
                "S" or "SI" or "SÍ" => true,
                "N" or "NO" => false,
                _ => PreguntarSiContinuar()
            };
        }

        // Muestra el encabezado de la aplicación.
        private static void MostrarEncabezado()
        {
            Console.WriteLine();
            Console.WriteLine("==========================================");
            Console.WriteLine("  SIC - Módulo de Capitalización Recursiva");
            Console.WriteLine("==========================================");
        }

        // Muestra el resultado del cálculo con formato monetario.
        private static void MostrarResultado(Inversion inversion, decimal resultado)
        {
            Console.WriteLine();
            Console.WriteLine("------------- Resultado -------------");
            Console.WriteLine($"Capital inicial : {FormatearMoneda(inversion.Capital)}");
            Console.WriteLine($"Tasa anual      : {inversion.TasaPorcentual.ToString("0.####", CultureInfo.InvariantCulture)} %");
            Console.WriteLine($"Tiempo          : {inversion.Tiempo} año(s)");
            Console.WriteLine($"Capital final   : {FormatearMoneda(resultado)}");
            Console.WriteLine("-------------------------------------");
        }

        // Formatea un valor decimal como moneda con dos decimales.
        private static string FormatearMoneda(decimal valor)
            => valor.ToString("C2", CultureInfo.CurrentCulture);

        // Muestra un mensaje de error en color rojo.
        private static void MostrarError(string mensaje)
        {
            ConsoleColor colorOriginal = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Error] {mensaje}");
            Console.ForegroundColor = colorOriginal;
        }
    }
}
