using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTarefasApi.Models
{
    /// <summary>
    /// Classe de TarefasRepositorio.
    /// </summary>
    public class TarefasRepositorio
    {
        private string _conexao;

        /// <summary>
        /// Construtor TarefasRepositorio.
        /// </summary>
        ///  
        /// <returns></returns>
        public TarefasRepositorio()
        {
            _conexao = ConfigurationManager.ConnectionStrings["BDApp"].ConnectionString;
        }

        #region Métodos Publicos
        /// <summary>
        /// Rotina responsável por retornar todas as tarefas do sistema.
        /// </summary>
        ///  
        /// <returns>Lista de Tarefas</returns>
        public List<Tarefas> GetTarefas()
        {
            List<Tarefas> tarefas = new List<Tarefas>();
            using (var conexao = new SqlConnection(_conexao))
            {
                conexao.Open();

                string sql = "Select * From Tarefa";
                tarefas = (List<Tarefas>)conexao.Query<Tarefas>(sql);

                conexao.Close();
            }

            return tarefas;
        }

        /// <summary>
        /// Rotina responsável por retornar uma lista de tarefas do projeto.
        /// </summary>
        ///
        ///  <param name="projetoID">ID do projeto</param>
        ///  
        /// <returns>Lista de Tarefas</returns>
        public List<Tarefas> GetTarefasProjeto(int projetoID)
        {
            List<Tarefas> tarefas = new List<Tarefas>();
            using (var conexao = new SqlConnection(_conexao))
            {
                conexao.Open();

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select Tarefa.* from Tarefa with(nolock) ");
                sql.AppendLine("inner join Projeto with(nolock) on Tarefa.ProjetoID = Projeto.ProjetoID ");
                sql.AppendLine($"where Tarefa.ProjetoID = {projetoID}");
                tarefas = (List<Tarefas>)conexao.Query<Tarefas>(sql.ToString());

                conexao.Close();
            }

            return tarefas;
        }

        /// <summary>
        /// Rotina responsável por inserir uma nova Tarefa.
        /// </summary>
        ///
        ///  <param name="tarefa">MOdel de Tarefa</param>
        ///  
        /// <returns>ID da nova Tarefa</returns>
        public async Task<int> InserirTarefas(Tarefas tarefa)
        {
            int tarefaID = 0;
            var validacao = ValidaTarefas(tarefa);
            if (!validacao.Sucesso)
                throw new ArgumentException(validacao.Menssagem);
            if (tarefa.PrioridadeID == 0)
                throw new ArgumentException("Informe uma Prioridade.");

            using (var conexao = new SqlConnection(_conexao))
            {
                tarefaID = await conexao.ExecuteScalarAsync<int>("INSERT INTO [Tarefa] VALUES(@Titulo, @Descricao, @DataVencimento, @ProjetoID, @StatusID, @PrioridadeID);SELECT CAST(scope_identity() AS INT)",
                 new
                 {
                     Titulo = tarefa.Titulo.FormataSQL(),
                     Descricao = tarefa.Descricao.FormataSQL(),
                     DataVencimento = tarefa.DataVencimento,
                     ProjetoID = tarefa.ProjetoID,
                     StatusID = tarefa.StatusID,
                     PrioridadeID = tarefa.PrioridadeID
                 });
            }

            return tarefaID;
        }

        /// <summary>
        /// Rotina responsável por atualizar tarefas.
        /// </summary>
        ///
        ///  <param name="usuarioID">ID do Usuário</param>
        ///  <param name="tarefas">Model Tarefa</param>
        ///  
        /// <returns>Model de Tarefas</returns>
        public async Task<Tarefas> AtualizaTarefas(int usuarioID, Tarefas tarefas)
        {
            var tarefa = GetTarefa(tarefas.TarefaID);
            if (tarefa.TarefaID == 0)
                throw new ArgumentException("ID da tarefa inválido.");

            var validacao = ValidaTarefas(tarefas);
            if (!validacao.Sucesso)
                throw new ArgumentException(validacao.Menssagem);

            tarefa.Descricao = tarefas.Descricao.FormataSQL();
            tarefa.DataVencimento = tarefas.DataVencimento;
            tarefa.Titulo = tarefas.Titulo.FormataSQL();

            var validaPrioridade = VerificaPrioridadeTarefa(tarefas.PrioridadeID);
            if (!validaPrioridade.Sucesso)
                throw new ArgumentException(validaPrioridade.Menssagem);

            using (var conexao = new SqlConnection(_conexao))
            {
                await conexao.ExecuteScalarAsync($"UPDATE [Tarefa] SET [Title] = @Titulo, [Descricao] = @Descricao, [DataVencimento] = @DataVencimento, [ProjetoID] = @ProjetoID, [StatusID] = @StatusID WHERE TarefaID = {tarefa.TarefaID}",
                new
                {
                    Titulo = tarefa.Titulo,
                    Descricao = tarefa.Descricao,
                    DataVencimento = tarefa.DataVencimento,
                    ProjetoID = tarefa.ProjetoID,
                    StatusID = tarefa.StatusID
                });
            }

            LogRepositorio log = new LogRepositorio();
            log.InsereLogTarefa(usuarioID, tarefas);

            return GetTarefa(tarefas.TarefaID);
        }

        /// <summary>
        /// Rotina responsável por deletar tarefa.
        /// </summary>
        ///
        ///  <param name="tarefaID">ID de Tarefa</param>
        ///  
        /// <returns></returns>
        public async void Deletar(int tarefaID)
        {
            using (var conexao = new SqlConnection(_conexao))
            {
                await conexao.ExecuteAsync("DELETE FROM Tarefa where TarefaID =@tarefaID",
                 new { TarefaID = tarefaID });
            }
        }

        /// <summary>
        /// Rotina responsável por retornar quantidade de tarefas por projeto.
        /// </summary>
        ///
        ///  <param name="projetoID">ID do projeto</param>
        ///  
        /// <returns>Quantidade de projetos</returns>
        public int GetQuantidadeTarefasPorProjeto(int projetoID)
        {
            var conexao = new SqlConnection(_conexao);
            conexao.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao;

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select COUNT(TarefaID) AS Quantidade from Tarefa with(nolock) ");
            sql.AppendLine("inner join Projeto with(nolock) on Tarefa.ProjetoID = Projeto.ProjetoID ");
            sql.AppendLine($"where Tarefa.ProjetoID = {projetoID}");
            cmd.CommandText = sql.ToString();

            var quantidade = cmd.ExecuteScalar() != null ? cmd.ExecuteScalar() : 0;

            conexao.Close();
            return Convert.ToInt32(quantidade);
        }

        /// <summary>
        /// Rotina responsável por retornar relatório de desempenho do usuario.
        /// </summary>
        ///
        ///  <param name="usuarioID">ID do usuário</param>
        ///  
        /// <returns>Lista de Desempenho</returns>
        public List<Desempenho> GetRelatorioDesempenho(int usuarioID)
        {
            List<Desempenho> desempenhos = new List<Desempenho>();

            if (RetornaFuncaoUsuario(usuarioID).ToLower() != "gerente")
                throw new ArgumentException("Relatório permitido somente para gerentes.");

            using (var conexao = new SqlConnection(_conexao))
            {
                conexao.Open();
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("Select Usuario.Nome, COUNT(Tarefa.TarefaID)AS Quantidade ");
                sql.AppendLine("From Tarefa with(nolock) ");
                sql.AppendLine("Inner Join Projeto with(nolock) on Projeto.ProjetoID = Tarefa.ProjetoID ");
                sql.AppendLine("Inner Join Usuario with(nolock) on Usuario.UsuarioID = Projeto.UsuarioID ");
                sql.AppendLine("Inner Join LogTarefa with(nolock) on LogTarefa.TarefaID = Tarefa.TarefaID ");
                sql.AppendLine(" AND LogTarefa.Descricao like '%StatusID: 3%' AND LogTarefa.Chave = 'Tarefa' ");
                sql.AppendLine("where Tarefa.StatusID = 3 ");
                sql.AppendLine(" AND LogTarefa.DataAlteracao BETWEEN DATEADD (DAY ,-30, GETDATE()) AND GETDATE()");
                sql.AppendLine("group by Usuario.Nome");

                desempenhos = (List<Desempenho>)conexao.Query<Desempenho>(sql.ToString());
                conexao.Close();
            }

            return desempenhos;
        }

        /// <summary>
        /// Rotina responsável por adicionar um comentário na tarefa.
        /// </summary>
        ///
        ///  <param name="comentario">Model do ComentarioTarefa</param>
        ///  
        /// <returns>Id a tabela ComentarioTarefa</returns>
        public async Task<int> AdicionaCometariosTarefa(ComentarioTarefa comentario)
        {
            int comentarioTarefaID = 0;

            using (var conexao = new SqlConnection(_conexao))
            {
                comentarioTarefaID = await conexao.ExecuteScalarAsync<int>("INSERT INTO [ComentarioTarefa] VALUES(@Observacao, @TarefaID, @UsuarioID);SELECT CAST(scope_identity() AS INT)",
                 new
                 {
                     Observacao = comentario.Observacao.FormataSQL(),
                     TarefaID = comentario.TarefaID,
                     UsuarioID = comentario.UsuarioID
                 });
            }

            LogRepositorio log = new LogRepositorio();
            log.InsereLogComentarioTarefa(comentario);

            return comentarioTarefaID;
        }
        #endregion

        #region Métodos Privados
        private Tarefas GetTarefa(int tarefaID)
        {
            Tarefas tarefa = new Tarefas();
            using (var conexao = new SqlConnection(_conexao))
            {
                conexao.Open();

                string sql = $"Select * From Tarefa Where TarefaID = {tarefaID}";
                tarefa = conexao.Query<Tarefas>(sql).FirstOrDefault();

                conexao.Close();
            }

            return tarefa;
        }

        private RetornoMetodos ValidaTarefas(Tarefas tarefas)
        {
            ProjetosRepositorio repositorio = new ProjetosRepositorio();
            StatusRepositorio statusRepositorio = new StatusRepositorio();
            PrioridadesRepositorio prioridadesRepositorio = new PrioridadesRepositorio();

            if (!tarefas.DataVencimento.DataValida())
                return new RetornoMetodos { Menssagem = "Informe uma data válida.", Sucesso = false };
            if (repositorio.GetProjetoID(tarefas.ProjetoID) == 0)
                return new RetornoMetodos { Menssagem = "Informe um ID de projeto válido.", Sucesso = false };
            if (statusRepositorio.GetStatus(tarefas.StatusID) == null)
                return new RetornoMetodos { Menssagem = "Informe um ID de status válido.", Sucesso = false };
            if (prioridadesRepositorio.GetPrioridades(tarefas.PrioridadeID) == null)
                return new RetornoMetodos { Menssagem = "Informe um ID de prioridade válido.", Sucesso = false };
            return new RetornoMetodos { Sucesso = true };
        }

        private RetornoMetodos VerificaPrioridadeTarefa(int prioridadeID)
        {
            var conexao = new SqlConnection(_conexao);
            conexao.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao;

            string sql = $"Select *  From Tarefa Where PrioridadeID = = {prioridadeID}";
            cmd.CommandText = sql;

            var projetoId = cmd.ExecuteScalar() != null ? cmd.ExecuteScalar() : 0;
            conexao.Close();

            if (Convert.ToInt32(projetoId) > 0)
                return new RetornoMetodos { Sucesso = true };
            else
                return new RetornoMetodos { Sucesso = false, Menssagem = "Não é permitido alterar a prioridade de uma tarefa." };
        }

        private string RetornaFuncaoUsuario(int usuarioID)
        {
            var conexao = new SqlConnection(_conexao);
            conexao.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao;

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select Funcao.Descricao from Usuario with(nolock) ");
            sql.AppendLine("inner join Funcao with(nolock) on Usuario.FuncaoID = Funcao.FuncaoID ");
            sql.AppendLine($"where Usuario.UsuarioID = {usuarioID}");
            cmd.CommandText = sql.ToString();

            var funcao = cmd.ExecuteScalar();
            conexao.Close();

            return funcao.ToString();
        }
        #endregion

    }
}