using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TitaniumColector.Utility;
using TitaniumColector.Classes.Dao;

namespace TitaniumColector.Forms
{
    public partial class FrmAcao : Form
    {

        public FrmAcao()
        {
            InitializeComponent();
            this.controlsConfig();
            
        }

        private void mnuAcao_Logout_Click(object sender, EventArgs e)
        {
            MainConfig.UserOn.registrarAcesso(TitaniumColector.Classes.Usuario.statusLogin.NAOLOGADO);
            frmLogin login = new frmLogin();
            login.Show();
            this.Close();
        }

        private void mnuAcao_Exit_Click(object sender, EventArgs e)
        {
            MainConfig.UserOn.registrarAcesso(TitaniumColector.Classes.Usuario.statusLogin.NAOLOGADO);
            Application.Exit();
        }

        private void btnVenda_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FrmProposta proposta = new FrmProposta();
            proposta.Show();
        }

        private void btnSaida_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Funcionalidade em desenvolvimento!!");
        }

    }
}