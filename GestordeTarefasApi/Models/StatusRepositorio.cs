using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace GestordeTarefasApi.Models
{
    /// <summary>
    /// Classe de StatusRepositorio.
    /// </summary>
    public class StatusRepositorio
    {
        private string _conexao;

        /// <summary>
        /// Construtor StatusRepositorio.
        /// </summary>
        ///  
        /// <returns></returns>
        public StatusRepositorio()
        {
            _conexao = ConfigurationManager.ConnectionStrings["BDApp"].ConnectionString;
        }

        /// <summary>
        /// Rotina responsável por retornar status.
        /// </summary>
        ///
        ///  <param name="statusId">ID de status</param>
        ///  
        /// <returns>Model de Status</returns>
        public Status GetStatus(int statusId)
        {
            Status status = new Status();
            using (var conexao = new SqlConnection(_conexao))
            {
                conexao.Open();

                string sql = $"Select * From Status Where StatusID = {statusId}";
                status = conexao.Query<Status>(sql).FirstOrDefault();

                conexao.Close();
            }

            return status;
        }
    }
}