using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestordeTarefasApi.Models
{
    public class ComentarioTarefa
    {
        public ComentarioTarefa() { }
        public int ComentarioTarefaID { get; set; }
        public string Observacao { get; set; }
        public int TarefaID { get; set; }
        public int UsuarioID { get; set; }
    }
}