using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections;
using TitaniumColector.Utility;
using System.IO;
using Microsoft.VisualBasic;
using TitaniumColector.Classes.Exceptions;


namespace TitaniumColector.SqlServer
{
    static class SqlServerConn
    {
        private static SqlConnection  conn = null;
        private static SqlTransaction transaction = null;
        private static string strPassword;
        private static string strUserId;
        private static string booSecurity;
        private static string strCatalog;
        private static string strDataSource;
        private static string strConnection;

        #region "Get & Set"


        private static string Password
        {
            get { return strPassword; }

            set
            {
                if ((!string.IsNullOrEmpty(value)))
                {
                    strPassword = value.Trim();
                }
            }
        }

        private static string Security
        {
            get { return booSecurity; }

            set
            {
                if ((!string.IsNullOrEmpty(value)))
                {
                    booSecurity = value;
                }
            }
        }

        private static string UserId
        {
            get { return strUserId; }

            set
            {
                if ((!string.IsNullOrEmpty(value)))
                {
                    strUserId =  value.Trim();
                }
            }
        }

        private static string Catalog
        {
            get { return strCatalog; }

            set
            {
                if ((!string.IsNullOrEmpty(value)))
                {
                    strCatalog = value.Trim();
                }
            }
        }

        private static string DataSource
        {
            get { return strDataSource; }

            set
            {
                if ((!string.IsNullOrEmpty(value)))
                {
                    strDataSource = value.Trim();
                }
            }
        }

        private static string StringConection
        {
            get { return strConnection; }
        }

        #endregion 

        /// <summary>
        /// Recupera todos aos parâmetros informados e configura a string de conexão.
        /// </summary>
        private static void makeStrConnection()
        {
            SqlServerConn.strConnection = "Password=" + Password + ";Persist Security Info=" + Security + ";User ID=" + UserId + ";Initial Catalog=" + Catalog + ";Data Source=" + DataSource;
        }

        public static SqlConnection openConn()
        {
           
            conn = new SqlConnection(StringConection);

            try
            {
                conn.Open();
            }
            catch (SqlException sqlEx)
            {   
                if(sqlEx.Number == 11 )
                {
                    throw new Exception("Problemas a recuperar informações da base de dados.\nError:" + sqlEx.Message,sqlEx);
                }

                throw sqlEx;
            }

            return conn;
        }

        /// <summary>
        /// Testa a conexão com o banco de dados SQLSERVER.
        /// </summary>
        /// <returns>True caso a conexão esteja OK.</returns>
        /// <exception cref="System.SqlException">Caso não seja possível se comunicar com o banco de dados.</exception>
        public static bool testConnection() 
        {
            bool result = false;

            try
            {
                if (openConn().State == ConnectionState.Open)
                {
                    result = true;
                }

                if (result == true) 
                {
                    closeConn();

                    if (conn.State == ConnectionState.Closed)
                    {
                        result = true;
                    }

                }

                return result;
            }
            catch(SqlException) 
            {
                conn = null;
                throw;
            }
            
        }

        /// <summary>
        /// Fecha a conexão
        /// </summary>
        public static void closeConn()
        {
            try
            {
                if ((conn.State == ConnectionState.Open))
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorre um problema na conexão com a base de dados." +  "Erro : " + ex.Message);
            }

        }

