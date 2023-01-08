using GerenciadorFolhaPagamento_Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Interfaces.Applications
{
    public interface IDepartamentoApplication
    {
        Task<int> SalvarDepartamento(NovoDepartamentoDto novoDepartamento);
        Task<List<DepartamentoDto>> RecuperaTodosDepartamentos();
        Task<int> RetornaIdDepartamentoPeloNome(string nome);
    }
}
