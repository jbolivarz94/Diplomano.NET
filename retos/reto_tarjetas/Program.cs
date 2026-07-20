using System;
using System.Text;
using System.IO;

namespace reto_tarjetas
{
    static class Program
    {   
        // Lista para almacenar las estadísticas de validación de tarjetas
        static List<(string Numero, bool Valida, string Marca)> estadisticas = new List<(string, bool, string)>();

        static void Main(string[] args)
        {
            bool salir = false;
            do{                
                Console.WriteLine("Bienvenido al reto de tarjetas.");
                Console.WriteLine("=== VALIDADOR DE TARJETAS ===");
                Console.WriteLine("1. Validar una tarjeta");
                Console.WriteLine("2. Validar desde archivo");
                Console.WriteLine("3. Generar numero valido");
                Console.WriteLine("4. Estadisticas");
                Console.WriteLine("5. Salir");
                Console.WriteLine("==============================");

                string opcion = Console.ReadLine();

                try{
                    switch (opcion)
                    {
                        case "1":
                            Console.WriteLine("======================================");
                            Console.WriteLine("Ingrese el número de tarjeta a validar:");
                            string numeroTarjeta = Console.ReadLine();
                            bool esValida = ValidarTarjeta(numeroTarjeta);
                            string estado = esValida ? "VÁLIDA" : "INVÁLIDA";
                            string marca = IdentificarMarca(numeroTarjeta);
                            Console.WriteLine($"Número: {numeroTarjeta}");
                            Console.WriteLine($"Válida: {estado}");
                            Console.WriteLine($"Marca: {marca}");
                            RegistrarEstadistica(numeroTarjeta, esValida, marca);
                            Console.WriteLine("======================================");
                            break;
                        case "2":
                            Console.WriteLine("======================================");
                            Console.WriteLine("Ingrese la ruta del archivo con los números de tarjeta:");
                            string rutaArchivo = Console.ReadLine();
                            Console.WriteLine("--------------------------------------");
                            ValidarDesdeArchivo(rutaArchivo);
                            Console.WriteLine("======================================");
                            break;
                        case "3":
                            Console.WriteLine("======================================");
                            string numeroGenerado = GenerarNumeroValido();
                            string esta = ValidarTarjeta(numeroGenerado) ? "VÁLIDA" : "INVÁLIDA";
                            string marcc = IdentificarMarca(numeroGenerado);
                            Console.WriteLine($"Número de tarjeta válido generado: {numeroGenerado}");                            
                            Console.WriteLine($"Número: {numeroGenerado}");
                            Console.WriteLine($"Válida: {esta}");
                            Console.WriteLine($"Marca: {marcc}");
                            RegistrarEstadistica(numeroGenerado, true, marcc);
                            Console.WriteLine("======================================");
                            break;
                        case "4":
                            Console.WriteLine("======================================");
                            MostrarEstadisticas();
                            Console.WriteLine("======================================");
                            break;
                        case "5":
                            salir = true;
                            break;
                        default:
                            Console.WriteLine("Opción no válida. Intente nuevamente.");
                            Console.WriteLine("======================================");
                            break;
                    }
                }catch(Exception ex){
                    Console.WriteLine($"Ocurrió un error: {ex.Message}");
                }                
            }while(!salir);
        }

        static long invertirNumero(long numero)
        {
            long numeroInvertido = 0;
            while (numero > 0)
            {
                long digito = numero % 10;
                numeroInvertido = (numeroInvertido * 10) + digito;
                numero /= 10;
            }
            return numeroInvertido;
        }

        // Método para validar el número de tarjeta utilizando el algoritmo de Luhn
        static bool ValidarTarjeta(string numeroTarjeta)
        {
            long numeroTarjetaLong = long.Parse(numeroTarjeta);
            long numeroInvertido = invertirNumero(numeroTarjetaLong);
            int suma = 0;
            for (int i = 0; i < numeroTarjeta.Length; i++)
            {
                int digito = (int)(numeroInvertido % 10);
                if (i % 2 == 1)
                {
                    digito *= 2;
                    if (digito > 9)
                    {
                        digito -= 9;
                    }
                }
                suma += digito;
                numeroInvertido /= 10;
            }
            
            return suma % 10 == 0; // Retorna true si la tarjeta es válida, false en caso contrario.
        }

