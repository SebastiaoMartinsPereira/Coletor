using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TitaniumColector.SqlServer;
using TitaniumColector.Classes;
using TitaniumColector.Classes.SqlServer;
using TitaniumColector.Forms;
using System.Reflection;
using System.Collections;
using TitaniumColector.Classes.Dao;
using TitaniumColector.Classes.Utility;


namespace TitaniumColector
{
    public partial class frmLogin : Form
    {   
        
        private Usuario objUsuario;
        private Usuario objUsuarioLoop;
        private SizeF sizeString;
        private DaoUsuario daoUsuario;
       
        public frmLogin()
        {
            try
            {

                daoUsuario = new DaoUsuario();

                //Configurações de criação do form
                InitializeComponent();
                //Seta as configurações principais a serem usadas na classe MainConfig
                MainConfig.setMainConfigurations();
                //Configura a string de conexão para conexão com o servidor.
                SqlServerConn.configuraStrConnection(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase), "strConn.txt");
                //Configura string de conexão E/OU cria a base mobile e todas as suas tabelas.
                BaseMobile.configurarBaseMobile();
                //Recupera Permissões a serem usadas no sistema mobile
                MainConfig.recuperaPermissoes();
                //Configura Layout
                this.configControls();
                //Preenche o combo de Usuários para que seja validado o Login
                this.preencheComboBoxUsuario(cbUsuario, daoUsuario.retornaListUsuarios(), Usuario.usuarioProperty.NOME, false);

            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                StringBuilder bdMsg = new StringBuilder();
                bdMsg.Append("\nOcorreu um problema durante a tentativa de conexão com a base de dados!");
                bdMsg.Append("\nA aplicação não poderá ser iniciada.");
                bdMsg.Append("\nDescription :" + sqlEx.Message);
                bdMsg.Append("\nSource :" + sqlEx.Source);
                MainConfig.errorMessage(bdMsg.ToString(), "Connection error");

                Application.Exit();

            }
            catch (Exception ex)
            {
                StringBuilder bdMsg = new StringBuilder();
                bdMsg.Append("\nO sistema não pode ser iniciado.");
                bdMsg.Append("\nFavor contate o administrador do sitema.");
                bdMsg.Append("\nDescription :" + ex.Message);
                MainConfig.errorMessage(bdMsg.ToString(), "Application Error.");

                Application.Exit();

            }
            finally 
            {
                daoUsuario = null;
            }


        }

        void btnImg_Click(object sender, EventArgs e)
        {
            MessageBox.Show("show deu certo");
        }

    #region "PRINCIPAIS MÉTODOS DO FORMULÁRIO"

        /// <summary>
        /// Preenche a ComboBox com um atributo de classe Usuario.
        /// </summary>
        /// <param name="cb">ComboBox a ser Preenchida</param>
        /// <param name="listUsuario">List carregada com objeto do tipo Usuario</param>
        /// <param name="prop">O atributo que será utilizado para carregar a ComboBox</param>
        /// <param name="dataSource">(TRUE)Define que a combo irá utilizar os dados da List como seu DataSource.
        ///                          (FALSE)Será feito um Loop nos objetos contidos na List não incluíndo na ComboBox os 
        ///                                 usuários com o statusUSUARIO = 0 (DESATIVADOS.)
        /// </param>
        private void preencheComboBoxUsuario(ComboBox cb, List<Usuario> listUsuario, Usuario.usuarioProperty prop,bool useDataSource)
        {
            string columnName = null;
            string displayName = null;

            if (useDataSource == true)
            {

                switch (prop)
                {

                    case Usuario.usuarioProperty.CODIGO:
                        columnName = "Codigo";
                        displayName = "Codigo";
                        break;
                    case Usuario.usuarioProperty.NOME:
                        columnName = "Nome";
                        displayName = "Nome";
                        break;
                    case Usuario.usuarioProperty.NOMECOMPLETO:
                        columnName = "NomeCompleto";
                        displayName = "NomeCompleto";
                        break;
                    default:
                        columnName = "Nome";
                        displayName = "Nome";
                        break;
                }

                cb.Items.Clear();
                cb.DataSource = listUsuario;
                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.DisplayMember = displayName;
                cb.ValueMember = columnName;
                cb.SelectedItem = null;

            }
            else 
            {
                
                cb.Items.Clear();

                //Loop em cada objeto contido no array
                objUsuarioLoop = new Usuario();
                foreach (Usuario user in listUsuario)
                {

                    objUsuarioLoop = user;

                    if (user.StatusUsuario == Usuario.statusUsuario.ATIVO) 
                    {
                        switch (prop)
                        {

                            case Usuario.usuarioProperty.CODIGO:

                                cb.DisplayMember = "Codigo";
                                cb.ValueMember = "Codigo";
                                cb.Items.Add(objUsuarioLoop.Codigo);
                                continue;

                            case Usuario.usuarioProperty.NOME:

                                cb.DisplayMember = "Nome";
                                cb.ValueMember = "Nome";
                                cb.Items.Add(objUsuarioLoop);
                                continue;

                            case Usuario.usuarioProperty.NOMECOMPLETO:

                                cb.DisplayMember = "NomeCompleto";
                                cb.ValueMember = "NomeCompleto";
                                cb.Items.Add(objUsuarioLoop.NomeCompleto);
                                continue;

                            default:

                                cb.DisplayMember = "Nome";
                                cb.ValueMember = "Nome";
                                cb.Items.Add(objUsuarioLoop.NomeCompleto);

                                break;
                        }

                    }

                }
                objUsuarioLoop = null;
            }

        }