        public static void fillDataSet(DataSet ds, string sql01)
        {

            openConn();

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql01, conn);
                da.Fill(ds);
                closeConn();
            }
            catch (Exception ex)
            {
                closeConn();
                throw new Exception("Ocorre um problema na conexão com a base de dados." + Environment.NewLine + "Erro : " + ex.Message);
            }

        }

        /// <summary>
        /// Preenche um dataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sql01"></param>
        public static void fillDataTable(DataTable dt, string sql01)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql01, openConn());
                da.Fill(dt);

            }
            catch (SqlException sqlEx)
            {
                if ((sqlEx.Number == 11))
                {
                    throw new Exception("Não foi possível se comunicar com a base de dados devido problemas com a rede." + Environment.NewLine  + "Erro : " + sqlEx.Message + Environment.NewLine + "Number:" + sqlEx.Number);
                }
                else
                {
                    throw new Exception("Problemas durante a carga do DataTable:" + Environment.NewLine + "Erro : " + sqlEx.Message + Environment.NewLine + "Number:" + sqlEx.Number);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Ocorre um problema na conexão com a base de dados." + Environment.NewLine + "Erro : " + ex.Message);

            }
            finally
            {
                closeConn();
            }

        }

        /// <summary>
        /// Preenche um objeto DataReader
        /// </summary>
        /// <param name="sql01">Query a ser executada</param>
        /// <returns>SqlDataReader preenchido</returns>
        public static SqlDataReader fillDataReader(string sql01)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sql01, openConn());
                SqlDataReader dr = cmd.ExecuteReader();
                return dr;
            }
            catch (SqlException ex)
            {
                throw new SqlQueryExceptions("\nErro: " + ex.Message,ex);
            }

        }

        public static void execCommandSql(string sql01)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sql01, openConn());
                cmd.ExecuteNonQuery();
            }
            catch (SqlException )
            {
                throw new SqlQueryExceptions("Error durante acesso a base de dados!!");
            }
            finally
            {
                closeConn();
            }

        }

        public static SqlCommand beginTransaction(SqlConnection conn)
        {
           
            try
            {
                if ((conn.State == ConnectionState.Open))
                {
                    SqlCommand command = conn.CreateCommand();
                    transaction = conn.BeginTransaction("SampleTransaction");
                    command.Connection = conn;
                    command.Transaction = transaction;
                    return command;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                transaction.Commit();
                transaction.Dispose();
                closeConn();
                throw new Exception("Ocorre um problema na conexão com a base de dados." + Environment.NewLine + "Erro : " + ex.Message);
            }

            return null;
        }

        public static void EndTransaction(ref bool flag)
        {
            if (flag == false)
            {
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }

        }

        /// <summary>
        /// Lê um arquivo de texto que esteja na pasta padrão da aplicação, e que tenha o Nome especificado no parâmetro fileName
        /// </summary>
        /// <param name="fileName">Nome do arquivo a ser lido</param>
        /// <returns>Uma string contendo o texto existente no arquivo.</returns>
        /// <remarks> Caso o arquivo não seja encontrado o método retornará o Valor null.
        /// </remarks>
        public static string readFileStrConnection(string fileName) 
        {
            string pathAplicativo = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

            if (File.Exists(pathAplicativo + fileName))
            {
                FileUtility fU = new FileUtility(pathAplicativo, fileName);
                List<string> fileStrConn = new List<string>(fU.readTextFile());
                string strConnection= fileStrConn[0];
                return strConnection;
            }

            return null;
        }

        /// <summary>
        ///  Lê um arquivo de texto que esteja no path informado no parâmetro mobilePath, e que tenha o Nome especificado no parâmetro fileName
        /// </summary>
        /// <param name="mobilePath">Diretório do dispositivo onde será buscado o arquivo</param>
        /// <param name="fileName">Nome do arquivo a ser buscado</param>
        /// <exception cref="System.FileNotFoundException">Lançada quando o arquivo não for encontrado</exception>
        /// <returns> Uma string contendo o texto existente no arquivo.</returns>
        /// <remarks> Caso o arquivo não seja encontrado o método retornará o Valor null.
        /// </remarks>
        public static string readFileStrConnection(string mobilePath,string fileName)
        {
            try
            {
                FileUtility fU = new FileUtility(mobilePath, fileName);
                
                if (File.Exists(fU.getFullPath()))
                {
                    List<string> fileStrConn = new List<string>(fU.readTextFile());
                    return fileStrConn[0];
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
            catch (FileNotFoundException FileEx) 
            {
                throw new FileNotFoundException("Problemas durante a configuração da string de conexão." + Environment.NewLine + 
                                                 "Favor contate o administrador do sistema." + Environment.NewLine + 
                                                 "Erro :" + FileEx.Message);

            }
            catch (Exception ex)
            {
                throw new Exception("Problemas durante a configuração da string de conexão." + Environment.NewLine + 
                                    "Favor contate o administrador do sistema." + Environment.NewLine + 
                                    "Erro :" + ex.Message);
            }

        }

        /// <summary>
        /// Realiza o processo de configuração da string de conexão.
        /// </summary>
        /// <param name="mobilePath">Path do dispositivo onde está armazenado o arquivo de texto contendo a string de conexão.</param>
        /// <param name="fileName">Nome do arquivo de texto onde está armazenado a string de Conexão.</param>
        public static void configuraStrConnection(string mobilePath,string fileName) 
        {
            try
            {
                string strConnection = readFileStrConnection(mobilePath, fileName);
                string[] arrayStrConnection = FileUtility.arrayOfTextFile(strConnection, FileUtility.splitType.PONTO_VIRGULA);
                setParametersStringConnection(arrayStrConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Monta a string de conexão a partir de um array contendo os dados nescessários.
        /// </summary>
        /// 
        /// <param name="array"> Array preenchido com o padrão sql de string de conexão</param>
        /// 
        /// <exemplo>
        ///  Itens padrão a ser usada : 
        ///  Provider=SQLOLEDB.1
        ///  Password=senha
        ///  Persist Security Info=False/True
        ///  User ID=usuario
        ///  Initial Catalog=basededados
        ///  Data Source=ip
        /// </exemplo>
        public static void setParametersStringConnection(string[] array)
        {
            foreach (string item in array)
            {

                string strItem = item.Substring(0, item.IndexOf("=", 0));

                if (strItem == "Password")
                {
                    Password = item.Substring(item.IndexOf("=", 0) + 1);
                }
                else if (strItem == "Persist Security Info")
                {
                    Security = item.Substring(item.IndexOf("=", 0) + 1);
                }
                else if (strItem == "User ID")
                {
                    UserId = item.Substring(item.IndexOf("=", 0) + 1);
                }
                else if (strItem == "Initial Catalog")
                {
                    Catalog = item.Substring(item.IndexOf("=", 0) + 1);
                }
                else if (strItem == "Data Source")
                {
                    DataSource = item.Substring(item.IndexOf("=", 0) + 1);
                }
            }
	        //'Monta a string de conexão
	        makeStrConnection();

        }

        /// <summary>
        /// Redefine os atributos da string de conexão
        /// </summary>
        /// <param name="strPassword">Senha</param>
        /// <param name="strUserID">Usuário</param>
        /// <param name="strInitialCatalog">Base de dados</param>
        /// <param name="strDataSource">IP/HostName</param>
        /// <param name="booSecurity">True ou False</param>
        /// <remarks></remarks>
        public static void setParametersStringConnection(string strPassword, string strUserID, string strInitialCatalog, string strDataSource, string booSecurity)
        {
	        //'Monta a string de conexão
	        Password= strPassword;
	        UserId = strUserID;
	        Catalog = strCatalog;
	        DataSource = strDataSource;
	        Security = booSecurity;
	        makeStrConnection();

        }
    }  
}
