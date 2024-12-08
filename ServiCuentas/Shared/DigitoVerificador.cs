namespace ServiCuentas.Shared
{
    public static class DigitoVerificador
    {
        private static readonly int[] coeficientes = { 2, 3, 4, 5, 6, 7 };

        public static int Calcular(Decimal valor)
        {
            // Filtrar solo los dígitos de 0 a 9
            var soloDigitos = valor.ToString().Where(char.IsDigit).ToArray();

            // Invertir los dígitos
            Array.Reverse(soloDigitos);

            // Multiplicar cada dígito por los coeficientes y sumar los productos
            int suma = 0;
            for (int i = 0; i < soloDigitos.Length; i++)
            {
                int digito = int.Parse(soloDigitos[i].ToString());
                suma += digito * coeficientes[i % coeficientes.Length];
            }

            // Calcular el módulo 11 de la suma
            int resto = suma % 11;

            // Calcular el dígito verificador que estará entre 0 y 9
            int digitoVerificador = resto % 10;

            return digitoVerificador;
        }

    }
}
