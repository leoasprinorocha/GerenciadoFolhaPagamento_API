using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GerenciadorFolhaPagamento_Domain_Test.Entities
{
    [TestClass]
    public class ProcessamentoFolhaTest
    {

        [TestMethod]
        public void CalculoDeDiasUteisDosMesDeveRetornarAoMenos20Dias()
        {
            ProcessamentoFolha processamentoFolha = new ProcessamentoFolha();
            string mes = "Fevereiro";
            string ano = "2016";
            var diasUteis = processamentoFolha.RetornaQuantidadeDeDiasUteisDoMes(mes, ano);
            Assert.IsTrue(diasUteis >= 20);
        }

        [TestMethod]
        public void ObjetoComPropriedadesTotaisDeveEstarCalculadoCorretamente()
        {
            ProcessamentoFolha processamentoFolha = new ProcessamentoFolha();

            List<RegistroPontoDto> listaRegistros = new List<RegistroPontoDto>()
            {
                new RegistroPontoDto(){
                    HoraEntrada = TimeSpan.FromHours(8),
                    HoraSaida= TimeSpan.FromHours(19),
                    HoraEntradaAlmoco = TimeSpan.FromHours(12),
                    HoraSaidaAlmoco = TimeSpan.FromHours(13),
                    ValorHora = (decimal)150.75,
                },

                 new RegistroPontoDto(){
                    HoraEntrada = TimeSpan.FromHours(9),
                    HoraSaida= TimeSpan.FromHours(18),
                    HoraEntradaAlmoco = TimeSpan.FromHours(12),
                    HoraSaidaAlmoco = TimeSpan.FromHours(14),
                    ValorHora = (decimal)125,
                }

            };

            var objetoTotal = processamentoFolha.RetornaObjetoComPropriedadesTotais(listaRegistros, "Fevereiro", "2021");
            Assert.AreEqual(2382.50, (double)objetoTotal.TotalPagamentos);
            Assert.AreEqual(2, objetoTotal.TotalHorasExtras);
            Assert.AreEqual(1, objetoTotal.TotalHorasNegativas);
            Assert.AreEqual(125, objetoTotal.TotalDescontos);
            Assert.AreEqual(17, objetoTotal.TotalHorasTrabalhadas);
        }
    }
}
