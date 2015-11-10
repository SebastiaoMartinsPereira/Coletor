using System;
using TitaniumColector.SqlServer;
using System.Text;

namespace TitaniumColector.Classes
{
    class Usuario
    {
        private int intCodigo;
        private int intPasta;
        private string strNome;
        private string strSenha;
        private string strNomeCompleto;
        private statusUsuario enumStatusUsuario;
        private statusLogin enumStatusLogin;
        private StringBuilder sql01;

    #region "ENUMS" 

        public enum usuarioProperty { CODIGO = 1, PASTA = 2, NOME = 3, SENHA = 4, NOMECOMPLETO = 5, STATUSUSUARIO = 6 }
        public enum statusLogin { LOGADO = 0, NAOLOGADO = 1 }
        public enum statusUsuario { DESATIVADO = 0, ATIVO = 1 }

    #endregion

    #region "GETS E SETS"

        public Usuario()
        {
        }

        public Usuario(int codigoUsuario, int pastaUsuario, string nomeUsuario, string senhaUsuario, string nomeCompletoUsuario, statusUsuario statusUsuario)
        {
            Codigo = codigoUsuario;
            Pasta = pastaUsuario;
            Nome = nomeUsuario;
            Senha = senhaUsuario;
            NomeCompleto = nomeCompletoUsuario;
            StatusUsuario = statusUsuario;
            StatusLogin = statusLogin.NAOLOGADO;
        }

        public Usuario(Usuario user)
        {
            Codigo = user.Codigo;
            Pasta = user.Pasta;
            Nome = user.Nome;
            Senha = user.Senha;
            NomeCompleto = user.NomeCompleto;
            StatusUsuario = user.StatusUsuario;
            StatusLogin = user.StatusLogin;
        }

        public int Codigo
        {
            get
            {
                return this.intCodigo;
            }

            set
            {

                if ((value != this.intCodigo))
                {
                    intCodigo = value;
                }

            }
        }

        public int Pasta
        {
            get
            {
                return intPasta;
            }

            set
            {
                if ((value != this.intPasta))
                {
                    intPasta = value;
                }
            }
        }

        public string Nome
        {
            get
            {
                return strNome;
            }

            set
            {
                if ((value != null))
                {
                    strNome = value.Trim();
                }
            }
        }

        public string Senha
        {
            get
            {
                return strSenha;
            }

            set
            {
                if ((value != null))
                {
                    strSenha = value.Trim();
                }
            }
        }

        public string NomeCompleto
        {

            get { return strNomeCompleto; }

            set
            {
                if ((value != null))
                {
                    strNomeCompleto = value.Trim();
                }
            }
        }

        internal statusLogin StatusLogin
        {
            get
            {
                return enumStatusLogin;
            }
            set
            {
                enumStatusLogin = value;
            }
        }

        internal statusUsuario StatusUsuario
        {
            get
            {
                return enumStatusUsuario;
            }
            set
            {
                enumStatusUsuario = value;
            }
        }


    #endregion

    #region "Métodos"

        public bool validaSenha(object obj, string strSenha)
        {

            bool retorno = false;

            if (obj == null || (obj.GetType() != typeof(Usuario)))
            {
                retorno = false;
            }
            else
            {
                if (strSenha == ((Usuario)obj).Senha)
                {
                    retorno = true;
                }
            }
            return retorno;
        }

        public bool validaNome(object obj, string strNome)
        {

            bool retorno = false;

            if (obj == null || (obj.GetType() != typeof(Usuario)))
            {
                retorno = false;
            }
            else
            {
                if (strNome == ((Usuario)obj).Nome)
                {
                    retorno = true;
                }
            }
            return retorno;
        }

        /// <summary>
        /// Valida usuário durante Login
        /// </summary>
        /// <param name="obj">Objeto a ser validado como Objeto Usuário</param>
        /// <param name="usuario">Valor do campo Usuario</param>
        /// <param name="senha">Valor do campo senha</param>
        /// <returns>Validação True
        ///          Nao validado False;</returns>
        public bool validaUsuario(object obj, string usuario, string senha)
        {
            bool retorno = false;
            if (Equals(obj))
            {
                if (validaNome(obj, usuario))
                {
                    if (validaSenha(obj, senha))
                    {
                        retorno = true;
                    }
                    else
                    {
                        retorno = false;
                    }
                }
                else
                {
                    retorno = false;
                }
            }
            else
            {
                retorno = false;
            }
            return retorno;
        }

        public override string ToString()
        {

            return " Código : " + Codigo + "\n" + Environment.NewLine +
                   " Pasta : " + Pasta + Environment.NewLine +
                   " Senha : " + Senha + Environment.NewLine +
                   " Nome : " + Nome + Environment.NewLine +
                   " Nome Completo :" + NomeCompleto + Environment.NewLine +
                   " Status Usuario :" + StatusUsuario;
        }

