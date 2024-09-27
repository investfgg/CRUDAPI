using CRUDAPI.Controllers;
using CRUDAPI.models;
using System.Text.RegularExpressions;

namespace CRUDAPI.Tests
{
    public class DepartamentoTest
    {
        const bool resultExpected = true;

        [Fact(DisplayName = "Nome do departamento não pode conter um conteúdo em branco, ou está vazio ou nulo.")]
        public void NomeDepto_NaoPodeConter_ApenasEspBranco_Vazio()
        {
            //arrange
            const string strNomeDepto = "";

            //act
            bool resultActual = DepartamentosController.ContemEspacos(strNomeDepto);

            //assert
            Assert.Equal(resultExpected, resultActual);
        }

        [Fact(DisplayName = "Nome do departamento não pode conter um ou mais caracteres especiais.")]
        public void NomeDepto_NaoPodeConter_CaracteresEspeciais()
        {
            //arrange
            const string strNomeDepto = "Gerente Senior 08/2024";

            //act
            bool resultActual = DepartamentosController.ContemCaractEspeciais(strNomeDepto);

            //assert
            Assert.Equal(resultExpected, resultActual);
        }
    }
}