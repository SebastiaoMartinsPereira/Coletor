using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TitaniumColector.Utility;
using TitaniumColector.Classes.Exceptions;
using System.Xml;
using System.Xml.Linq;
using TitaniumColector.Classes.Model;

namespace TitaniumColector.Classes.Procedimentos
{
     static class ProcedimentosLiberacao
    {
        private static Double totalItens;
        private static Double totalPecas;
        private static Double qtdPecasItem;
        private static Int32 totalVolumes;
        private static Int32 proximaEtiqueta;
        private static List<Etiqueta> listEtiquetasLidas;
        private static List<Etiqueta> listEtiquetas;
        private static List<EmbalagemSeparacao> listEmbalagensSeparacao;
        private static Array arrayStringToEtiqueta;
        private static String mensagem;

        /// <summary>
        /// Carrega as a serem trabalhadas durante o procedimento de liiberção dos itens da proposta.
        /// </summary>
        /// <param name="tItens">Total de Itens da Proposta</param>
        /// <param name="tPecas">Total de peças na Proposta</param>
        /// <param name="pecasItens">Quantidade de pecas do item a ser trabalhado.</param>
        public static void inicializarProcedimentos(Double tItens, Double tPecas, Double pecasItens)
        {
            TotalItens = tItens;
            TotalPecas = tPecas;
            qtdPecasItem = pecasItens;

            if (ListEtiquetasGeradas != null)
            {
                ListEtiquetasGeradas.Clear();

            }
            else 
            {
                listEtiquetasLidas = new List<Etiqueta>();
            }
            
            ProximaEtiqueta = 0;

            if(ListEtiquetasGeradas != null)
            {
                ListEtiquetasGeradas.Clear();
            }

            if (arrayStringToEtiqueta != null)
            {
                arrayStringToEtiqueta = null;
            }
        }

        public static void inicializarProcedimentos(Double tItens, Double tPecas, Double pecasItens,Int32 qtdVolumes)
        {
            TotalItens = tItens;
            TotalPecas = tPecas;
            qtdPecasItem = pecasItens;
            TotalVolumes = qtdVolumes;

            if (ListEtiquetasGeradas != null)
            {
                ListEtiquetasGeradas.Clear();

            }
            else
            {
                listEtiquetasLidas = new List<Etiqueta>();
            }

            ProximaEtiqueta = 0;

            if (ListEtiquetasGeradas != null)
            {
                ListEtiquetasGeradas.Clear();
            }

            if (arrayStringToEtiqueta != null)
            {
                arrayStringToEtiqueta = null;
            }
        }

        /// <summary>
         /// Não altera o total de peças e o total de itens atualmente setados.
         /// </summary>
         /// <param name="pecasItens">Quantidade de peças do item a ser trabalhado.</param>
        public static void inicializarProcedimentosProximoItem(Double pecasItens)
        {
            TotalItens = TotalItens;
            TotalPecas = TotalPecas;
            qtdPecasItem = pecasItens;

            if (listEtiquetasLidas != null)
            {
                listEtiquetasLidas.Clear();
            }
            else
            {
                listEtiquetasLidas = new List<Etiqueta>();
            }

            ProximaEtiqueta = 0;

            if (ListEtiquetasGeradas != null)
            {
                ListEtiquetasGeradas.Clear();
            }

            if (arrayStringToEtiqueta != null)
            {
                arrayStringToEtiqueta = null;
            }
        }

    #region "GETS E SETS"

        public static Double TotalPecas
        {
            get { return totalPecas; }
            set { totalPecas = value; }
        }

        public static Double QtdPecasItem
        {
            get { return qtdPecasItem; }
 
        }

        public static Int32 TotalVolumes
        {
            get { return ProcedimentosLiberacao.totalVolumes; }
            set { ProcedimentosLiberacao.totalVolumes = value; }
        }

