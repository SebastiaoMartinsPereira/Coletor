using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TitaniumColector.Classes.Dao;

namespace TitaniumColector.Classes.Utility
{
     class Permissoes
    {
        private List<Parametros> parametros;
        //private DaoPermissoes daoPermissoes;
        private List<String> listCodigoParametro;

        
        public Permissoes() {
            this.parametros = new List<Parametros>();
            //incializa a lista com os parametro a serem mantidos no mobile.
            gerarListaCodigoParametro();
        }

        //Adiciona um parametro a lista.
        public void addParametro(Parametros param) {
            this.parametros.Add(param);
        }

        /// <summary>
        /// retorna um parametro solicitado.
        /// caso ele não exista na lista retorna null.
        /// </summary>
        /// <param name="codigoParametro">Codigo do Parametro a ser retornado</param>
        /// <returns>obj Parametros caso exista ou null</returns>
        public Parametros retornarParametro(String codigoParametro) 
        {
            foreach (var item in this.parametros)
            {
                if (item.Codigo == codigoParametro)
                {
                    return item;
                }
            }

            return null;
        }

         /// <summary>
         /// retorna um parametro solicitado.
         /// caso ele não exista na lista retorna null
         /// </summary>
        /// <param name="codigoParametro">Codigo do Parametro a ser retornado</param>
         /// <param name="listaParametros">list de parametros onde realizar a busca.</param>
         /// <returns></returns>
        public Parametros retornarParametro(String codigoParametro,List<Parametros> listaParametros)
        {
            foreach (var item in listaParametros)
            {
                if (item.Codigo == codigoParametro)
                {
                    return item;
                }
            }

            return null;
        }

        public void gerarListaCodigoParametro()
        {
            this.listCodigoParametro = new List<string>();
            this.listCodigoParametro.Add("ValidarSequencia");
        }

        public List<String> ListCodigoParametro
        {
            get { return listCodigoParametro; }
            set { listCodigoParametro = value; }
        }

        internal List<Parametros> Parametros
        {
            get { return parametros; }
            set { parametros = value; }
        }

    }
}
