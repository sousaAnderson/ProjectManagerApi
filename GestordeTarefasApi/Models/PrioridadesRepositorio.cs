using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace GestordeTarefasApi.Models
{
    /// <summary>
    /// Classe de PrioridadesRepositorio.
    /// </summary>
    public class PrioridadesRepositorio
    {
        private string _conexao;

        /// <summary>
        /// Construtor PrioridadesRepositorio.
        /// </summary>
        ///  
        /// <returns></returns>
        public PrioridadesRepositorio()
        {
            _conexao = ConfigurationManager.ConnectionStrings["BDApp"].ConnectionString;
        }

        /// <summary>
        /// Rotina responsável por retornar uma lista de tarefas do projeto.
        /// </summary>
        ///
        ///  <param name="id">ID de prioridades</param>
        ///  
        /// <returns>Model de Prioridades</returns>
        public Prioridades GetPrioridades(int id)
        {
            Prioridades prioridade = new Prioridades();
            using (var conexao = new SqlConnection(_conexao))
            {
                conexao.Open();

                string sql = $"Select * From Prioridade Where PrioridadeID = {id}";
                prioridade = conexao.Query<Prioridades>(sql).FirstOrDefault();

                conexao.Close();
            }

            return prioridade;
        }
    }
}