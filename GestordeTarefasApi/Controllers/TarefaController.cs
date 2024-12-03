using GestordeTarefasApi.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GestordeTarefasApi.Controllers
{
    /// <summary>
    /// Controller de Tarefas.
    /// </summary>
    public class TarefaController : ApiController
    {
        /// <summary>
        /// GET que retona todas as tarefas.
        /// </summary>    
        ///  
        /// <returns>Lista de tarefas</returns>
        [HttpGet]
        [Route("GetTarefas")]
        public HttpResponseMessage GetTarefas()
        {
            try
            {
                TarefasRepositorio repositorio = new TarefasRepositorio();
                var tarefas = repositorio.GetTarefas();
                if (tarefas != null)
                    return Request.CreateResponse(HttpStatusCode.OK, tarefas);
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Não existe tarefas cadastrados no sistema.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// GET que retona todas as tarefas do projeto.
        /// </summary>       
        /// 
        ///  <param name="projetoID">ID do Projeto</param>
        ///  
        /// <returns>Lista de tarefas</returns>
        [HttpGet]
        [Route("GetTarefasProjeto/{projetoID}")]
        public HttpResponseMessage GetTarefasProjeto(int projetoID)
        {
            try
            {
                TarefasRepositorio repositorio = new TarefasRepositorio();
                var tarefas = repositorio.GetTarefasProjeto(projetoID);
                if (tarefas != null)
                    return Request.CreateResponse(HttpStatusCode.OK, tarefas);
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Não existe tarefas cadastrados para esse projeto.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// POST responsável criar uma nova Tarefa
        /// </summary>
        /// <param name="tarefas">Model Tarefas</param>
        /// 
        /// <returns></returns>
        [HttpPost]
        [Route("InserirTarefas")]
        public async Task<HttpResponseMessage> InserirTarefas(Tarefas tarefas)
        {
            try
            {
                TarefasRepositorio repositorio = new TarefasRepositorio();
                var tarefaID = await repositorio.InserirTarefas(tarefas);
                if (tarefaID == 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Ocorreu ao gerar o novo Projeto.");
                return Request.CreateResponse(HttpStatusCode.OK, $"ID da Tarefa: {tarefaID}");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// PUT responsável criar uma nova Tarefa
        /// </summary>
        /// 
        /// <param name="usuarioID">Usuário realizando a alteração</param>
        /// <param name="tarefas">Model Tarefas</param>
        /// 
        /// <returns></returns>
        [HttpPut]
        [Route("PutTarefas/{usuarioID}")]
        public async Task<HttpResponseMessage> PutTarefas(int usuarioID, Tarefas tarefas)
        {
            try
            {
                TarefasRepositorio repositorio = new TarefasRepositorio();
                if(usuarioID == 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Informe um UsuárioID válido.");
                if(tarefas.TarefaID == 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Informe uma TarefaID válida.");

                var tarefa = await repositorio.AtualizaTarefas(usuarioID, tarefas);
                if (tarefa == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Ocorreu ao gerar o novo Projeto.");
                return Request.CreateResponse(HttpStatusCode.OK, tarefa);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Responsável por excluir uma Tarefa
        /// </summary>
        /// <param name="tarefaID">ID de Tarefas</param>
        /// 
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteTarefas/{tarefaID}")]
        public HttpResponseMessage DeleteTarefas(int tarefaID)
        {
            try
            {
                TarefasRepositorio repositorio = new TarefasRepositorio();
                if (tarefaID == 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "TarefaID inválido");

                repositorio.Deletar(tarefaID);              
                return Request.CreateResponse(HttpStatusCode.OK, "Tarefa removida com sucesso!");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Responsável por adicionar um comentário a tarefa
        /// </summary>
        /// <param name="comentario">Model de ComentarioTarefa</param>
        /// 
        /// <returns></returns>
        [HttpPost]
        [Route("AdicionaCometariosTarefa")]
        public async Task<HttpResponseMessage> AdicionaCometariosTarefa(ComentarioTarefa comentario)
        {
            try
            {
                TarefasRepositorio repositorio = new TarefasRepositorio();
                var tarefaID = await repositorio.AdicionaCometariosTarefa(comentario);
                if (tarefaID == 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Ocorreu ao adicionar um comentário na tarefa.");
                return Request.CreateResponse(HttpStatusCode.OK, $"Comentário criado com sucesso: {tarefaID}");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}