using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using TitaniumColector.SqlServer;
using TitaniumColector.Classes.SqlServer;
using System.Data;
using System.Data.SqlServerCe;
using System.Windows.Forms;
using TitaniumColector.Classes.Exceptions;

namespace TitaniumColector.Classes
{
    class  BaseMobile
    {
        private static StringBuilder sql01;

    #region "CRIACAO BASE MOBILE "

        public void clearBaseMobile()
        {
            CeSqlServerConn.execCommandSqlCe("DELETE FROM tb0005_Embalagens");
            CeSqlServerConn.execCommandSqlCe("DELETE FROM tb0004_Etiquetas");
            CeSqlServerConn.execCommandSqlCe("DELETE FROM tb0003_Produtos");
            CeSqlServerConn.execCommandSqlCe("DELETE FROM tb0002_ItensProposta");
            CeSqlServerConn.execCommandSqlCe("DELETE FROM tb0001_Propostas");
 
        }

        /// <summary>
        /// Configura a conexão com a base mobile.
        /// Caso a base de dados ainda não exista ela será criada
        /// </summary>
        public static void configurarBaseMobile()
        {
            //Recupera arquivo dentro da pasta da aplicação
            if (System.IO.File.Exists("\\Program Files\\TitaniumColector\\EngineMobile.sdf"))
            {
                //Configura a string de conexão com a base mobile.
                CeSqlServerConn.createStringConectionCe("\\Program Files\\TitaniumColector\\EngineMobile.sdf", "tec9TIT16");
            }
            else //caso não encontre o arquivo é criado uma nova Base .mdf
            {
                String dataSource = "\\Program Files\\TitaniumColector\\EngineMobile.sdf";
                String senha = "tec9TIT16";
                String connectionString = string.Format("DataSource=\"{0}\"; Password='{1}'", dataSource, senha);
                SqlCeEngine SqlEng = new SqlCeEngine(connectionString);
                SqlEng.CreateDatabase();
                //Configura a string de conexão com a base mobile.
                CeSqlServerConn.createStringConectionCe(dataSource,senha);
                criarTabelas();
            }
        }
 
        /// <summary>
        /// Cria tabelas na base mobile.
        /// </summary>
        /// <remarks>A conexão com a base mobile já deve estar configurada.</remarks>
        public static void criarTabelas()
        {

            //TABELA tb0001_Propostas
            sql01 = new StringBuilder();
            sql01.Append("CREATE TABLE tb0001_Propostas (");
            sql01.Append("codigoPROPOSTA int not null CONSTRAINT PKPropostas Primary key,");
            sql01.Append("numeroPROPOSTA nvarchar(20) not null,");
            sql01.Append("dataliberacaoPROPOSTA nvarchar(20) not null,");
            sql01.Append("clientePROPOSTA int not null,");
            sql01.Append("razaoclientePROPOSTA nvarchar(200),");
            sql01.Append("volumesPROPOSTA smallint,");
            sql01.Append("operadorPROPOSTA int, ");
            sql01.Append("codigopickingmobilePROPOSTA INT) ");
            CeSqlServerConn.execCommandSqlCe(sql01.ToString());

            //TABELA tb0002_ItensProposta
            sql01.Length = 0;
            sql01.Append("CREATE TABLE tb0002_ItensProposta (");
            sql01.Append("codigoITEMPROPOSTA int not null CONSTRAINT PKItensProposta PRIMARY KEY,");
            sql01.Append("propostaITEMPROPOSTA int ,");
            sql01.Append("quantidadeITEMPROPOSTA real,");
            sql01.Append("pesoITEMPROPOSTA real,");
            sql01.Append("statusseparadoITEMPROPOSTA SMALLINT,");
            sql01.Append("codigoprodutoITEMPROPOSTA int,");
            sql01.Append("lotereservaITEMPROPOSTA int,");
            sql01.Append("alllotesreservaITEMPROPOSTA NVARCHAR(50),");
            sql01.Append("qtdembalagemITEMPROPOSTA real,");
            sql01.Append("allnomeslocaisITEMPROPOSTA NVARCHAR(50),");
            sql01.Append("xmlSequenciaITEMPROPOSTA nText)");
            CeSqlServerConn.execCommandSqlCe(sql01.ToString());

            //TABELA tb0003_Produtos
            sql01.Length = 0;
            sql01.Append("CREATE TABLE tb0003_Produtos (");
            sql01.Append("codigoPRODUTO				INT            NOT NULL ,");
            sql01.Append("ean13PRODUTO				NVARCHAR(15)   NOT NULL ,");
            sql01.Append("partnumberPRODUTO			NVARCHAR(100) ,");
            sql01.Append("descricaoPRODUTO			NVARCHAR(100) ,");
            sql01.Append("codigolotePRODUTO			INT,");
            sql01.Append("identificacaolotePRODUTO	NVARCHAR(100) ,");
            sql01.Append("codigolocalPRODUTO		INT , ");
            sql01.Append("nomelocalPRODUTO			NVARCHAR(100) )");
            CeSqlServerConn.execCommandSqlCe(sql01.ToString());

            //TABELA tb0004_ETIQUETA
            sql01.Length = 0;
            sql01.Append("CREATE TABLE tb0004_Etiquetas (");
            sql01.Append("codigoETIQUETA				    INT IDENTITY(1,1) NOT NULL CONSTRAINT PKSequencia PRIMARY KEY,");
            sql01.Append("eanitempropostaETIQUETA			NVARCHAR(20) NOT NULL,");
            sql01.Append("volumeETIQUETA	      			INT          NOT NULL,");
            sql01.Append("quantidadeETIQUETA	      		REAL         NOT NULL,");
            sql01.Append("sequenciaETIQUETA    			    INT          NOT NULL)");
            CeSqlServerConn.execCommandSqlCe(sql01.ToString());

            //TABELA TB0005_EMBALAGEM
            sql01.Length = 0;
            sql01.Append("CREATE TABLE tb0005_Embalagens (");
            sql01.Append("codigoEMBALAGEM				    INT  NOT NULL CONSTRAINT PKSequencia PRIMARY KEY,");
            sql01.Append("nomeEMBALAGEM         			NVARCHAR(20) NULL,");
            sql01.Append("produtoEMBALAGEM	      			INT          NOT NULL,");
            sql01.Append("quantidadeEMBALAGEM	      		REAL         NOT NULL,");
            sql01.Append("padraoEMBALAGEM    			    SMALLINT     NOT NULL,");
            sql01.Append("embalagemEMBALAGEM    			INT          NOT NULL,");
            sql01.Append("ean13EMBALAGEM    			    NVARCHAR(13) NOT NULL)");
            CeSqlServerConn.execCommandSqlCe(sql01.ToString());
        }

    #endregion

    }
}
