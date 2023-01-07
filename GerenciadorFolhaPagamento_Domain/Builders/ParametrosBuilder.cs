using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Entities;
using GerenciadorFolhaPagamento_Domain.Interfaces.Builders;
using System.Collections.Generic;


namespace GerenciadorFolhaPagamento_Domain.Builders
{
    public class ParametrosBuilder : IParametrosBuilder
    {
        private Parametros _parametros;

        public ParametrosBuilder() =>
            _parametros = new Parametros();


        public ParametrosBuilder VerificaSeParametroJaExiste(IList<string> listaParametros, NovoParametroDto novoParametro)
        {
            if (listaParametros.Contains(novoParametro.NomeParametro.ToUpper()))
            {
                _parametros = null;
                return this;
            }
            else
            {
                _parametros.NomeParametro = novoParametro.NomeParametro;
                _parametros.ValorParametro = novoParametro.ValorParametro;
                return this;
            }
        }

        public Parametros Build() =>
        _parametros;

    }
}
