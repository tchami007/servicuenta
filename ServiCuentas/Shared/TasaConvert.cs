namespace ServiCuentas.Shared
{
    public static class TasaConvert
    {
        public static decimal Nominal3652Efectiva(decimal Nominal)
        {
            decimal resultado = Nominal / 36500 * 30;
            return resultado;
        }

        public static decimal Efectiva2Nominal(decimal Efectiva)
        {
            decimal resultado = Efectiva / 3000 * 365;
            return resultado;
        }

        public static decimal Efectiva2DiariaLineal(decimal Efectiva)
        {
            var resultado = Efectiva / 3000;
            return resultado;
        }

        public static decimal Efectiva2DiariaExponencial(decimal Efectiva)
        {
            var coeficiente = Math.Pow((double)(1 + Efectiva/100), (double)(1.0 / 30.0)) - 1;
            return (decimal) coeficiente;
        }

        public static decimal DiariaLineal2Efectiva(decimal Diaria)
        {
            var resultado = Diaria * 30;
            return resultado;
        }

        public static decimal DiariaExponencial2Efectiva(decimal Diaria)
        {
            var coeficiente = Math.Pow((double)(1 + Diaria), (double)30) - 1;
            return (decimal) coeficiente;
        }
    }
}
