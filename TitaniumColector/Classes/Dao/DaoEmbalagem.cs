using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TitaniumColector.Classes.Model;
using System.Data.SqlClient;
using TitaniumColector.Classes.Exceptions;
using TitaniumColector.SqlServer;
using TitaniumColector.Classes.SqlServer;
using System.Data.SqlServerCe;

namespace TitaniumColector.Classes.Dao
{
    class DaoEmbalagem
    {
        StringBuilder sql01;
        private EmbalagemProduto embalagem = null;

        public DaoEmbalagem() 
        {

        }

        public List<EmbalagemProduto> cargaEmbalagensProduto(int codigoProposta) 
        {
            sql01 = new StringBuilder();
            List<EmbalagemProduto> listaEmbalagens = new List<EmbalagemProduto>();

            try
            {
                sql01.Append(" SELECT codigoEMBALAGEMPRODUTO,COALESCE(nomeEMBALAGEMPRODUTO,'ND') AS nomeEMBALAGEMPRODUTO,produtoEMBALAGEMPRODUTO,quantidadeEMBALAGEMPRODUTO,padraoEMBALAGEMPRODUTO,COALESCE(embalagemEMBALAGEMPRODUTO,0) AS embalagemEMBALAGEMPRODUTO,COALESCE(codigobarrasEMBALAGEMPRODUTO,'0000000000000') AS codigobarrasEMBALAGEMPRODUTO ");
                sql01.Append(" FROM tb0504_Embalagens_Produtos");
                sql01.Append(" INNER JOIN tb0501_Produtos ON codigoPRODUTO = produtoEMBALAGEMPRODUTO");
                sql01.Append(" WHERE produtoEMBALAGEMPRODUTO IN(");
                sql01.Append("						                SELECT produtoRESERVA AS codigoPRODUTO");
                sql01.Append("						                FROM tb1206_Reservas (NOLOCK)");
                sql01.Append("						                INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA");
                sql01.Append("						                INNER JOIN tb0501_Produtos (NOLOCK) ON produtoITEMPROPOSTA = codigoPRODUTO");
                sql01.AppendFormat("						        WHERE propostaITEMPROPOSTA = {0}",codigoProposta);
                sql01.Append("						                AND tipodocRESERVA = 1602 ");
                sql01.Append("						                AND statusITEMPROPOSTA = 3");
                sql01.Append("						                AND separadoITEMPROPOSTA = 0");
                sql01.Append("						                GROUP BY produtoRESERVA");
                sql01.Append("                                 )");
                sql01.Append(" AND lixeiraPRODUTO = 0");
                sql01.Append(" ORDER BY produtoEMBALAGEMPRODUTO");

                SqlDataReader dr = SqlServerConn.fillDataReader(sql01.ToString());

                while ((dr.Read()))
                {
                    {
                        embalagem = new EmbalagemProduto(Convert.ToInt32(dr["codigoEMBALAGEMPRODUTO"]), (string)dr["nomeEMBALAGEMPRODUTO"],(EmbalagemProduto.PadraoEmbalagem)dr["padraoEMBALAGEMPRODUTO"]
                                                         ,Convert.ToInt32(dr["produtoEMBALAGEMPRODUTO"]), Convert.ToDouble(dr["quantidadeEMBALAGEMPRODUTO"])
                                                         ,Convert.ToInt32(dr["embalagemEMBALAGEMPRODUTO"])
                                                         ,(string)dr["codigobarrasEMBALAGEMPRODUTO"]);

                        listaEmbalagens.Add(embalagem);

                    }

                }

                dr.Close();
                SqlServerConn.closeConn();

                if (listaEmbalagens.Count == 0)
                {
                    throw new SqlQueryExceptions("Não foi possível recuperar informações sobre embalagens para esta proposta :  " + codigoProposta);
                }

                embalagem = null;
                return listaEmbalagens;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void insertEmbalagemBaseMobile(List<EmbalagemProduto> listaEmbalagens) 
        {
            try
            {
                //Limpa a tabela..
                CeSqlServerConn.execCommandSqlCe("DELETE FROM tb0005_Embalagens");

                foreach (var item in listaEmbalagens)
                {

                    //Query de insert na Base Mobile
                    sql01 = new StringBuilder();
                    sql01.Append("INSERT INTO tb0005_Embalagens");
                    sql01.Append("(codigoEMBALAGEM, nomeEMBALAGEM, produtoEMBALAGEM, quantidadeEMBALAGEM, padraoEMBALAGEM, embalagemEMBALAGEM, ean13EMBALAGEM)");
                    sql01.Append("VALUES (");
                    sql01.AppendFormat("{0},", item.Codigo);
                    sql01.AppendFormat("'{0}',", item.Nome);
                    sql01.AppendFormat("{0},", item.ProdutoEmbalagem);
                    sql01.AppendFormat("{0},", item.Quantidade);
                    sql01.AppendFormat("{0},", (int)item.Padrao);
                    sql01.AppendFormat("{0},", item.TipoEmbalagem);
                    sql01.AppendFormat("'{0}')", item.Ean13Embalagem);

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

        public List<EmbalagemProduto> carregarEmbalagensProduto(Produto produto) 
        {

            EmbalagemProduto objEmbalagem = null;
            List<EmbalagemProduto> listaEmbalagens = new List<EmbalagemProduto>();

            sql01 = new StringBuilder();
            sql01.Append(" SELECT        TB_PROP.codigoPROPOSTA, TB_EMB.codigoEMBALAGEM, TB_EMB.nomeEMBALAGEM, TB_EMB.produtoEMBALAGEM, TB_EMB.quantidadeEMBALAGEM, TB_EMB.padraoEMBALAGEM, ");
            sql01.Append(" TB_EMB.embalagemEMBALAGEM, TB_EMB.ean13EMBALAGEM, TB_PROP.numeroPROPOSTA, TB_PROP.codigopickingmobilePROPOSTA, COUNT(*) AS TLINHAS");
            sql01.Append(" FROM            tb0002_ItensProposta AS TB_ITEM INNER JOIN");
            sql01.Append(" tb0001_Propostas AS TB_PROP ON TB_ITEM.propostaITEMPROPOSTA = TB_PROP.codigoPROPOSTA INNER JOIN");
            sql01.Append(" tb0005_Embalagens AS TB_EMB ON TB_ITEM.codigoprodutoITEMPROPOSTA = TB_EMB.produtoEMBALAGEM");
            sql01.Append(" GROUP BY TB_PROP.codigoPROPOSTA, TB_EMB.codigoEMBALAGEM, TB_EMB.nomeEMBALAGEM, TB_EMB.produtoEMBALAGEM, TB_EMB.quantidadeEMBALAGEM, TB_EMB.padraoEMBALAGEM, ");
            sql01.Append(" TB_EMB.embalagemEMBALAGEM, TB_EMB.ean13EMBALAGEM, TB_PROP.numeroPROPOSTA, TB_PROP.codigopickingmobilePROPOSTA");
            sql01.AppendFormat(" HAVING        (TB_EMB.produtoEMBALAGEM = {0})", produto.CodigoProduto);

            SqlCeDataReader dr = CeSqlServerConn.fillDataReaderCe(sql01.ToString());

            while ((dr.Read()))
            {
                objEmbalagem = new EmbalagemProduto(
                     
                      Convert.ToInt32(dr["codigoEMBALAGEM"])
                      ,(string)dr["nomeEMBALAGEM"]
                      ,(EmbalagemProduto.PadraoEmbalagem)Convert.ToInt32(dr["padraoEMBALAGEM"])
                      ,Convert.ToInt32(dr["produtoEMBALAGEM"])
                      ,Convert.ToDouble(dr["quantidadeEMBALAGEM"])
                      ,Convert.ToInt32(dr["embalagemEMBALAGEM"])
                      ,(string)dr["ean13EMBALAGEM"]);

                listaEmbalagens.Add(objEmbalagem);
            }

            return listaEmbalagens;
           
        }

        public List<EmbalagemSeparacao> carregarEmbalagensSeparacao()
        {

            EmbalagemSeparacao objEmbalagem = null;
            List<EmbalagemSeparacao> listaEmbalagens = new List<EmbalagemSeparacao>();

            sql01 = new StringBuilder();

            sql01.Append(" SELECT codigoEMBALAGEM,nomeEMBALAGEM,produtoEMBALAGEM,pesoEMBALAGEM,padraoEMBALAGEM");
            sql01.Append(" FROM tb0545_Embalagens");
            sql01.Append(" INNER JOIN tb0501_Produtos ON codigoPRODUTO = produtoEMBALAGEM");

            SqlDataReader dr = SqlServerConn.fillDataReader(sql01.ToString());

            while ((dr.Read()))
            {
                objEmbalagem = new EmbalagemSeparacao(

                      Convert.ToInt32(dr["codigoEMBALAGEM"])
                      , (string)dr["nomeEMBALAGEM"]
                      , (Embalagem.PadraoEmbalagem)Convert.ToInt32(dr["padraoEMBALAGEM"])
                      , Convert.ToInt32(dr["produtoEMBALAGEM"])
                      , Convert.ToDouble(dr["pesoEMBALAGEM"]));

                listaEmbalagens.Add(objEmbalagem);
            }

            return listaEmbalagens;

        }

        public List<EmbalagemProduto> carregarEmbalagensProduto(Proposta proposta)
        {
            EmbalagemProduto objEmbalagem = null;
            List<EmbalagemProduto> listaEmbalagens = new List<EmbalagemProduto>();

            sql01 = new StringBuilder();
            sql01.Append(" SELECT        TB_PROP.codigoPROPOSTA, TB_EMB.codigoEMBALAGEM, TB_EMB.nomeEMBALAGEM, TB_EMB.produtoEMBALAGEM, TB_EMB.quantidadeEMBALAGEM, TB_EMB.padraoEMBALAGEM, ");
            sql01.Append(" TB_EMB.embalagemEMBALAGEM, TB_EMB.ean13EMBALAGEM, TB_PROP.numeroPROPOSTA, TB_PROP.codigopickingmobilePROPOSTA, COUNT(*) AS TLINHAS");
            sql01.Append(" FROM            tb0002_ItensProposta AS TB_ITEM INNER JOIN");
            sql01.Append(" tb0001_Propostas AS TB_PROP ON TB_ITEM.propostaITEMPROPOSTA = TB_PROP.codigoPROPOSTA INNER JOIN");
            sql01.Append(" tb0005_Embalagens AS TB_EMB ON TB_ITEM.codigoprodutoITEMPROPOSTA = TB_EMB.produtoEMBALAGEM");
            sql01.Append(" GROUP BY TB_PROP.codigoPROPOSTA, TB_EMB.codigoEMBALAGEM, TB_EMB.nomeEMBALAGEM, TB_EMB.produtoEMBALAGEM, TB_EMB.quantidadeEMBALAGEM, TB_EMB.padraoEMBALAGEM, ");
            sql01.Append(" TB_EMB.embalagemEMBALAGEM, TB_EMB.ean13EMBALAGEM, TB_PROP.numeroPROPOSTA, TB_PROP.codigopickingmobilePROPOSTA");
            sql01.AppendFormat(" HAVING        (TB_EMB.produtoEMBALAGEM = {0})", proposta.ListObjItemProposta[0].CodigoProduto);

            SqlCeDataReader dr = CeSqlServerConn.fillDataReaderCe(sql01.ToString());

            while ((dr.Read()))
            {
                objEmbalagem = new EmbalagemProduto(
                    
                    Convert.ToInt32(dr["codigoEMBALAGEM"])
                    , (string)dr["nomeEMBALAGEM"]
                    , (EmbalagemProduto.PadraoEmbalagem)Convert.ToInt32(dr["padraoEMBALAGEM"])
                    , Convert.ToInt32(dr["produtoEMBALAGEM"])
                    , Convert.ToDouble(dr["quantidadeEMBALAGEM"])
                    , Convert.ToInt32(dr["embalagemEMBALAGEM"])
                    , (string)dr["ean13EMBALAGEM"]);

                listaEmbalagens.Add(objEmbalagem);
            }

            return listaEmbalagens;
        }


    }
}
