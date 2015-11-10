using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;
using TitaniumColector.SqlServer;
using System.Data.SqlClient;

namespace TitaniumColector.Classes.Dao
{
    class DaoUsuario
    {
        private StringBuilder sql01;
        private DataTable dt;

        public List<Usuario> retornaListUsuarios()
        {

            List<Usuario> listUsuario = new List<Usuario>();

            sql01 = new StringBuilder();
            sql01.Append(" SELECT codigoUSUARIO as Codigo,pastaUSUARIO as Pasta,nomeUSUARIO as Nome,");
            sql01.Append(" senhaUSUARIO as Senha,nomecompletoUSUARIO as NomeCompleto,ativoUSUARIO as StatusUsuario");
            sql01.Append(" FROM tb0201_usuarios ");
            sql01.Append(" ORDER BY nomeUSUARIO ");
            sql01.ToString();

            try
            {
                dt = new DataTable("Usuarios");
                DataRow dr = null;

                SqlServerConn.fillDataTable(dt, sql01.ToString());

                foreach (DataRow dr_loopVariable in dt.Rows)
                {
                    dr = dr_loopVariable;

                    Usuario objUsuario = new Usuario(Convert.ToInt32(dr["Codigo"]), Convert.ToInt32(dr["Pasta"]), (string)dr["Nome"], (string)dr["Senha"],
                        (string)dr["NomeCompleto"], (Usuario.statusUsuario)Convert.ToInt32(dr["StatusUsuario"]));

                    listUsuario.Add(objUsuario);
                }
                return listUsuario;
            }
            catch (Exception ex)
            { 
                throw ex;
            }

        }


    }
}
