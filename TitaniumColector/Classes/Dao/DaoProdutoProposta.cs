using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TitaniumColector.Classes.SqlServer;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using TitaniumColector.SqlServer;
using TitaniumColector.Classes.Exceptions;

namespace TitaniumColector.Classes.Dao
{
    class DaoProdutoProposta
    {
        private StringBuilder sql01;
        public DaoProdutoProposta() 
        {

        }

        /// <summary>
        ///  Carrega uma List com objetos da classe ItemProposta preenchida com dados sobre os itens de uma determinada Proposta 
        /// </summary>
        /// <param name="codigoProposta">Código da proposta da qual serão recuperados os itens.</param>
        /// <returns>List do tipo ItemProposta </returns>
        public IEnumerable<ProdutoProposta> fillListItensProposta(int codigoProposta)
        {
            ProdutoProposta objItemProposta = null; 

            List<ProdutoProposta> listItensProposta = new List<ProdutoProposta>();

            try
            {

                sql01 = new StringBuilder();

                ///ALTERADO EM CRITERIO DE TESTES PARA RETORNAR MAIS INFORMAÇOES SOBRE O PRODUTO DA PROPOSTA
                ///
                //sql01.Append("SELECT codigoITEMPROPOSTA,propostaITEMPROPOSTA,produtoRESERVA AS codigoPRODUTO,nomePRODUTO,partnumberPRODUTO,");
                //sql01.Append("ean13PRODUTO,SUM(quantidadeRESERVA) AS QTD");
                //sql01.Append(" FROM tb1206_Reservas (NOLOCK) ");
                //sql01.Append("INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA ");
                //sql01.Append("INNER JOIN tb0501_Produtos (NOLOCK) ON produtoITEMPROPOSTA = codigoPRODUTO ");
                //sql01.Append("LEFT JOIN tb1212_Lotes_Locais (NOLOCK) ON loteRESERVA = loteLOTELOCAL ");
                //sql01.Append("LEFT JOIN tb1211_Locais ON codigoLOCAL = localLOTELOCAL ");
                //sql01.AppendFormat("WHERE propostaITEMPROPOSTA = {0} ", codigoProposta);
                //sql01.Append("AND tipodocRESERVA = 1602 ");
                //sql01.Append("AND statusITEMPROPOSTA = 3 ");
                //sql01.Append("AND separadoITEMPROPOSTA = 0  ");
                //sql01.Append("GROUP BY codigoITEMPROPOSTA,propostaITEMPROPOSTA,ean13PRODUTO,produtoRESERVA,produtoITEMPROPOSTA,");
                //sql01.Append("nomePRODUTO,partnumberPRODUTO");

                sql01.Append(" SELECT codigoITEMPROPOSTA,propostaITEMPROPOSTA,");
                sql01.Append(" produtoRESERVA AS codigoPRODUTO,");
                sql01.Append(" nomePRODUTO,partnumberPRODUTO,ean13PRODUTO,");
                sql01.Append(" SUM(quantidadeRESERVA) AS QTD,pesobrutoPRODUTO,SUM(quantidadeRESERVA) * pesobrutoPRODUTO AS pesobrutototalPRODUTO");
                sql01.Append(" ,dbo.fn1211_LotesReservaProduto(produtoRESERVA,propostaITEMPROPOSTA) AS lotesRESERVA,");
                sql01.Append(" DBO.fn1211_LocaisLoteProduto(produtoRESERVA,dbo.fn1211_LotesReservaProduto(produtoRESERVA,propostaITEMPROPOSTA)) AS nomesLocaisLOTES ");
                sql01.Append(" FROM tb1206_Reservas (NOLOCK) ");
                sql01.Append(" INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA ");
                sql01.Append(" INNER JOIN tb0501_Produtos (NOLOCK) ON produtoITEMPROPOSTA = codigoPRODUTO  ");
                sql01.AppendFormat(" WHERE propostaITEMPROPOSTA = {0} ", codigoProposta);
                sql01.Append(" AND tipodocRESERVA = 1602");
                sql01.Append(" AND statusITEMPROPOSTA = 3");
                sql01.Append(" AND separadoITEMPROPOSTA = 0");
                sql01.Append(" GROUP BY codigoITEMPROPOSTA,propostaITEMPROPOSTA,ean13PRODUTO,produtoRESERVA,nomePRODUTO,partnumberPRODUTO,pesobrutoPRODUTO");
                sql01.Append(" ORDER BY codigoPRODUTO");

                SqlDataReader dr = SqlServerConn.fillDataReader( sql01.ToString());

                while ((dr.Read()))
                {
                    {
                        objItemProposta = new ProdutoProposta(  Convert.ToInt32(dr["codigoITEMPROPOSTA"]),
                                                                Convert.ToInt32(dr["propostaITEMPROPOSTA"]),
                                                                Convert.ToDouble(dr["QTD"]),
                                                                ProdutoProposta.statusSeparado.NAOSEPARADO,
                                                                (string)dr["lotesRESERVA"],
                                                                (string)dr["nomesLocaisLOTES"],
                                                                Convert.ToInt32(dr["codigoPRODUTO"]),
                                                                (string)dr["ean13PRODUTO"],
                                                                (string)dr["partnumberPRODUTO"],
                                                                (string)(dr["nomePRODUTO"]), 
                                                                Convert.ToDouble(dr["pesobrutoPRODUTO"]));

                        //Carrega a lista de itens que será retornada ao fim do procedimento.
                        listItensProposta.Add(objItemProposta);

                    }

                }

                dr.Close();
                SqlServerConn.closeConn();

                if (listItensProposta.Count == 0)
                {
                    throw new SqlQueryExceptions("Não foi possível recuperar informações sobre os itens da proposta " + codigoProposta);
                }

                return listItensProposta;

            }
            catch(SqlQueryExceptions ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Identifica qual o próximo item com status de NAOSEPARADO e o retorna.
        /// </summary>
        /// 
        /// <returns>Objeto ProdutoProposta com o próximo item da sequência da base mobile.</returns>
        /// 
        /// <remarks>
        ///       Caso a query não retorne valores da base mobile o método retorna um Valor NULL
        /// </remarks>
        public ProdutoProposta fillTop1ItemProposta()
        {
            Object obj = null;

            sql01 = new StringBuilder();
            sql01.Append(" SELECT TOP (1) TB_PROP.codigoPROPOSTA,");
            sql01.Append(" TB_ITEMPROPOP.codigoITEMPROPOSTA, TB_ITEMPROPOP.propostaITEMPROPOSTA, TB_ITEMPROPOP.quantidadeITEMPROPOSTA, TB_ITEMPROPOP.statusseparadoITEMPROPOSTA,");
            sql01.Append(" TB_ITEMPROPOP.lotereservaITEMPROPOSTA, TB_ITEMPROPOP.codigoprodutoITEMPROPOSTA,");
            sql01.Append(" TB_PROD.ean13PRODUTO, TB_PROD.partnumberPRODUTO,TB_PROD.descricaoPRODUTO, TB_PROD.identificacaolotePRODUTO, TB_PROD.codigolotePRODUTO,");
            sql01.Append(" TB_PROD.nomelocalPRODUTO");
            sql01.Append(" FROM   tb0001_Propostas AS TB_PROP ");
            sql01.Append(" INNER JOIN tb0002_ItensProposta AS TB_ITEMPROPOP ON TB_PROP.codigoPROPOSTA = TB_ITEMPROPOP.propostaITEMPROPOSTA");
            sql01.Append(" INNER JOIN tb0003_Produtos AS TB_PROD ON TB_ITEMPROPOP.codigoprodutoITEMPROPOSTA = TB_PROD.codigoPRODUTO");
            sql01.Append(" WHERE TB_ITEMPROPOP.statusseparadoITEMPROPOSTA = 0");
            sql01.Append(" ORDER BY TB_PROD.nomelocalPRODUTO ASC");
   
            SqlCeDataReader dr = CeSqlServerConn.fillDataReaderCe(sql01.ToString());

            if ((dr != null))
            {
                while ((dr.Read()))
                {
                    int statusSeparadoItem = Convert.ToInt32(dr["statusseparadoITEMPROPOSTA"]);
                    ProdutoProposta produto = new ProdutoProposta(Convert.ToInt32(dr["codigoITEMPROPOSTA"]),
                                                                        Convert.ToInt32(dr["codigoPROPOSTA"]),
                                                                        Convert.ToDouble(dr["quantidadeITEMPROPOSTA"]),
                                                                        (ProdutoProposta.statusSeparado)statusSeparadoItem,
                                                                        Convert.ToInt32(dr["lotereservaITEMPROPOSTA"]),
                                                                        Convert.ToInt32(dr["codigoprodutoITEMPROPOSTA"]),
                                                                        (string)dr["ean13PRODUTO"],
                                                                        (string)dr["partnumberPRODUTO"],
                                                                        (string)dr["descricaoPRODUTO"],
                                                                        (string)dr["nomelocalPRODUTO"],
                                                                        Convert.ToInt32(dr["codigolotePRODUTO"]),
                                                                        (string)dr["identificacaolotePRODUTO"]);
                    obj = produto;

                }


            }

            //fecha a conexão
            dr.Close();
            CeSqlServerConn.closeConnCe();

            if (obj != null)
            {
                return (ProdutoProposta)obj;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Insert na base Mobile tabela de itens da proposta
        /// </summary>
        /// <param name="listProposta">List com objetos do tipo ItemProposta </param>
        public void insertItemProposta(List<ProdutoProposta> listProdutoProposta)
        {

            try
            {
                //Limpa a tabela..
                CeSqlServerConn.execCommandSqlCe("DELETE FROM tb0002_ItensProposta");

                foreach (var item in listProdutoProposta)
                {

                    //Query de insert na Base Mobile
                    sql01 = new StringBuilder();
                    sql01.Append("INSERT INTO tb0002_ItensProposta");
                    sql01.Append("(codigoITEMPROPOSTA, propostaITEMPROPOSTA, quantidadeITEMPROPOSTA,pesoITEMPROPOSTA");
                    sql01.Append(",statusseparadoITEMPROPOSTA, codigoprodutoITEMPROPOSTA, lotereservaITEMPROPOSTA,alllotesreservaITEMPROPOSTA,qtdembalagemITEMPROPOSTA,allnomeslocaisITEMPROPOSTA) ");
                    sql01.Append("VALUES (");
                    sql01.AppendFormat("{0},", item.CodigoItemProposta);
                    sql01.AppendFormat("{0},", item.PropostaItemProposta);
                    sql01.AppendFormat("{0},", item.Quantidade);
                    sql01.AppendFormat("{0},", item.Peso);
                    sql01.AppendFormat("{0},", (int)item.StatusSeparado);
                    sql01.AppendFormat("{0},", item.CodigoProduto);
                    sql01.AppendFormat("{0},", item.LotereservaItemProposta);
                    sql01.AppendFormat("'{0}',", item.LotesReserva);
                    sql01.AppendFormat("{0},", item.QuantidadeEmbalagem); //nao utilizo este Valor
                    sql01.AppendFormat("'{0}')", item.NomeLocaisItemProposta);
      
                    CeSqlServerConn.execCommandSqlCe(sql01.ToString());
                }

            }
            catch (SqlCeException sqlEx)
            {
                throw sqlEx;

            }
            catch (Exception Ex)
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("Ocorreram problemas durante a carga de dados na tabela tb0002_ItensProposta. \n");
                strBuilder.Append("O procedimento não pode ser concluído \n");
                strBuilder.AppendFormat("Description : {0}", Ex.Message);

                MainConfig.errorMessage(strBuilder.ToString(), "Error in Query!!");
            }

        }

        /// <summary>
        /// Realiza Insert na base Mobile table tb0002_ItensProposta
        /// </summary>
        /// <param name="codigoItem">Código do Item da Proposta</param>
        /// <param name="propostaItemProposta">Proposta (ForeingKey)</param>
        /// <param name="Quantidade">Qunatidade de itens</param>
        /// <param name="statusSeparado">status (Separado ou não)</param>
        /// <param name="codigoProduto">Código do produto </param>
        /// <param name="loteReserva">Lote referente a reserva do item</param>
        public void insertItemProposta(Int64 codigoItem, Int64 propostaItemProposta, Double quantidade, ProdutoProposta.statusSeparado statusSeparado,
                                       Int64 codigoProduto, Int64 loteReserva, Int64 codigoLocalItemProposta)
        {
            try
            {
                //Limpa a tabela..
                CeSqlServerConn.execCommandSqlCe("DELETE FROM tb0002_ItensProposta");

                //Query de insert na Base Mobile
                sql01  = new StringBuilder();
                sql01.Append("INSERT INTO tb0002_ItensProposta");
                sql01.Append("(codigoITEMPROPOSTA, propostaITEMPROPOSTA, quantidadeITEMPROPOSTA,");
                sql01.Append("statusseparadoITEMPROPOSTA, codigoprodutoITEMPROPOSTA, lotereservaITEMPROPOSTA,localloteITEMPROPOSTA) ");
                sql01.Append("VALUES (");
                sql01.AppendFormat("{0},", codigoItem);
                sql01.AppendFormat("{0},", propostaItemProposta);
                sql01.AppendFormat("{0},", quantidade);
                sql01.AppendFormat("{0},", (int)statusSeparado);
                sql01.AppendFormat("{0},", codigoProduto);
                sql01.AppendFormat("{0},", loteReserva);
                sql01.AppendFormat("{0})", codigoLocalItemProposta);
   
                CeSqlServerConn.execCommandSqlCe(sql01.ToString());

            }
            catch (SqlCeException sqlEx)
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("Ocorreram problemas durante a carga de dados na tabela tb0002_ItensProposta. \n");
                strBuilder.Append("O procedimento não pode ser concluído");
                strBuilder.AppendFormat("Erro : {0}", sqlEx.Errors);
                strBuilder.AppendFormat("Description : {0}", sqlEx.Message);
                System.Windows.Forms.MessageBox.Show(strBuilder.ToString(), "Error:", System.Windows.Forms.MessageBoxButtons.OK,
                               System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button1);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Altera o status de separado do item na Base mobile.
        /// </summary>
        /// <param name="item">O status será alterado de acordo com o atual statdo so item passado com parâmetro.</param>
        public void updateStatusItemProposta(ProdutoProposta item)
        {
            try
            {
                sql01 = new StringBuilder();
                sql01.Append(" UPDATE      tb0002_ItensProposta");
                sql01.AppendFormat("  SET   statusseparadoITEMPROPOSTA ={0}", (int)item.StatusSeparado);
                sql01.AppendFormat(" WHERE (tb0002_ItensProposta.codigoITEMPROPOSTA = {0})", item.CodigoItemProposta);
               

                CeSqlServerConn.execCommandSqlCe(sql01.ToString());
            }
            catch (Exception ex )
            {
                throw ex;
            }

        }

        /// <summary>
        /// Realiza update nas informações dos itens da proposta durante 
        /// o retorno do mesmo para a base Principal após a finalização da liberação da proposta.
        /// </summary>
        public void updateItemPropostaRetorno() 
        {
            try
            {
                sql01 = new StringBuilder();
                sql01.Append("SELECT codigoITEMPROPOSTA,propostaITEMPROPOSTA,statusseparadoITEMPROPOSTA,codigoprodutoITEMPROPOSTA,xmlSequenciaITEMPROPOSTA ");
                sql01.Append(" FROM tb0002_ItensProposta");
                sql01.AppendFormat(" WHERE  statusseparadoITEMPROPOSTA = {0}", (int)ProdutoProposta.statusSeparado.SEPARADO);
                SqlCeDataReader dr = CeSqlServerConn.fillDataReaderCe(sql01.ToString());

                while(dr.Read())
                {
                    sql01 = new StringBuilder();
                    sql01.Append(" UPDATE tb1602_Itens_Proposta");
                    sql01.AppendFormat("  SET   separadoITEMPROPOSTA ={0}", Convert.ToInt32(dr["statusseparadoITEMPROPOSTA"]));
                    sql01.AppendFormat("  ,usuarioITEMPROPOSTA ={0}", MainConfig.CodigoUsuarioLogado.ToString());
                    sql01.AppendFormat(" ,xmlSequenciaITEMPROPOSTA ='{0}'", (string)dr["xmlSequenciaITEMPROPOSTA"]);
                    sql01.AppendFormat(" WHERE (codigoITEMPROPOSTA = {0})", Convert.ToInt32(dr["codigoITEMPROPOSTA"]));
                    SqlServerConn.execCommandSql(sql01.ToString());

                }

                dr.Close();
                CeSqlServerConn.closeConnCe();

            }
            catch (Exception ex)
            {
                throw ex as SqlException;
            }


        }

        /// <summary>
        /// Altera o status de separado do item na Base mobile.
        /// </summary>
        /// <param name="status">Status para qual será atualizado</param>
        /// <param name="codigoItem">código so item da proposta a ser alterado</param>
        /// <remarks>  
        ///             Os status são:
        ///             ProdutoProposta.statusSeparado.NAOSEPARADO       / 0
        ///             ProdutoProposta.statusSeparado.SEPARADO          / 1
        /// </remarks>
        public void updateStatusItemProposta(ProdutoProposta.statusSeparado status, int codigoItem)
        {
            sql01 = new StringBuilder();
            sql01.Append("UPDATE      tb0002_ItensProposta");
            sql01.AppendFormat("SET   statusseparadoITEMPROPOSTA ={0}", status);
            sql01.AppendFormat("WHERE tb0002_ItensProposta.codigoITEMPROPOSTA = {0})", codigoItem);

            CeSqlServerConn.execCommandSqlCe(sql01.ToString());
        }

        /// <summary>
        /// Atualiza informações do xml do item passado como parâmetro.
        /// </summary>
        /// <param name="xmlString">String no formato Xml</param>
        /// <param name="codigoItem">Codigo do Item a ser atualizado</param>
        public void updateXmlItemProposta(String xmlString,Int32 codigoItem) 
        {
            sql01 = new StringBuilder();
            sql01.Append(" UPDATE      tb0002_ItensProposta");
            sql01.AppendFormat(" SET   xmlSequenciaITEMPROPOSTA ='{0}'", xmlString);
            sql01.AppendFormat(" WHERE tb0002_ItensProposta.codigoITEMPROPOSTA = ({0})", codigoItem);

            CeSqlServerConn.execCommandSqlCe(sql01.ToString());

        }

    }
}
