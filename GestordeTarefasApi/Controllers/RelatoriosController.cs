using GestordeTarefasApi.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GestordeTarefasApi.Controllers
{
    /// <summary>
    /// Controller Relatórios.
    /// </summary>
    public class RelatoriosController : ApiController
    {
        /// <summary>
        /// GET que retona o Relatório de Desempenho.
        /// </summary>    
        ///  
        /// <returns>Lista de Desempenho</returns>
        [HttpGet]
        [Route("GetRelatorioDesempenho/{usuarioID}")]
        public HttpResponseMessage GetRelatorioDesempenho(int usuarioID)
        {
            try
            {
                TarefasRepositorio repositorio = new TarefasRepositorio();
                var projetos = repositorio.GetRelatorioDesempenho(usuarioID);
                if (projetos != null)
                    return Request.CreateResponse(HttpStatusCode.OK, projetos);
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Não existe tarefas concluídas");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}