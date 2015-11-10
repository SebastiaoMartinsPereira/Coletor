using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TitaniumColector.Classes.Model
{
    class EmbalagemSeparacao : Embalagem
    {
        private Double peso;
        private Double pesoTotal;
        private int quantidade;

        public EmbalagemSeparacao() { }

        public EmbalagemSeparacao(Int32 codigo, String nome, PadraoEmbalagem padrao, Int32 prodEmbalagem,Double pesoEmbalagem)
            : base(codigo, nome, padrao, prodEmbalagem)
        {
            this.Peso = pesoEmbalagem;
            this.Quantidade = 0;
            this.calcularPesoTotal();
        }

        public EmbalagemSeparacao(Int32 codigo, String nome, PadraoEmbalagem padrao, Int32 prodEmbalagem, Double peso,Int32 quantidade)
            : base(codigo, nome, padrao, prodEmbalagem)
        {
            this.Peso = peso;
            this.Quantidade = quantidade;
            this.calcularPesoTotal();
        }

        public double Peso
        {
            get { return peso; }
            set { peso = value; }
        }

        public int Quantidade
        {
            get { return quantidade; }
            set { quantidade = value; }
        }

        public void adicionar()
        {
            this.Quantidade++;
        }

        public void remover() 
        {
            if (!this.isVazio())
            {
                this.Quantidade--;
            }
            else 
            {
                throw new InvalidOperationException("Volume não pode ser menor que 1");
            }
        }

        public Boolean isVazio() 
        {
            return (this.Quantidade == 0);
        }

        public Double PesoTotal
        {
            get { return pesoTotal; }
            set { pesoTotal = value; }
        }

        public void inicializaQtdEmbalagem() 
        {
            if (base.isPadrao()) 
            {
                this.adicionar();
            }
        }

        public void calcularPesoTotal()
        {
            this.PesoTotal = this.Quantidade * this.Peso;
        }

        public override string ToString()
        {
            return String.Format("Codigo : {0}\nNome : {1}\nQuantidade : {2} \nPadrao: {3} ",this.Codigo,this.Nome,this.Quantidade,this.Padrao);
        }

        public override bool Equals(object obj)
        {
            return ((EmbalagemSeparacao)obj).Codigo == this.Codigo;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
