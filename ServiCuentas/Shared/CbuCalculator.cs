namespace ServiCuentas.Shared
{
    namespace Shared
    {
        public interface ICbuCalculator
        {
            string GenerarCbu(string cbu);
        }
        public class CbuCalculator : ICbuCalculator
        {
            private readonly Globales _globales;

            // Constructor con inyección de dependencias
            public CbuCalculator(Globales globales)
            {
                _globales = globales ?? throw new ArgumentNullException(nameof(globales));
            }

            /// <summary>
            /// Genera el CBU completo basado en los códigos de banco, sucursal y cuenta.
            /// </summary>
            /// <param name="numeroCuenta">El número de cuenta (máximo 13 dígitos).</param>
            /// <returns>El CBU generado.</returns>
            public string GenerarCbu(string numeroCuenta)
            {
                // Obtención de códigos de banco y sucursal desde la configuración
                string codigoBanco = _globales.CodigoBanco.PadLeft(3, '0');
                string codigoSucursal = _globales.CodigoSucursal.PadLeft(4, '0');

                // Verificación de longitud de cuenta
                string cuenta = numeroCuenta.PadLeft(13, '0');

                // Cálculo del primer bloque (código de banco, sucursal y dígito verificador)
                string primerBloque = $"{codigoBanco}{codigoSucursal}";
                int digitoVerificador1 = CalcularDigitoVerificador(primerBloque);

                // Cálculo del segundo bloque (número de cuenta y dígito verificador)
                int digitoVerificador2 = CalcularDigitoVerificador(cuenta);

                // Construcción del CBU
                return $"{primerBloque}{digitoVerificador1}{cuenta}{digitoVerificador2}";
            }

            /// <summary>
            /// Calcula el dígito verificador usando el algoritmo de módulo 11.
            /// </summary>
            /// <param name="numero">El número para calcular el dígito verificador.</param>
            /// <returns>El dígito verificador.</returns>
            private static int CalcularDigitoVerificador(string numero)
            {
                int[] pesos = { 3, 1, 7, 9 }; // Pesos para el cálculo
                int suma = 0;

                for (int i = 0; i < numero.Length; i++)
                {
                    int digito = int.Parse(numero[i].ToString());
                    suma += digito * pesos[i % pesos.Length];
                }

                int resto = suma % 10;
                return resto == 0 ? 0 : 10 - resto;
            }
        }
    }
}
