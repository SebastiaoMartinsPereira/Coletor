using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TitaniumColector.Classes
{

    class Proposta
    {
        private Int64 codigo;
        private String numero;
        private String dataLiberacao;
        private String razaoCliente;
        private int codigoCliente;
        private Double totalItens;
        private Double totalpecas;
        private Int32 qtdVolumesProposta;
        private List<ProdutoProposta> listItemProposta;
        private Int32 codigoPikingMobile;
        private Boolean isInterrompido;

        #region "CONSTRUCTORS"

        public Proposta()
        { }

        /// <summary>
        /// Recebe outro obj do tipo Proposta e set os parâmetros para a nova instância a ser criada.
        /// </summary>
        /// <param name="obj"></param>
        public Proposta(Object obj)
        {
           
            if (obj.GetType() == typeof(Proposta))
            {
                this.Codigo = ((Proposta)obj).Codigo;
                this.Numero = ((Proposta)obj).Numero;
                this.DataLiberacao = ((Proposta)obj).DataLiberacao;
                this.CodigoCliente = ((Proposta)obj).CodigoCliente;
                this.RazaoCliente = ((Proposta)obj).RazaoCliente;
                this.Volumes = ((Proposta)obj).Volumes;
                this.ListObjItemProposta = ((Proposta)obj).ListObjItemProposta;
                this.CodigoPikingMobile = ((Proposta)obj).CodigoPikingMobile;
            }
        }

        /// <summary>
        /// Instância um Obj Proposta
        /// </summary>
        /// <param name="codigoProposta">Código da Proposta</param>
        /// <param name="numeroProposta">Número da proposta</param>
        /// <param name="dataLiberacaoProposta">Data liberação Proposta</param>
        /// <param name="codigoCliente">Código cliente Proposta</param>
        /// <param name="razaoCliente">Razão cliente Proposta</param>
        /// <param name="statusOrdemSeparacao">Status de Ordem separação Proposta</param>
        public Proposta(Int64 codigoProposta, string numeroProposta, string dataLiberacaoProposta, int codigoCliente,
                        string razaoCliente)
        {
            this.Codigo = codigoProposta;
            this.Numero = numeroProposta;
            this.DataLiberacao = dataLiberacaoProposta;
            this.CodigoCliente = codigoCliente;
            this.RazaoCliente = razaoCliente;
        }

        /// <summary>
        /// Instância um Obj Proposta
        /// </summary>
        /// <param name="codigoProposta">Código da Proposta</param>
        /// <param name="numeroProposta">Número da proposta</param>
        /// <param name="dataLiberacaoProposta">Data liberação Proposta</param>
        /// <param name="codigoCliente">Código cliente Proposta</param>
        /// <param name="razaoCliente">Razão cliente Proposta</param>
        /// <param name="statusOrdemSeparacao">Status de Ordem separação Proposta</param>
        public Proposta(Int64 codigoProposta, string numeroProposta, string dataLiberacaoProposta, int codigoCliente,
                        string razaoCliente, Int32 qtdVolumes, Int32 codPKMob, Boolean isInterrompido)
        {
            this.Codigo = codigoProposta;
            this.Numero = numeroProposta;
            this.DataLiberacao = dataLiberacaoProposta;
            this.CodigoCliente = codigoCliente;
            this.RazaoCliente = razaoCliente;
            this.Volumes = qtdVolumes;
            this.CodigoPikingMobile = codPKMob;
            this.IsInterrompido = isInterrompido;
        }

        /// <summary>
        /// Instância um Obj Proposta
        /// </summary>
        /// <param name="codigoProposta">Código da Proposta</param>
        /// <param name="numeroProposta">Número da proposta</param>
        /// <param name="dataLiberacaoProposta">Data liberação Proposta</param>
        /// <param name="codigoCliente">Código cliente Proposta</param>
        /// <param name="razaoCliente">Razão cliente Proposta</param>
        /// <param name="statusOrdemSeparacao">Status de Ordem separação Proposta</param>
        public Proposta(Int64 codigoProposta, string numeroProposta, string dataLiberacaoProposta, int codigoCliente,
                        string razaoCliente, Int32 qtdVolumes, Int32 codPKMob)
        {
            this.Codigo = codigoProposta;
            this.Numero = numeroProposta;
            this.DataLiberacao = dataLiberacaoProposta;
            this.CodigoCliente = codigoCliente;
            this.RazaoCliente = razaoCliente;
            this.Volumes = qtdVolumes;
            this.CodigoPikingMobile = codPKMob;
        }

        /// <summary>
        /// Instância um Obj Proposta
        /// </summary>
        /// <param name="codigoProposta">Código da Proposta</param>
        /// <param name="numeroProposta">Número da proposta</param>
        /// <param name="dataLiberacaoProposta">Data liberação Proposta</param>
        /// <param name="codigoCliente">Código cliente Proposta</param>
        /// <param name="razaoCliente">Razão cliente Proposta</param>
        /// <param name="statusOrdemSeparacao">Status de Ordem separação Proposta</param>
        /// <param name="totalItensProposta">Total de itens na Proposta</param>
        /// <param name="totalPecasProposta">Total de Pecas na Proposta</param>
        public Proposta(Int64 codigoProposta, string numeroProposta, string dataLiberacaoProposta, int codigoCliente,
                        string razaoCliente, Int32 qtdVolumes, Double totalItensProposta, Double totalPecasProposta)
        {
            this.Codigo = codigoProposta;
            this.Numero = numeroProposta;
            this.DataLiberacao = dataLiberacaoProposta;
            this.CodigoCliente = codigoCliente;
            this.RazaoCliente = razaoCliente;
            //this.Volumes = QtdVolumes;
            this.setTotalValoresProposta(totalItensProposta, totalPecasProposta, qtdVolumes);
        }

        /// <summary>
        /// Recebe outro obj do tipo Proposta e set os parâmetros para a nova instância a ser criada.
        /// </summary>
        /// <param name="obj"></param>
        public Proposta(Object obj, List<ProdutoProposta> listItens)
        {
            if (obj.GetType() == typeof(Proposta))
            {
                this.Codigo = ((Proposta)obj).Codigo;
                this.Numero = ((Proposta)obj).Numero;
                this.DataLiberacao = ((Proposta)obj).DataLiberacao;
                this.CodigoCliente = ((Proposta)obj).CodigoCliente;
                this.RazaoCliente = ((Proposta)obj).RazaoCliente;
                this.Volumes = ((Proposta)obj).Volumes;
                this.ListObjItemProposta = listItens;
                this.CodigoPikingMobile = ((Proposta)obj).CodigoPikingMobile;
            }
        }


        /// <summary>
        /// Instância um Obj Proposta recebendo um List do tipo ProdutoProposta
        /// </summary>
        /// <param name="codigoProposta">Código da Proposta</param>
        /// <param name="numeroProposta">Número da proposta</param>
        /// <param name="dataLiberacaoProposta">Data liberação Proposta</param>
        /// <param name="codigoCliente">Código cliente Proposta</param>
        /// <param name="razaoCliente">Razão cliente Proposta</param>
        /// <param name="statusOrdemSeparacao">Status de Ordem separação Proposta</param>
        /// <param name="listItemProposta">List de objetos do tipo ProdutoProposta</param>
        public Proposta(Int64 codigoProposta, string numeroProposta, string dataLiberacaoProposta, int codigoCliente,
                string razaoCliente, Int32 qtdVolumes, List<ProdutoProposta> listItemProposta)
        {
            this.Codigo = codigoProposta;
            this.Numero = numeroProposta;
            this.DataLiberacao = dataLiberacaoProposta;
            this.CodigoCliente = codigoCliente;
            this.RazaoCliente = razaoCliente;
            this.Volumes = qtdVolumesProposta;
            this.ListObjItemProposta = listItemProposta;

        }

        #endregion

        /// <summary>
        /// Status do pedido na tabela de picking Mobile
        /// </summary>
        public enum StatusLiberacao { NAOFINALIZADO = 0, TRABALHANDO = 1, FINALIZADO = 2 }

        #region  "GETS E SETS"

        public enum statusOrdemSeparacao
        {
            NAOIMPRESA = 0,
            IMPRESA = 1
        }

        public Int64 Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public string Numero
        {
            get { return numero; }
            set { numero = value; }
        }

        public string DataLiberacao
        {
            get { return dataLiberacao; }
            set { dataLiberacao = value; }
        }

        public Int32 Volumes
        {
            get { return qtdVolumesProposta; }
            set { qtdVolumesProposta = value; }
        }

        public int CodigoCliente
        {
            get { return codigoCliente; }
            set { codigoCliente = value; }
        }

        public string RazaoCliente
        {
            get { return razaoCliente; }
            set { razaoCliente = value; }
        }

        internal List<ProdutoProposta> ListObjItemProposta
        {
            get { return listItemProposta; }
            set { listItemProposta = value; }
        }

        public Double TotalItens
        {
            get { return totalItens; }
        }

        public Double Totalpecas
        {
            get { return totalpecas; }
        }

        /// <summary>
        /// Total de Itens baseado nos dados do atributo ListObjItemProposta
        /// </summary>
        /// 
        public void setTotalItens()
        {
            if (ListObjItemProposta != null)
            {
                totalItens = ListObjItemProposta.Count;
            }

        }

        /// <summary>
        /// Set a Quantidade de itens recebendo um Valor double como parâmetro.
        /// </summary>
        /// <param name="qtdItens"></param>
        public void setTotalItens(Double qtdItens)
        {
            if (qtdItens > 0)
            {
                totalItens = qtdItens;
            }

        }

        /// <summary>
        /// Total de Peças baseado nos dados do atributo ListObjItemProposta
        /// </summary>
        public void setTotalPecas()
        {
            Double totPecas = 0;

            foreach (ProdutoProposta item in ListObjItemProposta)
            {
                totPecas += item.Quantidade;
            }

            totalpecas = totPecas;
        }

        public void setTotalPecas(Double qtdPecas)
        {
            if (qtdPecas > 0)
            {
                totalpecas = qtdPecas;
            }
        }

        /// <summary>
        /// Calcula o total de peças e de itens de uma proposta baseado nas informações ListObjItemProposta
        /// </summary>
        public void totalItensPecasProposta()
        {
            this.setTotalItens();
            this.setTotalPecas();
        }

        public Int32 CodigoPikingMobile
        {
            get { return codigoPikingMobile; }
            set { codigoPikingMobile = value; }
        }

        public Boolean IsInterrompido
        {
            get { return isInterrompido; }
            set { isInterrompido = value; }
        }

        /// <summary>
        /// Calcula o total de peças e de itens de uma proposta  recebendo os valores como parâmetro.
        /// </summary>
        public void setTotalValoresProposta(Double totItens, Double totPecas, int volumes)
        {
            this.setTotalItens(totItens);
            this.setTotalPecas(totPecas);
            //this.Volumes = (volumes > 0) ? volumes : 1;
        }

        /// <summary>
        /// Calcula o total de peças e de itens de uma proposta  recebendo os valores como parâmetro.
        /// </summary>
        public void setTotalValoresProposta(Double totItens, Double totPecas)
        {
            this.setTotalItens(totItens);
            this.setTotalPecas(totPecas);
            //this.Volumes = (volumes > 0) ? volumes : 1;
        }


        #endregion

        #region"GENERAL METHODS"

        /// <summary>
        /// Limpa a list de itens da proposta e adiciona o item passado como parâmetro.
        /// </summary>
        /// <param name="item">Objeto ProdutoProposta</param>
        public void setNextItemProposta(ProdutoProposta item)
        {
            this.ListObjItemProposta.Clear();
            this.ListObjItemProposta.Add(item);
        }

        public override bool Equals(object obj)
        {
            System.Type type = obj.GetType();
            if (obj == null || (type != typeof(Proposta)))
            {
                return false;
            }
            else
            {
                return Codigo == ((Proposta)obj).Codigo;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return " Código : " + Codigo +
                   "\n Número : " + Numero +
                   "\n Data Liberação : " + DataLiberacao +
                   "\n Código Cliente : " + CodigoCliente +
                   "\n Razao Cliente : " + RazaoCliente +
                   "\n numeroVolumes : " + Volumes +
                   "\n Quantidade de Itens : " + ListObjItemProposta.Count();
        }

        public void decrementaVolume()
        {
            if ((this.Volumes > 0) && this.Volumes - 1 >= 0)
            {
                this.Volumes--;
            }
        }

        public void incrementaVolume()
        {
            this.Volumes++;
        }

        #endregion

    }
}
