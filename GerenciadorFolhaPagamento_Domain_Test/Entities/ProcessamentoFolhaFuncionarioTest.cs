using GerenciadorFolhaPagamento_Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace GerenciadorFolhaPagamento_Domain_Test.Entities
{
    [TestClass]
    public class ProcessamentoFolhaFuncionarioTest
    {


        [TestMethod]
        public void DeveCalcularHorasNegativasMensaisCorretamente()
        {
            int diasUteisMes = 22;
            int horasTrabalhadasMes = 140;
            ProcessamentoFolha processamentoFolha = new ProcessamentoFolha();
            ProcessamentoFolha_Funcionario processamentoFolhaFuncionario = new ProcessamentoFolha_Funcionario();
            decimal horasNegativasMes = processamentoFolhaFuncionario.RetornasHorasNegativasMes(diasUteisMes, processamentoFolha.QuantidadeDeHorasTrabalhadasEsperadaDia, horasTrabalhadasMes);
            Assert.AreEqual(36, horasNegativasMes);
        }

        [TestMethod]
        public void DeveCalcularHorasExtrasMesCorretamente()
        {
            int diasUteisMes = 20;
            int horasTrabalhadasMes = 190;
            ProcessamentoFolha processamentoFolha = new ProcessamentoFolha();
            ProcessamentoFolha_Funcionario processamentoFolhaFuncionario = new ProcessamentoFolha_Funcionario();
            decimal horasExtrasMes = processamentoFolhaFuncionario.RetornaHorasExtrasMes(diasUteisMes, processamentoFolha.QuantidadeDeHorasTrabalhadasEsperadaDia, horasTrabalhadasMes);
            Assert.AreEqual(30, horasExtrasMes);
        }

    }
}
