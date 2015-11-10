using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using TitaniumColector.SqlServer;
using TitaniumColector.Classes.SqlServer;
using System.Data.SqlServerCe;
using System.Windows.Forms;
using System.Globalization;

namespace TitaniumColector.Classes.Dao
{
    class DaoProposta
    {
        private StringBuilder sql01;
        private SqlDataReader dr;
        private Proposta objProposta;

        public DaoProposta() 
        {
           

        }

        /// <summary>
        /// Preenche um objeto do tipo Proposta com todas as suas informações inclusive os itens e detalhes sobre os mesmos
        /// </summary>
        /// <returns>Objeto do tipo Proposta</returns>
        public Proposta fillProposta()
        {
            Proposta objProposta = null;

            List<ProdutoProposta> listProd = new List<ProdutoProposta>();

            sql01= new StringBuilder();

            sql01.Append(" SELECT TB_PROP.codigoPROPOSTA, TB_PROP.numeroPROPOSTA, TB_PROP.dataliberacaoPROPOSTA,TB_PROP.clientePROPOSTA, TB_PROP.razaoclientePROPOSTA,TB_PROP.volumesPROPOSTA,TB_PROP.codigopickingmobilePROPOSTA");
            sql01.Append(" TB_ITEMPROPOP.codigoITEMPROPOSTA, TB_ITEMPROPOP.propostaITEMPROPOSTA, TB_ITEMPROPOP.quantidadeITEMPROPOSTA, TB_ITEMPROPOP.statusseparadoITEMPROPOSTA,");
            sql01.Append(" TB_ITEMPROPOP.lotereservaITEMPROPOSTA, TB_ITEMPROPOP.localloteITEMPROPOSTA, TB_ITEMPROPOP.codigoprodutoITEMPROPOSTA,");
            sql01.Append(" TB_PROD.ean13PRODUTO, TB_PROD.partnumberPRODUTO,TB_PROD.descricaoPRODUTO, TB_PROD.identificacaolotePRODUTO, TB_PROD.codigolotePRODUTO, TB_PROD.codigolocalPRODUTO,");
            sql01.Append(" TB_PROD.nomelocalPRODUTO");
            sql01.Append(" FROM   tb0001_Propostas AS TB_PROP ");
            sql01.Append(" INNER JOIN tb0002_ItensProposta AS TB_ITEMPROPOP ON TB_PROP.codigoPROPOSTA = TB_ITEMPROPOP.propostaITEMPROPOSTA");
            sql01.Append(" INNER JOIN tb0003_Produtos AS TB_PROD ON TB_ITEMPROPOP.codigoprodutoITEMPROPOSTA = TB_PROD.codigoPRODUTO");
            sql01.Append(" WHERE TB_ITEMPROPOP.statusseparadoITEMPROPOSTA = 0");
            sql01.Append(" ORDER BY TB_PROD.nomelocalPRODUTO ASC");

            SqlCeDataReader dr = CeSqlServerConn.fillDataReaderCe(sql01.ToString());

            int i = 0;

            if ((dr != null))
            {
                while ((dr.Read()))
                {
                    i++;
                    if (i == 1)
                    {
                        int statusOrdemSeparacao = Convert.ToInt32(dr["ordemseparacaoimpressaPROPOSTA"]);
                        objProposta = new Proposta(Convert.ToInt64(dr["codigoPROPOSTA"]), (string)dr["numeroPROPOSTA"], (string)dr["dataLIBERACAOPROPOSTA"],
                                               Convert.ToInt32(dr["clientePROPOSTA"]), (string)dr["razaoclientePROPOSTA"], Convert.ToInt32(dr["volumesPROPOSTA"]),Convert.ToInt32(dr["codigopickingmobilePROPOSTA"]));

                    }

                    int statusSeparadoItem = Convert.ToInt32(dr["statusseparadoITEMPROPOSTA"]);

                    ProdutoProposta objProdProp = new ProdutoProposta(Convert.ToInt32(dr["codigoITEMPROPOSTA"]),
                                                                        Convert.ToInt32(objProposta.Codigo),
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

                    listProd.Add(objProdProp);
                }

                objProposta = new Proposta(objProposta, listProd);

            }

            SqlServerConn.closeConn();

            return objProposta;
        }

        /// <summary>
        /// Recupera a proposta TOP 1 e devolve um objeto do tipo Proposta com as informações resultantes.
        /// </summary>
        /// <returns>Objeto do tipo Proposta</returns>
        public Proposta fillTop1PropostaServidor()
        {

            sql01 = new StringBuilder();
            sql01.Append("SELECT TOP (1) codigoPROPOSTA,numeroPROPOSTA,dataLIBERACAOPROPOSTA,");
            sql01.Append("clientePROPOSTA,razaoEMPRESA,volumesPROPOSTA,codigoPICKINGMOBILE,isinterrompidoPICKINGMOBILE");
            sql01.Append(" FROM vwMobile_tb1601_Proposta ");
            //sql01.Append(" ORDER BY  Prioridade ASC,dataLIBERACAOPROPOSTA ASC ");

            SqlDataReader dr = SqlServerConn.fillDataReader(sql01.ToString());

            while ((dr.Read()))
            {
                objProposta = new Proposta(Convert.ToInt64(dr["codigoPROPOSTA"]), (string)dr["numeroPROPOSTA"], (string)dr["dataLIBERACAOPROPOSTA"],
                                         Convert.ToInt32(dr["clientePROPOSTA"]), (string)dr["razaoEMPRESA"], Convert.ToInt32(dr["volumesPROPOSTA"]), Convert.ToInt32(dr["codigoPICKINGMOBILE"]),Convert.ToBoolean (dr["isinterrompidoPICKINGMOBILE"]));


                if (objProposta.IsInterrompido) 
                {
                    objProposta.IsInterrompido = false;
                }
            }

            dr.Close();
            SqlServerConn.closeConn();
            return objProposta;
        }

        /// <summary>
        ///  Efetua carga na base mobile tb0001_Propostas com informações sobre a 
        ///  proposta top1 atualmente liberada no picking
        /// </summary>
        /// <param name="cat">Gatinho do método. </param>
        /// <remarks>Após realizar a consulta realiza a carga dos dados  na base mobile tabela TB0001_Propostas</remarks>
        public void insertTop1PropostaServidor(String cat)
        {

            sql01 = new StringBuilder();
            sql01.Append("SELECT codigoPROPOSTA,numeroPROPOSTA,dataLIBERACAOPROPOSTA,");
            sql01.Append("clientePROPOSTA,razaoEMPRESA,volumesPROPOSTA");
            sql01.Append(" FROM vwMobile_tb1601_Proposta ");

            SqlDataReader dr = SqlServerConn.fillDataReader(sql01.ToString());

            if ((dr.FieldCount > 0))
            {
                while ((dr.Read()))
                {
                    this.insertProposta(Convert.ToInt64(dr["codigoPROPOSTA"]), (string)dr["numeroPROPOSTA"], (string)dr["dataLIBERACAOPROPOSTA"],
                                             Convert.ToInt32(dr["clientePROPOSTA"]), (string)dr["razaoEMPRESA"],
                                             Convert.ToInt32(dr["volumesPROPOSTA"]),
                                             MainConfig.CodigoUsuarioLogado);
                }
            }

            dr.Close();
            SqlServerConn.closeConn();
        }

        /// <summary>
        /// Realiza o insert na tabela de Propostas
        /// </summary>
        /// <param name="codigoProposta">Código da Proposta</param>
        /// <param name="numeroProposta">Número da Proposta</param>
        /// <param name="dataliberacaoProposta">data de liberação da Proposta</param>
        /// <param name="clienteProposta">Código do cliente</param>
        /// <param name="razaoEmpreza">Nome da empreza cliente</param>
        /// <param name="ordemseparacaoimpresaProposta">Status 0 ou 1</param>
        /// <param name="UsuarioLogado1">Usuário logado</param>
        public void insertProposta(Proposta proposta, int usuarioLogado)
        {

            CeSqlServerConn.execCommandSqlCe("DELETE FROM tb0001_Propostas");

            //Query de insert na Base Mobile
            sql01 = new StringBuilder();
            sql01.Append("Insert INTO tb0001_Propostas");
            sql01.Append("(codigoPROPOSTA,numeroPROPOSTA,dataliberacaoPROPOSTA,clientePROPOSTA,razaoclientePROPOSTA,volumesPROPOSTA,codigopickingmobilePROPOSTA,operadorPROPOSTA)");
            sql01.Append(" VALUES (");
            sql01.AppendFormat("{0},", proposta.Codigo);
            sql01.AppendFormat("\'{0}\',",proposta.Numero);
            sql01.AppendFormat("\'{0}\',", proposta.DataLiberacao);
            sql01.AppendFormat("{0},", proposta.CodigoCliente);
            sql01.AppendFormat("\'{0}\',",proposta.RazaoCliente);
            sql01.AppendFormat("{0},",proposta.Volumes);
            sql01.AppendFormat("{0},", proposta.CodigoPikingMobile);
            sql01.AppendFormat("{0})", usuarioLogado);

            CeSqlServerConn.execCommandSqlCe(sql01.ToString());

        }

        /// <summary>
        /// Realiza o insert na tabela de Propostas
        /// </summary>
        /// <param name="codigoProposta">Código da Proposta</param>
        /// <param name="numeroProposta">Número da Proposta</param>
        /// <param name="dataliberacaoProposta">data de liberação da Proposta</param>
        /// <param name="clienteProposta">Código do cliente</param>
        /// <param name="razaoEmpreza">Nome da empreza cliente</param>
        /// <param name="ordemseparacaoimpresaProposta">Status 0 ou 1</param>
        /// <param name="UsuarioLogado1">Usuário logado</param>
        public void insertProposta(Int64 codigoProposta, string numeroProposta, string dataliberacaoProposta, Int32 clienteProposta,
                                    string razaoEmpresa, int volumesProposta, int usuarioLogado)
        {

            CeSqlServerConn.execCommandSqlCe("DELETE FROM tb0001_Propostas");

            //Query de insert na Base Mobile
            sql01 = new StringBuilder();
            sql01.Append("Insert INTO tb0001_Propostas");
            sql01.Append("(codigoPROPOSTA,numeroPROPOSTA,dataliberacaoPROPOSTA,clientePROPOSTA,razaoclientePROPOSTA,volumesPROPOSTA,operadorPROPOSTA)");
            sql01.Append(" VALUES (");
            sql01.AppendFormat("{0},", codigoProposta);
            sql01.AppendFormat("\'{0}\',", numeroProposta);
            sql01.AppendFormat("\'{0}\',", dataliberacaoProposta);
            sql01.AppendFormat("{0},", clienteProposta);
            sql01.AppendFormat("\'{0}\',", razaoEmpresa);
            sql01.AppendFormat("{0},", volumesProposta);
            sql01.AppendFormat("{0})", usuarioLogado);

            CeSqlServerConn.execCommandSqlCe(sql01.ToString());

        }

        /// <summary>
        /// Realiza o Insert na tabela de picking Mobile
        /// </summary>
        /// <param name="codigoProposta">Codigo da proposta a ser trabalhada</param>
        /// <param name="usuarioProposta">Usuário trabalhando a proposta</param>
        /// <param name="statusLiberacao">status atual de liberação da proposta</param>
        public void insertPropostaTbPickingMobile(long codigoProposta,int usuarioProposta,Proposta.StatusLiberacao statusLiberacao,DateTime horaInicio) 
        {
            sql01 = new StringBuilder();
            sql01.Append("Insert INTO tb1651_Picking_Mobile");
            sql01.Append("(propostaPICKINGMOBILE,usuarioPICKINGMOBILE,statusPICKINGMOBILE,horainicioPICKINGMOBILE,horafimPICKINGMOBILE)");
            sql01.Append(" VALUES (");
            sql01.AppendFormat("{0},", codigoProposta);
            sql01.AppendFormat("\'{0}\',", usuarioProposta);
            sql01.AppendFormat("\'{0}\',", (int)statusLiberacao);
            sql01.AppendFormat("\'{0}\',", horaInicio);
            sql01.AppendFormat("{0})", "NULL");

            SqlServerConn.execCommandSql(sql01.ToString());

        }

        /// <summary>
        /// Realiza o Insert na tabela de picking Mobile
        /// </summary>
        /// <param name="codigoProposta">Codigo da proposta a ser trabalhada</param>
        /// <param name="usuarioProposta">Usuário trabalhando a proposta</param>
        /// <param name="statusLiberacao">status atual de liberação da proposta</param>
        public void insertPropostaTbPickingMobile(int codigoProposta, int usuarioProposta, Proposta.StatusLiberacao statusLiberacao, DateTime horaInicio,DateTime horafim)
        {

            sql01 = new StringBuilder();
            sql01.Append("Insert INTO tb1651_Picking_Mobile");
            sql01.Append("(propostaPICKINGMOBILE,usuarioPICKINGMOBILE,statusPICKINGMOBILE,horainicioPICKINGMOBILE,horafimPICKINGMOBILE)");
            sql01.Append(" VALUES (");
            sql01.AppendFormat("{0},", codigoProposta);
            sql01.AppendFormat("\'{0}\',", usuarioProposta);
            sql01.AppendFormat("\'{0}\',", statusLiberacao);
            sql01.AppendFormat("\'{0}\',", horaInicio);
            sql01.AppendFormat("{0})", horafim);

            SqlServerConn.execCommandSql(sql01.ToString());

        }

        public int selectMaxCodigoPickingMobile(long codigoProposta) 
        {
            sql01 = new StringBuilder();
            sql01.Append("SELECT MAX(codigoPICKINGMOBILE) AS maxCodigo FROM tb1651_Picking_Mobile");
            sql01.AppendFormat(" WHERE propostaPICKINGMOBILE = {0}", codigoProposta);
            dr = SqlServerConn.fillDataReader(sql01.ToString());

            if(dr != null)
            {
                while(dr.Read())
                {
                    return Convert.ToInt32((dr["maxCodigo"]));
                }
            }

            dr.Close();
            return 0;
        }

        public void updatePropostaTbPickingMobileFinalizar(Proposta proposta, Proposta.StatusLiberacao statusPKMobile)
        {
           
            sql01 = new StringBuilder();
            sql01.Append("UPDATE tb1651_Picking_Mobile");
            sql01.Append(" SET");
            sql01.AppendFormat("[statusPICKINGMOBILE] = {0}", (int)statusPKMobile);
            sql01.AppendFormat(",[horafimPICKINGMOBILE] = '{0}'", DateTime.Now.ToString());
            sql01.AppendFormat(" WHERE propostaPICKINGMOBILE = {0} ", proposta.Codigo);
            sql01.AppendFormat(" AND codigoPICKINGMOBILE = {0}", proposta.CodigoPikingMobile);
            SqlServerConn.execCommandSql(sql01.ToString());
        }

        public void updatePropostaTbPickingMobile(Proposta proposta, Proposta.StatusLiberacao statusPKMobile,String horaFim)
        {

            if(horaFim.ToUpper() != "NULL")
            {
                try
                {
                    System.Globalization.CultureInfo culture = new CultureInfo("pt-BR");
                    horaFim = Convert.ToDateTime(horaFim, culture).ToString();
                }
                catch (Exception)
                {
                    horaFim = DateTime.Now.ToString();
                }

            }

            try
            {
                sql01 = new StringBuilder();
                sql01.Append("UPDATE tb1651_Picking_Mobile");
                sql01.Append(" SET");
                sql01.AppendFormat("[statusPICKINGMOBILE] = {0}", (int)statusPKMobile);
                sql01.AppendFormat(",[isinterrompidoPICKINGMOBILE] = {0}",Convert.ToInt16(proposta.IsInterrompido));
                if (horaFim.ToUpper() == "NULL")
                {
                    sql01.AppendFormat(",[horafimPICKINGMOBILE] = {0}", horaFim);
                }
                else
                {
                    sql01.AppendFormat(",[horafimPICKINGMOBILE] = '{0}'", horaFim);
                }

                sql01.AppendFormat(" WHERE propostaPICKINGMOBILE = {0} ", proposta.Codigo);
                sql01.AppendFormat(" AND codigoPICKINGMOBILE = {0}", proposta.CodigoPikingMobile);
                SqlServerConn.execCommandSql(sql01.ToString());
            }
            catch (Exception e)
            {
                
                throw new Exception("Problemas durante atualização de dados da proposta. ",e);
            }

        }

        public void updatePropostaTbPickingMobile(Proposta proposta, Proposta.StatusLiberacao statusPKMobile, DateTime horaFim)
        {
            sql01 = new StringBuilder();
            sql01.Append("UPDATE tb1651_Picking_Mobile");
            sql01.Append(" SET ");
            sql01.AppendFormat("[statusPICKINGMOBILE] = {0}", (int)statusPKMobile);
            sql01.AppendFormat(",[horafimPICKINGMOBILE] = '{0}'",horaFim);
            sql01.AppendFormat(" WHERE propostaPICKINGMOBILE = {0}", proposta.Codigo);
            sql01.AppendFormat(" AND codigoPICKINGMOBILE = {0}", proposta.CodigoPikingMobile);
            SqlServerConn.execCommandSql(sql01.ToString());
        }

        public void updatePropostaTbPickingMobile(Proposta proposta, Proposta.StatusLiberacao statusPKMobile,DateTime horaInicio, DateTime horaFim)
        {
            sql01 = new StringBuilder();
            sql01.Append("UPDATE tb1651_Picking_Mobile");
            sql01.Append("SET");
            sql01.AppendFormat("[statusPICKINGMOBILE] = {0}", statusPKMobile);
            sql01.AppendFormat(",[horafimPICKINGMOBILE] = {0}", horaInicio);
            sql01.AppendFormat(",[horafimPICKINGMOBILE] = {0}", horaFim);
            sql01.AppendFormat(" WHERE propostaPICKINGMOBILE = ", proposta.Codigo);
            sql01.AppendFormat(" AND codigoPICKINGMOBILE = {0}", proposta.CodigoPikingMobile);
            SqlServerConn.execCommandSql(sql01.ToString());
        }

        public void updatePropostaTbPickingMobile(Proposta proposta, Proposta.StatusLiberacao statusPKMobile, String horaInicio, String horaFim)
        {
            

            sql01 = new StringBuilder();
            sql01.Append("UPDATE tb1651_Picking_Mobile");
            sql01.Append("SET");
            sql01.AppendFormat("[statusPICKINGMOBILE] = {0}", statusPKMobile);
            sql01.AppendFormat(",[horafimPICKINGMOBILE] = {0}", (horaInicio != null)?horaInicio: "[horafimPICKINGMOBILE]" );
            sql01.AppendFormat(",[horafimPICKINGMOBILE] = {0}", horaFim);
            sql01.AppendFormat(" WHERE propostaPICKINGMOBILE = ", proposta.Codigo);
            sql01.AppendFormat(" AND codigoPICKINGMOBILE = {0}", proposta.CodigoPikingMobile);
            SqlServerConn.execCommandSql(sql01.ToString());
        }

        /// <summary>
        /// Preenche um objeto List com informações sobre a proposta que está sendo trabalhada.
        /// </summary>
        /// <returns>Objeto List do tipo String com informações da atual proposta na base de dados mobile.</returns>
        /// <remarks>
        ///            O list contém as seguintes informações
        ///            list.Add(dr["CodProp"].ToString());
        ///            list.Add(dr["NumProp"].ToString());
        ///            list.Add(dr["nomeCLIENTE"].ToString());
        ///            list.Add(dr["qtdPECAS"].ToString());
        ///            list.Add(dr["qtdITENS"].ToString());
        /// </remarks>
        public List<String> fillInformacoesProposta()
        {
            List<String> list = new List<String>();

            sql01 = new StringBuilder();
            sql01.Append("SELECT PROP.codigoPROPOSTA AS CodProp, PROP.numeroPROPOSTA as NumProp, PROP.razaoclientePROPOSTA AS nomeCLIENTE, ");
            sql01.Append("SUM(ITEMPROP.quantidadeITEMPROPOSTA) AS qtdPECAS, ");
            sql01.Append("COUNT(*) AS qtdITENS,PROP.volumesPROPOSTA AS qtdVOLUMES ");
            sql01.Append("FROM tb0001_Propostas AS PROP ");
            sql01.Append("INNER JOIN tb0002_ItensProposta AS ITEMPROP ON PROP.codigoPROPOSTA = ITEMPROP.propostaITEMPROPOSTA ");
            sql01.Append("GROUP BY PROP.codigoPROPOSTA, PROP.numeroPROPOSTA, PROP.razaoclientePROPOSTA,PROP.volumesPROPOSTA");
            sql01.ToString();

            SqlCeDataReader dr = CeSqlServerConn.fillDataReaderCe(sql01.ToString());

            if ((dr.FieldCount > 0))
            {
                while ((dr.Read()))
                {
                    list.Add(dr["CodProp"].ToString());
                    list.Add(dr["NumProp"].ToString());
                    list.Add(dr["nomeCLIENTE"].ToString());
                    list.Add(dr["qtdPECAS"].ToString());
                    list.Add(dr["qtdITENS"].ToString());
                    list.Add(dr["qtdVOLUMES"].ToString());

                }
            }

            SqlServerConn.closeConn();

            return list;
        }

        /// <summary>
        /// Preenche um objeto do tipo Proposta com todas as suas informações e com o item Top 1 da base de dados MOBILE
        /// de acordo com o campo Nome Local e o status de separado = 0; (NAOSEPARADO)
        /// </summary>
        /// <returns>Objeto do tipo Proposta</returns>
        public Proposta fillPropostaWithTop1Item()
        {
            Proposta objProposta = null;
            Proposta objAux = null;

            List<ProdutoProposta> listProd = new List<ProdutoProposta>();

            sql01 = new StringBuilder();

            sql01.Append(" SELECT TOP (1) TB_PROP.codigoPROPOSTA, TB_PROP.numeroPROPOSTA, TB_PROP.dataliberacaoPROPOSTA,TB_PROP.clientePROPOSTA, TB_PROP.razaoclientePROPOSTA,");
            sql01.Append("TB_PROP.volumesPROPOSTA,TB_PROP.codigopickingmobilePROPOSTA,");
            sql01.Append("TB_ITEMPROPOP.codigoITEMPROPOSTA, TB_ITEMPROPOP.propostaITEMPROPOSTA, TB_ITEMPROPOP.quantidadeITEMPROPOSTA, TB_ITEMPROPOP.statusseparadoITEMPROPOSTA,");
            sql01.Append("TB_ITEMPROPOP.lotereservaITEMPROPOSTA, TB_ITEMPROPOP.codigoprodutoITEMPROPOSTA,");
            sql01.Append("TB_PROD.ean13PRODUTO, TB_PROD.partnumberPRODUTO,TB_PROD.descricaoPRODUTO, TB_PROD.identificacaolotePRODUTO, TB_PROD.codigolotePRODUTO,TB_PROD.nomelocalPRODUTO");
            sql01.Append(" FROM   tb0001_Propostas AS TB_PROP ");
            sql01.Append(" INNER JOIN tb0002_ItensProposta AS TB_ITEMPROPOP ON TB_PROP.codigoPROPOSTA = TB_ITEMPROPOP.propostaITEMPROPOSTA ");
            sql01.Append(" INNER JOIN tb0003_Produtos AS TB_PROD ON TB_ITEMPROPOP.codigoprodutoITEMPROPOSTA = TB_PROD.codigoPRODUTO ");
            sql01.Append(" WHERE TB_ITEMPROPOP.statusseparadoITEMPROPOSTA = 0 ");
            sql01.Append(" GROUP BY TB_PROP.codigoPROPOSTA, TB_PROP.numeroPROPOSTA, TB_PROP.dataliberacaoPROPOSTA,TB_PROP.clientePROPOSTA, TB_PROP.razaoclientePROPOSTA,");
            sql01.Append("TB_PROP.volumesPROPOSTA,TB_PROP.codigopickingmobilePROPOSTA,");
            sql01.Append("TB_ITEMPROPOP.codigoITEMPROPOSTA, TB_ITEMPROPOP.propostaITEMPROPOSTA, TB_ITEMPROPOP.quantidadeITEMPROPOSTA, TB_ITEMPROPOP.statusseparadoITEMPROPOSTA,");
            sql01.Append("TB_ITEMPROPOP.lotereservaITEMPROPOSTA, TB_ITEMPROPOP.codigoprodutoITEMPROPOSTA,");
            sql01.Append("TB_PROD.ean13PRODUTO, TB_PROD.partnumberPRODUTO,TB_PROD.descricaoPRODUTO, TB_PROD.identificacaolotePRODUTO, TB_PROD.codigolotePRODUTO,TB_PROD.nomelocalPRODUTO");
            sql01.Append(" ORDER BY nomelocalPRODUTO ASC");

            SqlCeDataReader dr = CeSqlServerConn.fillDataReaderCe(sql01.ToString());

            int i = 0;

            if ((dr != null))
            {
                while ((dr.Read()))
                {
                    i++;
                    if (i == 1)
                    {
                        objProposta = new Proposta(Convert.ToInt64(dr["codigoPROPOSTA"]), (string)dr["numeroPROPOSTA"], (string)dr["dataLIBERACAOPROPOSTA"],
                                                   Convert.ToInt32(dr["clientePROPOSTA"]), (string)dr["razaoclientePROPOSTA"], Convert.ToInt32(0), Convert.ToInt32(dr["codigopickingmobilePROPOSTA"]));

                    }

                    int statusSeparadoItem = Convert.ToInt32(dr["statusseparadoITEMPROPOSTA"]);
                    ProdutoProposta objProdProp = new ProdutoProposta(Convert.ToInt32(dr["codigoITEMPROPOSTA"]),
                                                                        Convert.ToInt32(objProposta.Codigo),
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

                    listProd.Add(objProdProp);
                }

                objAux = new Proposta(objProposta,listProd);
                //objProposta = new Proposta(objProposta, listProd);

            }

            CeSqlServerConn.closeConnCe();

            return objAux;
        }

        public void updateVolumeProposta(Int64 codProposta) 
        {
            sql01 = new StringBuilder();
            sql01.Append("UPDATE tb1601_Propostas");
            sql01.AppendFormat(" SET volumesPROPOSTA = {0} ",Procedimentos.ProcedimentosLiberacao.TotalVolumes);
            sql01.AppendFormat(" WHERE codigoPROPOSTA = {0} " ,codProposta.ToString());
            SqlServerConn.execCommandSql(sql01.ToString());

        }

        public void InsertOrUpdatePickingMobile(Proposta proposta, int usuarioProposta, Proposta.StatusLiberacao statusLiberacao, DateTime horaInicio) 
        {
            if (proposta.CodigoPikingMobile == 0)
            {
                insertPropostaTbPickingMobile(proposta.Codigo,usuarioProposta,statusLiberacao,horaInicio);
            }
            else 
            {
                updatePropostaTbPickingMobile(proposta, statusLiberacao,"NULL");
            }
        }

        public void retiraPropostaListaPrioridade(Int64 codigoProposta,Int32 usuarioProposta) 
        {

            sql01 = new StringBuilder();
            sql01.AppendFormat("EXECUTE sps1601_manipulaPRIORIDADEPICKING {0},{1},{2}",2,codigoProposta,usuarioProposta);
            SqlServerConn.execCommandSql(sql01.ToString());

        }
    }
}

