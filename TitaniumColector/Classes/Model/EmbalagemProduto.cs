using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TitaniumColector.Classes.Model
{
    class EmbalagemProduto : Embalagem 
    {

        
        private Double quantidade;
        private Int32 tipoEmbalagem;
        private String ean13Embalagem;

        //private Int32 produtoEmbalagem;
        //private Int32 codigo;
        //private String nome;
        //private PadraoEmbalagem isPadrao;

        public EmbalagemProduto() { }

        public EmbalagemProduto(
             Int32 codigo
            ,String nome
            ,TitaniumColector.Classes.Model.Embalagem.PadraoEmbalagem padrao
            ,Int32 produtoEmb
            ,Double qtd
            ,Int32 tipo
            ,String ean13Emb) : base (codigo,nome,padrao,produtoEmb)
        {

            
            this.Quantidade = qtd;
            this.TipoEmbalagem = tipo;
            this.Ean13Embalagem = ean13Emb;

            //this.ProdutoEmbalagem = produtoEmb;
            //parametros passados para a classe Pai
            //this.Codigo = codigo;
            //this.Nome = nome;
            //this.IsPadrao = isPadrao;

        }

    #region "GET E SETS"

        public String Ean13Embalagem
        {
            get { return ean13Embalagem; }
            set { ean13Embalagem = value; }
        }

        public Int32 TipoEmbalagem
        {
            get { return tipoEmbalagem; }
            set { tipoEmbalagem = value; }
        }

        public Double Quantidade
        {
            get { return quantidade; }
            set { quantidade = value; }
        }

        //public Int32 ProdutoEmbalagem
        //{
        //    get { return produtoEmbalagem; }
        //    set { produtoEmbalagem = value; }
        //}


        //parametros passados para a classe Pai

        //public PadraoEmbalagem IsPadrao
        //{
        //    get { return isPadrao; }
        //    set { isPadrao = value; }
        //}

        //public String Nome
        //{
        //    get { return nome; }
        //    set { nome = value; }
        //}

        //public Int32 Codigo
        //{
        //    get { return codigo; }
        //    set { codigo = value; }
        //}

    #endregion
        
    }
}
