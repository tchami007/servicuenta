using ServiCuentas.Shared;

namespace PruebaServiCuentas
{
    public class DigitoVerificadorTests
    {
        [Fact]
        public void Calculo_ValidInput_ReturnsExpectedValue1()
        {
            // Arrange
            decimal valor = 7;
            int expected = ((2 * 7) % 11);

            expected = expected % 10;

            // Act
            int actual = DigitoVerificador.Calcular(valor);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Calculo_ValidInput_ReturnsExpectedValue2()
        {
            // Arrange
            decimal valor = 75;
            int expected = ((2 * 5 + 3 * 7) % 11);

            expected = expected % 10;

            // Act
            int actual = DigitoVerificador.Calcular(valor);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Calculo_ValidInput_ReturnsExpectedValue1000punto15()
        {
            // Arrange
            decimal valor = 1000.15m;
            int expected = ((2*5+3*1+4*0+5*0+6*0+7*1) % 11);

            expected = expected % 10;

            // Act
            int actual = DigitoVerificador.Calcular(valor);

            // Assert
            Assert.Equal(expected, actual);
        }

        public void Calculo_ValidInput_ReturnsExpectedValue1000()
        {
            // Arrange
            decimal valor = 1000;
            int expected = ((2 * 0 + 3 * 0 + 4 * 0 + 5 * 1) % 11);

            expected = expected % 10;

            // Act
            int actual = DigitoVerificador.Calcular(valor);

            // Assert
            Assert.Equal(expected, actual);
        }

    }
}
