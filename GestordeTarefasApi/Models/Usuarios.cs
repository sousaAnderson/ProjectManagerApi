using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestordeTarefasApi.Models
{
    public class Usuarios
    {
        public Usuarios() { }
        public int UsuarioID { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public List<Projetos> Projetos { get; set; }
    }
}