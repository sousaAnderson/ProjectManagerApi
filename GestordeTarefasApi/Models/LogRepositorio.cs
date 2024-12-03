using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GestordeTarefasApi.Models
{
    /// <summary>
    /// Classe de LogRepositorio.
    /// </summary>
    public class LogRepositorio
    {
        private string _conexao;
        /// <summary>
        /// Construtor LogRepositorio.
        /// </summary>
        ///  
        /// <returns></returns>
        public LogRepositorio()
        {
            _conexao = ConfigurationManager.ConnectionStrings["BDApp"].ConnectionString;
        }

        /// <summary>
        /// Rotina responsável por inserir log da tarefa atualizada.
        /// </summary>
        /// 
        /// <param name="usuarioID">Id do Usuário</param>
        ///  <param name="tarefa">MOdel de Tarefa</param>
        ///  
        /// <returns></returns>
        public async void InsereLogTarefa(int usuarioID, Tarefas tarefa)
        {
            string descricao = "";
            if (!string.IsNullOrEmpty(tarefa.Descricao))
                 descricao = $"Descricao: {tarefa.Descricao}";
            if (!string.IsNullOrEmpty(tarefa.Titulo))
                descricao = descricao == "" ? $"Titulo: {tarefa.Titulo}" : descricao + $", Titulo: {tarefa.Titulo}";
            if(tarefa.StatusID > 0)
                descricao = descricao == "" ? $"StatusID: {tarefa.StatusID}" : descricao + $", StatusID: {tarefa.StatusID}";

            using (var conexao = new SqlConnection(_conexao))
            {
                await conexao.ExecuteScalarAsync("INSERT INTO [LogTarefa] VALUES(@Descricao, @DataAlteracao, @UsuarioID, @Chave)",
                 new
                 {
                     Descricao = descricao,
                     DataAlteracao = DateTime.Now,
                     UsuarioID = usuarioID,
                     Chave = "Tarefa"
                 });
            }
        }

        /// <summary>
        /// Rotina responsável por inserir log do comentario inserido na tarefa tarefa.
        /// </summary>
        /// 
        /// <param name="comentario">Model de ComentarioTarefa</param>
        ///  
        /// <returns></returns>
        public void InsereLogComentarioTarefa(ComentarioTarefa comentario)
        {
            string descricao = $@"Usuario: {comentario.UsuarioID}: \r\n
                Comentário: {comentario.Observacao}";
            
            using (var conexao = new SqlConnection(_conexao))
            {
                conexao.ExecuteScalar("INSERT INTO [LogTarefa] VALUES(@Descricao, @DataAlteracao, @UsuarioID, @Chave)",
                 new
                 {
                     Descricao = descricao,
                     DataAlteracao = DateTime.Now,
                     UsuarioID = comentario.UsuarioID,
                     Chave = "ComentarioTarefa"
                 });
            }
        }
    }
}