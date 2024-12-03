using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTarefasApi.Models
{
    /// <summary>
    /// Classe de ProjetosRepositorio.
    /// </summary>
    public class ProjetosRepositorio
    {
        private string _conexao;
        /// <summary>
        /// Construtor ProjetosRepositorio.
        /// </summary>
        ///  
        /// <returns></returns>
        public ProjetosRepositorio()
        {
            _conexao = ConfigurationManager.ConnectionStrings["BDApp"].ConnectionString;
        }

        /// <summary>
        /// Rotina responsável por retornar todas os Projetos do sistema.
        /// </summary>
        ///  
        /// <returns>Lista de Projetos</returns>
        public List<Projetos> GetProjetos()
        {
            List<Projetos> projetos = new List<Projetos>();

            using (var conexao = new SqlConnection(_conexao))
            {
                conexao.Open();

                string sql = "Select * From Projeto";
                projetos = (List<Projetos>)conexao.Query<Projetos>(sql);

                conexao.Close();
            }

            return projetos;
        }

        /// <summary>
        /// Rotina responsável por retornar todas os Projetos por usuário.
        /// </summary>
        /// 
        ///  <param name="usuarioID">ID do usuário</param>
        ///  
        /// <returns>Lista de Projetos</returns>
        public List<Projetos> GetProjetosUsuario(int usuarioID)
        {
            List<Projetos> projetos = new List<Projetos>();

            using (var conexao = new SqlConnection(_conexao))
            {
                conexao.Open();

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("Select Projeto.* From Projeto with(nolock) ");
                sql.AppendLine("inner join Usuario with(nolock) on Usuario.UsuarioID = Projeto.UsuarioID ");
                sql.AppendLine($"where Projeto.UsuarioID = {usuarioID}");
                projetos = (List<Projetos>)conexao.Query<Projetos>(sql.ToString());

                conexao.Close();
            }

            return projetos;
        }

        /// <summary>
        /// Rotina responsável por inserir um novo projeto.
        /// </summary>
        /// 
        ///  <param name="projeto">Model de Projeto</param>
        ///  
        /// <returns>Model RetornoMetodos</returns>
        public async Task<RetornoMetodos> InserirProjetos(Projetos projeto)
        {
            TarefasRepositorio tarefas = new TarefasRepositorio();
            if(tarefas.GetQuantidadeTarefasPorProjeto(projeto.ProjetoID) > 20)
                return new RetornoMetodos { Menssagem = "Projeto atingiu o número máximo de tarefas.", Sucesso = false };

            using (var conexao = new SqlConnection(_conexao))
            {
                 await conexao.ExecuteScalarAsync<int>("INSERT INTO [Projeto] VALUES(@Descricao, @UsuarioID)",
                 new
                 {
                     Descricao = projeto.Descricao,
                     UsuarioID = projeto.UsuarioID
                 });
            }

            return new RetornoMetodos { Menssagem = "Projeto criado com sucesso.", Sucesso = true };
        }

        /// <summary>
        /// Rotina responsável por retornar o id do Projetos.
        /// </summary>
        /// 
        ///  <param name="id">ID do projeto</param>
        ///  
        /// <returns>Id de Projetos</returns>
        public int GetProjetoID(int id)
        {
            var conexao = new SqlConnection(_conexao);
            conexao.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao;

            string sql = $"Select ProjetoID From Projeto Where ProjetoID = {id}";
            cmd.CommandText = sql;

            var projetoId = cmd.ExecuteScalar() != null ? cmd.ExecuteScalar() : 0;

            conexao.Close();
            return Convert.ToInt32(projetoId);
        }

        /// <summary>
        /// Rotina responsável por retornar por deletar um Projeto.
        /// </summary>
        /// 
        ///  <param name="projetoID">ID do projeto</param>
        ///  
        /// <returns>Model RetornoMetodos</returns>
        public async Task<RetornoMetodos> Deletar(int projetoID)
        {
            List<Tarefas> tarefas = new List<Tarefas>();
            TarefasRepositorio tarefasRepositorio = new TarefasRepositorio();
            tarefas = tarefasRepositorio.GetTarefasProjeto(projetoID);
            if(tarefas.Exists(x => x.StatusID == 1))
                return new RetornoMetodos { Menssagem = "Não é permitido remover projetos com tarefas pendentes associadas. Favor concluir ou remover as tarefas pendentes.", Sucesso = false };

            using (var conexao = new SqlConnection(_conexao))
            {
                await conexao.ExecuteAsync("DELETE FROM Projeto where ProjetoID =@projetoID",
                 new { ProjetoID = projetoID });
            }
            return new RetornoMetodos {Menssagem = "Projeto removida com sucesso!", Sucesso = true };
        }
    }
}