        public static Double subtrairQtdPecasItem(Double value)
        {
            if (QtdPecasItem - value>= 0)
            {
                return qtdPecasItem -= value;
            }
            else 
            {
                throw new QuantidadeInvalidaException(String.Format ("O Valor informado é maior que a Quantidade de peças existentes."));
            }
        }

        public static void somarQtdPecasItem(Double value)
        {
            totalPecas = value;
        }

        public static Int32 ProximaEtiqueta
        {
            get { return proximaEtiqueta; }
            set { proximaEtiqueta = value; }
        }

        public static Double TotalItens
        {
            get { return totalItens; }
            set { totalItens = value; }
        }

        internal static List<Etiqueta> EtiquetasLidas
        {
            get { return listEtiquetasLidas; }
            set { listEtiquetasLidas = value; }
        }
      
        internal static List<Etiqueta> ListEtiquetasGeradas
        {
          get { return listEtiquetas; }
          set { listEtiquetas = value; }
        }

        public static Array ArrayStringToEtiqueta
        {
            get { return ProcedimentosLiberacao.arrayStringToEtiqueta; }
            set { ProcedimentosLiberacao.arrayStringToEtiqueta = value; }
        }

        internal static List<EmbalagemSeparacao> ListEmbalagensSeparacao
        {
            get { return ProcedimentosLiberacao.listEmbalagensSeparacao; }
            set { ProcedimentosLiberacao.listEmbalagensSeparacao = value; }
        }

    #endregion

