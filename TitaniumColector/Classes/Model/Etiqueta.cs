using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using TitaniumColector.Classes.Dao;

namespace TitaniumColector.Classes
{
    class Etiqueta
    {
        private string partnumber;
        private string descricaoProduto;
        private Int64 ean13Embalagem;
        private String lote;
        private Int32 sequencia;
        private Double quantidade;
        private Int32 volumeEtiqueta;
        private DateTime dataHoraValidacao;
        private Tipo tipoEtiqueta;
        private DaoProduto daoProduto;

        public enum Tipo {INVALID=0,QRCODE=1,BARRAS=2 }

    #region "CONTRUTORES"

        public Etiqueta() { }

        public Etiqueta(String partnumber,String descricao,Int64 ean13,String lote,Int32 sequencia,Double quantidade,Tipo tipoEtiqueta) 
        {

            switch (tipoEtiqueta )
            {
                case Tipo.QRCODE:

                    PartnumberEtiqueta = partnumber;
                    DescricaoProdutoEtiqueta = descricao;
                    Ean13Etiqueta = ean13;
                    LoteEtiqueta = lote;
                    SequenciaEtiqueta = sequencia;
                    QuantidadeEtiqueta = quantidade;
                    DataHoraValidacao = DateTime.Now;
                    TipoEtiqueta = Tipo.QRCODE;
                    break;

                case Tipo.BARRAS:

                    PartnumberEtiqueta = partnumber;
                    DescricaoProdutoEtiqueta = descricao;
                    Ean13Etiqueta = ean13;
                    LoteEtiqueta = lote;
                    SequenciaEtiqueta = 0;
                    QuantidadeEtiqueta = quantidade;
                    DataHoraValidacao = DateTime.Now;
                    TipoEtiqueta = Tipo.BARRAS;
                    break;

                default:
                    break;
            }

        }

