

namespace GerenciadorFolhaPagamento_Domain.Dtos
{
    public class NovoFuncionarioDto
    {
        public string NomeFuncionario{ get; set; }
        public decimal ValorHora{ get; set; }
        public int IdDepartamento{ get; set; }
        public int IdFuncionario{ get; set; }
    }
}
