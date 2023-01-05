﻿using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Interfaces.Repositories
{
    public interface IDepartamentoRepository
    {
        Task SalvaNovoDepartamento(Departamento novoDepartamento);
        Task<List<string>> RecuperaOsNomesDeTodosOsDepartamentos();

        Task<List<DepartamentoDto>> RecuperaTodosOsDepartamentos(); 
    }
}
