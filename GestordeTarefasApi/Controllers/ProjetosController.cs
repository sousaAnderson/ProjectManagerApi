using GestordeTarefasApi.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GestordeTarefasApi.Controllers
{
    /// <summary>
    /// Controller Projetos.
    /// </summary>
    public class ProjetosController : ApiController
    {
        /// <summary>
        /// GET que retona todos os projetos.
        /// </summary>    
        ///  
        /// <returns>Lista de projetos</returns>
        [HttpGet]
        [Route("GetProjetos")]
        public HttpResponseMessage GetProjetos()
        {
            try
            {
                ProjetosRepositorio repositorio = new ProjetosRepositorio();
                var projetos = repositorio.GetProjetos();
                if (projetos != null)
                    return Request.CreateResponse(HttpStatusCode.OK, projetos);
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Não existe projetos cadastrados no sistema");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// GET que retona todos os projetos do usuário.
        /// </summary>       
        /// 
        ///  <param name="usuarioID">ID do Usuário</param>
        ///  
        /// <returns>Lista de projetos</returns>
        [HttpGet]
        [Route("GetProjetosUsuario/{usuarioID}")]
        public HttpResponseMessage GetProjetosUsuario(int usuarioID)
        {
            try
            {
                ProjetosRepositorio repositorio = new ProjetosRepositorio();
                var projetos = repositorio.GetProjetosUsuario(usuarioID);
                if(projetos != null)
                    return Request.CreateResponse(HttpStatusCode.OK, projetos);
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Não existe projetos cadastrados para esse usuário");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// POST responsável criar um novo Projeto
        /// </summary>
        /// <param name="projetos">Model Projetos</param>
        /// 
        /// <returns></returns>
        [HttpPost]
        [Route("InserirProjetos")]
        public async Task<HttpResponseMessage> InserirProjetos(Projetos projetos)
        {
            try
            {
                ProjetosRepositorio repositorio = new ProjetosRepositorio();
                var projeto = await  repositorio.InserirProjetos(projetos);
                if(!projeto.Sucesso)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, projeto.Menssagem);
                return Request.CreateResponse(HttpStatusCode.OK, $"ID do Projeto: {projeto.Menssagem}");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Responsável por excluir uma Projeto
        /// </summary>
        /// <param name="projetoID">ID do Projeto</param>
        /// 
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteProjetos/{projetoID}")]
        public async Task<HttpResponseMessage> DeleteProjetos(int projetoID)
        {
            try
            {
                ProjetosRepositorio repositorio = new ProjetosRepositorio();
                if (projetoID == 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "ID do Projeto inválido");

                var retorno = await repositorio.Deletar(projetoID);
                if(retorno.Sucesso)
                    return Request.CreateResponse(HttpStatusCode.OK, retorno.Menssagem);
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, retorno.Menssagem);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}