using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TitaniumColector.Classes.Utility
{
    class Parametros
    {
        private String codigo;
        private String descricao;
        private String valor;
        private Int32 auxiliar;


        public Parametros(String codigo,String descricao,String valor,Int32 auxiliar) 
        {
            this.Codigo = codigo;
            this.Descricao = descricao;
            this.Valor = valor;
            this.Auxiliar = auxiliar;
        }

        public String Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public String Descricao
        {
            get { return descricao; }
            set { descricao = value; }
        }

        public String Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        public Int32 Auxiliar
        {
            get { return auxiliar; }
            set { auxiliar = value; }
        }
    }
}
