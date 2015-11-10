using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TitaniumColector.Classes.Utility;
using System.Data;
using System.Data.SqlClient;
using TitaniumColector.SqlServer;
using TitaniumColector.Classes.Exceptions;

namespace TitaniumColector.Classes.Dao
{
    class DaoPermissoes
    {
        private StringBuilder sql01;
        //private DataTable dt;

        /// <summary>
        /// Recupera informações sobre as permissões existente no Titanium
        /// </summary>
        /// <param name="codigoPermissoes"> Lista de permissões a serem salvas na base mobile.</param>
        /// <returns>List com informações sobre os parâmetros das permissões a serem utilizadas no mobile. </returns>
        public Permissoes recuperarPermissoes(List<String> codigoPermissoes)
        {
            try
            {
                Permissoes permissoes = new Permissoes();
                Parametros param;

                sql01 = new StringBuilder();
                sql01.Append(" SELECT codigoPARAMETRO,descricaoPARAMETRO,valorPARAMETRO,COALESCE(auxiliarPARAMETRO,0) as auxiliarPARAMETRO FROM tb1210_Parametros");
                SqlDataReader dr = SqlServerConn.fillDataReader(sql01.ToString());

                while ((dr.Read()))
                {
                    //Valida as permissões contida na lista de permissões a serem utilizadas no mobile.
                    foreach (string item in codigoPermissoes)
                    {
                        if (item == (string)dr["codigoParametro"])
                        {
                            param = new Parametros((string)dr["codigoParametro"], (string)dr["descricaoPARAMETRO"], (string)dr["valorPARAMETRO"], Convert.ToInt32(dr["auxiliarPARAMETRO"]));
                            permissoes.addParametro(param);
                        }
                    }
                }

                return permissoes;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao recuperar parâmetros na base de dados.\nMotivo:" + ex.Message , ex);
            }
        }
    }
}
