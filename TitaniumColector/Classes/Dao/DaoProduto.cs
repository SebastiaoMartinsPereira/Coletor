using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TitaniumColector.Classes.SqlServer;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using TitaniumColector.Classes.Exceptions;
using TitaniumColector.SqlServer;
using System.Windows.Forms;

namespace TitaniumColector.Classes.Dao
{
    class DaoProduto
    {
        private StringBuilder sql01;


        public DaoProduto()
        {

        }

        /// <summary>
        /// Efetua o insert na base mobile tb0003_Produtos 
        /// </summary>
        /// <param name="listProduto">List preenchida com objetos da classe Produto</param>
        public void insertProdutoBaseMobile(List<Produto> listProduto)
        {

            try
            {
                //Limpa a tabela..
                CeSqlServerConn.execCommandSqlCe("DELETE FROM tb0003_Produtos");

                foreach (var item in listProduto)
                {
                    //Query de insert na Base Mobile
                    sql01 = new StringBuilder();
                    sql01.Append("INSERT INTO tb0003_Produtos ");
                    sql01.Append("(codigoPRODUTO, ean13PRODUTO, partnumberPRODUTO, descricaoPRODUTO, codigolotePRODUTO,");
                    sql01.Append("identificacaolotePRODUTO, codigolocalPRODUTO, nomelocalPRODUTO)");
                    sql01.Append("VALUES (");
                    sql01.AppendFormat("{0},", item.CodigoProduto);
                    sql01.AppendFormat("\'{0}\',", item.Ean13);
                    sql01.AppendFormat("\'{0}\',", item.Partnumber);
                    sql01.AppendFormat("\'{0}\',", item.Descricao);
                    sql01.AppendFormat("{0},", item.CodigoLoteProduto);
                    sql01.AppendFormat("\'{0}\',", item.IdentificacaoLoteProduto);
                    sql01.AppendFormat("{0},", item.CodigoLocalLote);
                    sql01.AppendFormat("\'{0}\')", item.NomeLocalLote);

                    CeSqlServerConn.execCommandSqlCe(sql01.ToString());
                }

            }
            catch (SqlCeException sqlEx)
            {
                System.Windows.Forms.MessageBox.Show("Erro durante a carga de dados na base Mobile tb0003_Produtos.\n Erro : " + sqlEx.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Preenche um objeto list com objetos da classe Produto 
        /// </summary>
        /// <param name="codigoProposta"></param>
        /// <returns>Lista de Produtos</returns>
        public IEnumerable<Produto> fillListProduto(Int32 codigoProposta)
        {
            Produto objProd = new Produto();
            List<Produto> listProduto = new List<Produto>();

            try
            {
                sql01 = new StringBuilder();
                sql01.Append("SELECT codigoPRODUTO,partnumberPRODUTO,nomePRODUTO,ean13PRODUTO,codigolotePRODUTO,identificacaolotePRODUTO,dbo.fn1211_LocaisLoteProduto(codigoPRODUTO,codigolotePRODUTO) AS nomelocalPRODUTO");
                sql01.AppendFormat(" FROM dbo.fn0003_informacoesProdutos({0})", codigoProposta);
                sql01.Append("GROUP BY codigoPRODUTO,partnumberPRODUTO,nomePRODUTO,ean13PRODUTO,codigolotePRODUTO,identificacaolotePRODUTO,dbo.fn1211_LocaisLoteProduto(codigoPRODUTO,codigolotePRODUTO)");
                sql01.Append("ORDER BY nomelocalPRODUTO ASC");

                SqlDataReader dr = SqlServerConn.fillDataReader(sql01.ToString());

                while ((dr.Read()))
                {
                    objProd = new Produto(Convert.ToInt32(dr["codigoPRODUTO"]),
                                            (String)dr["ean13PRODUTO"],
                                            (String)dr["partnumberPRODUTO"],
                                            (String)dr["nomePRODUTO"],
                                            (String)dr["nomelocalPRODUTO"],
                                            Convert.ToInt64(dr["codigolotePRODUTO"]),
                                            (String)dr["identificacaolotePRODUTO"]);

                    //Carrega a lista de itens que será retornada ao fim do procedimento.
                    listProduto.Add(objProd);
                }

                if (listProduto == null || listProduto.Count == 0)
                {
                    throw new TitaniumColector.Classes.Exceptions.SqlQueryExceptions("Query não retornou Valor.");
                }

                dr.Close();
                SqlServerConn.closeConn();

                return listProduto;
            }
            catch (SqlQueryExceptions queryEx)
            {
                SqlServerConn.closeConn();
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Não foi possível obter informações sobre a proposta {0}", codigoProposta);
                sb.Append("\nError :" + queryEx.Message);
                sb.Append("\nFavor contate o administrador do sistema.");
                MainConfig.errorMessage(sb.ToString(), "Carga Base Mobile.");
                return listProduto = null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public String recuperarLocalEstoqueProduto(int produto, int lote)
        {
            string nomesLocais = "";
            sql01 = new StringBuilder();
            sql01.Append(" SELECT nomeLOCAL ");
            sql01.Append(" FROM tb1205_Lotes ");
            sql01.Append(" INNER JOIN tb1212_Lotes_Locais ON codigoLOTE = loteLOTELOCAL ");
            sql01.Append(" INNER JOIN tb1211_Locais ON codigoLOCAL = localLOTELOCAL ");
            sql01.AppendFormat(" WHERE produtoLOTE ={0} AND codigoLOTE = {1} ", produto, lote);

            SqlDataReader dr = SqlServerConn.fillDataReader(sql01.ToString());

            while ((dr.Read()))
            {
                nomesLocais += dr["nomeLOCAL"];
            }

            dr.Close();
            SqlServerConn.closeConn();

            return nomesLocais;
        }

        public Etiqueta recuperarInformacoesPorEan13Etiqueta(Etiqueta etiqueta)
        {

            sql01 = new StringBuilder();
            sql01.Append(" SELECT        tb0005_Embalagens.produtoEMBALAGEM, tb0005_Embalagens.codigoEMBALAGEM, tb0005_Embalagens.quantidadeEMBALAGEM AS qtdEMBALAGEM, tb0005_Embalagens.ean13EMBALAGEM, ");
            sql01.Append(" tb0003_Produtos.ean13PRODUTO, tb0003_Produtos.partnumberPRODUTO, tb0003_Produtos.descricaoPRODUTO, tb0003_Produtos.codigolotePRODUTO, tb0003_Produtos.identificacaolotePRODUTO, ");
            sql01.Append(" tb0003_Produtos.codigolocalPRODUTO, tb0003_Produtos.nomelocalPRODUTO");
            sql01.Append(" FROM            tb0005_Embalagens INNER JOIN");
            sql01.Append(" tb0003_Produtos ON tb0005_Embalagens.produtoEMBALAGEM = tb0003_Produtos.codigoPRODUTO");
            sql01.AppendFormat(" WHERE        (tb0005_Embalagens.ean13EMBALAGEM = {0})", etiqueta.Ean13Etiqueta);

            SqlCeDataReader dr = CeSqlServerConn.fillDataReaderCe(sql01.ToString());

            while (dr.Read())
            {
                etiqueta = new Etiqueta((string)dr["partnumberPRODUTO"], (string)dr["descricaoPRODUTO"], Convert.ToInt64(dr["ean13EMBALAGEM"]), (string)dr["identificacaolotePRODUTO"]
                                        , 0, Convert.ToDouble(dr["qtdEMBALAGEM"]), etiqueta.TipoEtiqueta);
            }

            return etiqueta;
        }
    }
}
