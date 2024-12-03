using System;
using System.Text.RegularExpressions;

namespace GestordeTarefasApi
{
    /// <summary>
    /// Classe de Util.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Rotina responsavel formatar string.
        /// </summary>
        /// 
        ///  <param name="valor">String que sera formatada</param>
        ///  
        /// <returns>string</returns>
        public static string FormataSQL(this string valor)
        {
            string retorno;
            if (valor != null)
            {
                retorno = valor;
                retorno = retorno.Replace("'", "' + Char(39) + '");
                retorno = retorno.Replace("\"", "' + Char(34) + '");
                retorno = retorno.Replace("%", "' + Char(37) + '");
                retorno = retorno.Replace("[", "' + Char(91) + '");
                retorno = retorno.Replace("]", "' + Char(93) + '");
                retorno = retorno.Replace("\r", "' + Char(13) + '");
                retorno = retorno.Replace("\n", "' + Char(10) + '");
            }
            else
            {
                retorno = "";
            }
            return retorno;
        }

        /// <summary>
        /// Rotina responsavel por verificar se string no formato de data é valida.
        /// </summary>
        /// 
        ///  <param name="data">String data</param>
        ///  
        /// <returns>True/False</returns>
        public static bool DataValida(this string data)
        {
            DateTime temp;
            if (DateTime.TryParse(data, out temp))
                return true;
            else
                return false;

        }

        /// <summary>
        /// Rotina responsavel por remover caracteres especiais de string.
        /// </summary>
        /// 
        ///  <param name="valor">String que sera formatada</param>
        ///  
        /// <returns>Retorna string formatada</returns>
        public static string RemoveCaracteresEspeciais(this string valor)
        {
            return Regex.Replace(valor, "[^0-9a-zA-Z]+", "");
        }
    }
}