using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestordeTarefasApi.Models
{
    public class Projetos
    {
        public Projetos(){}
        public int ProjetoID { get; set; }
        public string Descricao { get; set; }
        public int UsuarioID { get; set; }
    }
}