        /// <summary>
        /// Válida a combo de usuário antes que o foco seja mudado para a TextBox Senha.
        /// </summary>
        /// <param name="e">Tecla digitada</param>
        /// <remarks>Para que a validação seja realizada a tecla enviada deve ser o Enter  char(13)</remarks>
        private void validarComboUsuario(KeyEventArgs e)
        {

            if ((e.KeyValue == (char)Keys.Enter))
            {
                if (cbUsuario.SelectedItem != null && txtSenha.Text != "")
                {
                    btLogin.Focus();
                }
                else if (cbUsuario.SelectedItem != null)
                {
                    txtSenha.Enabled = true;
                    txtSenha.Text = "";
                    txtSenha.Focus();
                }
            }
            else if ((e.KeyValue == (char)Keys.Space))
            {
                cbUsuario.Text = cbUsuario.Text.Trim();
                cbUsuario.SelectionStart = cbUsuario.Text.Length + 1;
            }
        }

        private void Logar()
        {
            try
            {
                if (cbUsuario.SelectedItem != null && txtSenha.Text.Trim() != "")
                {
                    if (cbUsuario.SelectedItem != null)
                    {
                        objUsuario = new Usuario((Usuario)cbUsuario.SelectedItem);

                        if (objUsuario.validaUsuario(cbUsuario.SelectedItem, cbUsuario.Text, txtSenha.Text))
                        {
                            MainConfig.UserOn = objUsuario;
                            MainConfig.CodigoAcesso = (Int64)objUsuario.registrarAcesso(MainConfig.UserOn, Usuario.statusLogin.LOGADO);
                            this.cbUsuario.Text = "";
                            this.txtSenha.Text = "";
                            FrmAcao frmAcao = new FrmAcao();

                            frmAcao.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show(" A senha digitada \n é inválida!!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            txtSenha.Text = "";
                            txtSenha.Focus();
                        }
                    }
                }
                else
                {
                    if (cbUsuario.SelectedItem != null && txtSenha.Text.Trim() == "")
                    {
                        txtSenha.Text = "";
                        txtSenha.Focus();
                    }
                    else
                    {
                        cbUsuario.Text = "";
                        cbUsuario.Focus();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

    #endregion  

    #region "//MÉTODOS COMUNS AO FORMULÁRIO"

        //               //
        // CB_USUARIO    //
        //               //

        private void cbUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                cbUsuario.Text = "";
            }
        }

        private void cbUsuario_KeyUp(object sender, KeyEventArgs e)
        {
            this.validarComboUsuario(e);
        }

        private void cbUsuario_Validate(KeyPressEventArgs e)
        {
            this.validarComboUsuario(new KeyEventArgs(Keys.Enter));
        }

        private void cbUsuario_GotFocus(object sender, EventArgs e)
        {
            cbUsuario.Text = "";
            txtSenha.Text = "";
        }

        private void cbUsuario_LostFocus(object sender, EventArgs e)
        {
            if(cbUsuario.Text.Trim() == "")
            {
                return;
            }
            else if (cbUsuario.Text != null && cbUsuario.SelectedItem != null)
            {
                this.validarComboUsuario(new KeyEventArgs(Keys.Enter));
            }
            else if (cbUsuario.SelectedItem == null)
            {
                cbUsuario.Focus();
            }
        }

        //               //
        //   TXT_SENHA   //
        //               //
        private void txtSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                MainConfig.GetFocusedControl(this);
                this.validarComboUsuario(new KeyEventArgs(Keys.Enter));
            }
        }

        //               //
        //  FRM_LOGIN    //
        //               //
        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && MainConfig.GetFocusedControl(this) != cbUsuario)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }

        //               //
        //    BUTTONS    //
        //               //
        private void btLogin_Click(object sender, EventArgs e)
        {
            this.Logar();
        }

        private void btSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    #endregion 
       
    }
}