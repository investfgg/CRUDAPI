using CRUDAPI.Controllers;
using CRUDAPI.models;
using System.Text.RegularExpressions;

namespace CRUDAPI.Tests
{
    public class EmpregadoTest
    {
        const bool resultExpected = true;

        [Fact(DisplayName = "Nome do empregado não pode conter um conteúdo em branco, ou está vazio ou nulo.")]
        public void NomeEmpregado_NaoPodeConter_ApenasEspBranco_Vazio()
        {
            //arrange
            const string strNomeEmpr = "";

            //act
            bool resultActual = EmpregadosController.ContemEspacos(strNomeEmpr);

            //assert
            Assert.Equal(resultExpected, resultActual);
        }

        [Fact(DisplayName = "Nome do empregado não pode conter caracteres especiais.")]
        public void NomeEmpregado_NaoPodeConter_CaracteresEspeciais()
        {
            //arrange
            const string strNomeEmpr = "Arquibaldo$Geraldino";

            //act
            bool resultActual = EmpregadosController.ContemCaractEspeciais(strNomeEmpr);

            //assert
            Assert.Equal(resultExpected, !resultActual);
        }

        [Fact(DisplayName = "Cargo do empregado não pode conter um conteúdo em branco, ou está vazio ou nulo.")]
        public void CargoEmpregadoNaoPodeConter_ApenasEspBranco_Vazio()
        {
            //arrange
            const string strNomeEmpr = "";

            //act
            bool resultActual = EmpregadosController.ContemEspacos(strNomeEmpr);

            //assert
            Assert.Equal(resultExpected, resultActual);
        }

        [Fact(DisplayName = "Cargo do empregado não pode conter caracteres especiais.")]
        public void CargoEmpregado_NaoPodeConter_CaracteresEspeciais()
        {
            //arrange
            const string strNomeEmpr = "Magnata Sr.";

            //act
            bool resultActual = EmpregadosController.ContemCaractEspeciais(strNomeEmpr);

            //assert
            Assert.Equal(resultExpected, !resultActual);
        }

        [Fact(DisplayName = "Formato de email do empregado está fora do padrão.")]
        public void FormatoEmailForadoPadrao()
        {
            //arrange
            const string strEmailEmpr = "Arquibaldo_Geraldino@yahoo";

            //act
            bool resultActual = EmpregadosController.EmailValido(strEmailEmpr);

            //assert
            Assert.Equal(resultExpected, !resultActual);
        }
    }
}