        // Método para identificar la marca de la tarjeta según el número
        static string IdentificarMarca(string numeroTarjeta)
        {
            // Expresiones regulares para identificar las marcas de tarjetas
            string visaPattern = @"^4[0-9]{12}(?:[0-9]{3})?$";
            string masterCardPattern = @"^5[1-5][0-9]{14}$";
            string amexPattern = @"^3[47][0-9]{13}$";
            string discoverPattern = @"^6(?:011|5[0-9]{2})[0-9]{12}$";
            
            switch (numeroTarjeta)
            {
                case string s when System.Text.RegularExpressions.Regex.IsMatch(s, visaPattern):
                    return "Visa";
                case string s when System.Text.RegularExpressions.Regex.IsMatch(s, masterCardPattern):
                    return "MasterCard";
                case string s when System.Text.RegularExpressions.Regex.IsMatch(s, amexPattern):
                    return "American Express";
                case string s when System.Text.RegularExpressions.Regex.IsMatch(s, discoverPattern):
                    return "Discover";
                default:
                    return "Desconocida";
            }

        }

        // Método para validar tarjetas desde un archivo
        static void ValidarDesdeArchivo(string rutaArchivo)
        {
            if(!File.Exists(rutaArchivo))
            {
                Console.WriteLine("El archivo no existe.");
                return;
            }

            // Leer todas las líneas del archivo y validar cada número de tarjeta
            string[] numerosTarjeta = File.ReadAllLines(rutaArchivo);
            foreach (string numero in numerosTarjeta)
            {
                bool esValida = ValidarTarjeta(numero);
                string estado = esValida ? "VÁLIDA" : "INVÁLIDA";
                string marca = IdentificarMarca(numero);
                Console.WriteLine($"Número: {numero}");
                Console.WriteLine($"Válida: {estado}");
                Console.WriteLine($"Marca: {marca}");
                RegistrarEstadistica(numero, esValida, marca);
                Console.WriteLine("------------------------------");
            }
        }

        // Método para generar un número de tarjeta válido
        static string GenerarNumeroValido()
        {   
            // Definir las marcas de tarjetas y sus prefijos
            string[] marcas = { "Visa", "MasterCard", "American Express", "Discover" };
            // Seleccionar una marca aleatoria
            string marca = marcas[new Random().Next(marcas.Length)];

            string pref;
            int longitud;
            
            switch (marca)
            {
                case "Visa":
                    pref = "4";
                    longitud = 16;
                    break;
                case "MasterCard":
                    pref = (51 + new Random().Next(5)).ToString(); // Genera un prefijo entre 51 y 55
                    longitud = 16;
                    break;
                case "American Express":
                    pref = new Random().Next(2) == 0 ? "34" : "37"; // Genera un prefijo de 34 o 37
                    longitud = 15;
                    break;
                case "Discover":
                    pref = new Random().Next(6011, 6012).ToString(); // Genera un prefijo entre 6011 y 6012
                    longitud = 16;
                    break;
                default:
                    throw new Exception("Marca desconocida");
            }
            
            // Generar el número de tarjeta sin el dígito verificador
            StringBuilder numeroTarjeta = new StringBuilder(pref);
            while (numeroTarjeta.Length < longitud - 1)
            {
                numeroTarjeta.Append(new Random().Next(0, 10));
            }
            
            // Calcular el dígito verificador y agregarlo al final del número de tarjeta
            if(!ValidarTarjeta(numeroTarjeta.ToString()))
            {
                int ultimoDigito = CalcularDigitoVerificador(numeroTarjeta.ToString());
                numeroTarjeta.Append(ultimoDigito);
            }
            
            return numeroTarjeta.ToString(); // Retorna un número de tarjeta válido generado.
        }

        // Método para calcular el dígito verificador utilizando el algoritmo de Luhn
        static int CalcularDigitoVerificador(string numeroSinDigito)
        {
            int suma = 0;
            bool esPar = numeroSinDigito.Length % 2 == 0;
            for (int i = numeroSinDigito.Length - 1; i >= 0; i--)
            {
                int digito = int.Parse(numeroSinDigito[i].ToString());
                if (esPar)
                {
                    digito *= 2;
                    if (digito > 9)
                    {
                        digito -= 9;
                    }
                }
                suma += digito;
                esPar = !esPar;
            }
            return (10 - (suma % 10)) % 10; // Retorna el dígito verificador calculado.
        }

        // Método para registrar las estadísticas de validación
        static void RegistrarEstadistica(string numero, bool valida, string marca)
        {
            estadisticas.Add((numero, valida, marca));
        }

        // Método para mostrar las estadísticas de validación
        static void MostrarEstadisticas()
        {
            Console.WriteLine("=== ESTADÍSTICAS DE VALIDACIÓN ===");
            foreach (var estadistica in estadisticas)
            {
                string estado = estadistica.Valida ? "VÁLIDA" : "INVÁLIDA";
                Console.WriteLine($"Número: {estadistica.Numero}, Válida: {estado}, Marca: {estadistica.Marca}");
            }
            Console.WriteLine("==================================");
        }
    }
}