        public override bool Equals(object obj)
        {
            System.Type type = obj.GetType();
            if (obj == null || (type != typeof(Usuario)))
            {
                return false;
            }
            else
            {
                return Codigo == ((Usuario)obj).Codigo;
            }

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Registra o acesso do usuário na Tabela tb0207_Acessos.
        /// E define o status de Login do usuário como Usuario.StatusLogin.LOGADO OU NAOLOGADO
        /// </summary>
        /// <param name="user">Código do usuário.</param>
        /// <param name="tipodeAcao"> ENUM Usuario.StatusLogin da classe usuário</param>
        /// <returns>Retorna o código do acesso atual do usuário.</returns>
        public long registrarAcesso(Usuario user, Usuario.statusLogin tipodeAcao)
        {

            Int64 retorno = 0;
            this.StatusLogin = tipodeAcao;

            MainConfig.UsuarioLogado = user.Nome;
            MainConfig.CodigoUsuarioLogado = user.Codigo;
            sql01 = new StringBuilder();

            switch (tipodeAcao)
            {
                case statusLogin.LOGADO:
                    sql01.Length = 0;
                    //Insere o acesso e inicia a transação
                    sql01.Append("INSERT INTO tb0207_Acessos (usuarioACESSO, maquinaACESSO)");
                    sql01.Append(" VALUES (" + user.Codigo + ",'" + MainConfig.HostName + "')");
                    SqlServerConn.execCommandSql(sql01.ToString());
                    break;
                case statusLogin.NAOLOGADO:
                    sql01.Length = 0;
                    sql01.Append("UPDATE tb0207_Acessos");
                    sql01.Append(" SET encerradoACESSO = 1,horaencerramentoACESSO = getdate(),duracaoACESSO = DATEDIFF(MINUTE,horaaberturaACESSO,getDATE())");
                    sql01.AppendFormat(" WHERE codigoACESSO = {0}",MainConfig.CodigoAcesso);
                    SqlServerConn.execCommandSql(sql01.ToString());
                    return 0;
                default:
                    break;
            }

            //Recupera o código do acesso
            sql01.Length = 0;
            sql01.Append("SELECT MAX(codigoACESSO) AS novoACESSO");
            sql01.Append(" FROM tb0207_Acessos");
            System.Data.SqlClient.SqlDataReader dr = SqlServerConn.fillDataReader(sql01.ToString());
            if ((dr.FieldCount > 0))
            {
                while ((dr.Read()))
                {
                    retorno = (Int32)dr["novoACESSO"];
                }
            }

            SqlServerConn.closeConn();
            dr.Close();
            return retorno;

        }

        public long registrarAcesso(Usuario.statusLogin tipodeAcao)
        {

            Int64 retorno = 0;
            this.StatusLogin = tipodeAcao;

            MainConfig.UsuarioLogado = this.Nome;
            MainConfig.CodigoUsuarioLogado = this.Codigo;
            sql01 = new StringBuilder();

            switch (tipodeAcao)
            {
                case statusLogin.LOGADO:
                    sql01.Length = 0;
                    //Insere o acesso e inicia a transação
                    sql01.Append("INSERT INTO tb0207_Acessos (usuarioACESSO, maquinaACESSO)");
                    sql01.Append(" VALUES (" + this.Codigo + ",'" + MainConfig.HostName + "')");
                    SqlServerConn.execCommandSql(sql01.ToString());
                    break;
                case statusLogin.NAOLOGADO:
                    sql01.Length = 0;
                    sql01.Append("UPDATE tb0207_Acessos");
                    sql01.Append(" SET encerradoACESSO = 1,horaencerramentoACESSO = getdate(),duracaoACESSO = DATEDIFF(MINUTE,horaaberturaACESSO,getDATE())");
                    sql01.AppendFormat(" WHERE codigoACESSO = {0}", MainConfig.CodigoAcesso);
                    SqlServerConn.execCommandSql(sql01.ToString());
                    return 0;
                default:
                    break;
            }


            //Recupera o código do acesso
            sql01.Length = 0;
            sql01.Append("SELECT MAX(codigoACESSO) AS novoACESSO");
            sql01.Append(" FROM tb0207_Acessos");
            System.Data.SqlClient.SqlDataReader dr = SqlServerConn.fillDataReader(sql01.ToString());
            if ((dr.FieldCount > 0))
            {
                while ((dr.Read()))
                {
                    retorno = (Int32)dr["novoACESSO"];
                }
            }

            SqlServerConn.closeConn();
            dr.Close();
            return retorno;

        }

    #endregion
        
    }
}