        /// <summary>
        /// Verifica se a Etiqueta já foi lida.
        /// </summary>
        /// <returns>FALSE --> se a etiqueta for encontrada na list
        ///          TRUE --> se a etiqueta ainda não foii lida.
        /// </returns>
        public static bool validaEtiquetaNaoLida(Etiqueta objEtiqueta)
        {
            //Verifica se o List foi iniciado

            switch (objEtiqueta.TipoEtiqueta )
            {
                case Etiqueta.Tipo.INVALID:

                    return false;

                case Etiqueta.Tipo.QRCODE:

                    if (EtiquetasLidas != null)
                    {
                        if (EtiquetasLidas.Count == 0)
                        {
                            return true;
                        }
                        else
                        {
                            //Verifica se a etiqueta está na lista de etiquetas lidas.
                            if (Etiqueta.validarEtiqueta(objEtiqueta, EtiquetasLidas))
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


                case Etiqueta.Tipo.BARRAS:

                    return true;

                default :
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
                    if (Etiqueta.validarEtiqueta(objEtiqueta, listEtiquetas))
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
        /// 
        /// </summary>
        /// <param name="produto"></param>
        /// <param name="tbProduto"></param>
        /// <param name="tblote"></param>
        /// <param name="tbSequencia"></param>
        /// <param name="tbQuantidade"></param>
        /// <param name="tbMensagem"></param>
        public static void lerEtiqueta(ProdutoProposta produto,TextBox tbProduto,TextBox tblote,TextBox tbSequencia,TextBox tbQuantidade,TextBox tbMensagem)
        {
            tbMensagem.Text = "";

            if (ProximaEtiqueta <= ListEtiquetasGeradas.Count - 1)
            {
                ArrayStringToEtiqueta = FileUtility.arrayOfTextFile(ListEtiquetasGeradas[ProximaEtiqueta].ToString(), FileUtility.splitType.PIPE);
                Etiqueta objEtiqueta = new Etiqueta();
                objEtiqueta = Etiqueta.arrayToEtiqueta(ArrayStringToEtiqueta);
                efetuaLeituraEtiqueta(produto,tbProduto,tblote,tbSequencia,tbQuantidade,tbMensagem,objEtiqueta);
                
            }
            else
            {
                tbMensagem.Text = "Próximo Item.";
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="produto"></param>
        /// <param name="tbProduto"></param>
        /// <param name="tblote"></param>
        /// <param name="tbSequencia"></param>
        /// <param name="tbQuantidade"></param>
        /// <param name="tbMensagem"></param>
        public static void lerEtiqueta(String inputValue,ProdutoProposta produto, TextBox tbProduto, TextBox tblote, TextBox tbSequencia, TextBox tbQuantidade, TextBox tbMensagem)
        {
            tbMensagem.Text = "";

            ArrayStringToEtiqueta = FileUtility.arrayOfTextFile(inputValue, FileUtility.splitType.PIPE);
            Etiqueta objEtiqueta = new Etiqueta();
            objEtiqueta = new Etiqueta(arrayStringToEtiqueta,Etiqueta.Tipo.QRCODE);
            efetuaLeituraEtiqueta(produto, tbProduto, tblote, tbSequencia, tbQuantidade, tbMensagem, objEtiqueta);
        }
      
        /// <summary>
        /// Valida as informações lidas pelo coletor e as transformam em um objeto do tipo Etiqueta e continua o procedimento de leitura da etiqueta.
        /// </summary>
        /// <param name="inputValue">Valor fornecido pelo coletor de dados</param>
        /// <param name="tipoEtiqueta">Tipo de etiqueta lida  (EAN13) OU (QRCOODE)</param>
        /// <param name="produto">produto a ser validado durante processo de liberação do item</param>
        /// <param name="tbProduto">Campo para informações ao usuário</param>
        /// <param name="tblote">Campo para informações ao usuário</param>
        /// <param name="tbSequencia">Campo para informações ao usuário</param>
        /// <param name="tbQuantidade">Campo para informações ao usuário</param>
        /// <param name="tbMensagem">Campo para informações ao usuário</param>
        public static void lerEtiqueta(String inputValue,Etiqueta.Tipo tipoEtiqueta, ProdutoProposta produto, TextBox tbProduto, TextBox tblote, TextBox tbSequencia, TextBox tbQuantidade, TextBox tbMensagem)
        {
            tbMensagem.Text = "";

            ArrayStringToEtiqueta = FileUtility.arrayOfTextFile(inputValue, FileUtility.splitType.PIPE);
            Etiqueta objEtiqueta = new Etiqueta(arrayStringToEtiqueta, tipoEtiqueta);
            efetuaLeituraEtiqueta(produto, tbProduto, tblote, tbSequencia, tbQuantidade, tbMensagem, objEtiqueta);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="produto"></param>
        /// <param name="tbProduto"></param>
        /// <param name="tblote"></param>
        /// <param name="tbSequencia"></param>
        /// <param name="tbQuantidade"></param>
        /// <param name="tbMensagem"></param>
        /// <param name="arrayEtiqueta"></param>
        public static void lerEtiqueta(ProdutoProposta produto,TextBox tbProduto, TextBox tblote, TextBox tbSequencia, TextBox tbQuantidade, TextBox tbMensagem, Array arrayEtiqueta)
        {
            tbMensagem.Text = "";

            if (ProximaEtiqueta  <= ListEtiquetasGeradas.Count - 1)
            {
                arrayEtiqueta = FileUtility.arrayOfTextFile(ListEtiquetasGeradas[ProximaEtiqueta].ToString(), FileUtility.splitType.PIPE);
                Etiqueta objEtiqueta = new Etiqueta();
                objEtiqueta = Etiqueta.arrayToEtiqueta(arrayEtiqueta);
                efetuaLeituraEtiqueta(produto,tbProduto, tblote, tbSequencia, tbQuantidade, tbMensagem, objEtiqueta);
            }
            else
            {
                tbMensagem.Text = "Próximo Item.";
            }
        }
         
         /// <summary>
        /// realiza todo o procedimento para validação de tipo de etiqueta,etiqueta, produto,
        /// validação de sequência,subtração de quantidade de itens registrados na etiqueta.
        /// </summary>
        /// <param name="produto">Produto a ser validado</param>
        /// <param name="tbProduto">Text Box Nome do Produto</param>
        /// <param name="tbLote">Text Box Lote</param>
        /// <param name="tbSequencia">Text Box  Sequencia</param>
        /// <param name="tbQuantidade">Text Box Quantidade</param>
        /// <param name="tbMensagem">Text Box Mensagem</param>
        /// <param name="objEtiqueta">Objeto Etiqueta.</param>
        public static void efetuaLeituraEtiqueta(ProdutoProposta produto,TextBox tbProduto,TextBox tbLote,TextBox tbSequencia,TextBox tbQuantidade,
                                                 TextBox tbMensagem,Etiqueta objEtiqueta)
        {
            try
            {
                if (comparaProdutoEtiquetaProdutoTrabalhado(produto, objEtiqueta))
                {
                    if (validaEtiquetaNaoLida(objEtiqueta))
                    {
                        if (QtdPecasItem > 0)
                        {
                            tbProduto.Text = objEtiqueta.PartnumberEtiqueta.ToString() + " - " + objEtiqueta.DescricaoProdutoEtiqueta.ToString();
                            tbLote.Text = objEtiqueta.LoteEtiqueta;
                            tbSequencia.Text = objEtiqueta.SequenciaEtiqueta.ToString();
                            tbQuantidade.Text = (subtrairQtdPecasItem(objEtiqueta.QuantidadeEtiqueta)).ToString();
                            objEtiqueta.VolumeEtiqueta = ProcedimentosLiberacao.totalVolumes;
                            addToListEtiquetasLidas(objEtiqueta);
                        }
                    }
                    else
                    {
                        tbMensagem.Text = String.Format("A etiqueta {0} já foi validada.", objEtiqueta.SequenciaEtiqueta);
                        ProximaEtiqueta += 1;
                    }
                }
                else
                {
                    if (ProcedimentosLiberacao.mensagem != "" || ProcedimentosLiberacao.mensagem != null)
                    {
                        tbMensagem.Text = ProcedimentosLiberacao.mensagem;
                    }
                    else 
                    {
                        tbMensagem.Text = String.Format("Produto da etiqueta lida não confere com o item a ser liberado.");
                    }
                    
                    ProximaEtiqueta += 1;
                }

            }
            catch ( QuantidadeInvalidaException qtdEx)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Quantidade de peças na embalagem é maior que a Quantidade a ser liberada!");
                sb.AppendFormat("error : {0} ", qtdEx.Message);
                MainConfig.errorMessage(sb.ToString(),"Etiqueta Inválida!!");
            }
            catch (Exception ex)
            {
                
                throw ex;
            }   
        }

        /// <summary>
         /// Verifica se  Produto trabalhado e Produto Etiqueta são os mesmos.
         /// </summary>
         /// <param name="propostaProduto">Obj Produto que será verificado </param>
         /// <param name="etiquetaLida"> Obj Etiqueta que será verificado </param>
         /// <returns>True --> Caso sejam iguais.</returns>
         /// 
        public static bool comparaProdutoEtiquetaProdutoTrabalhado(ProdutoProposta produtoProposta, Etiqueta etiquetaLida)
        {
            //Verifica se os produtos são iguais
            switch (etiquetaLida.TipoEtiqueta)
            {
                case Etiqueta.Tipo.QRCODE:

                    if (produtoProposta.Partnumber.Equals(etiquetaLida.PartnumberEtiqueta))
                    {
                         if(produtoProposta.Embalagens.Count==0){
                         
                         }


                        foreach (var item in produtoProposta.Embalagens)
                        {
                            
                            if ((etiquetaLida.Ean13Etiqueta.ToString() == item.Ean13Embalagem))
                            {
                                if (etiquetaLida.QuantidadeEtiqueta == item.Quantidade)
                                {
                                    return true;
                                }
                                else 
                                {
                                    mensagem = "Quantidade da etiqueta não confere com a Quantidade de itens da embalagem do produto.";
                                }
                            }
                            else 
                            {
                                mensagem = "Ean informado na Etiqueta não confere com o Ean do produto";
                            }
                        }
                    }
                    else 
                    {
                        mensagem = "Partnumber produto etiqueta não confere com partnumber do item da proposta";
                    }

                    break;

                case Etiqueta.Tipo.BARRAS:

                    if(produtoProposta.Partnumber.Equals(etiquetaLida.PartnumberEtiqueta))
                    {
                        foreach (var item in produtoProposta.Embalagens)
                        {
                            if ((etiquetaLida.Ean13Etiqueta.ToString() == item.Ean13Embalagem))
                            {
                                return true;
                            }
                            else
                            {
                                mensagem = "Ean informado na Etiqueta não confere com o Ean do produto";
                            }
                        }
                    }
                    else
                    {
                        mensagem = "Partnumber produto etiqueta não confere com partnumber do item da proposta";
                    }
                    
                    break;

                case Etiqueta.Tipo.INVALID:

                    break;

                default:

                    break;
            }


            return false;
        }

        /// <summary>
         /// Adiciona uma atiqueta a List Etiquetas Lidas.
         /// </summary>
         /// <param name="etiquetaLida"></param>
        public static void addToListEtiquetasLidas(Etiqueta etiquetaLida)
        {
            EtiquetasLidas.Add(etiquetaLida);
        }

        /// <summary>
        /// Limpa a list de Etiquetas Lidas.
        /// </summary>
        public static void clearListEtiquetasLidas() 
        {
            EtiquetasLidas.Clear();
            EtiquetasLidas = null;
            EtiquetasLidas = new List<Etiqueta>();
        }

        ///<summary>
         ///Altera o Valor do atributo Auxiliar que armazena informações sobre a Quantidade de Pecas
         ///</summary>
         ///<param name="qtd">Quantidade a ser diminuida</param>
         ///<returns>Retorna true caso não ocorra erros
         ///         false se o calculo não ocorrer com esperado.</returns>
        public static Boolean decrementaQtdTotalPecas(double qtd)
        {
            try
            {
                if (ProcedimentosLiberacao.TotalPecas > 0 && (ProcedimentosLiberacao.TotalPecas - qtd >= 0))
                {
                    ProcedimentosLiberacao.TotalPecas -= qtd;
                    return true;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw new QuantidadeInvalidaException();
            }
        }

        /// <summary>
        /// Altera o Valor do atributo Auxiliar que armazena informações sobre a Quantidade de Itens
        /// </summary>
        /// <param name="qtd">Quantidade a ser diminuida</param>
        /// <returns>Retorna true caso não ocorra erros
        ///          false se o calculo não ocorrer com esperado.</returns>
        public static Boolean decrementaQtdTotalItens(double qtd )
        {
            try
            {
                if (ProcedimentosLiberacao.TotalItens > 0 && (ProcedimentosLiberacao.TotalItens - qtd >= 0))
                {
                    ProcedimentosLiberacao.TotalItens -= qtd;
                    return true;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw new QuantidadeInvalidaException();
            }
        }

        /// <summary>
        /// Altera o status do produto para separado.
        /// </summary>
        /// <param name="item">item que tera o seu status alterado.</param>
        public static void setStatusProdutoParaSeparado(ProdutoProposta item)
        {

            if (item.StatusSeparado == ProdutoProposta.statusSeparado.SEPARADO)
            {
                return;
            }
            else
            {
                item.alteraStatusSeparado();
            }    

        }

        /// <summary>
        /// Valida o tipo de etiqueta lido
        /// </summary>
        /// <param name="inputValue">informação capturada pelo leitor</param>
        /// <returns>Etiqueta.tipo (EAN13,QRCODE,INVALID)</returns>
        public static Etiqueta.Tipo validaInputValueEtiqueta(String inputValue)
         {
            Etiqueta.Tipo tipoEtiqueta;

            int inputLength = inputValue.Length;

            if(inputLength==13)
            {
                tipoEtiqueta = Etiqueta.Tipo.BARRAS;
            }
            else if (inputLength > 13)
            {
                if (inputValue.Contains("PNUMBER:"))
                {
                    if (inputValue.Contains("DESCRICAO:"))
                    {
                        if (inputValue.Contains("EAN13:"))
                        {
                            if (inputValue.Contains("LOTE:"))
                            {
                                if (inputValue.Contains("SEQ:"))
                                {
                                    if (inputValue.Contains("QTD:"))
                                    {
                                        tipoEtiqueta = Etiqueta.Tipo.QRCODE;
                                    }
                                    else
                                    {
                                        tipoEtiqueta = Etiqueta.Tipo.INVALID;
                                    }
                                }
                                else
                                {
                                    tipoEtiqueta = Etiqueta.Tipo.INVALID;
                                }
                            }
                            else
                            {
                                tipoEtiqueta = Etiqueta.Tipo.INVALID;
                            }
                        }
                        else
                        {
                            tipoEtiqueta = Etiqueta.Tipo.INVALID;
                        }
                    }
                    else
                    {
                        tipoEtiqueta = Etiqueta.Tipo.INVALID;
                    }
                }
                else
                {
                    tipoEtiqueta = Etiqueta.Tipo.INVALID;
                }

            }
            else 
            {
                tipoEtiqueta = Etiqueta.Tipo.INVALID;
            }
        
            return tipoEtiqueta;
        }

        /// <summary>
        /// Valida a Quantidade de volumes existentes e decrementa 1 se caso for possível.
        /// </summary>
        /// <returns>String com a Quantidade restante, ou uma mensagem informando que não foi possível realizar a alteração.</returns>
        public static String decrementaVolume() 
        {
            if (TotalVolumes > 1)
            {
                --TotalVolumes;
                return TotalVolumes.ToString();
            }
            return "Qtd Volumes não pode ser menor que 1.";
        }

        /// <summary>
        /// Valida a Quantidade de volumes existentes e decrementa 1 se caso for possível.
        /// </summary>
        /// <returns>String com a Quantidade restante, ou uma mensagem informando que não foi possível realizar a alteração.</returns>
        public static String decrementaVolume(int valor)
        {
            if (TotalVolumes > valor)
            {
                TotalVolumes -= valor;
                return TotalVolumes.ToString();
            }
            return "Qtd Volumes não pode ser menor que 1.";
        }

        /// <summary>
         /// Encrementa mais 1 a Quantidade de volumes atual.
         /// </summary>
         /// <returns>String com a Quantidade de volumes após a alteração
         /// </returns>
        public static String incrementaVolume() 
        {
            ++TotalVolumes;
            return TotalVolumes.ToString();
        }

        /// <summary>
        /// Encrementa mais 1 a Quantidade de volumes atual.
        /// </summary>
        /// <returns>String com a Quantidade de volumes após a alteração
        /// </returns>
        public static String incrementaVolume(int valor)
        {
            String teste = TotalVolumes.ToString();
            TotalVolumes += valor;
            return TotalVolumes.ToString();
        }

        public static void interromperLiberacao(Proposta proposta)
        {
            if (!proposta.IsInterrompido) 
            {
                proposta.IsInterrompido = true;
            }
        }

        public static void continuarLiberacao(Proposta proposta)
        {
            if (proposta.IsInterrompido)
            {
                proposta.IsInterrompido = false;
            }
        }

        public static EmbalagemSeparacao retornaEmbalagem(int codigo) 
        {
            foreach (var item in ListEmbalagensSeparacao)
            {
                if (item.Codigo == codigo) 
                {
                    return item;
                }
            }

            return null;
        }

         /// <summary>
         /// Retorna a Embalagem Setada Como padrão.
         /// </summary>
         /// <returns>Objeto Embalagem Separação setado como padrão.</returns>
        public static EmbalagemSeparacao retornaEmbalagemPadrao()
        {
            try
            {
                foreach (var item in ListEmbalagensSeparacao)
                {
                    if (item.isPadrao())
                    {
                        return item;
                    }
                }

                throw new InvalidOperationException("Não foi encontrada uma embalagem padrão!\n");
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("Problemas durante inicialização de volumes!\n" + ex.Message + "\n", ex);
            }
            catch (Exception)
            {
                throw new Exception("Problemas durante Procedimentos de Liberação!!\n");
            }         
        }

        /// <summary>
        /// Inicializa a quantidade de embalagem como 1 para a embalagem que está setada como padrão
        /// </summary>
        /// <exception cref="InvalidOperationException"> Caso não exista uma embalagem padrão setada um erro será mostrado ao usuário.</exception>
        public static void setarVolumeInicial() 
        {
            try 
            {
	            foreach (var item in ListEmbalagensSeparacao)
                {
                    if (item.isPadrao())
                    {
                         item.inicializaQtdEmbalagem();
                         setTotalVolumes();
                         return;
                    }
                }

                throw new InvalidOperationException("Não foi encontrada uma embalagem padrão!\n");
            }
            catch(InvalidOperationException ex)
            {
                throw new Exception("Problemas durante inicialização de volumes!\n" + ex.Message + "\n", ex);
            }
            catch (Exception)
            {
	            throw new Exception("Problemas durante Procedimentos de Liberação!!\n");
            }         
        }

        public static void decrementaQtdEmbalagem(int codigoEmbalagem)
        {
            try
            {
                foreach (var item in ListEmbalagensSeparacao)
                {
                    if (item.Codigo == codigoEmbalagem)
                    {
                        item.remover();
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                MainConfig.errorMessage(ex.Message,"Gerenciar Volumes");
            }   
        }

        public static void incrementaQtdEmbalagem(int codigoEmbalagem)
        {
            foreach (var item in ListEmbalagensSeparacao)
            {
                if (item.Codigo == codigoEmbalagem)
                {
                     item.adicionar();
                     return;
                }
            }

        }

        public static int setTotalVolumes() 
        {
            foreach (var item in ProcedimentosLiberacao.ListEmbalagensSeparacao)
            {
                ProcedimentosLiberacao.TotalVolumes += item.Quantidade;
            }

            return TotalVolumes;
        }

        #region "Descontinuado"

            ///// <summary>
            // /// Gera etiquetas para testes.
            // /// </summary>
            //public static void gerarEtiquetas()
            //{
            //    Etiqueta objEtiqueta;
            //    List<Etiqueta> list = new List<Etiqueta>();

            //    ProximaEtiqueta = 0;
            //    list.Add(new Etiqueta("8031", "Chicote Soquete luz", 7895479042576, "LT-10051", Convert.ToInt32("12349"), 25));
            //    for (int i = 1; i <= 8; i++)
            //    {
            //        if (i < 5)
            //        {
            //            objEtiqueta = new Etiqueta("8031", "Chicote Soquete luz", 7895479042575, "LT-10051", Convert.ToInt32("1234" + i), 50);
            //        }
            //        else 
            //        {
            //            objEtiqueta = new Etiqueta("7085", "Soquete pisca dianteiro lateral", 7895479000995, "LT-27796", Convert.ToInt32("1234" + (i - 4)), 100);
            //        }

            //        list.Add(objEtiqueta);
            //        objEtiqueta = null;
            //    }

            //    list.Add(new Etiqueta("8030", "Chicote Soquete luz", 7895479042576, "LT-10051", Convert.ToInt32("12340"), 25));
            //    ListEtiquetasGeradas = list;
            //}

            //public static String decrementaVolume(int codigoEmbalagem)
            //{
            //    if (QtdVolumes > 1)
            //    {
            //        --QtdVolumes;
            //        return QtdVolumes.ToString();
            //    }
            //    return "Qtd Volumes não pode ser menor que 1.";
            //}

        #endregion 
    }
}