        /// <summary>
        /// Contrutor recebe um array com os valores a serem passados para cada atributo do objEtiqueta;
        /// </summary>
        /// <param name="arrayEtiqueta">Array no seguite formato 
        ///                             "EAN13:?|LOTE:?|SEQ:?|QTD:?"
        /// </param>
        public Etiqueta(Array arrayEtiqueta,Tipo tipoEtiqueta) 
        {
            try
            {

                switch (tipoEtiqueta)
                {
                    case Tipo.QRCODE:

                        foreach (string item in arrayEtiqueta)
                        {
                            string strItem = item.Substring(0, item.IndexOf(":", 0));

                            if (strItem == "PNUMBER")
                            {
                                PartnumberEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                            }
                            else if (strItem == "DESCRICAO")
                            {
                                DescricaoProdutoEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                            }
                            else if (strItem == "EAN13")
                            {
                                Ean13Etiqueta = Convert.ToInt64(item.Substring(item.IndexOf(":", 0) + 1));
                            }
                            else if (strItem == "LOTE")
                            {
                                LoteEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                            }
                            else if (strItem == "SEQ")
                            {
                                SequenciaEtiqueta = Convert.ToInt32(item.Substring(item.IndexOf(":", 0) + 1));
                            }
                            else if (strItem == "QTD")
                            {
                                QuantidadeEtiqueta = Convert.ToDouble(item.Substring(item.IndexOf(":", 0) + 1));
                            }
                        }

                        break;

                    case Tipo.BARRAS:

                        foreach (string item in arrayEtiqueta)
                        {
                            daoProduto = new DaoProduto();

                            this.TipoEtiqueta = Tipo.BARRAS;
                            Ean13Etiqueta = Convert.ToInt64(item);
                            Etiqueta aux = daoProduto.recuperarInformacoesPorEan13Etiqueta(this);

                            if(aux !=null)
                            {
                                DescricaoProdutoEtiqueta = aux.DescricaoProdutoEtiqueta;
                                PartnumberEtiqueta = aux.PartnumberEtiqueta;
                                Ean13Etiqueta = aux.Ean13Etiqueta;
                                LoteEtiqueta = aux.LoteEtiqueta;
                                SequenciaEtiqueta = aux.SequenciaEtiqueta;
                                QuantidadeEtiqueta = aux.QuantidadeEtiqueta;
                            }
                        }

                        break;

                    default:
                        MainConfig.errorMessage("Tipo de Etiqueta indefinido!!", "Leitura Etiquetas");
                        break;
                }

                DataHoraValidacao = DateTime.Now;
                this.TipoEtiqueta = tipoEtiqueta;

            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }

    #endregion

    #region "GET E SETS "

        public Int64 Ean13Etiqueta
        {
            get { return ean13Embalagem; }
            set { ean13Embalagem = value; }
        }

        public String LoteEtiqueta
        {
            get { return lote; }
            set { lote = value; }
        }

        public Int32 SequenciaEtiqueta
        {
            get { return sequencia; }
            set { sequencia = value; }
        }

        public Double QuantidadeEtiqueta
        {
            get 
            {
                return quantidade; 
            }
            set 
            { 
                quantidade = value; 
            }
        }

        public string PartnumberEtiqueta
        {
            get { return partnumber; }
            set { partnumber = value; }
        }

        public string DescricaoProdutoEtiqueta
        {
            get { return descricaoProduto; }
            set { descricaoProduto = value; }
        }

        public Int32 VolumeEtiqueta
        {
            get { return volumeEtiqueta; }
            set { volumeEtiqueta = value; }
        }

        public DateTime DataHoraValidacao
        {
            get { return dataHoraValidacao; }
            set { dataHoraValidacao = value; }
        }

        public Tipo TipoEtiqueta
        {
            get { return tipoEtiqueta; }
            set { tipoEtiqueta = value; }
        }


    #endregion

    #region "METODOS"
        ///<summary>
        /// Recebe um array de strings referentes aos atributos do obj Etiqueta.
        /// retorna Um objeto do tipo Etiqueta
        /// </summary>
        /// <param name="array">Array de String referentes aos atributos de uma etiqueta</param>
        public static Etiqueta arrayToEtiqueta(Array array)
        {
            Etiqueta objEtiqueta = new Etiqueta();

            foreach (string item in array)
            {
                string strItem = item.Substring(0, item.IndexOf(":", 0));

                if (strItem == "PNUMBER")
                {
                    objEtiqueta.PartnumberEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                }
                else if (strItem == "DESCRICAO")
                {
                    objEtiqueta.DescricaoProdutoEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                }
                else if (strItem == "EAN13")
                {
                    objEtiqueta.Ean13Etiqueta = Convert.ToInt64(item.Substring(item.IndexOf(":", 0) + 1));
                }
                else if (strItem == "LOTE")
                {
                    objEtiqueta.LoteEtiqueta = item.Substring(item.IndexOf(":", 0) + 1);
                }
                else if (strItem == "SEQ")
                {
                    objEtiqueta.SequenciaEtiqueta = Convert.ToInt32(item.Substring(item.IndexOf(":", 0) + 1));
                }
                else if (strItem == "QTD")
                {
                    objEtiqueta.QuantidadeEtiqueta = Convert.ToDouble(item.Substring(item.IndexOf(":", 0) + 1));
                }
            }
            return objEtiqueta;
        }
        
        /// <summary>
        /// Verifica se já existe um determinado Objeto Etiqueta em um list.
        /// </summary>
        /// <param name="etiqueta">Obj etiqueta que será buscado</param>
        /// <param name="ListEtiquetas">list do tipo Etiqueta onde derá feita a varredura</param>
        /// <returns>TRUE(Se o objeto existe na list)
        ///          FALSE (Se o objeto não existe na list)</returns>
        public static bool validarEtiqueta(Etiqueta etiqueta,List<Etiqueta> listEtiquetas) 
        {
            switch (etiqueta.TipoEtiqueta )
            {
                case Tipo.QRCODE:

                    foreach (Etiqueta itemList in listEtiquetas.ToList<Etiqueta>())
                    {

                        if (itemList.Equals(etiqueta))
                        {
                            return true;
                        }
                    }
                    return false;

                case Tipo.BARRAS:

                    foreach (Etiqueta itemList in listEtiquetas.ToList<Etiqueta>())
                    {
                        if (etiqueta.Equals(itemList))
                        {
                            return true;
                        }
                    }
                    return false;

                default:
                    return false;

            }
             
        }

        /// <summary>
        /// Verifica se a Etiqueta já foi lida.
        /// </summary>
        /// <returns>FALSE --> se a etiqueta for encontrada na list
        ///          TRUE --> se a etiqueta ainda não foii lida.
        /// </returns>
        public static bool validaEtiquetaNaoLida(Etiqueta objEtiqueta, List<Etiqueta> listEtiquetas)
        {
            //Verifica se o List foi iniciado
            if (listEtiquetas != null)
            {
                if (listEtiquetas.Count == 0)
                {
                    return true;
                }
                else
                {
                    //Verifica se a etiqueta está na lista de etiquetas lidas.
                    if (validarEtiqueta(objEtiqueta, listEtiquetas))
                    {
                        //Caso esteja na lista
                        return false;
                    }
                    else
                    {
                        //caso não esteja na lista.
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Monta Xml para detalhar as estiquetas equivalentes ao item passado como parâmetro
        /// </summary>
        /// <param name="listaEtiquetas">Lista de Etiquetas dos item que foi separado</param>
        /// <returns>String no formato de Xml</returns>
        /// <remarks> Na forma em que etá o código abaixo o trabalho é feito em uma lista que contém informações de apenas um item liberado
        ///           --Para se trabalhar com uma lista que possua informações de mais de um item é nescessário alterção do código ou 
        ///           criação de outro método mais  apropriado.
        /// </remarks>
        public static String gerarXmlItensEtiquetas(List<Etiqueta> listaEtiquetas)
        {
            String result = "";
            try
            {
                System.IO.StringWriter str = new System.IO.StringWriter();

                //Variável que irá receber o Xml na forma de String.
                XmlTextWriter writer = new XmlTextWriter(str);

                //inicia o documento xml
                writer.WriteStartDocument();

                //define a indentação do arquivo
                writer.Formatting = Formatting.Indented;

                //escreve o elemento raiz
                writer.WriteStartElement("Item");
                //escrever o atributo para o Elemento Raiz Item
                writer.WriteAttributeString("Ean", listaEtiquetas[0].Ean13Etiqueta.ToString());

                foreach (var item in listaEtiquetas)
                {
                    //Elemento Raiz Seq
                    writer.WriteStartElement("Seq");
                    //Escreve atributos IdEtiqueta
                    writer.WriteAttributeString("ID", item.SequenciaEtiqueta.ToString());
                    //Escreve atributos TIPO Etiqueta
                    writer.WriteAttributeString("TIPO", item.TipoEtiqueta.ToString());
                    //Escreve elemento entre a tag Seq
                    writer.WriteElementString("Qtd", item.QuantidadeEtiqueta.ToString());
                    writer.WriteElementString("Vol", item.VolumeEtiqueta.ToString());
                    writer.WriteElementString("Time",item.DataHoraValidacao.ToString());
                    writer.WriteElementString("Usuario", MainConfig.CodigoUsuarioLogado.ToString());
                    //Encerra o elemento Seq
                    writer.WriteEndElement();
                }

                //Encerra o elemento Item
                writer.WriteEndDocument();

                // O resultado é uma string.
               return result = str.ToString();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public override bool Equals(object obj)
        {
            System.Type type = obj.GetType();

            if (obj == null || (type != typeof(Etiqueta)))
            {
                return false;
            }
            else
            {
                switch (((Etiqueta)obj).TipoEtiqueta)
                {
                    case Tipo.QRCODE:

                        return (Ean13Etiqueta == ((Etiqueta)obj).Ean13Etiqueta && SequenciaEtiqueta == ((Etiqueta)obj).SequenciaEtiqueta);

                    case Tipo.BARRAS:

                        return (Ean13Etiqueta == ((Etiqueta)obj).Ean13Etiqueta );

                    default:
                        return false;
                }
            }

        }

        public override string ToString()
        {
            return String.Format("PNUMBER:{0}|DESCRICAO:{1}|EAN13:{2}|LOTE:{3}|SEQ:{4}|QTD:{5}",PartnumberEtiqueta,DescricaoProdutoEtiqueta, Ean13Etiqueta, LoteEtiqueta, SequenciaEtiqueta, QuantidadeEtiqueta);
        }

        /// <summary>
        /// Detalha todas as informações sobre a etiqueta.
        /// </summary>
        /// <returns>texto com informações sobre a etiqueta.</returns>
        public String etiquetaFullDetail() 
        {
            return String.Format("PartNumber:{0}|Descrição:{1}|Ean13:{2}|Lote:{3}|Sêquencia:{4}|Quantidade:{5}|Volumes:{6}|HoraValidação:{7}|TipoEtiqueta:{8}",
                                  PartnumberEtiqueta, DescricaoProdutoEtiqueta, Ean13Etiqueta, LoteEtiqueta, SequenciaEtiqueta, QuantidadeEtiqueta, VolumeEtiqueta, DataHoraValidacao, TipoEtiqueta);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    #endregion

    }
}
