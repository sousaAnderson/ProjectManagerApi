using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestordeTarefasApi.Models
{
    public class Log
    {
        public Log() { }
        public int LogID { get; set; }
        public string Descricao { get; set; }
        public string DataAlteracao { get; set; }
        public int UsuarioID { get; set; }
    }
}