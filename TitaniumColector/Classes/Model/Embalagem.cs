using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TitaniumColector.Classes.Model
{

    class Embalagem
    {
        private Int32 codigo;
        private String nome;
        private PadraoEmbalagem padrao;
        private Int32 produtoEmbalagem;

        public enum PadraoEmbalagem { NAOPADRAO = 0, PADRAO = 1 }

        public Embalagem() { }

        public Embalagem(Int32 intCodigo,String strNome,PadraoEmbalagem padrao,Int32 prodEmb) 
        {
            this.Codigo = intCodigo;
            this.Nome = strNome;
            this.padrao = padrao;
            this.ProdutoEmbalagem = prodEmb; 
        }

        public Int32 Codigo
        {
          get { return codigo; }
          set { codigo = value; }
        }

        public String Nome
        {
          get { return nome; }
          set { nome = value; }
        }
                
        internal PadraoEmbalagem Padrao
        {
          get { return padrao; }
          set { padrao = value; }
        }

        public Int32 ProdutoEmbalagem
        {
            get { return produtoEmbalagem; }
            set { produtoEmbalagem = value; }
        }

        public Boolean isPadrao() 
        {
            return this.padrao == PadraoEmbalagem.PADRAO;
        }

    }
}
