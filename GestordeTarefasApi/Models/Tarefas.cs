using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestordeTarefasApi.Models
{
    public class Tarefas
    {
        public Tarefas() { }
        public int TarefaID { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string DataVencimento { get; set; }
        public int ProjetoID { get; set; }
        public int StatusID { get; set; }
        public int PrioridadeID { get; set; }
    }
}